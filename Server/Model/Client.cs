using System.Net.Sockets;

namespace Server.Model;

public class Client
{
    public string Name;
    public Socket SocketClient;
    public DateTime DateTimeConnect;

    public Client(string name, Socket socketClient)
    {
        Name = name;
        SocketClient = socketClient;
        DateTimeConnect = DateTime.Now;
    }
}