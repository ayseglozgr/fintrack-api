using FinTrack.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
// Infrastructure katmanındaki tüm servisleri ve PostgreSQL ayarını tek satırda içeri alıyoruz
builder.Services.AddInfrastructureServices(builder.Configuration);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
