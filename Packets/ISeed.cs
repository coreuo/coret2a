namespace Packets;

public interface ISeed
{
    public uint Seed { get; set; }

    internal void ReadSeed<TData>(TData data)
        where TData : IData
    {
        Seed = data.ReadUInt();
    }
}