using Core.Abstract.Attributes;

namespace Scripts.MobileMovement
{
    [Entity("Shard", "State")]
    public interface IState<TMobile>
        where TMobile : IMobile
    {
        TMobile Character { get; }
    }
}
