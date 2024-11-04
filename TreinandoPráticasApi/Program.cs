using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using TreinandoPr�ticasApi.Context;
using TreinandoPr�ticasApi.Exceptions;
using TreinandoPr�ticasApi.Filters;
using TreinandoPr�ticasApi.Logging;
using TreinandoPr�ticasApi.Repositories;
using TreinandoPr�ticasApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Adiciona o provedor de log personalizado(CustomLoggerProvider) ao sistema de log do ASP.NET Core, 
//definindo o n�vel m�nimo de log como o logLevel.Information
builder.Logging.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfiguration
{
    LogLevel = LogLevel.Information
}));

//Ignorando refer�ncia ciclica com JSON
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

//Configurando conex�o com banco de dados
string mySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");

//Conex�o com o banco
builder.Services.AddDbContext<AppDbContext>(options => options.UseMySql(
        mySqlConnection, ServerVersion.AutoDetect(mySqlConnection)
  ));

var valor1 = builder.Configuration["chave1"];

//Vai criar uma instancia unica por request
builder.Services.AddScoped<IProduto, ProdutoService>();
builder.Services.AddScoped<IUsuario, UsuarioService>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<ApiLoggingFilter>();


var app = builder.Build();

//Aqui fazemos as configura��es dos middlewares usando a vari�vel app
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ConfigureExceptionHandler();
}


//Defino os middlewares para direcionar a aaplica��o de http para https
app.UseHttpsRedirection();
//app.Use(async (context, next) =>
//        {
//            //adicionar c�digo antes do request
//            await next(context);
//            //Adicionar c�digo depois do request 
//        });
// Define o middleware de autoriza��o para verificar os acessos
app.UseAuthorization();
//Mapeamento dos controladores
app.MapControllers();
//Usado apra adiconar um middleware terminal tambem
app.Run();
