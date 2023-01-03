namespace Packets.Shard.Shared
{
    public interface ISerial
    {
        int Id { get; }

        internal void WriteSerial<TData>(TData data)
            where TData : IData
        {
            data.WriteInt(Id);
        }
    }
}
