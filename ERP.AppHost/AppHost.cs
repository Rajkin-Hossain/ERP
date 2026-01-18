var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.ERP_Products_Api>("erp-products-api");

builder.AddProject<Projects.ERP_MessageOrchestrator>("erp-messageorchestrator");

builder.Build().Run();
