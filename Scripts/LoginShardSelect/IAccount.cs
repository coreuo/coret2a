using Core.Abstract.Attributes;

namespace Scripts.LoginShardSelect;

[Entity("Login", "Account")]
public interface IAccount
{
    int AccessKey { get; set; }
}