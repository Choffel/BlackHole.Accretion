using System.Numerics;
using Accretion.Domain.Models;

namespace Accretion.Application.Extensions;

public static class GenerateInitialDisk
{
    public static Particle[] GenerateAccretionDisk(this BlackHole blackHole, int count, int seed = 42)
    {
        var particles = new Particle[count];
        var random = new Random(seed);

        double isco = blackHole.IscoRadius;

        for (int i = 0; i < count; i++)
        {
            float radius = (float)(isco * (2.0 + random.NextDouble() * 8.0));
            float angle = (float)(random.NextDouble() * Math.PI * 2.0);
            
            var position = new Vector3(
                radius * MathF.Cos(angle),
                radius * MathF.Sin(angle),
                (float)(random.NextDouble() - 0.5) * (radius * 0.05f)
            );
            
            float orbitalSpeed = MathF.Sqrt((float)((BlackHole.GravitationalConstant * blackHole.Mass) / radius));
            
            var velocity = new Vector3(
                -orbitalSpeed * MathF.Sin(angle),
                orbitalSpeed * MathF.Cos(angle),
                0f
            );

            particles[i] = new Particle(Guid.NewGuid(), position, velocity);
        }

        return particles;
    }
}
