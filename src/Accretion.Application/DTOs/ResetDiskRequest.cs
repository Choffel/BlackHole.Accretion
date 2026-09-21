namespace Accretion.Application.DTOs;

public record ResetDiskRequest(int Count = 200,  int? Seed = null);