using System.Threading.Channels;
using Accretion.Domain.Models;

namespace Accretion.Application.PipLine;

public class SimulationChannel
{
    private readonly Channel<SimulationFrame> frames;

    public SimulationChannel()
    {
        var options = new BoundedChannelOptions(10)
        {
            FullMode = BoundedChannelFullMode.DropOldest,
            SingleReader = true,
            SingleWriter = false
        };
        
        frames = Channel.CreateBounded<SimulationFrame>(options);
    }
    
    public ChannelWriter<SimulationFrame> Writer => frames.Writer;
    public ChannelReader<SimulationFrame> Reader => frames.Reader;
    
}