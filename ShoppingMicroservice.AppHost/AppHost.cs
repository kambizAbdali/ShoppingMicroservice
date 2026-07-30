var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Basket_Api>("basket-api");

builder.Build().Run();
