using MediatR;
using System.Xml.Linq;

namespace QBSync.API.QBSDK.SalesOrder;

public class CreateSalesOrder : IRequest<Result<XElement>>
{
    public ListRef? CustomerRef { get; set; }
    public ListRef? TemplateRef { get; set; }
    public DateOnly? TxnDate { get; set; }
    public string? RefNumber { get; set; }
    public string? PONumber { get; set; }
    public List<SalesOrderLine>? SalesOrderLines { get; set; }
}


// This request will be updated to not only produce the XElement but also try to send it 
// to QuickBooks. For testing, we are just sending back the XElement.
internal class CreateSalesOrderHandler : IRequestHandler<CreateSalesOrder, Result<XElement>>
{
    public Task<Result<XElement>> Handle(CreateSalesOrder request, CancellationToken cancellationToken)
    {
        var add = new XElement("SalesOrderAdd");
        add.Append(request.CustomerRef);
        add.Append(request.TemplateRef);
        add.Append(request.TxnDate);
        add.Append(request.RefNumber);
        add.Append(request.PONumber);

        return Task.FromResult(Result.Ok(new XElement("SalesOrderAddRq", add)));
    }
}
