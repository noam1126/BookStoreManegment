using Book.Store.Interfaces;
using Book.Store.Services;
using Microsoft.AspNetCore.Mvc;
using DM = Book.Store.Models.DM;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IBooksService, BooksService>();
builder.Services.AddControllers(); 

var app = builder.Build();

//app.UseCors("AllowAngular");

app.UseCors("AllowAll");


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
