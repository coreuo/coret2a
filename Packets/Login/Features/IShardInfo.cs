using System.Net;
using Core.Abstract.Attributes;
using Core.Abstract.Extensions;

namespace Packets.Login.Features;

public interface IShardInfo
{
    int Id { get; }

    [Size(30)]
    Span<char> Name { get; }

    [Size(15)]
    Span<char> IpAddress { get; }

    int Port { get; }

    internal void WriteShardDescription<TData>(TData data)
        where TData : IData
    {
        data.WriteShort((short)Id);

        data.WriteAscii(Name, 32);

        data.WriteByte(0x12);

        data.WriteByte(0x1);

        data.WriteIpAddress(IPAddress.Parse(IpAddress.AsText()));
    }

    internal void WriteShardConnection<TData>(TData data)
        where TData : IData
    {
        data.WriteIpAddress(IPAddress.Parse(IpAddress.AsText()));

        data.WriteShort((short)Port);
    }
}