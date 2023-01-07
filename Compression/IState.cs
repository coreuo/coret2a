using Core.Abstract.Attributes;

namespace Compression;

[Entity("Shard", "State")]
public interface IState<in TData>
    where TData : IData
{
    [Priority(0.9)]
    public void OnSend(TData data)
    {
        Huffman.Compress(data.Value[data.Start..data.Length], data.Value[(data.Start+data.Length)..], out var length);

        data.Start += data.Length;

        data.Length = length;
    }
}