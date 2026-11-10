using OmniFlowHub.Infrastructure;
using OmniFlowHub.Worker;

var builder = Host.CreateApplicationBuilder(args);
//Registrar dependencias de la capa de infraestructura (MySQL, S3, etc.)
builder.Services.AddInfrastructure(builder.Configuration);

//Registrar el servicio
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();