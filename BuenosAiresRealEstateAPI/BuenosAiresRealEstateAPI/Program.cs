using BuenosAiresRealEstate.API.Data;
using BuenosAiresRealEstate.API.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Collections.Generic;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using BuenosAiresRealEstate.API;
using BuenosAiresRealEstate.API.Utilities;
using BuenosAiresRealEstate.API.RepositoryInterfaces;
using BuenosAiresRealEstate.API.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

/******* Add Database *******/
builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQLConnection"));
});

/******* Add .NET Default Identity Configurations *******/
//ApplicationUser is a custom class that inherits from IdentityUser
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();

/******* Add Caching *******/
builder.Services.AddResponseCaching();

/******* Add Repository *******/
builder.Services.AddScoped<IApartmentComplexRepository, ApartmentComplexRepository>();
builder.Services.AddScoped<IApartmentUnitRepository, ApartmentUnitRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

/*
When you use AddScoped, a new instance of the service is created for each HTTP request. 
The same instance is then reused throughout the entire request processing pipeline. This means that 
all the classes within a single HTTP request will get the same instance of the service. Once the 
request ends, the scoped instances are disposed of.

When you use AddSingleton, a single instance of the service is created and shared across the 
entire application's lifetime. It means that the same instance of the service will be used for 
all requests. If you have multiple HTTP requests, they will all use the same instance of the service.

With AddTransient, a new instance of the service is created every time it is requested. 
This means that different classes within the same request or different requests will receive 
different instances of the service.
*/

/******* Add AutoMapper *******/
/** Add support for converting DTOS to Models **/
builder.Services.AddAutoMapper(typeof(Mapping));

/******* Add Versioning Services *******/
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true; // in the response header we'll get the api version supported

});
builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true; // substitutes to v1 in most urls in swagger (and the url more generally)
});

/******* Serilog Logging *******/
// Configure Serilog
Log.Logger = new LoggerConfiguration().MinimumLevel.Debug()
    .WriteTo.File("Logs/ApartmentComplexLogs.txt", rollingInterval: RollingInterval.Infinite)
    .CreateLogger();
// Tell .NET to use Serilog - this comes with the Serilog.ASPNETCore package
builder.Host.UseSerilog();

/**** Here in AddControllers parenthesis we can add the cache profile ****/
builder.Services.AddControllers(option =>
{
    option.CacheProfiles.Add("Default30",
        new CacheProfile()
        {
            Duration = 30
        });
}).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

/******* Configure Swagger *******/
builder.Services.AddSwaggerGen(options =>
{
    // configure documentation for the swagger UI
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1.0",
        Title = "Buenos Aires Real Estate API",
        Description = "API to manage Apartment Rental",
        TermsOfService = new Uri("HTTPS://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Jose Iriarte",
            Url = new Uri("https://example.com")
        },
        License = new OpenApiLicense
        {
            Name = "Example License",
            Url = new Uri("https://example.com/license")
        }
    });
    options.SwaggerDoc("v2", new OpenApiInfo
    {
        Version = "v2.0",
        Title = "Buenos Aires Real Estate API V2",
        Description = "API to manage Apartment Rental",
        TermsOfService = new Uri("HTTPS://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Jose Iriarte",
            Url = new Uri("https://example.com")
        },
        License = new OpenApiLicense
        {
            Name = "Example License",
            Url = new Uri("https://example.com/license")
        }
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "BuenosAiresRealEstateV1");
        options.SwaggerEndpoint("/swagger/v2/swagger.json", "BuenosAiresRealEstateV2");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
