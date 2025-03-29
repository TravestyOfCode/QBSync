using System.Xml.Linq;

namespace QBSync.API.QBSDK;

public class SalesOrder
{
    public string? TxnID { get; set; }
    public string? EditSequence { get; set; }
    public ListRef? CustomerRef { get; set; }
    public ListRef? TemplateRef { get; set; }
    public DateOnly? TxnDate { get; set; }
    public string? RefNumber { get; set; }
    public string? PONumber { get; set; }
    public List<SalesOrderLine> SalesOrderLines { get; set; } = [];

    internal static SalesOrder Create(string qbxml)
    {
        var msgsRs = XDocument.Parse(qbxml)?.Root?.Element("QBXMLMsgsRs");
        if (msgsRs == null)
        {
            throw new InvalidOperationException("Unable to parse a valid QBXMLMsgsRs.");
        }

        foreach (var element in msgsRs.Elements())
        {
            if (element.Name.LocalName.Equals("SalesOrderAddRs") || element.Name.LocalName.Equals("SalesOrderModRs"))
            {
                var salesOrderRet = element.Element("SalesOrderRet");
                if (salesOrderRet == null)
                {
                    throw new InvalidOperationException("Unable to get SalesOrderRet from SalesOrder response.");
                }

                var so = new SalesOrder();
                so.Parse(salesOrderRet);
                return so;
            }
        }

        throw new InvalidOperationException("Unable to parse a valid SalesOrder from the response.");
    }

    internal void Parse(XElement salesOrderRet)
    {
        foreach (var element in salesOrderRet.Elements())
        {
            switch (element.Name.LocalName)
            {
                case nameof(TxnID): TxnID = element.Value; break;
                case nameof(EditSequence): EditSequence = element.Value; break;
                case nameof(CustomerRef): CustomerRef = ListRef.Create(element); break;
                case nameof(TemplateRef): TemplateRef = ListRef.Create(element); break;
                case nameof(RefNumber): RefNumber = element.Value; break;
                case nameof(PONumber): PONumber = element.Value; break;
                case "SalesOrderLineRet": SalesOrderLines.Add(SalesOrderItemLine.Create(element)); break;
                case "SalesOrderLineGroupRet": SalesOrderLines.Add(SalesOrderGroupItemLine.Create(element)); break;
            }
        }
    }
}
