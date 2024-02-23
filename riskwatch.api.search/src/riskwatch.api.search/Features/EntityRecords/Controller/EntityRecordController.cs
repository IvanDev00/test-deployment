using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using riskwatch.api.search.Features.Common.Dto;
using riskwatch.api.search.Features.EntityRecords.Queries.GetAllEntityRecord;

namespace riskwatch.api.search.Features.EntityRecords.Controller;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/")]
[ApiController]

public class EntityRecordController : ControllerBase
{
    private readonly IMediator _mediator;
    public EntityRecordController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    // GET: api/EntityRecord
    [HttpGet("entity-record")]
    public async Task<ActionResult<List<EntityRecordDto>>> GetAll()
    {
        var response = await _mediator.Send(new GetEntityRecordQuery());
        return Ok(response);
    }    
}