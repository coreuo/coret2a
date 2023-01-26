using Core.Abstract.Attributes;

namespace Scripts.MobileEquip
{
    [Entity("Shard", "State")]
    public interface IState<TMobile>
        where TMobile : IMobile
    {
        TMobile Character { get; }
    }
}
