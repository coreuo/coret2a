using Core.Abstract.Attributes;

namespace Scripts.ShardPing
{
    [Entity("Shard", "Server")]
    public interface IShard<TState>
        where TState : IState
    {
        void PacketPingResponse(TState state);

        [Priority(1.0)]
        public void OnPacketPingRequest(TState state)
        {
            PacketPingResponse(state);
        }
    }
}
