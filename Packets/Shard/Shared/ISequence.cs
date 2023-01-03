namespace Packets.Shard.Shared
{
    public interface ISequence
    {
        byte Sequence { get; set; }

        internal void ReadSequence<TData>(TData data)
            where TData : IData
        {
            Sequence = data.ReadByte();
        }

        internal void WriteSequence<TData>(TData data)
            where TData : IData
        {
            data.WriteByte(Sequence);
        }
    }
}
