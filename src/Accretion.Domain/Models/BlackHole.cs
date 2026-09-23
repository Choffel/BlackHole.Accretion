using System.Numerics;

namespace Accretion.Domain.Models;

public class BlackHole
{
    public const double SpeedOfLight = 299_792_458.0;
    
    public const double GravitationalConstant = 6.67430e-11;
    
    public double Mass { get; private set; }
    
    public Vector3 Position { get; private set; }
    
    public double SchwarzschildRadius { get; private set; }
    
    public double IscoRadius => 3.0 * SchwarzschildRadius;
    
    public BlackHole(double mass, Vector3 position)
    {
        Mass = mass;
        Position = position;
    }

    public void  UpdateMass(double newMass)
    {
        if (double.IsNaN(newMass) || double.IsInfinity(newMass))
        {
            throw new ArgumentException("Mass must be a valid finite number.", nameof(newMass));
        }

        if (newMass <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(newMass), "Mass must be strictly positive.");
        }
        Mass = newMass;
        
        SchwarzschildRadius = (2.0 * GravitationalConstant * Mass) / (SpeedOfLight * SpeedOfLight);
    }
    
    public double GetTimeDilationFactor(double distanceToCenter)
    {
        if (distanceToCenter <= SchwarzschildRadius)
        {
            return 0.0; 
        }

        return Math.Sqrt(1.0 - (SchwarzschildRadius / distanceToCenter));
    }
}