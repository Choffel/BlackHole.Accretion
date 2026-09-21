namespace Accretion.Application.DTOs;

public record InjectParticleRequest(
    float PosX, 
    float PosY, 
    float PosZ, 
    float VelX, 
    float VelY, 
    float VelZ, 
    float Temperature = 1000f
    );