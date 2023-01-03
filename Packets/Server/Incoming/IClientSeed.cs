namespace Packets.Server.Incoming;

public interface IClientSeed :
    ISeed
{
    internal void ReadClientSeed<TData>(TData data)
        where TData : IData
    {
        Seed = data.ReadUInt();
    }
}