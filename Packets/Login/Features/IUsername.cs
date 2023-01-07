using Core.Abstract.Attributes;

namespace Packets.Login.Features;

public interface IUsername
{
    [Size(30)] Span<char> Username { get; }

    internal void ReadUsername<TData>(TData data)
        where TData : IData
    {
        data.ReadAscii(Username, 30);
    }

    internal void WriteUsername<TData>(TData data)
        where TData : IData
    {
        data.WriteAscii(Username, 30);
    }
}