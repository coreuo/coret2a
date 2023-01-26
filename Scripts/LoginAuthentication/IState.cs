using Core.Abstract.Attributes;
using Core.Abstract.Extensions;
using Scripts.LoginAuthentication;

namespace Scripts.AccountLogin;

[Entity("Login", "State")]
public interface IState<TAccount>
    where TAccount : IAccount
{
    TAccount Account { get; set; }

    Span<char> Name { get; }

    Span<char> Password { get; }

    byte Status { get; set; }

    bool AuthenticationSuccess { set; }
}