using System.Net;
using Core.Abstract.Extensions;
using NetworkPackets.Shared;
using NetworkPackets.Shared.Features;

namespace NetworkPackets.Login.Features;

public interface IShardInfo :
    IName,
    IEndPoint
{
    int Id { get; }

    internal void WriteShardDescription<TData>(TData data)
        where TData : IData
    {
        data.WriteUShort((ushort)Id);

        data.WriteAscii(Name, 32);

        data.WriteByte(0x12);

        data.WriteByte(0x1);

        data.WriteIpAddress(IPAddress.Parse(IpAddress.AsText()));
    }

    internal void WriteShardConnection<TData>(TData data)
        where TData : IData
    {
        data.WriteIpAddress(IPAddress.Parse(IpAddress.AsText()));

        data.WriteUShort(Port);
    }
}