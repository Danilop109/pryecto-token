using Api.Extensions;
using Microsoft.EntityFrameworkCore;
using Persistencia;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAplicacionServices();
builder.Services.AddJwt(builder.Configuration);
builder.Services.AddHttpContextAccessor();//JWT 
builder.Services.AddDbContext<ApiTokenContext>(Options =>
{
    string connectionString = builder.Configuration.GetConnectionString("conexMysql");
    Options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
}
);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.UseAuthentication();//se coloca con el JWT, EN ESTE CASO ES IMPOTANTE QUE AUTENTICACION VAYA PRIMERO
app.UseAuthorization();

app.MapControllers();

app.Run();
