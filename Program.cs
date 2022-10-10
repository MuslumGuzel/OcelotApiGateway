using ApiGateway.Middlewares;
using ApiGateway.Models;
using ApiGateway.Services;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Cache.CacheManager;
using Serilog;
using Serilog.Core;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<LogDatabaseSettings>(builder.Configuration.GetSection("LogStoreDatabase"));

builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true )
    .AddEnvironmentVariables();

builder.WebHost.UseUrls("http://localhost:7000");
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddOcelot(builder.Configuration)
    .AddCacheManager(x => {
        x.WithDictionaryHandle();
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ILogsService, LogsService>();

Logger log = new LoggerConfiguration().WriteTo.File($@"Logs/Log_{DateTime.Now.ToString("yyyyMMddHHmmssffff")}.txt", rollingInterval: RollingInterval.Day).WriteTo.Console().CreateLogger();
builder.Host.UseSerilog(log);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<RequestResponseLoggingMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.UseOcelot().Wait();
app.Run();
