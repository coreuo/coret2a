using Core.Abstract.Attributes;

namespace Packets.Server.Features;

public interface IName
{
    [Size(30)] 
    Span<char> Name { get; }

    internal void ReadName<TData>(TData data)
        where TData : IData
    {
        data.ReadAscii(Name, 30);
    }

    internal void WriteName<TData>(TData data)
        where TData : IData
    {
        data.WriteAscii(Name, 30);
    }
}