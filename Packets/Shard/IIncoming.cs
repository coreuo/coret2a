using Core.Abstract.Attributes;
using Packets.Attributes.Incoming;
using Packets.Shard.Domain;
using Packets.Shard.Features;
using Packets.Shared;
using Packets.Shared.Features;

namespace Packets.Shard
{
    [Entity("Shard", "Server")]
    public interface IIncoming<in TState, TData, TAccount, TMobile, out TMobileCollection, TMap, TSkill, TSkillArray> :
        ITransfer<TData>,
        ICityList,
        IGameTime,
        IName,
        IPassword,
        IAccessKey
        where TState : IState<TData, TAccount, TMobile, TMobileCollection, TMap, TSkill, TSkillArray>
        where TData : IData
        where TAccount : IAccount<TMobile, TMobileCollection>
        where TMobile : IMobile<TMap, TSkill, TSkillArray>
        where TMobileCollection : ICollection<TMobile>
        where TMap : IMap
        where TSkill : ISkill
        where TSkillArray : IReadOnlyList<TSkill>
    {
        [InternalPacket(0x00)]
        public void OnInternalShardAuthorization(TData data)
        {
            ReadName(data);

            ReadPassword(data);

            ReadAccessKey(data);
        }

        [Packet(0x02)]
        public void OnPacketRequestMove(TState state, TData data)
        {
            state.ReadDirection(data);

            state.ReadStatus(data);
        }

        [Packet(0x06)]
        public void OnPacketRequestObjectUse(TState state, TData data)
        {
            state.ReadTarget(data);
        }

        [Packet(0x34)]
        public void OnPacketClientQuery(TState state, TData data)
        {
            state.ReadPattern(data);

            state.ReadMode(data);

            state.ReadTarget(data);
        }

        [Packet(0x5D)]
        public void OnPacketPreLogin(TState state, TData data)
        {
            state.ReadPattern(data);

            state.ReadName(data);

            state.ReadPassword(data);

            state.ReadCharacterSlot(data);

            state.ReadSeed(data);
        }

        [Packet(0x72)]
        public void OnPacketCombatRequest(TState state, TData data)
        {
            state.ReadCombat(data);
        }

        [Packet(0x73)]
        public void OnPacketPingRequest(TState state, TData data)
        {
            state.ReadPing(data);
        }

        [Packet(0x91)]
        public void OnPacketPostLogin(TState state, TData data)
        {
            state.ReadAccessKey(data);

            state.ReadName(data);

            state.ReadPassword(data);
        }

        [Packet(0xA7)]
        public void OnPacketRequestTip(TState state, TData data)
        {
            state.ReadTipRequest(data);

            state.ReadDirection(data);
        }
    }
}
