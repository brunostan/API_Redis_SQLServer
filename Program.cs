using BookStore.Data;
using BookStore.DevTest;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Redis Cache
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379";
    options.InstanceName = "SampleInstance";
});

// SQL Server BD
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlServer(builder.Configuration
        .GetConnectionString("DefaultConnection")));

// Registrar o serviço PopulateDatabase
builder.Services.AddTransient<PopulateDatabase>();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.Services
        .CreateScope().ServiceProvider
        .GetRequiredService<PopulateDatabase>()
        .Seed();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
