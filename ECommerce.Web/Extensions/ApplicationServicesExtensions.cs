using DomainLayer.Contracts;
using DomainLayer.Models.IdentityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using Persistence;
using Persistence.Data;
using Persistence.Repos;
using Service;
using ServiceAbstraction;
using StackExchange.Redis;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ECommerce.Web.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services,
            IConfiguration config)
        {
            services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });

            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var redisConnection = config.GetConnectionString("Redis")
                                      ?? "localhost:6379";

                return ConnectionMultiplexer.Connect(redisConnection);
            });

            services.AddDbContext<StoreIdentityDbContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("IdentityDbConnection"));
            });

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;

            })
                .AddEntityFrameworkStores<StoreIdentityDbContext>()
                .AddDefaultTokenProviders(); 

            services.AddScoped<IDataSeeding, DataSeeding>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IServiceManager, ServiceManager>();
            services.AddScoped<IRedisBasketRepo, RedisBasketRepo>();
            services.AddScoped<IBasketService, BasketService>();


            services.AddAutoMapper(cfg => { }, typeof(Service.AssemblyReference).Assembly);

            return services;
        }
    }
}
