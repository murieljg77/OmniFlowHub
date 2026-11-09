using OmniFlowHub.Infrastructure;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

//1. Registrar servicios de Infrastructure (Base de datos, S3, etc.)
builder.Services.AddInfrastructure(builder.Configuration);

//2. Registrar controladores y el OpenAPI (nativo .NET 9)
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

//3. Configurar el pipeline de desarrollo
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