using Core.Abstract.Attributes;
using NetworkPackets.Shared;
using NetworkPackets.Shared.Features;

namespace NetworkPackets.Shard.Features
{
    public interface IFullName : IName
    {
        [Length(30)]
        Span<char> Title { get; }

        internal void WriteFullName<TData>(TData data)
            where TData : IData
        {
            var size = data.WriteAscii(Title);

            size += data.WriteAscii(Name);

            data.Offset += 60 - size;
        }
    }
}
