using System.Numerics;
using Accretion.Application.Extensions;
using Accretion.Application.Interfaces;
using Accretion.Application.PipLine;
using Accretion.Domain.Models;
using Microsoft.Extensions.Hosting;

namespace Accretion.Application.Worker;

public class PhysicsWorker : BackgroundService
{
    private readonly IPhysicsEngine _physicsEngine;
    private readonly SimulationFrame _simulationFrame;
    private readonly SimulationChannel _simulationChannel;

    private readonly BlackHole _blackHole;
    private Particle[] _particles;

    public PhysicsWorker(IPhysicsEngine physicsEngine, SimulationFrame simulationFrame, SimulationChannel simulationChannel)
    {
        _physicsEngine = physicsEngine;
        _simulationFrame = simulationFrame;
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
            _particles = _physicsEngine.ProcessStep(_particles, _blackHole, deltaTime: 0.1);
            
            var frame = new SimulationFrame(
                frameNumber,
                simulationTime,
                _blackHole.SchwarzschildRadius,
                _particles
                );
            
            await _simulationChannel.Writer.WriteAsync(frame, stoppingToken);
            
            await Task.Delay(TimeSpan.FromSeconds(dt), stoppingToken);
        }
    }
}