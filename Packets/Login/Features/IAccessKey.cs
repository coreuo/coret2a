using Packets.Shared;

namespace Packets.Login.Features;

public interface IAccessKey
{
    int AccessKey { get; }

    internal void WriteAccessKey<TData>(TData data)
        where TData : IData
    {
        data.WriteInt(AccessKey);
    }
}