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
//builder.Services.AddResponseCaching();

/******* Add Repository *******/
//builder.Services.AddScoped<IVillaRepository, VillaRepository>();
//builder.Services.AddScoped<IVillaNumberRepository, VillaNumberRepository>();
//builder.Services.AddScoped<IUserRepository, UserRepository>();

/******* Add AutoMapper *******/
/** Add support for converting DTOS to Models **/
//builder.Services.AddAutoMapper(typeof(MappingConfig));

/******* Add Versioning Services *******/
//builder.Services.AddApiVersioning(options =>
//{
//    options.AssumeDefaultVersionWhenUnspecified = true;
//    options.DefaultApiVersion = new ApiVersion(1, 0);
//    options.ReportApiVersions = true; // in the response header we'll get the api version supported

//});
//builder.Services.AddVersionedApiExplorer(options =>
//{
//    options.GroupNameFormat = "'v'VVV";
//    options.SubstituteApiVersionInUrl = true; // substitutes to v1 in most urls in swagger (and the url more generally)
//});

/******* Serilog Logging *******/
//// Configure Serilog
//Log.Logger = new LoggerConfiguration().MinimumLevel.Debug()
//    .WriteTo.File("log/villaLogs.txt", rollingInterval: RollingInterval.Infinite)
//    .CreateLogger();
//// Tell .NET to use Serilog - this comes with the Serilog.ASPNETCore package
//builder.Host.UseSerilog();

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
