using Core.Abstract.Attributes;

namespace NetworkPackets.Attributes.Outgoing;

public class SizedAttribute : AlsoAttribute
{
    public SizedAttribute() : base("SendSize", null!, default)
    {
    }
}