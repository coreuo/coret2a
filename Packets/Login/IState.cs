using Core.Abstract.Attributes;
using Packets.Login.Incoming;
using Packets.Login.Outgoing;

namespace Packets.Login;

[Entity("Login", "State")]
public interface IState<in TData, TAccount, TShard> : Server.IState<TData>,
    ILoginRequest,
    IHardwareInfo,
    ISelectedShard<TShard>,
    IReason
    where TData : IData
    where TAccount : IAccount
    where TShard : IShard
{
    TAccount Account { get; set; }
}