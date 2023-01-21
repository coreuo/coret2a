using Core.Abstract.Attributes;
using Packets.Shared;

namespace Packets.Shard.Features;

public interface IGender
{
    [Flag("Status", 1)]
    bool Female { get; }

    internal void WriteGender<TData>(TData data)
        where TData : IData
    {
        data.WriteByte(Female ? (byte)1 : (byte)0);
    }
}