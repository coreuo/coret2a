namespace Packets.Shard.Features;

public interface IIntelligence
{
    ushort Intelligence { get; set; }

    internal void ReadIntelligence<TData>(TData data)
        where TData : IData
    {
        Intelligence = data.ReadUShort();
    }

    internal void WriteIntelligence<TData>(TData data)
        where TData : IData
    {
        data.WriteUShort(Intelligence);
    }
}