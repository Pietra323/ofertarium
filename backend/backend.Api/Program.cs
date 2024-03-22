using System.Text.Json.Serialization;
using backend.Data.Models;
using backend.Data.Models.DataBase;
using backend.Data.Repositories;
using backend.Data.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c=>{c.EnableAnnotations();});
builder.Services.AddSingleton<DataBase>();
builder.Services.AddSingleton<IUserRepository, UserRepository>();
builder.Services.AddSingleton<IProductRepository, ProductRepository>();


builder.Services.AddControllers().AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(builder =>
    builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
);

app.UseHttpsRedirection();

app.MapControllers();

app.Run();