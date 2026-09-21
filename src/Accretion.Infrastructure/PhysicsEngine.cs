using System.Numerics;
using Accretion.Application.Interfaces;
using Accretion.Domain.Models;

namespace Accretion.Infrastructure;

public class PhysicsEngine : IPhysicsEngine
{
    public Particle[] ProcessStep(Particle[] particles, BlackHole blackHole, double deltaTime)
    {
        var updatedParticles = new Particle[particles.Length];

        for (int i = 0; i < particles.Length; i++)
        {
            ref readonly var current = ref particles[i];
            
            if(current.IsConsumed)
            {
                updatedParticles[i] = current;
                continue;
            }
            
            Vector3 directionToCenter = blackHole.Position - current.Position;
            
            float distance = directionToCenter.Length();
            
            if(distance <= blackHole.SchwarzschildRadius)
            {
                var consumedParticle = current;
                consumedParticle.IsConsumed = true;
                
                consumedParticle.Velocity = Vector3.Zero;
                
                updatedParticles[i] = consumedParticle;
                continue;
            }
            
            double timeDilation = blackHole.GetTimeDilationFactor(distance);
            
            float localDeltaTime = (float)(deltaTime * timeDilation);
            
            
            Vector3 normalizedDirection = Vector3.Normalize(directionToCenter);
            
            
            float distanceSqr = MathF.Max(distance * distance, 1.0f);
            
            float accelerationMagnitude = (float)((BlackHole.GravitationalConstant * blackHole.Mass) / distanceSqr);
            Vector3 acceleration = normalizedDirection * accelerationMagnitude;
            
            Vector3 newVelocity = current.Velocity + acceleration * localDeltaTime;
            Vector3 newPosition = current.Position + newVelocity * localDeltaTime;
            
            float newTemperature = (float)(1000.0 + (blackHole.IscoRadius / MathF.Max(distance, 1.0f)) * 5000.0);

            var updatedParticle = current;
            updatedParticle.Velocity = newVelocity;
            updatedParticle.Position = newPosition;
            updatedParticle.Temperature = newTemperature;

            updatedParticles[i] = updatedParticle;
        }

        return updatedParticles;
        }
    }