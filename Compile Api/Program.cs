using Microsoft.EntityFrameworkCore;
using Compile_Api.Models;
using Compile_Api;
using Compile_Api.Controllers;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//app.MapPost("/compilecode", async (User user) =>
//{
//    CompileController con = new CompileController();
//    return JsonConvert.SerializeObject(con.GetCode(user));
//});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
