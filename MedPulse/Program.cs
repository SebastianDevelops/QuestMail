using MedPulse.DbContext;
using MedPulse.Infrastructure;
using MedPulse.Plugins;
using MedPulse.Repositories;
using MedPulse.Repositories.Interfaces;
using MedPulse.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("./etc/secrets/appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile(Path.Combine((AppContext.BaseDirectory.StartsWith("/") ? AppContext.BaseDirectory.Substring(1) : AppContext.BaseDirectory), "appsettings.json"), optional: true, reloadOnChange: true)
    .AddJsonFile($"./etc/secrets/appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();
// Add services to the container.
builder.Services.Configure<Settings>(builder.Configuration.GetSection("Settings"));
builder.Services.AddSingleton(sp =>
    sp.GetRequiredService<IOptions<Settings>>().Value);

builder.Services.Configure<AzureOpenAI>(
    builder.Configuration.GetSection("Settings"));

if (builder.Environment.IsDevelopment())
{
    Environment.SetEnvironmentVariable("Settings.AzureOpenAI.apikey", builder.Configuration["Settings:AzureOpenAI:apikey"]);
    Environment.SetEnvironmentVariable("Settings.AzureOpenAI.model", builder.Configuration["Settings:AzureOpenAI:model"]);
    Environment.SetEnvironmentVariable("Settings.AzureOpenAI.endpoint", builder.Configuration["Settings:AzureOpenAI:endpoint"]);
    
    Environment.SetEnvironmentVariable("Settings.GoogleGemini.apikey", builder.Configuration["Settings:GoogleGemini:apikey"]);
    Environment.SetEnvironmentVariable("Settings.GoogleGemini.model", builder.Configuration["Settings:GoogleGemini:model"]);
    
    Environment.SetEnvironmentVariable("Settings.Pinata.JWT", builder.Configuration["Settings:Pinata:JWT"]);
    Environment.SetEnvironmentVariable("Settings.Pinata.BaseUrl", builder.Configuration["Settings:Pinata:BaseUrl"]);
    
    Environment.SetEnvironmentVariable("Settings.Postmark.apikey", builder.Configuration["Settings:Postmark:apikey"]);
    Environment.SetEnvironmentVariable("Settings.Postmark.fromEmail", builder.Configuration["Settings:Postmark:fromEmail"]);
    Environment.SetEnvironmentVariable("Settings.Postmark.replyTo", builder.Configuration["Settings:Postmark:replyTo"]);
    
    Environment.SetEnvironmentVariable("DefaultConnection", builder.Configuration.GetConnectionString("DefaultConnection"));
}


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<QuestMailContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString(Environment.GetEnvironmentVariable("DefaultConnection"))));

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