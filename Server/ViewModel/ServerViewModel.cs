using System.Collections.ObjectModel;
using System.Windows.Input;
using Server.ViewModel.Helper;

namespace Server.ViewModel;

public class ServerViewModel : BindingHelper
{
    private ObservableCollection<string> _logs;
    private string _message;
    private ObservableCollection<string> _messages;
    private readonly TcpClient _tcpClient;
    private readonly TcpServer _tcpServer;

    public ServerViewModel(string name)
    {
        _tcpServer = new TcpServer();
        _tcpClient = new TcpClient(name, "127.0.0.1");
        Messages = _tcpClient.Message;
        Logs = _tcpServer.Logs;
    }

    public ObservableCollection<string> Messages
    {
        get => _messages;
        set => SetField(ref _messages, value);
    }

    public ObservableCollection<string> Logs
    {
        get => _logs;
        set => SetField(ref _logs, value);
    }

    public string Message
    {
        get => _message;
        set => SetField(ref _message, value);
    }

    public event EventHandler Close;

    public async void CloseWindow()
    {
        await _tcpClient.SendMessage("/disconnect");
        await _tcpServer.MainToken.CancelAsync();
        var tcpClient = new TcpClient("/disconnect", "127.0.0.1");
        await tcpClient.TokenClient.CancelAsync();
        await _tcpClient.TokenClient.CancelAsync();

        foreach (var item in _tcpServer.Clients.Values) await item.CancelAsync();

        Close(this, EventArgs.Empty);
    }

    public async void SendMessage(object sender, KeyEventArgs args)
    {
        if (Message == "/disconnect")
        {
            CloseWindow();
            return;
        }

        if (Message != string.Empty)
            await _tcpClient.SendMessage(Message);

        Message = string.Empty;
    }

    // public void SwitchMode()
    // {
    //     if (Logs == _tcpServer.Logs)
    //     {
    //         Logs = _tcpServer.ExtendedLogs;
    //     }
    //     else
    //     {
    //         Logs = _tcpServer.Logs;
    //     }
    // }
}