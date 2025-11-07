using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;
using PokemonGame.Core.Interfaces.Repositories;
using PokemonGame.Domain.Services;
using PokemonGame.Core.Interfaces.Services;
using PokemonGame.Domain.Repositories;
using PokemonGame.Middlewares;
using PokemonGame.Domain.Mapper;
using PokemonGame.Core.Models.Entities;
using PokemonGame.Core.Settings;
using PokemonGame.Hubs;
using PokemonGame.Persistence.DAL;
using MongoDB.Bson;
using MongoDB.Driver;
using AspNetCore.Identity.MongoDbCore.Infrastructure;
using AspNetCore.Identity.MongoDbCore.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//add mongoIdentityConfiguration
var dbConnection = builder.Configuration["PokemonDatabase:DBConnection"];
var databaseName = builder.Configuration["PokemonDatabase:DatabaseName"];
var mongoDbIdentityConfig = new MongoDbIdentityConfiguration
{
    MongoDbSettings = new MongoDbSettings
    {
        ConnectionString = dbConnection,
        DatabaseName = databaseName,
    },
    IdentityOptionsAction = options =>
    {
        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 8;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequireLowercase = false;

        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
        options.Lockout.MaxFailedAccessAttempts = 5;

        options.User.RequireUniqueEmail = true;
    }
};

builder.Services.AddSignalR();
builder.Services.ConfigureMongoDbIdentity<ApplicationUser, ApplicationRole, ObjectId>(mongoDbIdentityConfig)
    .AddUserManager<UserManager<ApplicationUser>>()
    .AddSignInManager<SignInManager<ApplicationUser>>()
    .AddRoleManager<RoleManager<ApplicationRole>>()
    .AddDefaultTokenProviders();

builder.Services.AddStackExchangeRedisCache(redisOptions =>
{
    var connectionHost = builder.Configuration["RedisCache:Host"];

    redisOptions.Configuration = connectionHost;
    redisOptions.InstanceName = "PokemonGame";
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            Array.Empty<string>()
        }
    });
});

builder.Services.AddControllers()
        .AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
            options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
        });

builder.Services.AddCors(option =>
{
    option.AddPolicy("AllowFE", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "https://localhost:7217")
       .AllowAnyHeader()
       .AllowAnyMethod()
       .AllowCredentials();
    });
});

//mapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

//Configuration
builder.Services.Configure<DatabaseSetting>(
    builder.Configuration.GetSection("PokemonDatabase")
);

builder.Services.Configure<AppSetting>(
    builder.Configuration.GetSection("AppSettings")
);

//Identity
var mongoDbSetting = builder.Configuration.GetSection("PokemonDatabase").Get<DatabaseSetting>();
var settings = MongoClientSettings.FromConnectionString(mongoDbSetting.DBConnection);
settings.ConnectTimeout = TimeSpan.FromSeconds(60);
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddMongoDbStores<ApplicationUser, ApplicationRole, ObjectId>(
        mongoDbSetting.DBConnection, mongoDbSetting.DatabaseName
);
builder.Services.AddControllersWithViews();

//Repository
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPokemonRepository, PokemonRepository>();
builder.Services.AddScoped<IMoveRepository, MoveRepository>();
builder.Services.AddScoped<IRoomChatRepository, RoomChatRepository>();
builder.Services.AddScoped<IRoomBattleRepository, RoomBattleRepository>();

//Services
builder.Services.AddSingleton<IDatabaseSetting, DatabaseSetting>(st =>
    st.GetRequiredService<IOptions<DatabaseSetting>>().Value
);
builder.Services.AddSingleton<IMongoContext, MongoContext>();
builder.Services.AddScoped<IUserContextService, UserContextService>();
builder.Services.AddScoped<IPokemonService, PokemonService>();
builder.Services.AddScoped<IRoomChatService, RoomChatService>();
builder.Services.AddScoped<IRoomBattleService, RoomBattleService>();
builder.Services.AddScoped<IMoveService, MoveService>();
builder.Services.AddSingleton<IMongoDatabase>(sp =>
{
    var databaseSetting = sp.GetRequiredService<IOptions<DatabaseSetting>>().Value;
    var client = new MongoClient(databaseSetting.DBConnection);
    return client.GetDatabase(databaseSetting.DatabaseName);
});
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRankingService, RankingService>();
builder.Services.AddSingleton<ICacheService, CacheService>();

//JWT
var secretKey = builder.Configuration["AppSettings:SecretKey"];
var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
builder.Services.Configure<DataProtectionTokenProviderOptions>(opt =>
{
    opt.TokenLifespan = TimeSpan.FromMinutes(20);
});
//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//    //options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
//}).AddJwtBearer(options =>
//{
//    options.SaveToken = true;
//    options.RequireHttpsMetadata = false;
//    options.TokenValidationParameters = new
//    Microsoft.IdentityModel.Tokens.TokenValidationParameters
//    {
//        ValidateIssuer = true,
//        ValidateAudience = true,
//        ValidateLifetime = true,
//        ClockSkew = TimeSpan.Zero,
//        ValidAudience = builder.Configuration["JWT:ValidAudience"],
//        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
//        IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes)
//    };
//});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");

            if (string.IsNullOrEmpty(accessToken))
            {
                accessToken = context.Request.Query["access_token"];
            }

            if (!string.IsNullOrEmpty(accessToken))
            {
                context.Token = accessToken;
            }

            return Task.CompletedTask;
        }
    };

    options.TokenValidationParameters = new
    Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes)
    };
});

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration["RedisCacheSettings:ConnectionString"];
    options.InstanceName = builder.Configuration["RedisCacheSettings:InstanceName"];
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//middelewares
app.UseMiddleware<ResponseApiMiddleware>();

app.UseCors("AllowFE");

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

//seed data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var seedData = new SeedData(
        services.GetRequiredService<UserManager<ApplicationUser>>(),
        services.GetRequiredService<RoleManager<ApplicationRole>>(),
        services.GetRequiredService<IMongoDatabase>());

    await seedData.SeedingData();
}

//hubs
app.MapHub<ChatHub>("/chatHub");
app.MapHub<GameHub>("/gameHub");

app.Run();
