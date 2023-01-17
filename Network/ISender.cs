using Core.Abstract.Attributes;
using Core.Abstract.Managers;
using System.Net.Sockets;

namespace Network;

public interface ISender<TData> : IState, ITransfer<TData>, IThread
    where TData : IData
{
    int Sending { get; set; }

    [Priority(1.0)]
    public void OnSend(TData data)
    {
        _ = OnSendAsync(data);
    }

    private async ValueTask<int> OnSendAsync(TData data)
    {
        if (!Locked) return 3;

        using var manager = SharedMemoryManager<byte>.Lease(data.Value.Slice(data.Start, data.Length));

        Sending++;

        try
        {
            data.Length = await SendAsync(manager.Memory);
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
            Debug("Cannot send.", exception);
#endif
            return Halt(data, 1);
        }

        if (data.Length > 0)
        {
#if DEBUG
            Debug($"sent {data.Length} bytes to {EndPoint}");
#endif
            Sending--;

            ReleaseData(data);
        }
        else
        {
#if DEBUG
            Debug($"zero buffer {EndPoint}");
#endif
            return Halt(data, 2);
        }

        return 0;
    }

    private int Halt(TData data, int code)
    {
        Locked = false;

        Sending--;

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