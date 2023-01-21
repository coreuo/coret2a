using Core.Abstract.Attributes;
using Packets.Shared;

namespace Packets.Shard.Features;

public interface IExpansions
{
    byte Expansions { get; }

    [Flag("Expansions", 0)]
    bool T2A { get; }

    internal void WriteExpansions<TData>(TData data)
        where TData : IData
    {
        data.WriteByte(Expansions);
    }
}