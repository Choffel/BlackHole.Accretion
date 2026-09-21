using Accretion.Application.Interfaces;
using Accretion.Application.PipLine;
using Accretion.Application.Service;
using Accretion.Application.Worker;
using Accretion.Infrastructure;
using Accretion.Infrastructure.Hub;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddSingleton<SimulationStateService>();
builder.Services.AddSingleton<IPhysicsEngine, PhysicsEngine>();
builder.Services.AddSingleton<SimulationChannel>();

builder.Services.AddHostedService<PhysicsWorker>();

builder.Services.AddControllers();
builder.Services.AddSignalR();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Accretion API v1");
        options.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapControllers();
app.MapHub<SimulationHub>("/hubs/simulation");

app.Run();