using Packets.Shared;

namespace Packets.Login.Features;

public interface IStatus
{
    byte Status { get; set; }

    internal void ReadStatus<TData>(TData data)
        where TData : IData
    {
        Status = data.ReadByte();
    }

    internal void WriteStatus<TData>(TData data)
        where TData : IData
    {
        data.WriteByte(Status);
    }
}