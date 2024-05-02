using System.Collections.ObjectModel;
using System.Net;
using Server.ViewModel.Helper;

namespace Server.ViewModel;

public class ServerViewModel : BindingHelper
{
    private TcpServer _tcpServer;
    private TcpClient _tcpClient;
    public event EventHandler Close;

    private ObservableCollection<string> _messages;

    public ObservableCollection<string> Messages
    {
        get => _messages;
        set => SetField(ref _messages, value);
    }

    private ObservableCollection<string> _logs;

    public ObservableCollection<string> Logs
    {
        get => _logs;
        set => SetField(ref _logs, value);
    }
    
    private string _message;

    public string Message
    {
        get => _message;
        set => SetField(ref _message, value);
    }
    
    public ServerViewModel(string name)
    {
        _tcpServer = new TcpServer();
        _tcpClient = new TcpClient(name, "127.0.0.1");
        Messages = _tcpClient.Message;
        Logs = _tcpServer.Logs;
    }

    public async void CloseWindow()
    {
        await _tcpClient.SendMessage("/disconnect");
        _tcpServer.MainToken.Cancel();
        TcpClient tcpClient = new TcpClient("/disconnect", "127.0.0.1");
        tcpClient.TokenClient.Cancel();
        _tcpClient.TokenClient.Cancel();

        foreach (var item in _tcpServer.Clients.Values)
        {
            item.Cancel();
        }
        
        Close(this, EventArgs.Empty);
    }

    public async void SendMessage()
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

    public async void SwitchMode()
    {
        if (Logs == _tcpServer.Logs)
        {
            Logs = _tcpServer.ExtendedLogs;
        }
        else
        {
            Logs = _tcpServer.Logs;
        }
    }
}