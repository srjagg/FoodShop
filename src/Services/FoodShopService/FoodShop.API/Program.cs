using FoodShop.API.Inicializator;
using FoodShop.Core.CoreImplement;
using FoodShop.Core.CoreInterface;
using FoodShop.Core.FluentValidation;
using FoodShop.Model.Models;
using FoodShop.Persistence;
using FoodShop.Repository.RepositoryImplement;
using FoodShop.Repository.RepositoryInterface;
using FoodShop.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Inyectamos el contexto
builder.Services.AddDbContext<FoodShopDbContext>();

//FluentValidation
builder.Services.AddTransient<UserValidator>();
builder.Services.AddTransient<FoodValidator>();

//UnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//repositories
builder.Services.AddScoped<IFoodRepository, FoodRepository>();
builder.Services.AddScoped<ILoginRepository, LoginRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

//Core
builder.Services.AddScoped<ILoginCore, LoginCore>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IUserCore, UserCore>();
builder.Services.AddScoped<IFoodCore, FoodCore>();
builder.Services.AddScoped<IOrderCore, OrderCore>();
builder.Services.AddScoped<IOrderCore, OrderCore>();

//Initializer
builder.Services.AddScoped<IBDInitializer, BDInitializer>();

// Registra las credenciales SMTP como opciones
builder.Services.Configure<SmtpOptions>(builder.Configuration.GetSection("Smtp"));

// Registra EmailCore como un servicio Singleton, proporcionando las dependencias necesarias en su constructor
builder.Services.AddSingleton(sp =>
{
    var smtpOptions = sp.GetRequiredService<IOptions<SmtpOptions>>().Value;
    return new EmailCore(smtpOptions.Host, smtpOptions.Port, smtpOptions.Username, smtpOptions.Password);
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Bearer",
        Description = "JWT Authorization header using the Bearer scheme",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    };
    c.AddSecurityDefinition("Bearer", securityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            Array.Empty<string>()
        }
    });
});

// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

//Aplicar migraciones y datos iniciales
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var initializer = services.GetRequiredService<IBDInitializer>();
        await initializer.InitializeAsync();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating or seeding the database.");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
