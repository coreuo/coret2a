using NetworkPackets.Shared;

namespace NetworkPackets.Shard.Features
{
    public interface ICharacterCreate
    {
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

        internal void ReadStrength<TData>(TData data)
            where TData : IData
        {
            Strength = data.ReadByte();
        }

        internal void ReadDexterity<TData>(TData data)
            where TData : IData
        {
            Dexterity = data.ReadByte();
        }

        internal void ReadIntelligence<TData>(TData data)
            where TData : IData
        {
            Intelligence = data.ReadByte();
        }

        internal void ReadFirstSkillId<TData>(TData data)
            where TData : IData
        {
            FirstSkillId = data.ReadByte();
        }

        internal void ReadFirstSkillValue<TData>(TData data)
            where TData : IData
        {
            FirstSkillValue = data.ReadByte();
        }

        internal void ReadSecondSkillId<TData>(TData data)
            where TData : IData
        {
            SecondSkillId = data.ReadByte();
        }

        internal void ReadSecondSkillValue<TData>(TData data)
            where TData : IData
        {
            SecondSkillValue = data.ReadByte();
        }

        internal void ReadThirdSkillId<TData>(TData data)
            where TData : IData
        {
            ThirdSkillId = data.ReadByte();
        }

        internal void ReadThirdSkillValue<TData>(TData data)
            where TData : IData
        {
            ThirdSkillValue = data.ReadByte();
        }

        internal ushort ReadSkinHue<TData>(TData data)
            where TData : IData
        {
            return SkinHue = data.ReadUShort();
        }

        internal ushort ReadHairId<TData>(TData data)
            where TData : IData
        {
            return HairId = data.ReadUShort();
        }

        internal ushort ReadHairHue<TData>(TData data)
            where TData : IData
        {
            return HairHue = data.ReadUShort();
        }

        internal ushort ReadBeardId<TData>(TData data)
            where TData : IData
        {
            return BeardId = data.ReadUShort();
        }

        internal ushort ReadBeardHue<TData>(TData data)
            where TData : IData
        {
            return BeardHue = data.ReadUShort();
        }

        internal void ReadTown<TData>(TData data)
            where TData : IData
        {
            Town = data.ReadByte();
        }

        internal void ReadCity<TData>(TData data)
            where TData : IData
        {
            City = data.ReadByte();
        }
    }
}
