using Core.Abstract.Attributes;
using Core.Abstract.Extensions;

namespace Server.Login
{
    [Entity("Login", "Server")]
    public interface ILogin<in TState, TAccount>
        where TState : IState<TAccount>
        where TAccount : IAccount
    {
        void Listen();

        void Slice();

        void PacketAccountLoginFailed(TState state);

        void PacketBritanniaList(TState state);

        void PacketUserServer(TState state);

        [Priority(1.0)]
        public void OnPacketAccountLoginRequest(TState state)
        {
            if (Is.Default(state.Account)) PacketAccountLoginFailed(state);

            else PacketBritanniaList(state);
        }

        [Priority(1.0)]
        public void OnPacketBritanniaSelect(TState state)
        {
            PacketUserServer(state);
        }
    }
}
