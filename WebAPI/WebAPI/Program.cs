using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using WebAPI.Models; 

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApidbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("CadenaSQL")));
builder.Services.AddControllers().AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

var reglasCors = "ReglasCors";
builder.Services.AddCors(opt =>
{
    opt.AddPolicy(name : reglasCors, builder =>
        {
            builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(reglasCors);

app.UseAuthorization();

app.MapControllers();

app.Run();
