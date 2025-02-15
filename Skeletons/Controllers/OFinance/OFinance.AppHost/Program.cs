var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.OFinance_API>("ofinance");

builder.Build().Run();
