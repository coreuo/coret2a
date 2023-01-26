using Core.Abstract.Attributes;

namespace Scripts.MobileCombat
{
    [Entity("Shard", "State")]
    public interface IState<TMobile>
        where TMobile : IMobile
    {
        TMobile Character { get; }

        bool Combat { get; }
    }
}
