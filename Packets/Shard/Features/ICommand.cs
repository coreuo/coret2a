namespace Packets.Shard.Features;

public interface ICommand
{
    byte Command { get; set; }

    internal void ReadCommand<TData>(TData data)
        where TData : IData
    {
        Command = data.ReadByte();
    }

    internal void WriteCommand<TData>(TData data)
        where TData : IData
    {
        data.WriteByte(Command);
    }
}