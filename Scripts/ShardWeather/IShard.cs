using Core.Abstract.Attributes;

namespace Scripts.ShardWeather
{
    [Entity("Shard", "Server")]
    public interface IShard<TState, TMobile>
        where TState : IState<TMobile>
        where TMobile : IMobile
    {
        void PacketSunlight(TState state, byte sunLight);

        void PacketLight(TState state, TMobile mobile, byte light);

        void PacketWeatherChange(TState state, byte type, byte particles, byte temperature);

        void PacketGameTime(TState state, byte hour, byte minute, byte second);

        [Priority(3.0)]
        [Link("LoginCharacter")]
        public void OnLoginCharacterSunlight(TState state, TMobile character)
        {
            PacketSunlight(state, character.Sunlight);
        }

        [Priority(4.0)]
        [Link("LoginCharacter")]
        public void OnLoginCharacterLight(TState state, TMobile character)
        {
            PacketLight(state, character, character.Light);
        }

        [Priority(11.0)]
        [Link("LoginCharacter")]
        public void OnLoginCharacterWeather(TState state, TMobile character)
        {
            PacketWeatherChange(state, 0xFE, 0, 0);
        }

        [Priority(12.0)]
        [Link("LoginCharacter")]
        public void OnLoginCharacterTime(TState state, TMobile character)
        {
            PacketGameTime(state, 12, 0, 0);
        }
    }
}
