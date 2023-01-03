namespace Packets.Shard.Outgoing
{
    public interface ICombat
    {
        internal void WriteCombat<TData>(TData data)
            where TData : IData
        {
            data.WriteByte(0); //IsFighting

            data.WriteByte(0); //AutoFight

            data.WriteByte(0x32); //Aggressiveness

            data.WriteByte(0); //RetreatSensitivity
        }
    }
}
