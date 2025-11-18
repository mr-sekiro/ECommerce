
using DomainLayer.Contracts;
using ECommerce.Web.CustomMiddlewares;
using ECommerce.Web.Extensions;
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

            //Add Services (Clean Extension Method)
            builder.Services.AddApplicationServices(builder.Configuration);

            //Add Controller + Validation Error Factory
            builder.Services.AddApiControllers();

            //Add Swagger
            builder.Services.AddSwaggerDocumentation();

            var app = builder.Build();

            //Seed Database
            await app.SeedDatabaseAsync();

            //Use Middlewares (Clean Extension Method)
            app.UseApplicationMiddlewares();

            //Swagger
            if (app.Environment.IsDevelopment())
                app.UseSwaggerDocumentation();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthorization();

            app.MapControllers();
            app.Run();
        }
    }
}
