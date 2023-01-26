using Core.Abstract.Attributes;

namespace NetworkPackets.Attributes.Outgoing;

[Priority(0.1)]
public class PacketAttribute : AlsoAttribute
{
    public PacketAttribute(int value) : base("PacketSend", "id", value)
    {
    }
}