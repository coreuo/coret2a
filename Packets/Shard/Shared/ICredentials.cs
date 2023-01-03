using Core.Abstract.Attributes;

namespace Packets.Shard.Shared
{
    public interface ICredentials
    {
        [Size(30)] Span<char> Name { get; }

        [Size(30)] Span<char> Password { get; }

        internal void ReadCredentials<TData>(TData data)
            where TData : IData
        {
            data.ReadAscii(Name, 30);

            data.ReadAscii(Password, 30);
        }

        internal void WriteCredentials<TData>(TData data)
            where TData : IData
        {
            data.WriteAscii(Name, 30);

            data.WriteAscii(Password, 30);
        }
    }
}
