using Core.Abstract.Attributes;
using Packets.Login.Features;
using Packets.Server.Features;

namespace Packets.Login;

[Entity("Login", "Account")]
public interface IAccount :
    IName,
    IPassword,
    IAccessKey
{
}