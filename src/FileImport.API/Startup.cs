using FileImport.Application.Handler;
using FileImport.Application.Interfaces;
using FileImport.Application.Services;
using FileImport.Domain;
using FileImport.Persistence.Database;
using FileImport.Persistence.Json;
using FileImport.Persistence.MultipleStorageServices;
using FileImport.Persistence.MultipleStorageServices.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;

namespace FileImport.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;

            var builder = new ConfigurationBuilder()
                .SetBasePath(environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var fileSettings = Configuration.GetSection("FileSettings");
            services.Configure<FileSettings>(fileSettings);

            services.AddDbContext<FileImportDbContext>(options => options.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"]));
            services.AddTransient<IFileProcessHandler, FileProcessHandler>();
            services.AddTransient<INormalizationService, NormalizationService>();
            services.AddTransient<ICsvParserService, CsvParserService>();

            services.AddScoped<IDataStorageRepository, DataStorageRepository>();
            services.AddScoped<DatabaseStorageService>();
            services.AddScoped<JsonDataStorageService>();

            services.AddScoped<Func<string, IDataStorageService>>(serviceProvider => key =>
            {
                return key switch
                {
                    "Database" => serviceProvider.GetService<DatabaseStorageService>(),
                    "JSON" => serviceProvider.GetService<JsonDataStorageService>(),
                    _ => serviceProvider.GetService<DatabaseStorageService>(),
                };
            });

            services.AddControllers();

            services.AddSwaggerDocument(config =>
            {
                config.PostProcess = document =>
                {
                    document.Info.Version = "v1";
                    document.Info.Title = "File Import API";
                    document.Info.Description = "File Import API documentation";
                };
            });

            services.Configure<FormOptions>(x =>
            {
                x.MultipartBodyLengthLimit = 209715200;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddFile("Logs/File-Import-{Date}.txt");
            loggerFactory.AddSerilog();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseOpenApi();
            app.UseSwaggerUi3();
        }
    }
}
