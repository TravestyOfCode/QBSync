using MediatR;
using Microsoft.AspNetCore.Mvc;
using QBSync.API.QBSDK;

namespace QBSync.API.Controllers;

[ApiController()]
[Route("{controller}/{action}")]
public class SalesOrderController : Controller
{
    private readonly IMediator _mediator;

    public SalesOrderController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSalesOrder request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);

        return StatusCode(result.StatusCode, result.Value?.ToString());
    }

    [HttpPost]
    public async Task<IActionResult> Update([FromBody] UpdateSalesOrder request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);

        return StatusCode(result.StatusCode, result.Value?.ToString());
    }
}
