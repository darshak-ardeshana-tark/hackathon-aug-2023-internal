using System.Text.Json.Serialization;
using TaskExecutor.Repository;
using TaskExecutor.Repository.Implementation;
using TaskExecutor.Services;
using TaskExecutor.Services.Implementation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSingleton<INodeRepository, NodeRepository>();
builder.Services.AddSingleton<ITaskRepository, TaskRepository>();
builder.Services.AddSingleton<INodeService, NodeService>();
builder.Services.AddSingleton<ITaskService, TaskService>();
builder.Services.AddSingleton<ITaskOrchestrator, TaskOrchestrator>();
builder.Services.AddSingleton<IHealthChecker, HealthChecker>();

builder.Services.AddControllers().AddJsonOptions(o => o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options => options.AddDefaultPolicy(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

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

var provider = builder.Services.BuildServiceProvider();
var healthChecker = provider.GetRequiredService<IHealthChecker>();
healthChecker.CheckWorkersHealth();

app.Run();