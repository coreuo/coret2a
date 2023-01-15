using Core.Abstract.Attributes;

namespace Packets.Login.Features
{
    public interface IEndPoint
    {
        [Size(15)]
        Span<char> IpAddress { get; }

        int Port { get; }
    }
}
