using Core.Abstract.Attributes;

namespace NetworkPackets.Login.Features
{
    public interface IEndPoint
    {
        [Length(15)]
        Span<char> IpAddress { get; }

        ushort Port { get; }
    }
}
