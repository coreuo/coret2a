using Core.Abstract.Attributes;
using Packets.Login.Features;

namespace Packets.Login;

[Entity("Login", "Account")]
public interface IAccount :
    IUsername,
    IPassword,
    IAccessKey
{
}