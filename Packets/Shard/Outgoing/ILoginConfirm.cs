using Packets.Shard.Shared;

namespace Packets.Shard.Outgoing
{
    public interface ILoginConfirm : ISerial
    {
        internal void WriteLoginConfirm<TData>(TData data)
            where TData : IData
        {
            data.WriteInt(Id); //Serial

            data.WriteInt(0); //0

            data.WriteShort(0x190); //Body

            data.WriteShort(0x660); //X

            data.WriteShort(0x68A); //Y

            data.WriteShort(0xF); //Z

            data.WriteByte(0x4); //direction

            data.WriteByte(0x71); //area server ID

            data.WriteInt(0x007f0000); //random

            data.WriteShort(0x0); //area left

            data.WriteShort(0x0); //area top

            data.WriteShort(0x1800); //area width

            data.WriteShort(0x1000); //area height

            data.WriteInt(0); //0

            data.WriteShort(0); //0
        }
    }
}
