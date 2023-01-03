using Core.Abstract.Attributes;

namespace Server.Login;

[Entity("Login", "State")]
public interface IState<out TAccount>
    where TAccount : IAccount
{
    TAccount Account { get; }
}