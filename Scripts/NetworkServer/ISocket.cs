using System.Net;
using System.Net.Sockets;

namespace Scripts.NetworkServer;

public interface ISocket
{
    Socket Socket { get; set; }

    EndPoint EndPoint { get; set; }

    void HoldSocket(Socket socket);

    void HoldEndPoint(EndPoint endPoint);

    internal void Listen(IPAddress address, int port)
    {
        var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        HoldSocket(socket);

        Socket = socket;

        var endPoint = new IPEndPoint(address, port);

        HoldEndPoint(endPoint);

        EndPoint = endPoint;

        Socket.Bind(endPoint);

        Socket.Listen(100);
    }

    internal async ValueTask AcceptAsync<TState>(TState state)
        where TState : ISocket
    {
        var socket = await Socket.AcceptAsync();

        HoldSocket(socket);

        state.Socket = socket;

        var endPoint = socket.RemoteEndPoint!;

        HoldEndPoint(endPoint);

        state.EndPoint = endPoint;
    }

    internal ValueTask<int> ReceiveAsync(Memory<byte> buffer)
    {
        return Socket.ReceiveAsync(buffer);
    }

    internal ValueTask<int> SendAsync(Memory<byte> buffer)
    {
        return Socket.SendAsync(buffer);
    }

    internal void Close()
    {
        Socket.Close();
    }
}