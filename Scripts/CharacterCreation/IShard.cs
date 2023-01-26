using Core.Abstract.Attributes;

namespace Scripts.CharacterCreation
{
    [Entity("Shard", "Server")]
    public interface IShard<TState, TMobile>
        where TState : IState<TMobile>
        where TMobile : IMobile
    {
        TMobile LeaseMobile();

        TMobile Get(int id);

        /*public void OnPacketLogin(TState state)
        {
            var character = LeaseMobile();


        }*/
    }
}
