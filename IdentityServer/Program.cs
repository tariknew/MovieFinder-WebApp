using IdentityServer.Configuration;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MovieShop.Database;
using MovieShop.Security;

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", false)
    .Build();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MWSContext>(options =>
options.UseSqlServer(config.GetConnectionString("Database")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole<int>>(o =>
{
    o.SignIn.RequireConfirmedAccount = true;
    o.Password.RequireDigit = false;
    o.Password.RequireLowercase = false;
    o.Password.RequireUppercase = false;
    o.Password.RequireNonAlphanumeric = false;
    o.Password.RequiredLength = 0;
}).AddEntityFrameworkStores<MWSContext>()
                .AddDefaultTokenProviders();

builder.Services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, UserClaimsPrincipalFactory<ApplicationUser, IdentityRole<int>>>();
builder.Services.AddTransient<IProfileService, ProfileService<ApplicationUser>>();

builder.Services.AddIdentityServer(options =>
{
    options.IssuerUri = "https://localhost:7244";
})
        .AddInMemoryApiScopes(InMemoryConfig.GetApiScopes())
        .AddAspNetIdentity<ApplicationUser>()
        .AddInMemoryClients(InMemoryConfig.GetClients())
        .AddDeveloperSigningCredential()
        .AddProfileService<ProfileService<ApplicationUser>>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.UseIdentityServer();

app.Run();
