using Core.Abstract.Attributes;
using Packets.Login.Features;
using Packets.Shared.Features;

namespace Packets.Login.Domain;

[Entity("Login", "Account")]
public interface IAccount :
    IName,
    IPassword,
    IAccessKey
{
}