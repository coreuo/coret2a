using Core.Abstract.Attributes;

namespace Scripts.ShardAuthorization;

[Entity("Shard", "Account")]
public interface IAccount
{
    Span<char> Name { get; }

    Span<char> Password { get; }

    int AccessKey { get; set; }
}