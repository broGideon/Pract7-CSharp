using System.Collections.ObjectModel;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using Server.View;

namespace Server.ViewModel;

public class TcpClient
{
    private readonly Socket _socket;
    public ObservableCollection<string> Message = new();
    public CancellationTokenSource TokenClient;
    public ObservableCollection<string> Users = new();
    private object _viewModel;

    public TcpClient(string name, string ip, object viewModel)
    {
        _viewModel = viewModel;
        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        _socket.Connect(ip, 9999);
        _ = SendMessage(name);
        TokenClient = new CancellationTokenSource();
        _ = RecieveMessage(TokenClient.Token);
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
            var bytes = new byte[1024];
            await _socket.ReceiveAsync(bytes, SocketFlags.None);
            var sortByte = bytes.Where(item => item != 0).ToArray();
            var message = Encoding.UTF8.GetString(sortByte);

            if (message.Substring(0, 5) != "/logs" && message != "/disconnect")
            {
                Message.Add(Encoding.UTF8.GetString(bytes));
            }
            else if (message == "/disconnect")
            {
                if (_viewModel.GetType() == typeof(ServerViewModel))
                {
                    (_viewModel as ServerViewModel).CloseWindow();
                }
                else
                {
                    (_viewModel as ClientViewModel).CloseWindow();
                }
            }
            else
            {
                Users = new ObservableCollection<string>(message.Split('\n'));
                Users.RemoveAt(0);
            }
        }
    }
}