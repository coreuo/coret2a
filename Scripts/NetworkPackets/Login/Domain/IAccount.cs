using Core.Abstract.Attributes;
using NetworkPackets.Login.Features;
using NetworkPackets.Shared.Features;

namespace NetworkPackets.Login.Domain;

[Entity("Login", "Account")]
public interface IAccount :
    IName,
    IPassword,
    IAccessKey
{
}