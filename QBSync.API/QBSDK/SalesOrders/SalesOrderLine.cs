using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace QBSync.API.QBSDK;

[JsonDerivedType(typeof(SalesOrderItemLine), typeDiscriminator: "SalesOrderItemLine")]
[JsonDerivedType(typeof(SalesOrderGroupItemLine), typeDiscriminator: "SalesOrderGroupItemLine")]
public abstract class SalesOrderLine
{
    public string TxnLineID { get; set; } = "-1";
    public ListRef? ItemRef { get; set; }
    public string? Desc { get; set; }
    public decimal? Quantity { get; set; }

    public abstract XElement ToAddRq();
    public abstract XElement ToModRq();
}

public class SalesOrderGroupItemLine : SalesOrderLine
{
    public List<SalesOrderItemLine> Items { get; } = new List<SalesOrderItemLine>();

    public override XElement ToAddRq()
    {
        var elem = new XElement("SalesOrderLineGroupAdd");
        elem.Append(ItemRef, "ItemGroupRef");
        elem.Append(Quantity);
        return elem;
    }

    public override XElement ToModRq()
    {
        var elem = new XElement("SalesOrderLineGroupMod");
        elem.Append(TxnLineID);
        elem.Append(ItemRef, "ItemGroupRef");
        elem.Append(Quantity);
        foreach (var item in Items ?? [])
        {
            elem.Add(item.ToModRq());
        }
        return elem;
    }
}

public class SalesOrderItemLine : SalesOrderLine
{
    public decimal? Rate { get; set; }
    public decimal? Amount { get; set; }

    public override XElement ToAddRq()
    {
        var elem = new XElement("SalesOrderLineAdd");
        elem.Append(ItemRef);
        elem.Append(Desc);
        elem.Append(Quantity);
        elem.Append(Rate);
        elem.Append(Amount);
        return elem;
    }

    public override XElement ToModRq()
    {
        var elem = new XElement("SalesOrderLineMod");
        elem.Append(TxnLineID);
        elem.Append(ItemRef);
        elem.Append(Desc);
        elem.Append(Quantity);
        elem.Append(Rate);
        elem.Append(Amount);
        return elem;
    }
}