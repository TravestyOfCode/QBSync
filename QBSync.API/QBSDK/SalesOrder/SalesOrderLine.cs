namespace QBSync.API.QBSDK.SalesOrder;

public abstract class SalesOrderLine
{
    public string TxnLineID { get; set; } = "-1";
    public ListRef? ItemRef { get; set; }
    public string? Desc { get; set; }
    public decimal? Quantity { get; set; }
}

public class SalesOrderGroupItemLine : SalesOrderLine
{
    public List<SalesOrderItemLine> Items { get; } = new List<SalesOrderItemLine>();
}

public class SalesOrderItemLine : SalesOrderLine
{
    public decimal? Rate { get; set; }
    public decimal? Amount { get; set; }
}