using MediatR;
using System.Xml.Linq;

namespace QBSync.API.QBSDK;

public class UpdateSalesOrder : IRequest<Result<XElement>>
{
    public required string TxnID { get; set; }
    public required string EditSequence { get; set; }
    public ListRef? CustomerRef { get; set; }
    public ListRef? TemplateRef { get; set; }
    public DateOnly? TxnDate { get; set; }
    public string? RefNumber { get; set; }
    public string? PONumber { get; set; }
    public List<SalesOrderLine>? SalesOrderLines { get; set; }

    internal XElement ToModRq()
    {
        var elem = new XElement("SalesOrderMod");
        elem.Append(TxnID);
        elem.Append(EditSequence);
        elem.Append(CustomerRef);
        elem.Append(TemplateRef);
        elem.Append(TxnDate);
        elem.Append(RefNumber);
        elem.Append(PONumber);
        foreach (var line in SalesOrderLines ?? [])
        {
            elem.Add(line.ToModRq());
        }
        return new XElement("SalesOrderModRq", elem);
    }
}

internal class UpdateSalesOrderHandler : IRequestHandler<UpdateSalesOrder, Result<XElement>>
{
    public Task<Result<XElement>> Handle(UpdateSalesOrder request, CancellationToken cancellationToken)
    {
        return Task.FromResult(Result.Ok(request.ToModRq()));
    }
}
