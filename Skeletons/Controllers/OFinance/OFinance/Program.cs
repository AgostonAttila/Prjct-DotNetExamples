using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OFinance.API.Services;
using OFinance.Application.Services;
using OFinance.Domain.Repositories;
using OFinance.Inrastructure.Data;
using OFinance.Inrastructure.Repositories;
using Serilog;
using Scalar.AspNetCore;
using OFinance.API.Models;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// Add Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("logs/items-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


// Database configuration
builder.Services.AddDbContext<ItemDbContext>(options =>
    options.UseInMemoryDatabase("InMemoryDb"));

// Identity configuration

builder.Services.AddIdentityCore<AppUser>(options =>
{
    //Lockout
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;
    //Password
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 10;
    options.Password.RequiredUniqueChars = 2;
    //SignIn 
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;
    //User
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;
}).AddEntityFrameworkStores<ItemDbContext>()
         .AddSignInManager<SignInManager<AppUser>>()
         .AddDefaultTokenProviders();

// Authentication



builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
          .AddJwtBearer(options =>
          {
              options.RequireHttpsMetadata = false;
              options.SaveToken = false;
              options.TokenValidationParameters = new TokenValidationParameters
              {
                  ValidateIssuer = true,
                  ValidateAudience = true,
                  ValidateLifetime = true,
                  ValidateIssuerSigningKey = true,
                  ClockSkew = TimeSpan.Zero,
                  ValidIssuer = builder.Configuration["Jwt:Issuer"],
                  ValidAudience = builder.Configuration["Jwt:Audience"],
                  IssuerSigningKey = new SymmetricSecurityKey(
                  Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT key not configured")))

              };
              options.Events = new JwtBearerEvents()
              {
                  OnMessageReceived = context =>
                  {
                      var accesToken = context.Request.Query["acces_token"];
                      if (!String.IsNullOrWhiteSpace(accesToken))
                      {
                          context.Token = accesToken;
                      }
                      return Task.CompletedTask;
                  },
                  OnAuthenticationFailed = c =>
                  {
                      c.NoResult();
                      c.Response.StatusCode = 500;
                      c.Response.ContentType = "text/plain";
                      return c.Response.WriteAsync(c.Exception.ToString());
                  },
                  OnChallenge = context =>
                  {
                      context.HandleResponse();
                      context.Response.StatusCode = 401;
                      context.Response.ContentType = "application/json";
                      var result = JsonConvert.SerializeObject("You are not Authorized");
                      return context.Response.WriteAsync(result);
                  },
                  OnForbidden = context =>
                  {
                      context.Response.StatusCode = 403;
                      context.Response.ContentType = "application/json";
                      var result = JsonConvert.SerializeObject("You are not authorized to access this resource");
                      return context.Response.WriteAsync(result);
                  },
              };
          });

// Add services
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<IItemService, ItemService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddAuthorization();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// Global error handling
app.UseExceptionHandler(appError =>
{
    appError.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";
        var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
        if (contextFeature != null)
        {
            Log.Error(contextFeature.Error, "Unhandled exception");
            await context.Response.WriteAsJsonAsync(new
            {
                StatusCode = context.Response.StatusCode,
                Message = "Internal Server Error"
            });
        }
    });
});

app.MapControllers();





app.Run();


