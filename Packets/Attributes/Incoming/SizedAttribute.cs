using Core.Abstract.Attributes;

namespace Packets.Attributes.Incoming;

public class SizedAttribute : AlsoAttribute
{
    public SizedAttribute() : base("ReceiveSize", null!, default)
    {
    }
}