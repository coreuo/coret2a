using Core.Abstract.Attributes;

namespace Packets.Login.Incoming;

public interface ILoginRequest
{
    [Size(30)]
    Span<char> Name { get; }

    [Size(30)]
    Span<char> Password { get; }

    byte NextLoginKey { set; }

    internal void ReadLoginRequest<TData>(TData data)
        where TData : IData
    {
        data.ReadAscii(Name, 30);

        data.ReadAscii(Password, 30);

        NextLoginKey = data.ReadByte();
    }
}