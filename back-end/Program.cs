using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Http;
using System.Collections.ObjectModel;
using Backend.Models;
using Backend.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin();
    });
});

var app = builder.Build();

app.UseCors("AllowFrontend");
app.UseHttpsRedirection();

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new Exception();

app.MapProductEndpoints(connectionString);
app.MapCategoryEndpoints(connectionString);
app.MapDeliveryOptionEndpoints(connectionString);
app.MapOrderEndpoints(connectionString);
app.MapProductPagesEndpoints(connectionString);

app.Run();