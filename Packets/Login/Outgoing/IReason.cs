namespace Packets.Login.Outgoing;

public interface IReason
{
    byte Reason { get; }

    internal void WriteReason<TData>(TData data)
        where TData : IData
    {
        data.WriteByte(Reason);
    }
}