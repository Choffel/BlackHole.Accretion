using System.Runtime.CompilerServices;
using Accretion.Application.PipLine;
using Accretion.Domain.Models;


namespace Accretion.Infrastructure.Hub;

public class SimulationHub : Microsoft.AspNetCore.SignalR.Hub
{
    private readonly SimulationChannel _simulationChannel;

    public SimulationHub(SimulationChannel simulationChannel)
    {
        _simulationChannel = simulationChannel;
    }

    public async IAsyncEnumerable<SimulationFrame> StreamSimulationFrames(
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var reader = _simulationChannel.Reader;

        while (await reader.WaitToReadAsync(cancellationToken))
        {
            while (reader.TryRead(out var frame))
            {
                yield return frame;
            }
        }
    }
}