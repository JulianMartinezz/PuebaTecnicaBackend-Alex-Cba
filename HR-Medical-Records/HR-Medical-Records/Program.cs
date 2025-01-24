using HR_Medical_Records.Data;
using HR_Medical_Records.Middleware;
using HR_Medical_Records.Repository;
using HR_Medical_Records.Repository.Imp;
using HR_Medical_Records.Service.Imp;
using HR_Medical_Records.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors();

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<HRContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


//Remember Add Scopes for services!!!
builder.Services.AddScoped<IMedicalRecordService, MedicalRecordService>();
builder.Services.AddScoped<IMedicalRecordRepository, MedicalRecordRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseCors(c =>
    {
        c.AllowAnyHeader();
        c.AllowAnyMethod();
        c.WithOrigins("http://localhost:3000", "http://localhost:3001", "https://localhost:7123");
        c.AllowCredentials();
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<ErrorMiddleware>();

app.Run();
