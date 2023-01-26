using Core.Abstract.Attributes;

namespace Scripts.LoginShardList;

[Entity("Login", "Server")]
public interface ILogin<TState, TShard, TShardCollection>
    where TState : IState
    where TShard : IShard
    where TShardCollection : ICollection<TShard>
{
    TShardCollection Shards { get; }

    void PacketBritanniaList(TState state, TShardCollection shards);

    [Priority(2.0)]
    [Case("PacketAccountLoginRequest", nameof(state), nameof(state.AuthenticationSuccess), true)]
    public void OnAccountLoginSuccess(TState state)
    {
        var shards = Shards;

        PacketBritanniaList(state, shards);
    }
}