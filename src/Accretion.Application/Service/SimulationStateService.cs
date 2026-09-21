using System.Numerics;
using Accretion.Application.Extensions;
using Accretion.Domain.Models;

namespace Accretion.Application.Service;

public class SimulationStateService
{
    private readonly object _lock = new();

    public BlackHole BlackHole { get; private set; }
    public Particle[] Particles { get; private set; }
    public bool IsPaused { get; private set; }

    public SimulationStateService()
    {
        BlackHole = new BlackHole(1.989e31, Vector3.Zero);
        
        Particles = BlackHole.GenerateAccretionDisk(count: 200);
        
        IsPaused = false;
    }

    public bool TogglePause(bool? pause = null)
    {
        lock (_lock)
        {
            IsPaused = pause ?? !IsPaused;
            return IsPaused;
        }
    }

    public void UpdateBlackHoleMass(double newMassInKg)
    {
        lock (_lock)
        {
            BlackHole.UpdateMass(newMassInKg);
        }
    }

    public void ResetDisk(int particleCount = 200, int? seed = null)
    {
        lock (_lock)
        {
            int actualSeed = seed ?? Random.Shared.Next();
            Particles = BlackHole.GenerateAccretionDisk(particleCount, actualSeed);
        }
    }

    public void InjectParticle(Vector3 position, Vector3 velocity, float temperature = 1000f)
    {
        lock (_lock)
        {
            var newParticle = new Particle(Guid.NewGuid(), position, velocity, temperature);
            
            var newArray = new Particle[Particles.Length + 1];
            Array.Copy(Particles, newArray, Particles.Length);
            newArray[^1] = newParticle;
            
            Particles = newArray;
        }
    }

    public void UpdateParticlesFromWorker(Particle[] updatedParticles)
    {
        lock (_lock)
        {
            Particles = updatedParticles;
        }
    }
}