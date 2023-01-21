using Core.Abstract.Attributes;
using Packets.Shared;

namespace Packets.Shard.Domain;

[Entity("Shard", "Server")]
public interface IShard<TLogin, TData>
    where TData : IData
    where TLogin : ILogin<TData>
{
#if DEBUG
    string Identity { get; }
#endif
    [Link("LoginServer.Shards.Owner")]
    TLogin Login { get; }

    [Priority(2.0)]
    public void OnInternalSendToLogin(TData data)
    {
        Login.SendInternal(data);
#if DEBUG
        Debug($"internal sent {data.Length} bytes to login");
#endif
    }
#if DEBUG
    private void Debug(string text)
    {
        Console.WriteLine($"[{Identity}] {text}");
    }
#endif
}