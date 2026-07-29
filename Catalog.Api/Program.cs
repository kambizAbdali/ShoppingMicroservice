using AutoMapper;
using Catalog.Application.Commands.Products;
using Catalog.Application.Mapper;
using Catalog.Application.Queries.Brands;
using Catalog.Application.Queries.Products;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using Catalog.Infrastructure.Data;
using Catalog.Infrastructure.Repositories;
using MediatR;
using Microsoft.OpenApi;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// 1️⃣ Add services
builder.Services.AddControllers();

// AutoMapper
builder.Services.AddAutoMapper(typeof(ProfileMapper).Assembly);

// API Versioning
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});

// MediatR
var assemblies = new Assembly[]
{
    Assembly.GetExecutingAssembly(),
    typeof(GetAllProductsQueryHandler).Assembly
};
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assemblies));

// Repositories
builder.Services.AddScoped<ICatalogContext, CatalogContext>();
builder.Services.AddScoped<IBrandRepository, BrandRepository>();
builder.Services.AddScoped<ITypeRepository, TypeRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Catalog.Api",
        Version = "v1",
        Description = "Catalog API"
    });
});

// 2️⃣ ✅ CORS must be registered BEFORE Build
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

// 3️⃣ Build app
var app = builder.Build();

// 4️⃣ Exception handling middleware (FIRST)
app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Unhandled Exception: {ex}");
        context.Response.StatusCode = 500;
        await context.Response.WriteAsync($"Error: {ex.Message}");
    }
});

// 5️⃣ Seed data (after Build but before running)
using (var scope = app.Services.CreateScope())
{
    var catalogContext = scope.ServiceProvider.GetRequiredService<ICatalogContext>();
    ProductSeedData.SeedData(catalogContext.Products);
    BrandSeedData.SeedData(catalogContext.Brands);
    TypeSeedData.SeedData(catalogContext.Types);
}

// 6️⃣ Configure pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

// 7️⃣ ✅ CORS middleware (after HttpsRedirection, before Authorization)
app.UseCors("AllowAll");

app.UseAuthorization();

// 8️⃣ Endpoints
app.MapGet("/", () => Results.Redirect("/swagger"));
app.MapControllers();

// 9️⃣ Run
app.Run();