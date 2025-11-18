
using DomainLayer.Contracts;
using ECommerce.Web.CustomMiddlewares;
using ECommerce.Web.Factories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens.Experimental;
using Microsoft.OpenApi.Validations;
using Persistence;
using Persistence.Data;
using Persistence.Repos;
using PresentationLayer.CustomMiddlewares;
using Service;
using Service.MappingProfiles;
using ServiceAbstraction;
using Shared.ErrorModels;

namespace ECommerce.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddScoped<IDataSeeding, DataSeeding>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            //builder.Services.AddAutoMapper(X => X.AddProfile(new ProductProfile()));
            builder.Services.AddAutoMapper(cfg => { }, typeof(Service.AssemblyReference).Assembly);
            builder.Services.AddScoped<IServiceManager, ServiceManager>();

            builder.Services.AddControllers()
            .ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = ApiResponseFactory.GenerateApiValidationError;
            });

            var app = builder.Build();

            using var scope = app.Services.CreateScope();
            var objectDataSeeding = scope.ServiceProvider.GetRequiredService<IDataSeeding>();
            await objectDataSeeding.DataSeedAsync();

            app.UseMiddleware<ExceptionMiddleware>();
            app.UseMiddleware<NotFoundEndpointMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();

        }
    }
}
