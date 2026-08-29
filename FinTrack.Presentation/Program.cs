using FinTrack.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructureServices(builder.Configuration);

var jwtSection = builder.Configuration.GetSection("Jwt");
var jwtSecret = jwtSection["SecretKey"] ?? throw new InvalidOperationException("Jwt:SecretKey is missing.");
var jwtIssuer = jwtSection["Issuer"] ?? "FinTrack";
var jwtAudience = jwtSection["Audience"] ?? "FinTrack.Client";

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
            ClockSkew = TimeSpan.Zero
        };

        options.Events = new JwtBearerEvents
        {
            OnChallenge = async context =>
            {
                context.HandleResponse();
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";

                var payload = new
                {
                    statusCode = StatusCodes.Status401Unauthorized,
                    message = "Unauthorized. A valid Bearer token is required.",
                    path = context.Request.Path.Value
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(payload));
            }
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("FinTrackFrontend", policy =>
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi().AllowAnonymous();

    // 1. Scalar arayüzünü aktif ediyoruz
    app.MapScalarApiReference(options =>
    {
        options
            .WithTitle("FinTrack API")
            .WithTheme(ScalarTheme.Mars);
    }).AllowAnonymous();

    // 2. Kök dizine (/) gelen istekleri otomatik olarak /scalar/v1 adresine yönlendiriyoruz
    app.MapGet("/", async context =>
    {
        context.Response.Redirect("/scalar/v1");
        await Task.CompletedTask;
    }).AllowAnonymous();
}

app.UseCors("FinTrackFrontend");

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();