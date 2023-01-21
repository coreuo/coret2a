using Packets.Shared;

namespace Packets.Login.Features;

public interface IHardwareInfo
{
    internal void ReadHardwareInfo<TData>(TData data)
        where TData : IData
    {
        data.Offset = 149;
    }
}