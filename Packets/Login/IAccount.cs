using Core.Abstract.Attributes;
using Packets.Login.Outgoing;

namespace Packets.Login;

[Entity("Account")]
public interface IAccount :
    IAccessKey
{
}