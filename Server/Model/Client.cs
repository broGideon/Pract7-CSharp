using System.Net.Sockets;

namespace Server.Model;

public class Client
{
    public string Name { get; set; }
    public Socket SocketClient;
    public DateTime DateTimeConnect { get; set; }

    public Client(string name, Socket socketClient)
    {
        Name = name;
        SocketClient = socketClient;
        DateTimeConnect = DateTime.Now;
    }
}