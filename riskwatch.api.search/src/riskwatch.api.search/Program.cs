using APIBoilerplate.Common;
using APIBoilerplate.Common.Middleware;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using riskwatch.api.search.Common;
using riskwatch.api.search.Common.Middlewares;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);

//using serilog 
#region serilog

//Loging configuration
string version = "1.0.0";  //version ng api  
string instanceId = Guid.NewGuid().ToString();
string index = "RiskwatchFuzzySearch"; // name of your project

Log.Logger = LogConfig.ConfigureLogger(builder, new LoggerConfiguration(), version, instanceId, index).CreateLogger();
builder.Host.UseSerilog();

#endregion

try
{
    Log.Logger.Information("APPLICATION_STARTING");

    builder.Services.AddControllers().ConfigureValidators();

    //cache 
    builder.Services.AddMemoryCache();

    //automapper
    builder.Services.AddAutoMapper(typeof(Program));

    //mediatR
    builder.Services.AddMediatR(typeof(Program).Assembly);

    //Dependency Injection
    builder.Services
    .AddPresentation(builder.Configuration);

    //API Versioning
    builder.Services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
        }
    ).AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
      .AddJwtBearer(options => { SwaggerConfig.ConfigureAddAuthentication(options, builder.Configuration); });

    builder.Services.AddAuthorization();

    builder.Services.AddSwaggerGen(options => SwaggerConfig.ConfigureSwaggerGen(options, builder.Configuration));

    var app = builder.Build();
        
    using (var scope = app.Services.CreateScope())
    {
        var initializer = scope.ServiceProvider.GetRequiredService<ElasticsearchInitializer>();
        await initializer.CreateIndexIfNotExistsAsync();
    }
    
    Log.Information("APPLICATION_READY");

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(_ =>
        {
            _.SwaggerEndpoint("/swagger/v1/swagger.json", "Riskwatch Search API v1");
            _.DocExpansion(DocExpansion.List);
            _.EnableDeepLinking();

            // Enable OAuth2 authorization support in Swagger UI
            _.OAuthClientId("codex");
            _.OAuthAppName("Swagger");
        });
    }

    app.UseRouting();

    app.UseSerilogRequestLogging();

    app.UseExceptionHandler("/error");
    
    app.UseCors("corspolicy");

    app.UseHttpsRedirection();

    app.UseAuthentication();

    app.UseAuthorization();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Logger.Fatal(ex, "APPLICATION_ABORTED");

}
finally
{
    Log.CloseAndFlush();
}
