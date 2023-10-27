using FFS.Application.Data;
using FFS.Application.DTOs;
using FFS.Application.DTOs.Common;
using FFS.Application.DTOs.Email;
using FFS.Application.Entities;
using FFS.Application.Infrastructure.Interfaces;
using FFS.Application.Repositories;
using FFS.Application.Repositories.Impls;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(ApplicationMapper).Assembly);


#region database
var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
 options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
builder.Services.Configure<EmailSettingModel>(configuration.GetSection("MailSettings"));
#endregion

#region authentication
builder.Services.Configure<AppSetting>(builder.Configuration.GetSection("JWT"));
builder.Services.Configure<EmailSettingModel>(builder.Configuration.GetSection("MailSettings"));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"])),
    };
});

builder.Services.Configure<DataProtectionTokenProviderOptions>(opts => opts.TokenLifespan = TimeSpan.FromMinutes(30));
#endregion

#region repository
builder.Services.AddTransient(typeof(IRepository<,>), typeof(EntityRepository<,>));
builder.Services.AddTransient<IAuthRepository, AuthRepository>();
builder.Services.AddTransient<ITokenRepository, TokenRepository>();
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddTransient<IStoreRepository, StoreRepository>();
builder.Services.AddTransient<IInventoryRepository, InventoryRepository>();
builder.Services.AddTransient<ILocationRepository, LocationRepository>();
builder.Services.AddTransient<IFoodRepository, FoodRepository>();
builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();
builder.Services.AddTransient<IComboRepository, ComboRepository>();

#endregion

#region swagger
builder.Services.AddSwaggerGen((option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Project API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
}));
#endregion

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}




app.UseHttpsRedirection();
app.UseCors(opt => opt.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
