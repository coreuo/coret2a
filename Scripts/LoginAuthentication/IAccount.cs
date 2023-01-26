using Core.Abstract.Attributes;

namespace Scripts.LoginAuthentication;

[Entity("Login", "Account")]
public interface IAccount
{
    Span<char> Name { get; }

    Span<char> Password { get; }
}