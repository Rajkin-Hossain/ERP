var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.ERP_Products_Api>("erp-products-api");

builder.Build().Run();
