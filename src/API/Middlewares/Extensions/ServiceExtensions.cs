using AutoMapper;
using Contexts;
using Microsoft.EntityFrameworkCore;
using Persistence.Repositories;
using Services.Abstraction.IApplicationServices;
using Services.Abstraction.ILoggerServices;
using Services.Abstraction.IRepositoryServices;
using Services.Implementation;
using Services.Implementation.Extensions;

namespace API.Extensions
{
    public static class ServiceExtensions
    {
        //  CORS
        public static void ConfigureCors(this IServiceCollection services) =>
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                builder.AllowAnyOrigin() // method which allows requests from any source
                .AllowAnyMethod()  // allow requests only from that concrete source
                .AllowAnyHeader() //  allows all HTTP methods
                // WithMethods("POST", "GET") that will allow only specific HTTP methods.
                //  AllowAnyHeader()
                // WithHeaders("accept", "contenttype") 
                .WithExposedHeaders("X-Pagination")
                );
            });

        // IIS config -  Internet Information Services
        public static void ConfigureIISIntegration(this IServiceCollection services) =>
            services.Configure<IISOptions>(options =>
            {
            });

        // Logger Service
        public static void ConfigureLoggerService(this IServiceCollection services) =>
            services.AddSingleton<ILoggerManager, LoggerManager>();

        public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration) =>
            services.AddDbContext<HsmsDbContext>(opts =>
            opts.UseSqlServer(configuration.GetConnectionString("sqlConnection")));

        public static void ConfigureRepositoryManager(this IServiceCollection services) =>
            services.AddScoped<IRepositoryManager, RepositoryManager>();

        public static void ConfigureServiceManager(this IServiceCollection services) =>
            services.AddScoped<IServiceManager, ServiceManager>();

        public static void AddAutoMapper(this IServiceCollection services) =>
            services.AddSingleton(x =>
                new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile(new MappingProfile());
                }).CreateMapper()
            );
    }
}
