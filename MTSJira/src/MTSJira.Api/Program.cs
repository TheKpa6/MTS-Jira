using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
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
using MTSJira.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MTS.Jira.Api", Version = "v1" });
    c.CustomSchemaIds(type => type.FullName);
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
//var jwtSettings = builder.Configuration.GetSection("Jwt");
//var key = Encoding.ASCII.GetBytes(jwtSettings["Secret"]);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.RequireHttpsMetadata = false;
                        options.TokenValidationParameters = JwtAuthOptions.GetSystemTokenValidationParameters();
                    });

// Services
//builder.Services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<ITaskService, TaskService>();

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseMiddleware<JwtMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

//// Apply migrations
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<JiraDbContext>();
    dbContext.Database.Migrate();
}

app.Run();