using Core.Abstract.Attributes;
using Packets.Shared;

namespace Packets.Shard.Features
{
    public interface IText : ILength
    {
        [Length(256)]
        Span<char> Text { get; }

        internal void ReadText<TData>(TData data)
            where TData : IData
        {
            var length = ReadLength(data);

            if (length > 256)
                throw new InvalidOperationException($"Invalid text length {length} (only 256 characters is allowed).");

            data.ReadBigUnicode(Text, length);
        }
    }
}
