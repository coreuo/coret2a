using Core.Abstract.Attributes;

namespace Scripts.CharacterCreation
{
    [Entity("Mobile")]
    public interface IMobile
    {
        int Id { get; }
    }
}
