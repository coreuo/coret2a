using Core.Abstract.Attributes;

namespace Packets.Attributes.Outgoing
{
    [Priority(0.1)]
    [Also("InternalSendToShard", null!, default)]
    public class InternalShardPacketAttribute : AlsoAttribute
    {
        public InternalShardPacketAttribute(int value) : base("InternalSend", "id", value)
        {
        }
    }
}
