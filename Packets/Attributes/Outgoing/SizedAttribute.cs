using Core.Abstract.Attributes;

namespace Packets.Attributes.Outgoing;

public class SizedAttribute : AlsoAttribute
{
    public SizedAttribute() : base("SendSize", null!, default)
    {
    }
}