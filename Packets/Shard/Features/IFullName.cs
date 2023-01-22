using Core.Abstract.Attributes;
using Packets.Shared;
using Packets.Shared.Features;

namespace Packets.Shard.Features
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
