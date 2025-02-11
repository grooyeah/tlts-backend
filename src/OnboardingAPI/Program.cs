using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OnboardingAPI.Database;
using OnboardingAPI.Models;
using OnboardingAPI.Repositories.Users;
using OnboardingAPI.Services.Users;
using OnboardingAPI.Validators.Users;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUserService, UserServiceImpl>();
builder.Services.AddScoped<IUserRepository, UserRepositoryImpl>();

builder.Services.AddScoped<IValidator<User>, UserValidator>();

builder.Services.AddLogging();
// Database Connection
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Enable CORS (for Angular frontend later)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

var app = builder.Build();

// Configure Middleware
app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();

app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI();

app.Run();
