using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using TreinandoPráticasApi._4__Data;
using TreinandoPráticasApi.Configs.DTO;
using TreinandoPráticasApi.Configs.Filters;
using TreinandoPráticasApi.Configs.Logging;
using TreinandoPráticasApi.Configs.Providers;
using TreinandoPráticasApi.Data.Context;
using TreinandoPráticasApi.Exceptions;
using TreinandoPráticasApi.Logging;
using TreinandoPráticasApi.Providers;
using TreinandoPráticasApi.Repositories;
using TreinandoPráticasApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string file = Environment.GetEnvironmentVariable(@"LOG_FILE_PATH") ?? @"C:\Users\oaugu\source\repos\TreinandoPráticasApi\log.txt";

builder.Logging.AddConfigurationsLogger(file);

if (builder?.Logging == null)
{
    throw new InvalidOperationException("O sistema de logging não foi inicializado corretamente.");
}

var loggers = builder.Services.BuildServiceProvider().GetRequiredService<ILoggerFactory>().CreateLogger<Program>(); 

builder.Services.AddDIPScoppedClasse(loggers);
builder.Services.AddDIPSingletonClasse(loggers);
builder.Services.AddMapperStartup();
builder.Services.AddCofigurationJson();


string dbPassWord = Environment.GetEnvironmentVariable("DB_PASSWORD");

//Configurando conexão com banco de dados
string mySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection")
    .Replace("%DB_PASSWORD%", dbPassWord);

builder.Services.AddConectionBD(mySqlConnection);



var app = builder.Build();

var logger = app.Services.GetRequiredService<ILogger<Program>>();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var seedingService = services.GetRequiredService<SeedingServiceData>();
        seedingService.Seeding(); // Executa o método de seeding
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro ao executar o seeding: {ex.Message}");
    }
}

//Aqui fazemos as configurações dos middlewares usando a variável app
// Configure the HTTP request pipeline.



if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ConfigureExceptionHandler(logger);
}


//Defino os middlewares para direcionar a aplicação de http para https
app.UseHttpsRedirection();
//app.Use(async (context, next) =>
//        {
//            //adicionar código antes do request
//            await next(context);
//            //Adicionar código depois do request 
//        });
// Define o middleware de autorização para verificar os acessos
app.UseAuthorization();
//Mapeamento dos controladores
app.MapControllers();
//Usado apra adiconar um middleware terminal tambem

logger.LogInformation("Aplicação iniciada em modo {Environment}.", app.Environment.EnvironmentName);

app.Run();
