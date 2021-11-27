using System.IO;
using HippoServiceExample.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using OpenServiceBroker;
using OpenServiceBroker.Catalogs;
using OpenServiceBroker.Instances;

namespace HippoServiceExample;

public class Startup
{

    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _hostingEnvironment;

    public Startup(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
    {
        _configuration = configuration;
        _hostingEnvironment = hostingEnvironment;
    }

    // Register services
    public void ConfigureServices(IServiceCollection services)
    {
        // Catalog of available services
        string catalogPath = File.ReadAllText(Path.Combine(_hostingEnvironment.ContentRootPath, "catalog.json"));
        services.AddSingleton(JsonConvert.DeserializeObject<Catalog>(catalogPath)!);

        // Database for storing provisioned service instances
        services.AddDbContext<Repositories.DbContext>(options => options.UseSqlite(_configuration.GetConnectionString("Database")));

        // Implementations of OpenServiceBroker.Server interfaces
        services.AddTransient<ICatalogService, CatalogService>()
            .AddTransient<IServiceInstanceBlocking, ServiceInstanceService>();

        // Open Service Broker REST API
        services.AddControllers()
            .AddOpenServiceBroker();

        // Swagger for testing
        services.AddSwaggerGen(options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Title = "Hippo Service Example",
                        Version = "v1"
                    });
                }).AddSwaggerGenNewtonsoftSupport();
    }

    // Configure HTTP request pipeline
    public void Configure(IApplicationBuilder app)
    {
        app.UseSwagger()
            .UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My Service Broker API v1"));

        app.UseRouting()
            .UseEndpoints(endpoints => endpoints.MapControllers());
    }

}
