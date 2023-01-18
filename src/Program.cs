using System.Reflection;

using Microsoft.EntityFrameworkCore;

using BackendApi.Business;
using BackendApi.Middleware;
using BackendApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BackendApiContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("BackendApiDatabase")));

builder.Services.AddScoped<ILessonBusiness, LessonBusiness>();
builder.Services.AddScoped<IAchievementBusiness, AchievementBusiness>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,
    $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Custom middleware for a better error handling
app.UseBusinessExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
