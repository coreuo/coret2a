using Core.Abstract.Attributes;

namespace Packets.Shared.Features;

public interface IName
{
    [Length(30)] 
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

    internal void WriteNameTerminated<TData>(TData data)
        where TData : IData
    {
        data.WriteAsciiTerminated(Name);
    }
}