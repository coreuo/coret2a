using Core.Abstract.Attributes;

namespace Compression
{
    [Entity("Shard", "Server")]
    public interface IShard<TData>
        where TData : IData
    {
    }
}
