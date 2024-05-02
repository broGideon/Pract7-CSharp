using System.Collections.ObjectModel;
using System.Net.Sockets;
using System.Text;

namespace Server.ViewModel;

public class TcpClient
{
    private Socket _socket;
    public ObservableCollection<string> Message = new ObservableCollection<string>();
    public CancellationTokenSource TokenClient;
    
    public TcpClient(string name, string ip)
    {
        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        _socket.Connect(ip, 9999);
        SendMessage(name);
        TokenClient = new CancellationTokenSource();
        RecieveMessage(TokenClient.Token);
    }
    
    public async Task SendMessage(string message)
    {
        var bytes = Encoding.UTF8.GetBytes(message);
        await _socket.SendAsync(bytes, SocketFlags.None);
    }
    
    private async Task RecieveMessage(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            byte[] bytes = new byte[1024];
            await _socket.ReceiveAsync(bytes, SocketFlags.None);
            Message.Add(Encoding.UTF8.GetString(bytes));
        }
    }
}