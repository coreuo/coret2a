using Core.Abstract.Attributes;

namespace Scripts.LoginShardList;

[Entity("Login", "State")]
public interface IState
{
    bool AuthenticationSuccess { get; }
}