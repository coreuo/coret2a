using Core.Abstract.Attributes;

namespace Scripts.MobileSkills
{
    [Entity("Shard", "State")]
    public interface IState<TMobile>
        where TMobile : IMobile
    {
        TMobile Character { get; }
    }
}
