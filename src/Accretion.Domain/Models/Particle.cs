using System.Numerics;

namespace Accretion.Domain.Models;

public struct Particle
{
    public Guid Id { get; init; }
    
    public Vector3 Position { get; set; }
    
    public Vector3 Velocity { get; set; }
    
    public float Temperature { get; set; }
   
    public bool IsConsumed { get; set; }
    
    public Particle(Guid id, Vector3 position, Vector3 velocity, float temperature = 1000f, bool isConsumed = false)
    {
        Id = id;
        Position = position;
        Velocity = velocity;
        Temperature = temperature;
        IsConsumed = isConsumed;
    }
}