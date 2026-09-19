using Accretion.Domain.Models;

namespace Accretion.Application.Interfaces;

public interface IPhysicsEngine
{
    Particle[] ProcessStep(Particle[] particles, BlackHole blackHole, double deltaTime);
}