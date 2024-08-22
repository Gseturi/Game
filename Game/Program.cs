using Game.Client.Pages;
using Game.Components;
using Game.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.AspNetCore.Identity;

using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using Game.HUbs;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();
builder.Services.AddAntiforgery();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer("Server=WINDOWS-NJ9LCT3;Database=GameUserDataBase;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;"));

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddHttpClient();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
});




builder.Services.AddIdentity<AppUser, IdentityRole>()
       .AddEntityFrameworkStores<ApplicationDbContext>()
       .AddDefaultTokenProviders();




builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "https://localhost:7195/Login",
        ValidAudience = "https://localhost:7195/",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("wJnGzW5oGzStbHj2ZxTrkCvJ8xRhfLt2NzDpPv9rZtQ="))
    };

    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
            logger.LogError("Authentication failed.", context.Exception);
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("Token validated successfully.");
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddControllers();
builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();


builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}




app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting(); // Set up routing first

app.UseAuthentication(); // Authentication should be configured before Authorization
app.UseAuthorization();  // This is the key middleware that needs to be added and correctly placed

// If you are using Anti-Forgery, it should go here
app.UseAntiforgery();


app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Game.Client._Imports).Assembly);

app.MapHub<GameHub>("/Game");
app.MapHub<lobbyHub>("/LobyHub");

app.MapControllers();




app.Run();
