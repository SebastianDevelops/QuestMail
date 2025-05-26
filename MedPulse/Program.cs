using MedPulse.DbContext;
using MedPulse.Infrastructure;
using MedPulse.Plugins;
using MedPulse.Repositories;
using MedPulse.Repositories.Interfaces;
using MedPulse.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();
// Add services to the container.

builder.Services.Configure<AzureOpenAI>(
    builder.Configuration.GetSection("Settings"));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<QuestMailContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<UserMessageRepository>();
builder.Services.AddScoped<TrophyRepository>();
builder.Services.AddScoped<CompanionRepository>();
builder.Services.AddScoped<QuestRepository>();
builder.Services.AddScoped<CompanionPlugin>();
builder.Services.AddScoped<TrophyPlugin>();
builder.Services.AddScoped<UserPlugin>();
builder.Services.AddTransient<ISemanticKernelService, SemanticKernelService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICompanionService, CompanionService>();
builder.Services.AddScoped<IPostmarkService, PostmarkService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();