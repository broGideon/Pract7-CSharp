using System.Net.Sockets;

namespace Server.Model;

public class Client
{
    public Socket SocketClient;

    public Client(string name, Socket socketClient)
    {
        Name = name;
        SocketClient = socketClient;
        DateTimeConnect = DateTime.Now;
    }

    public string Name { get; set; }
    public DateTime DateTimeConnect { get; set; }
}