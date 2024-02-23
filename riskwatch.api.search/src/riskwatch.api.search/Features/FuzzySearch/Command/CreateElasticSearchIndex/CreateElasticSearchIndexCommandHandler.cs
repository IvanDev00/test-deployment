using MediatR;
using riskwatch.api.search.Features.FuzzySearch.Service.ElasticSearchService;

namespace riskwatch.api.search.Features.FuzzySearch.Command.CreateElasticSearchIndex;

public class CreateElasticSearchIndexCommandHandler : IRequestHandler<CreateElasticSearchIndexCommand, bool>
{
    private readonly IElasticSearchService _elasticSearchService;
    public CreateElasticSearchIndexCommandHandler(IElasticSearchService elasticSearchService)
    {
        _elasticSearchService = elasticSearchService;
    }
    
    public async Task<bool> Handle(CreateElasticSearchIndexCommand request, CancellationToken cancellationToken)
    {
        await _elasticSearchService.CreateIndexAsync();

        return true;
    }
}