using System.Numerics;

namespace Accretion.Domain.Models;

public class BackHole
{
    public const double SpeedOfLight = 299_792_458.0;
    
    public const double GravitationalConstant = 6.67430e-11;
    
    public double Mass { get; private set; }
    
    public Vector3 Position { get; private set; }
    
    public double SchwarzschildRadius { get; private set; }
    
    public double PhotonSphereRadius => 1.5 * SchwarzschildRadius;
    
    public double IscoRadius => 3.0 * SchwarzschildRadius;
    
    public BackHole(double mass, Vector3 position)
    {
        Mass = mass;
        Position = position;
    }

    public void  UpdateMass(double NewMass)
    {
        Mass = NewMass;
        
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