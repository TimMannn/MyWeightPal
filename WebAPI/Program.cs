using System.IdentityModel.Tokens.Jwt;
using BLL;
using DAL;
using DAL.Models;
using DAL.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebAPI.Hubs;
using IdentityUser = Microsoft.AspNetCore.Identity.IdentityUser;

var builder = WebApplication.CreateBuilder(args);

var key = Encoding.ASCII.GetBytes("MySuperSecretKeyForJWT2024!ExtraLongKey123");

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "MyWeightPalApi",
            ValidAudience = "MyWeightPalClient",
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                // Laat toe dat SignalR token uit querystring haalt
                var accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;

                if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/gewichthub"))
                {
                    context.Token = accessToken;
                }

                return Task.CompletedTask;
            },
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"Authentication failed: {context.Exception.Message}");
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                Console.WriteLine("Token successfully validated");
                return Task.CompletedTask;
            }
        };
    });


JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

builder.Services.AddScoped<IGewichtData, GewichtData>();
builder.Services.AddScoped<IAccountData, AccountData>();
builder.Services.AddScoped<IUserData, UserData>();

builder.Services.AddScoped<GewichtService>();
builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<UserService>();

builder.Services.AddSignalR();

builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173", "https://localhost:5173", "http://localhost:3000", "https://localhost:3000")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseCors("AllowLocalhost");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<GewichtHub>("/gewichthub");
app.Run();

public partial class Program { }
