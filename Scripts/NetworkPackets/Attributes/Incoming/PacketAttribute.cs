using Core.Abstract.Attributes;

namespace NetworkPackets.Attributes.Incoming
{
    [Priority(0.1)]
    public class PacketAttribute : CaseAttribute
    {
        public PacketAttribute(int value) : base("PacketReceived", null, "id", value)
        {
        }
    }
}
