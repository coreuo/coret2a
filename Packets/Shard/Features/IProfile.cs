using Core.Abstract.Attributes;

namespace Packets.Shard.Features
{
    public interface IProfile : ILength
    {
        [Length(256)]
        Span<char> StaticProfileText { get; }

        [Length(256)]
        Span<char> DynamicProfileText { get; }
    }
}
