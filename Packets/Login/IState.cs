using Core.Abstract.Attributes;
using Packets.Login.Incoming;
using Packets.Login.Outgoing;
using Packets.Server.Incoming;

namespace Packets.Login;

[Entity("Login", "State")]
public interface IState<TData, TAccount, TShard> : Server.IState<TData>,
    IClientSeed,
    ILoginRequest,
    IHardwareInfo,
    ISelectedShard<TShard>,
    ILoginFailedReason
    where TData : IData
    where TAccount : IAccount
    where TShard : IShard
{
    TAccount Account { get; set; }
}