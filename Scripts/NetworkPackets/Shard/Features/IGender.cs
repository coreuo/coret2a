using Core.Abstract.Attributes;
using NetworkPackets.Shared;

namespace NetworkPackets.Shard.Features;

public interface IGender
{
    [Flag("Status", 1)]
    bool Female { get; set; }

    internal void WriteGender<TData>(TData data)
        where TData : IData
    {
        data.WriteByte(Female ? (byte)1 : (byte)0);
    }

    internal void ReadGender<TData>(TData data)
        where TData : IData
    {
        Female = data.ReadByte() > 1;
    }
}