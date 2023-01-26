using Core.Abstract.Attributes;

namespace NetworkPackets.Attributes.Incoming;

public class SizedAttribute : AlsoAttribute
{
    public SizedAttribute() : base("ReceiveSize", null!, default)
    {
    }
}