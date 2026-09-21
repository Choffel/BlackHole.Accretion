using System.Numerics;
using Accretion.Application.Extensions;
using Accretion.Application.Interfaces;
using Accretion.Application.PipLine;
using Accretion.Application.Service;
using Accretion.Domain.Models;
using Microsoft.Extensions.Hosting;

namespace Accretion.Application.Worker;

public class PhysicsWorker : BackgroundService, IHostedService
{
    private readonly IHostedService _host;
    
    private readonly IPhysicsEngine _physicsEngine;
    private readonly SimulationChannel _simulationChannel;
    
    private readonly SimulationStateService _simulationStateService;

    private readonly BlackHole _blackHole;
    private Particle[] _particles;

    public PhysicsWorker(IPhysicsEngine physicsEngine,
        SimulationChannel simulationChannel, SimulationStateService simulationStateService)
    {
        _simulationStateService = simulationStateService;
        _physicsEngine = physicsEngine;
        _simulationChannel = simulationChannel;
        _blackHole = new BlackHole(1.989e31,Vector3.Zero);

        _particles = _blackHole.GenerateAccretionDisk(count: 200);
;        
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        long frameNumber = 0;
        double simulationTime = 0.0;
        const double dt = 0.016;

        while (!stoppingToken.IsCancellationRequested)
        {
            if (_simulationStateService.IsPaused)
            {
                await Task.Delay(100, stoppingToken);
                continue;
            }

            var blackHole = _simulationStateService.BlackHole;
            var currentParticles = _simulationStateService.Particles;

            var updatedParticles = _physicsEngine.ProcessStep(currentParticles, blackHole, dt);
        
            _simulationStateService.UpdateParticlesFromWorker(updatedParticles);

            simulationTime += dt;
            frameNumber++;

            var frame = new SimulationFrame(
                frameNumber,
                simulationTime,
                blackHole.SchwarzschildRadius,
                updatedParticles
            );

            await _simulationChannel.Writer.WriteAsync(frame, stoppingToken);

            await Task.Delay(TimeSpan.FromSeconds(dt), stoppingToken);
        }
    }
}