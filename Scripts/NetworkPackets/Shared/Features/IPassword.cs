using Core.Abstract.Attributes;

namespace NetworkPackets.Shared.Features;

public interface IPassword
{
    [Length(30)] Span<char> Password { get; }

    internal void ReadPassword<TData>(TData data)
        where TData : IData
    {
        data.ReadAscii(Password, 30);
    }

    internal void WritePassword<TData>(TData data)
        where TData : IData
    {
        data.WriteAscii(Password, 30);
    }
}