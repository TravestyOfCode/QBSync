using MediatR;
using System.Xml.Linq;

namespace QBSync.API.QBSDK;

public class CreateSalesOrder : IRequest<Result<SalesOrder>>
{
    public string? TxnID { get; set; }
    public string? EditSequence { get; set; }
    public ListRef? CustomerRef { get; set; }
    public ListRef? TemplateRef { get; set; }
    public DateOnly? TxnDate { get; set; }
    public string? RefNumber { get; set; }
    public string? PONumber { get; set; }
    public List<SalesOrderLine>? SalesOrderLines { get; set; }

    internal XElement ToAddRq()
    {
        var elem = new XElement("SalesOrderAdd");
        elem.Append(CustomerRef);
        elem.Append(TemplateRef);
        elem.Append(TxnDate);
        elem.Append(RefNumber);
        elem.Append(PONumber);
        foreach (var line in SalesOrderLines ?? [])
        {
            elem.Add(line.ToAddRq());
        }
        return new XElement("SalesOrderAddRq", elem);
    }
}

internal class CreateSalesOrderHandler : IRequestHandler<CreateSalesOrder, Result<SalesOrder>>
{
    private readonly QBConnection _conn;

    private readonly ILogger<CreateSalesOrderHandler> _logger;

    public CreateSalesOrderHandler(QBConnection conn, ILogger<CreateSalesOrderHandler> logger)
    {
        _conn = conn;
        _logger = logger;
    }

    public async Task<Result<SalesOrder>> Handle(CreateSalesOrder request, CancellationToken cancellationToken)
    {
        try
        {
            var qbxml = request.ToAddRq().CreateQBXML().ToXML();

            var result = await _conn.ProcessRequest(qbxml, cancellationToken);

            return SalesOrder.Create(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error handling request: {request}", request);

            return Error.ServerError();
        }


        //return Task.FromResult(Result.Ok(request.ToAddRq()));
    }
}
