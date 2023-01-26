using Core.Abstract.Attributes;

namespace NetworkPackets.Attributes.Outgoing
{
    [Priority(0.1)]
    [Also("InternalSendToLogin", null!, default)]
    public class InternalLoginPacketAttribute : AlsoAttribute
    {
        public InternalLoginPacketAttribute(int value) : base("InternalSend", "id", value)
        {
        }
    }
}
