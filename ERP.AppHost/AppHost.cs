var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.ERP_Orchestrator>("erp-orchestrator");

builder.Build().Run();
