/* * Program.cs
 * * Application Startup & Configuration
 * -----------------------------------
 * This file configures the "Dependency Injection Container" (DI).
 * It registers all services (Database, Auth, AI) so they can be injected 
 * into your Controllers and Services automatically.
 */

using AI_Sensei.API.Data;
using AI_Sensei.API.Models;
using AI_Sensei.API.Repositories;
using AI_Sensei.API.Services;
using AI_Sensei.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using OpenAI.Chat;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Data Source=aisensei.db";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddControllers();

builder.Services.AddSingleton<ChatClient>(sp =>
{
    var key = Environment.GetEnvironmentVariable("OPEN_AI_KEY");

    // Check if key is missing and throw a clear error
    if (string.IsNullOrEmpty(key))
    {
        throw new InvalidOperationException("CRITICAL ERROR: 'OPEN_AI_KEY' environment variable is missing or empty.");
    }

    return new ChatClient(model: "gpt-4o-mini", apiKey: key);
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });
builder.Services.AddAuthorization();
builder.Services.AddScoped<IQuizRepository, QuizRepository>();
builder.Services.AddScoped<IQuizGenerator, OpenAIQuizService>();
builder.Services.AddScoped<QuizService>();

builder.Services.AddSwaggerGen(c =>
{
    // 1. Definition (The Lock Icon)
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter your valid token in the text input below.\r\n\r\nExample: \"ey...\""
    });

    // 2. Requirement (The Connection)
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            // The Key
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            // The Value (Scopes)
            new string[] {}
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000", "http://localhost:5173") // Frontend Ports
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
