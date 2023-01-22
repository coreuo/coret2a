using Core.Abstract.Attributes;

namespace Scripts.CharacterProfile
{
    [Entity("Mobile")]
    public interface IMobile
    {
        int Id { get; }

        Span<char> DynamicProfileText { get; }
    }
}
