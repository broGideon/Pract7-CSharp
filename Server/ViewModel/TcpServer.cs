using System.Net;
using System.Net.Sockets;
using System.Text;
using Server.Model;

namespace Server.ViewModel;

public class TcpServer
{
    
    private Socket _socket;
    public CancellationTokenSource MainToken;
    public Dictionary<Client, CancellationTokenSource> Clients = new Dictionary<Client, CancellationTokenSource>();
    
    public TcpServer()
    {
        IPEndPoint ipAddress = new IPEndPoint(IPAddress.Any, 9999);
        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        _socket.Bind(ipAddress);
        _socket.Listen(100);
        MainToken = new CancellationTokenSource();
        ListenToClient(MainToken.Token);
    }

    private async Task ListenToClient(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            var clientSocet = await _socket.AcceptAsync();
            
            byte[] bytes = new byte[1024];
            await clientSocet.ReceiveAsync(bytes, SocketFlags.None);
            string name = Encoding.UTF8.GetString(bytes);
            
            Client client = new Client(name, clientSocet);
            Clients.Add(client, new CancellationTokenSource());
            
            ReceiveMessage(client, Clients[client].Token);
        }
    }

    private async Task ReceiveMessage(Client client, CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            byte[] bytes = new byte[1024];
            await client.SocketClient.ReceiveAsync(bytes, SocketFlags.None);
            string message = Encoding.UTF8.GetString(bytes);

            if (message == "/disconnect")
            {
                 Clients[client].Cancel();
                 Clients.Remove(client);
            }
            else
            {
                foreach (var clientsKey in Clients.Keys)
                {
                    SendMessage(clientsKey, message);
                }
            }
        }
    }

    private async Task SendMessage(Client client, string message)
    {
        message = $"[{DateTime.Now.ToString()}][{client.Name}]: {message}";
        byte[] bytes = Encoding.UTF8.GetBytes(message);
        await client.SocketClient.SendAsync(bytes, SocketFlags.None);
    }
}