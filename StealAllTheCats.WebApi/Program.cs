using Microsoft.EntityFrameworkCore;
using StealAllTheCats.Application.Interfaces;
using StealAllTheCats.Application.Services;
using StealAllTheCats.Domain.Repositories;
using StealAllTheCats.Infrastructure.Database;
using StealAllTheCats.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();


// Register repositories & services
builder.Services.AddScoped<ICatRepository, CatRepository>();
builder.Services.AddScoped<ICatService, CatService>();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();
