using Packets.Shard.Shared;

namespace Packets.Shard.Outgoing
{
    public interface IEquippedMobile : ISerial
    {
        internal void WriteEquippedMobile<TData>(TData data)
            where TData : IData
        {
            WriteSerial(data);

            data.WriteShort(0x190); //Body

            data.WriteShort(0x660); //X

            data.WriteShort(0x68A); //Y

            data.WriteByte(0xF); //Z

            data.WriteByte(0x4); //direction

            data.WriteUShort(0x83EA); //Hue

            data.WriteByte(0x0); //Status

            data.WriteByte(0x1); //Notoriety

            data.WriteInt(0);
        }
    }
}
