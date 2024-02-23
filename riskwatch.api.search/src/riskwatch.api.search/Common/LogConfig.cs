// AppInfo.cs
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Core.Enrichers;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;

namespace APIBoilerplate.Common
{
    public static class LogConfig
    {
        #region logger config
        public static LoggerConfiguration ConfigureLogger(WebApplicationBuilder builder, LoggerConfiguration loggerConfiguration, string version, string instanceId, string index)
        {
            var elasticsearchUri = new Uri(Environment.GetEnvironmentVariable("ELK_Con"));
            var username = Environment.GetEnvironmentVariable("USERNAME_ELK");
            var password = Environment.GetEnvironmentVariable("PASSWORD_ELK");
            var credintials = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{username}:{password}"));

            return loggerConfiguration
                .ReadFrom.Configuration(builder.Configuration)
                .WriteTo.Console()
                .WriteTo.File(
                    "Logs/Debug/appname-debug-.json",
                    rollingInterval: RollingInterval.Day,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
                )
                .WriteTo.Logger(lc => lc
                    .Filter.ByExcluding(logEvent => logEvent.Level == LogEventLevel.Error)
                    .WriteTo.File(
                        "Logs/Info/appname-info-.json",
                        rollingInterval: RollingInterval.Day,
                        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
                    )
                )
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(elasticsearchUri)
                {
                    AutoRegisterTemplate = true,
                    IndexFormat = "riskwatch-search-logs-{0:yyyy.MM.dd}",
                    MinimumLogEventLevel = LogEventLevel.Debug,
                    ModifyConnectionSettings = conn => conn.BasicAuthentication(username, password)
                                                      .ServerCertificateValidationCallback((sender, certificate, chain, sslPolicyErrors) => true),

                })
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Error)
                .Enrich.WithProperty("version", version)
                .Enrich.WithProperty("instanceId", instanceId)
                .Enrich.WithProperty("index", index);
        }
    }
    #endregion
}


