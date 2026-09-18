namespace Accretion.Domain.Models;

public readonly record struct SimulationFrame
{
    public long FrameNumber { get; init; }
    
    public double SimulationTime { get; init; }
    
    public double SchwarzschildRadius { get; init; }
    
    public Particle[] Particles { get; init; }
    
    public SimulationFrame(long frameNumber, double simulationTime, double schwarzschildRadius, Particle[] particles)
    {
        FrameNumber = frameNumber;
        SimulationTime = simulationTime;
        SchwarzschildRadius = schwarzschildRadius;
        Particles = particles;
    }
}