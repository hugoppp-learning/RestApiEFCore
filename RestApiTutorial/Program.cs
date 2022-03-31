using DataStore.EF;
using Microsoft.EntityFrameworkCore;
using RestApiTutorial.DTOs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(); //.AddJsonOptions(o => o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<MyContext>(
    option =>
    {
        string provider = builder.Configuration.GetValue<string>("provider");
        _ = provider switch
        {
            "InMemory" => option.UseInMemoryDatabase("MyDb"),
            "Postgres" =>
                option.UseNpgsql(
                    builder.Configuration.GetConnectionString("postgres"),
                    x => x.MigrationsAssembly("DataStore.EF")
                ),
            _ => throw new ArgumentOutOfRangeException($"Unsuported provider {provider}")
        };
    });


builder.Services.AddAutoMapper(expression => expression.AddProfile(typeof(MapperProfile)));


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<MyContext>().Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
