using Discount.Api.Services;
using Discount.Application.CQRS.Queries;
using Discount.Application.Mapper;
using Discount.Core.Interfaces;
using Discount.Infrastructure.Extensions;
using Discount.Infrastructure.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddAutoMapper(typeof(ProfileMapper));

var assemblies = new Assembly[]
{
    Assembly.GetExecutingAssembly(),
    typeof(GetDiscountByProductIdQueryHandler).Assembly
};

builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();

builder.Services.AddGrpc();

builder.Services.AddMediatR(r => r.RegisterServicesFromAssemblies(assemblies));



// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();
app.MigrateDatabase<Program>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    //app.MapOpenApi();
}
app.UseRouting();
app.MapGrpcService<DiscountService>();

app.Map("/", async context =>
{
    await context.Response.WriteAsync("Connect with gRPC....");
});
//app.UseAuthorization();

//app.MapControllers();

app.Run();
