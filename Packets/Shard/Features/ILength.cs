using Packets.Shared;

namespace Packets.Shard.Features
{
    public interface ILength
    {
        ushort Length { get; set; }

        internal ushort ReadLength<TData>(TData data)
            where TData : IData
        {
            return Length = data.ReadUShort();
        }

        internal void WriteLength<TData>(TData data)
            where TData : IData
        {
            data.WriteUShort(Length);
        }
    }
}
