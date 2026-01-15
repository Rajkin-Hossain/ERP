var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.ERP_Orchestrator>("erp-orchestrator");

builder.AddProject<Projects.ERP_ProductModule_Presentation>("erp-productmodule-presentation");

builder.Build().Run();
