using NetworkPackets.Shared;

namespace NetworkPackets.Shard.Features
{
    public interface ICommand
    {
        ushort Command { get; set; }

        internal void ReadCommand<TData>(TData data)
            where TData : IData
        {
            Command = data.ReadUShort();
        }

        internal void WriteCommand<TData>(TData data)
            where TData : IData
        {
            data.WriteUShort(Command);
        }
    }
}
