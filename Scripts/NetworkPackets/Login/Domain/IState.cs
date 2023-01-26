using Core.Abstract.Attributes;
using NetworkPackets.Login.Features;
using NetworkPackets.Shared;
using NetworkPackets.Shared.Features;

namespace NetworkPackets.Login.Domain;

[Entity("Login", "State")]
public interface IState<TData, TAccount> : IState<TData>,
    IName,
    IPassword,
    ILoginKey,
    IShardSelect,
    IHardwareInfo,
    IStatus
    where TData : IData
    where TAccount : IAccount
{
    int Id { get; }

    TAccount Account { get; set; }
}