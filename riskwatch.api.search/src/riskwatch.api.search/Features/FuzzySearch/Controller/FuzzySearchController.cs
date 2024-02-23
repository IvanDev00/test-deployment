using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using riskwatch.api.search.Features.FuzzySearch.Command.BulkDocumentImport;
using riskwatch.api.search.Features.FuzzySearch.Command.CreateElasticSearchIndex;
using riskwatch.api.search.Features.FuzzySearch.Command.UpdateElasticSearchDocument;
using riskwatch.api.search.Features.FuzzySearch.Queries.Search;
using riskwatch.api.search.Features.FuzzySearch.Queries.SearchSuggestion;

namespace riskwatch.api.search.Features.FuzzySearch.Controller;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/")]

public class FuzzySearchController : ControllerBase
{
    private readonly IMediator _mediator;

    public FuzzySearchController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // POST: api/create-elasticsearch-index
    [HttpPost("create-elasticsearch-index")]
    public async Task<ActionResult> CreateIndex()
    {
        var response = await _mediator.Send(new CreateElasticSearchIndexCommand());
        if (!response)
        {
            return BadRequest();
        }
        return Ok();
    }

    // POST: api/bulk-document-import
    [HttpPost("bulk-document-import")]
    public async Task<ActionResult> BulkIndex()
    {
        var response = await _mediator.Send(new BulkDocumentImportCommand());
        if (!response)
        {
            return BadRequest();
        }
        return Ok();
    }

    // POST: api/update-elasticsearch-document
    [HttpPost("update-elasticsearch-document")]
    public async Task<ActionResult> UpdateDocument()
    {
        var response = await _mediator.Send(new UpdateElasticSearchDocumentCommand());
        if (!response)
        {
            return BadRequest();
        }
        return Ok();
    }

    // GET: api/search
    [HttpGet("search")]
    public async Task<ActionResult> Search(string searchTerm, int threshold = 85)
    {
        var response = await _mediator.Send(new SearchQuery(searchTerm, threshold));
        return Ok(response);
    }

    // GET: api/suggestion
    [HttpGet("search-suggestion")]
    public async Task<ActionResult> GetSearchSuggestions(string searchTerm)
    {
        var response = await _mediator.Send(new SearchSuggestionQuery { SearchTerm = searchTerm });
        return Ok(response);
    }
}