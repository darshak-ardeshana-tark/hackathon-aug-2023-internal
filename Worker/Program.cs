using System.Text.Json.Serialization;
using Worker;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(o => o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<WorkerRegistrar>();
builder.Services.AddSingleton<Executor>();
builder.Services.AddSingleton<WorkerInfo>(provider => provider.GetRequiredService<WorkerRegistrar>().GetWorkerInfo());

builder.Services.AddCors(options => options.AddDefaultPolicy(builder => builder.WithOrigins("http://localhost:5000").AllowAnyMethod().AllowAnyHeader()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

var provider = builder.Services.BuildServiceProvider();

var worker = provider.GetRequiredService<WorkerInfo>();
app.Urls.Add($"http://0.0.0.0:{worker.Port}");

var registrar = provider.GetRequiredService<WorkerRegistrar>();
await registrar.RegisterWorkerAsync();

Console.WriteLine($"Worker started, listening on port: {worker.Port}. Memes will be saved at: {worker.WorkDir}");

app.Services.GetService<IHostApplicationLifetime>().ApplicationStopping.Register(registrar.RegisterWorkerShutdownConfigurationAsync);

app.Run();
