using Catalog.Application.Commands.Products;
using Catalog.Application.Mapper;
using Catalog.Application.Queries.Brands;
using Catalog.Application.Queries.Products;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using Catalog.Infrastructure.Data;
using Catalog.Infrastructure.Repositories;
using AutoMapper; 
using MediatR;
using Microsoft.OpenApi;
using System.Reflection;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add Automapper
builder.Services.AddAutoMapper(typeof(ProfileMapper).Assembly);

// Add Versioning
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});

// Add MediatR
var assemblies = new Assembly[]
{
    Assembly.GetExecutingAssembly(), //اگر همه هندلرها در یک اسمبلی باشند، آوردن فقط یکی از تایپ های همان اسمبلی کافی است.
    typeof(GetAllProductsQueryHandler).Assembly // حتماً اسمبلی لایه اپلیکیشن را اضافه کنید
};

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assemblies));

//instead of following DIs, We can use previous code
//builder.Services.AddTransient<IRequestHandler<CreateProductCommand, ProductResponse>, CreateProductCommandHandler>();
//builder.Services.AddTransient<IRequestHandler<DeleteProductCommand, bool>, DeleteProductCommandHandler>();
//builder.Services.AddTransient<IRequestHandler<UpdateProductCommand, bool>, UpdateProductCommandHandler>();

//DIs
builder.Services.AddScoped<ICatalogContext, CatalogContext>();
builder.Services.AddScoped<IBrandRepository, BrandRepository>();
builder.Services.AddScoped<ITypeRepository, TypeRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddEndpointsApiExplorer();

// اضافه کردن Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Catalog.Api",
        Version = "v1",
        Description = "Catalog API"
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var catalogContext = scope.ServiceProvider.GetRequiredService<ICatalogContext>();
    ProductSeedData.SeedData(catalogContext.Products);
    BrandSeedData.SeedData(catalogContext.Brands);
    TypeSeedData.SeedData(catalogContext.Types);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // فعال‌سازی Swagger و Swagger UI
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

// مسیریاب پیش‌فرض: هدایت ریشه به Swagger UI
app.MapGet("/", () => Results.Redirect("/swagger"));

app.MapControllers();

app.Run();