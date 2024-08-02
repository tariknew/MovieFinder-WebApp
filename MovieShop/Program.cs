using InventoryManagementSoftware.Service.Hubs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MovieShop.Database;
using MovieShop.IServices;
using MovieShop.Security;
using MovieShop.Services;

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", false)
    .Build();

var builder = WebApplication.CreateBuilder(args);

// Database configuration
builder.Services.AddDbContext<MWSContext>(options =>
options.UseSqlServer(config.GetConnectionString("Database")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();

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

builder.Services.AddSwaggerGen(swagger =>
{
    //This is to generate the Default UI of Swagger Documentation    
    swagger.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "ASP.NET 5 Web API",
        Description = "Authentication and Authorization in ASP.NET 5 with JWT and Swagger"
    });
    // To Enable authorization using Swagger (JWT)    
    swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
    });
    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}

                    }
                });
});

// Injection
builder.Services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, UserClaimsPrincipalFactory<ApplicationUser, IdentityRole<int>>>();
builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<IActorService, ActorService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IAuthManagementService, AuthManagementService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IFavoriteMoviesService, FavoriteMoviesService>();


// SignalR
builder.Services.AddSignalR();

// AutoMapper configuration
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication("Bearer")
        .AddJwtBearer("Bearer", options =>
        {
            options.Authority = "https://localhost:7244";
            //options.Audience = "company-employee"; // ClientId of your API
            options.TokenValidationParameters.ValidateAudience = false;
            //Potrebno dodati kako bi AccessToken mogao isteci
            options.TokenValidationParameters.ClockSkew = TimeSpan.Zero;

        });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApiScope", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "openid");
    });
});

var app = builder.Build();
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Custom user-exception
app.UseMiddleware<ExceptionMiddleWare>();

app.MapHub<NotificationHub>("/notificationHub");

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseCors(
    options => options
        .SetIsOriginAllowed(x => _ = true)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials()
);

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
