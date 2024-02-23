using MediatR;

namespace riskwatch.api.search.Features.FuzzySearch.Command.CreateElasticSearchIndex;

public class CreateElasticSearchIndexCommand : IRequest<bool>
{
}