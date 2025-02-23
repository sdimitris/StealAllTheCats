using Microsoft.EntityFrameworkCore;
using StealAllTheCats.Application.Interfaces;
using StealAllTheCats.Application.Services;
using StealAllTheCats.Domain.Configuration;
using StealAllTheCats.Domain.Repositories;
using StealAllTheCats.Infrastructure.Database;
using StealAllTheCats.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();


// Register repositories & services
builder.Services.AddTransient<ICatRepository, CatRepository>();
builder.Services.AddTransient<ICatService, CatService>();

builder.Services.AddTransient<ICatTagRepository, CatTagRepository>();
builder.Services.AddTransient<IBreedService, BreedService>();
builder.Services.AddTransient<IBreedService, BreedService>();

builder.Services.AddTransient<ICatManager, CatManager>();
builder.Services.AddTransient<ICatsApiHttpService, CatsApiHttpService>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.Configure<CatsApiSettings>(
    builder.Configuration.GetSection("CatsApiSettings"));

builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddConsole());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();
