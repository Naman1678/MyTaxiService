using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyTaxiService.Data;
using MyTaxiService.Hubs;
using MyTaxiService.Repository;
using MyTaxiService.Repository.Interfaces;
using MyTaxiService.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Authentication
builder.Services.AddAuthentication(o =>
{
    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "http://localhost:5199",
        ValidAudience = "http://127.0.0.1:5500", // must match the client origin
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes("superSecretKey@345_superSecureKey!789"))
    };

    o.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/rideHub"))
            {
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization();

// Register DbContext
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register Repositories
builder.Services.AddScoped<IDriverRepository, DriverRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Register Services
builder.Services.AddScoped<BookingService>();

// Register SignalR
builder.Services.AddSignalR();

// Enable CORS
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("AllowSpecificOrigin", p =>
        p.WithOrigins("http://127.0.0.1:5500") // your frontend port
         .AllowAnyHeader()
         .AllowAnyMethod()
         .AllowCredentials());
});

// Add Controllers
builder.Services.AddControllers();

// Build the app
var app = builder.Build();

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowSpecificOrigin");
app.UseAuthentication();
app.UseAuthorization();

// Map Controllers and SignalR Hub
app.MapControllers();
app.MapHub<RideHub>("/rideHub");

app.Run();
