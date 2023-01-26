using Core.Abstract.Attributes;

namespace NetworkPackets.Attributes.Incoming
{
    [Priority(0.1)]
    public class InternalPacketAttribute : CaseAttribute
    {
        public InternalPacketAttribute(int value) : base("InternalPacketReceived", null, "id", value)
        {
        }
    }
}
