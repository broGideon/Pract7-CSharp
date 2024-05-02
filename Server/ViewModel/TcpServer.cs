using System.Collections.ObjectModel;
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
    public ObservableCollection<string> Logs = new ObservableCollection<string>();
    public ObservableCollection<string> ExtendedLogs = new ObservableCollection<string>();
    
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
            var sortByte = bytes?.Where(x => x != 0).ToArray();
            string name = Encoding.UTF8.GetString(sortByte);
            if (name != "/disconnect")
            {
                Client client = new Client(name, clientSocet);
                Clients.Add(client, new CancellationTokenSource());
                Logs.Add(client.Name);
                ExtendedLogs.Add($"{client.Name}\n{client.DateTimeConnect.ToString()}");
                ReceiveMessage(client, Clients[client].Token);
            }
            
        }
    }

    private async Task ReceiveMessage(Client client, CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            byte[] bytes = new byte[1024];
            await client.SocketClient.ReceiveAsync(bytes, SocketFlags.None);
            var sortByte = bytes?.Where(x => x != 0).ToArray();
            string message = Encoding.UTF8.GetString(sortByte);
            
            if (message == "/disconnect")
            {
                Clients[client].Cancel();
            }
            else
            {
                foreach (var clientsKey in Clients.Keys)
                {
                    SendMessage(clientsKey, message);
                }
            }
        }
        
        Clients.Remove(client);
        Logs.Remove(client.Name);
        ExtendedLogs.Remove($"{client.Name}\n{client.DateTimeConnect.ToString()}");
    }

    private async Task SendMessage(Client client, string message)
    {
        message = $"[{DateTime.Now.ToString()}][{client.Name}]: {message}";
        byte[] bytes = Encoding.UTF8.GetBytes(message);
        await client.SocketClient.SendAsync(bytes, SocketFlags.None);
    }
}