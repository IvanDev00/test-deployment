using APIBoilerplate.Common;
using Microsoft.EntityFrameworkCore;
using Quartz;
using riskwatch.api.search.Database;
using riskwatch.api.search.Features.Common.Helper;
using riskwatch.api.search.Features.FuzzySearch.Jobs;
using riskwatch.api.search.Features.FuzzySearch.Service.ElasticSearchService;
using riskwatch.api.search.Features.FuzzySearch.Service.EntityRecordService;
using riskwatch.api.search.Features.Service.ElasticSearchService;

namespace riskwatch.api.search.Common
{
    public static class DependencyInjections
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services, Microsoft.Extensions.Configuration.ConfigurationManager configuration)
        {
            var RedisConnectionString = Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING");
            //Elasticsearch configuration
            var ElasticSearchUrl = Environment.GetEnvironmentVariable("ELK_Con");
            var ElasticSearchDefaultIndex = Environment.GetEnvironmentVariable("ELASTIC_SEARCH_DEFAULT_INDEX");

            services.AddStackExchangeRedisCache(redisOptions =>
            {
                redisOptions.Configuration = RedisConnectionString;
            });

            GlobalDbConnection.MysqlConnection = Environment.GetEnvironmentVariable("MYSQL_CONNECTION_STRING");
            string MysqlConnectionString = Environment.GetEnvironmentVariable("MYSQL_CONNECTION_STRING");
            services.AddDbContext<ApplicationDbContext>(options => options.UseMySql(MysqlConnectionString, MySqlServerVersion.AutoDetect(MysqlConnectionString))); //database config
            //ElasticSeach Configuration
            services.AddElasticSearch(ElasticSearchUrl, ElasticSearchDefaultIndex);
            services.AddScoped<ElasticsearchInitializer>();
            
            services.AddScoped<IElasticSearchService, ElasticSearchService>();
            services.AddScoped<IEntityRecordService, EntityRecordService>();

            //Cors policy
            services.AddCors(p => p.AddPolicy("corspolicy", build =>
                {
                    build.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
                }
            ));

            //Background service
            services.AddQuartz(q =>
            {
                q.UseMicrosoftDependencyInjectionJobFactory();

                var jobKey = new JobKey("BulkUpsertJob");
                q.AddJob<BulkUpsertJob>(j => j.WithIdentity(jobKey));


                q.AddTrigger(t => t
                               .ForJob(jobKey)
                               .WithIdentity("BulkUpsertTrigger")
                               .WithCronSchedule("0 0 0 * * ?", x => x.InTimeZone(TimeZoneInfo.FindSystemTimeZoneById("Asia/Manila"))));
            });

            services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
            
            services.AddTransient<StringSimilarityComparer>();
            //redis
            return services;
        }
    }
}
