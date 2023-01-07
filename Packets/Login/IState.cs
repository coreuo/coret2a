using Core.Abstract.Attributes;
using Packets.Login.Features;

namespace Packets.Login;

[Entity("Login", "State")]
public interface IState<in TData, TAccount, TShard> : Server.IState<TData>,
    IUsername,
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