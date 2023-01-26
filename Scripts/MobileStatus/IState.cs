using Core.Abstract.Attributes;

namespace Scripts.MobileStatus
{
    [Entity("Shard", "State")]
    public interface IState<TMobile>
        where TMobile : IMobile
    {
        TMobile Character { get; }
    }
}
