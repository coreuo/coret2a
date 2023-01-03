using Packets.Login.Shared;

namespace Packets.Login.Outgoing;

public interface ILoginFailedReason :
    IReason
{
    internal void WriteLoginFailedReason<TData>(TData data)
        where TData : IData
    {
        data.WriteByte(Reason);
    }
}