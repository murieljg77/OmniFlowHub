using OmniFlowHub.Application.Documents.Commands;
using OmniFlowHub.Application.Interfaces;
using OmniFlowHub.Infrastructure;
using OmniFlowHub.Infrastructure.Services;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

//1. Registrar servicios de Infrastructure (Base de datos, S3, etc.)
builder.Services.AddInfrastructure(builder.Configuration); 
builder.Services.AddScoped<IStorageService, S3StorageService>();

//2. Registrar los servicios y/o handlers de Application  
builder.Services.AddScoped<UploadDocumentCommandHandler>();

//3. Registrar controladores y el OpenAPI (nativo .NET 9)
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

//4. Configurar el pipeline de desarrollo
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    //Configurar Scalar en la ruta /scalar/v1
    app.MapScalarApiReference(options =>
    {
        options
            .WithTitle("OmniFlowHub API")
            .WithTheme(ScalarTheme.Mars);
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();