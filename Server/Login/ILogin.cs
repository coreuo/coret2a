using Core.Abstract.Attributes;
using Core.Abstract.Extensions;

namespace Server.Login;

[Entity("Login", "Server")]
public interface ILogin<TState, TAccount>
    where TState : IState<TAccount>
    where TAccount : IAccount
{
    TAccount Account { get; }

    void Listen();

    void Slice();
}