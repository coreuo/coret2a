using System.Collections.Concurrent;
using System.Net.Sockets;
using Core.Abstract.Attributes;
using Core.Abstract.Managers;

namespace Network;

public interface IReceiver<TData, out TDataConcurrentQueue> : IState, ITransfer<TData>, IThread
    where TData : IData
    where TDataConcurrentQueue : IProducerConsumerCollection<TData>
{
    bool Receiving { get; set; }

    DateTime Last { set; }

    TDataConcurrentQueue ReceiveQueue { get; }

    [Priority(1.0)]
    public void OnReceive()
    {
        _ = OnReceiveAsync();
    }

    private async ValueTask<int> OnReceiveAsync()
    {
#if DEBUG
        Debug($"connected from {EndPoint}");
#endif
        Locked = true;

        Receiving = true;

        var data = LeaseData();

        data.Reset();

        using var manager = SharedMemoryManager<byte>.Lease(data.Value.Slice(data.Start, data.Length));

        while (true)
        {
            try
            {
                data.Length = await ReceiveAsync(manager.Memory);
            }
            catch (SocketException) when (!Locked)
            {
#if DEBUG
                Debug("stopped");
#endif
                return Halt(data, -1);
            }
            catch
#if DEBUG
                (Exception exception)
#endif
            {
#if DEBUG
                Debug("cannot receive", exception);
#endif
                return Halt(data, 1);
            }

            if (data.Length > 0)
            {
#if DEBUG
                Debug($"received {data.Length} bytes from {EndPoint}");
#endif
                ReceiveQueue.TryAdd(data);

                Last = DateTime.Now;
            }
            else
            {
#if DEBUG
                Debug($"disconnected from {EndPoint}");
#endif
                return Halt(data, 2);
            }

            data = LeaseData();

            data.Reset();

            manager.Initialize(data.Value);
        }
    }

    private int Halt(TData data, int code)
    {
        Locked = false;

        Receiving = false;

        ReleaseData(data);

        return code;
    }

#if DEBUG
    private void Debug(string text, Exception? exception = null)
    {
        Console.WriteLine($"[{Identity}] {text}{(exception != null ? $"\n{exception}" : null)}");
    }
#endif
}