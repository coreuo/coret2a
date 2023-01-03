using Packets.Shard.Shared;

namespace Packets.Shard.Outgoing
{
    public interface IMove : ISerial
    {
        internal void WriteMove<TData>(TData data)
            where TData : IData
        {
            data.WriteInt(Id); //Serial

            data.WriteUShort(0x190); //Body

            data.WriteByte(0x0); //Hidden

            data.WriteUShort(0x83EA); //Hue

            data.WriteByte(0x0); //Status

            data.WriteShort(0x660); //X

            data.WriteShort(0x68A); //Y

            data.WriteShort(0x71); //Area server ID

            data.WriteByte(0x4); //Direction

            data.WriteByte(0xF); //Z
        }
    }
}
