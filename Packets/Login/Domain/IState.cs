using Core.Abstract.Attributes;
using Packets.Login.Features;
using Packets.Shared.Features;

namespace Packets.Login.Domain;

[Entity("Login", "State")]
public interface IState<TData, TAccount, TShard> : Shared.IState<TData>,
    IStatus,
    IShardSelect<TShard>
    where TData : Shared.IData
    where TAccount : IAccount
    where TShard : IShard<TData>
{
    TAccount Account { get; set; }
}

[Entity("Login", "State")]
public interface IState<TData, TShard> : Shared.IState<TData>,
    IName,
    IPassword,
    ILoginKey,
    IShardSelect<TShard>,
    IHardwareInfo
    where TData : Shared.IData
    where TShard : IShard<TData>
{
    int Id { get; }
}