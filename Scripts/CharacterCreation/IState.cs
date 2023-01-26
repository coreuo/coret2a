using Core.Abstract.Attributes;

namespace Scripts.CharacterCreation
{
    [Entity("Shard", "State")]
    public interface IState<TMobile>
        where TMobile : IMobile
    {
        TMobile Character { get; set; }

        Span<char> Name { get; }

        Span<char> Password { get; }

        byte Strength { get; set; }

        byte Dexterity { get; set; }

        byte Intelligence { get; set; }

        byte FirstSkillId { get; set; }

        byte FirstSkillValue { get; set; }

        byte SecondSkillId { get; set; }

        byte SecondSkillValue { get; set; }

        byte ThirdSkillId { get; set; }

        byte ThirdSkillValue { get; set; }

        ushort SkinHue { get; set; }

        ushort HairId { get; set; }

        ushort HairHue { get; set; }

        ushort BeardId { get; set; }

        ushort BeardHue { get; set; }

        byte Town { get; set; }

        byte City { get; set; }
    }
}
