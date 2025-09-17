using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MTSJira.Api.Filters;
using MTSJira.Application.Commands;
using MTSJira.Application.InfrastructureContracts.Repositories;
using MTSJira.Application.Services.JwtService;
using MTSJira.Application.Services.JwtService.Contracts;
using MTSJira.Application.Services.JwtService.Options;
using MTSJira.Application.Services.TaskService;
using MTSJira.Application.Services.TaskService.Contract;
using MTSJira.Infrastructure.Database.Contexts;
using MTSJira.Infrastructure.Database.Extensions;
using MTSJira.Infrastructure.Repositories;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MTS.Jira.Api", Version = "v1" });
    c.CustomSchemaIds(type => type.FullName);

    string directory = AppContext.BaseDirectory;
    string filePath = Path.Combine(directory, "MTSJira.Api.xml");
    c.IncludeXmlComments(filePath);

    c.OperationFilter<AuthResponsesOperationFilter>();
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
});

//Database
builder.Services.AddDbContext<JiraDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// MediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CreateTaskCommand).Assembly));


// JWT Authentication
var jwtAuthOptions = builder.Configuration.GetSection(nameof(JwtAuthOptions)).Get<JwtAuthOptions>();
builder.Services.Configure<JwtAuthOptions>(builder.Configuration.GetSection(nameof(JwtAuthOptions)));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.RequireHttpsMetadata = false;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidIssuer = jwtAuthOptions!.Issuer,
                            ValidateAudience = true,
                            ValidAudience = jwtAuthOptions!.Audience,
                            ValidateLifetime = true,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtAuthOptions!.Key)),
                            ValidateIssuerSigningKey = true,

                            RequireExpirationTime = true,

                            ClockSkew = TimeSpan.Zero
                        };
                    });

// Services
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<ITaskService, TaskService>();

// Repository
builder.Services.AddScoped<ITaskRepository, TaskRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.MigrateDatabase();

app.Run();