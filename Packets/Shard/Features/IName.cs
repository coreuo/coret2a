using Core.Abstract.Attributes;

namespace Packets.Shard.Features;

public interface IName
{
    [Size(60)] Span<char> Name { get; }

    internal void WriteName<TData>(TData data)
        where TData : IData
    {
        data.WriteAscii(Name, 60);
    }
}