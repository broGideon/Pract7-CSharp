using System.Collections.ObjectModel;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Server.Model;

namespace Server.ViewModel;

public class TcpServer
{
    public readonly Socket _socket;
    public Dictionary<Client, CancellationTokenSource> Clients = new();
    public ObservableCollection<string> ExtendedLogs = new();
    public ObservableCollection<string> Logs = new();
    public CancellationTokenSource MainToken;

    public TcpServer()
    {
        var ipAddress = new IPEndPoint(IPAddress.Any, 9999);
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

            var bytes = new byte[1024];
            await clientSocet.ReceiveAsync(bytes, SocketFlags.None);
            var sortByte = bytes?.Where(x => x != 0).ToArray();
            var name = Encoding.UTF8.GetString(sortByte);

            if (name != "/disconnect")
            {
                var client = new Client(name, clientSocet);
                Clients.Add(client, new CancellationTokenSource());
                Logs.Add(client.Name);
                ExtendedLogs.Add($"{client.Name}\n{client.DateTimeConnect.ToString()}");
                await SendLogsToClient();
                _ = ReceiveMessage(client, Clients[client].Token);
            }
            else
            {
                await MailingMessage("/disconnect");
            }
        }
    }

    private async Task ReceiveMessage(Client client, CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            var bytes = new byte[1024];
            await client.SocketClient.ReceiveAsync(bytes, SocketFlags.None);
            var sortByte = bytes?.Where(x => x != 0).ToArray();
            var message = Encoding.UTF8.GetString(sortByte);

            if (message == "/disconnect")
                Clients[client].Cancel();
            else
                await MailingMessage($"[{DateTime.Now.ToString()}][{client.Name}]: {message}");
        }

        Clients.Remove(client);
        Logs.Remove(client.Name);
        ExtendedLogs.Remove($"{client.Name}\n{client.DateTimeConnect.ToString()}");
        await SendLogsToClient();
    }

    private async Task SendLogsToClient()
    {
        var logsString = "/logs\n" + string.Join("\n", Logs);
        await MailingMessage(logsString);
    }

    private async Task SendMessage(Client client, string message)
    {
        var bytes = Encoding.UTF8.GetBytes(message);
        await client.SocketClient.SendAsync(bytes, SocketFlags.None);
    }

    private async Task MailingMessage(string message)
    {
        foreach (var item in Clients.Keys) await SendMessage(item, message);
    }
}