using Nest;

namespace riskwatch.api.search.Common;

public static class ElasticSearchConfig
{
    public static void AddElasticSearch(this IServiceCollection services, string baseUrl, string defaultIndex)
    {
        var settings = new ConnectionSettings(new Uri(baseUrl ?? ""))
            .PrettyJson()
            .DefaultIndex(defaultIndex);
        // .CertificateFingerprint("805813f22c23b826656f34dfea99219c57f3d783693b49573576adfc7ecd29cb")
        //.BasicAuthentication("elastic", "2WYgmBSdUELjWE*uSzb-")

        settings.EnableApiVersioningHeader();
        // AddDefaultMappings(settings);
        var client = new ElasticClient(settings);
        services.AddSingleton<IElasticClient>(client);
    }
}