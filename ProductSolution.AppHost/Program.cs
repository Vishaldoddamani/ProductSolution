var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Product_WebApi>("product-webapi");

builder.Build().Run();
