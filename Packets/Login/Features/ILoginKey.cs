using Packets.Shared;

namespace Packets.Login.Features;

public interface ILoginKey
{
    byte LoginKey { set; }

    internal void ReadLoginKey<TData>(TData data)
        where TData : IData
    {
        LoginKey = data.ReadByte();
    }
}