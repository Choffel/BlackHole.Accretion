using System.Numerics;
using Accretion.Application.DTOs;
using Accretion.Application.Service;
using Microsoft.AspNetCore.Mvc;

namespace Accretion.Api.Controller;

[ApiController]
[Route("api/[controller]")]
public class SimulationController : ControllerBase
{
    private readonly SimulationStateService _simulationStateService;

    public SimulationController(SimulationStateService simulationStateService)
    {
        _simulationStateService = simulationStateService;
    }
    
    [HttpGet("state")]
    public IActionResult GetState()
    {
        return Ok(new
        {
            IsPaused = _simulationStateService.IsPaused,
            BlackHoleMass = _simulationStateService.BlackHole.Mass,
            SchwarzschildRadius = _simulationStateService.BlackHole.SchwarzschildRadius,
            ParticleCount = _simulationStateService.Particles.Length
        });
    }
    
    [HttpPost("pause")]
    public IActionResult TogglePause([FromQuery] bool? state)
    {
        var isPaused = _simulationStateService.TogglePause(state);
        return Ok(new { IsPaused = isPaused });
    }

    [HttpPut("mass")]
    public IActionResult UpdateMass([FromBody] UpdateMassRequest request)
    {
        if (request.MassInKg <= 0)
        {
            return BadRequest("Масса должна быть больше 0.");
        }

        _simulationStateService.UpdateBlackHoleMass(request.MassInKg);

        return Ok(new
        {
            NewMass = _simulationStateService.BlackHole.Mass,
            NewRsRadius = _simulationStateService.BlackHole.SchwarzschildRadius
        });
    }

    [HttpPost("reset")]
    public IActionResult ResetDisk([FromBody] ResetDiskRequest request)
    {
        if (request.Count <= 0)
        {
            return BadRequest("Количество частиц должно быть больше 0.");
        }

        _simulationStateService.ResetDisk(request.Count, request.Seed);
        return Ok(new { Message = $"Диск успешно пересоздан с {request.Count} частицами." });
    }

    [HttpPost("inject")]
    public IActionResult InjectParticle([FromBody] InjectParticleRequest request)
    {
        var position = new Vector3(request.PosX, request.PosY, request.PosZ);
        var velocity = new Vector3(request.VelX, request.VelY, request.VelZ);

        _simulationStateService.InjectParticle(position, velocity, request.Temperature);
        return Ok(new { Message = "Частица успешно добавлена в систему." });
    }
}