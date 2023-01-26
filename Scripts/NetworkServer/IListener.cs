using System.Collections.Concurrent;
using System.Net;
using Core.Abstract.Attributes;
using Core.Abstract.Extensions;

namespace Scripts.NetworkServer;

public interface IListener<TState, out TStateConcurrentQueue> : ISocket, IThread
    where TState : IState
    where TStateConcurrentQueue : IProducerConsumerCollection<TState>
{
#if DEBUG
    string Identity { get; }
#endif
    Span<char> IpAddress { get; }

    ushort Port { get; set; }

    TStateConcurrentQueue ListenQueue { get; }

    bool Listening { get; set; }

    TState LeaseState();

    void ReleaseState(TState state);

    void Listen();

    [Priority(1.0)]
    public void OnListen()
    {
        _ = OnListenAsync();
    }

    private async ValueTask<int> OnListenAsync()
    {
        Locked = true;

        Listening = true;

        if (!IPAddress.TryParse(IpAddress.AsText(), out var address))
        {
#if DEBUG
            Debug($"Unable to listen on ({IpAddress}), please set valid IP address.");
#endif
            throw new InvalidOperationException("Unable to listen, please set valid IP address.");
        }

        try
        {
            Listen(address, Port);
#if DEBUG
            Debug($"listening on {EndPoint}");
#endif
        }
        catch
#if DEBUG
            (Exception exception)
#endif
        {
#if DEBUG
            Debug("cannot listen", exception);
#endif
            return HaltListen(1);
        }

        return await AcceptAsync();
    }

    private int HaltListen(int code)
    {
        Locked = false;

        Listening = false;

        return code;
    }

    private async ValueTask<int> AcceptAsync()
    {
        var state = LeaseState();

        while (true)
        {
            try
            {
                await AcceptAsync(state);
            }
            catch (ObjectDisposedException) when (!Locked)
            {
#if DEBUG
                Debug("closed");
#endif
                return HaltAccept(state, -1);
            }
            catch
#if DEBUG
                (Exception exception)
#endif
            {
#if DEBUG
                Debug("cannot accept", exception);
#endif
                return HaltAccept(state, 2);
            }

            ListenQueue.TryAdd(state);

            state = LeaseState();
        }
    }

    private int HaltAccept(TState state, int code)
    {
        Locked = false;

        Listening = false;

        ReleaseState(state);

        return code;
    }
#if DEBUG
    private void Debug(string text, Exception? exception = null)
    {
        Console.WriteLine($"[{Identity}] {text}{(exception != null ? $"\n{exception}" : null)}");
    }
#endif
}