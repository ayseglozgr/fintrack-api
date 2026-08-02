using FinTrack.Infrastructure;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    // 1. Scalar arayüzünü aktif ediyoruz
    app.MapScalarApiReference(options =>
    {
        options
            .WithTitle("FinTrack API")
            .WithTheme(ScalarTheme.Mars);
    });

    // 2. Kök dizine (/) gelen istekleri otomatik olarak /scalar/v1 adresine yönlendiriyoruz
    app.MapGet("/", async context =>
    {
        context.Response.Redirect("/scalar/v1");
        await Task.CompletedTask;
    });
}

app.UseAuthorization();
app.MapControllers();

app.Run();