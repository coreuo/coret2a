using Core.Abstract.Attributes;
using Packets.Login.Features;
using Packets.Shared;
using Packets.Shared.Features;

namespace Packets.Login.Domain;

[Entity("Login", "State")]
public interface IState<TData, TAccount, TShard> : IState<TData>,
    IName,
    IPassword,
    ILoginKey,
    IShardSelect<TShard>,
    IHardwareInfo,
    IStatus
    where TData : IData
    where TAccount : IAccount
    where TShard : IShard<TData>
{
    int Id { get; }

    TAccount Account { get; set; }
}