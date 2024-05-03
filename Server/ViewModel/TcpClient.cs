using System.Collections.ObjectModel;
using System.Net.Sockets;
using System.Text;

namespace Server.ViewModel;

public class TcpClient
{
    public readonly Socket _socket;
    private readonly object _viewModel;
    public ObservableCollection<string> Message = new();
    public CancellationTokenSource TokenClient;
    public ObservableCollection<string> Users = new();

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
                    (_viewModel as ServerViewModel).CloseWindow();
                else
                    (_viewModel as ClientViewModel).CloseWindow();
            }
            else
            {
                var obs = new ObservableCollection<string>(message.Split('\n'));
                obs.RemoveAt(0);
                Users.Clear();
                foreach (var item in obs)
                {
                    Users.Add(item);
                }
            }
        }
    }
}