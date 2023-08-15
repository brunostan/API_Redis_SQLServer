using BookStore.Data;
using BookStore.DevTest;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DatabaseContext>(options
    => options
        .UseSqlServer(builder.Configuration
            .GetConnectionString("DefaultConnection")));

// Registrar o serviço PopulateDatabase
builder.Services.AddTransient<PopulateDatabase>();

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // Usar o método app.Use para criar um escopo de serviço e resolver o serviço PopulateDatabase dentro dele.
    app.Services
        .CreateScope().ServiceProvider
        .GetRequiredService<PopulateDatabase>()
        .Seed();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
