using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Server.ViewModel.Helper;

namespace Server.ViewModel;

public class ServerViewModel : BindingHelper
{
    private ObservableCollection<string> _logs;
    private ObservableCollection<string> _users;
    private string _message;
    private ObservableCollection<string> _messages;
    private readonly TcpClient _tcpClient;
    private readonly TcpServer _tcpServer;
    private const string IP = "127.0.0.1";

    public ServerViewModel(string name)
    {
        _tcpServer = new TcpServer();
        _tcpClient = new TcpClient(name, IP);
        Messages = _tcpClient.Message;
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

    public ObservableCollection<string> Users
    {
        get => _users;
        set => SetField(ref _users, value);
    }
    
    public string Message
    {
        get => _message;
        set => SetField(ref _message, value);
    }

    public event EventHandler Close;
    public event EventHandler OpenLogs;
    public event EventHandler OpenUsers;

    public async void CloseWindow()
    {
        await _tcpClient.SendMessage("/disconnect");
        await _tcpServer.MainToken.CancelAsync();
        var tcpClient = new TcpClient("/disconnect", IP);
        await tcpClient.TokenClient.CancelAsync();
        await _tcpClient.TokenClient.CancelAsync();

        foreach (var item in _tcpServer.Clients.Values) await item.CancelAsync();

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
    
    public async void SendMessageKB(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            MessageBox.Show(Message);
            if (Message == "/disconnect")
            {
                CloseWindow();
                return;
            }

            if (Message != string.Empty)
                await _tcpClient.SendMessage(Message);

            Message = string.Empty;
        }
    }


    public void InputLogs()
    {
        Logs = _tcpServer.ExtendedLogs;
        OpenLogs?.Invoke(this, EventArgs.Empty);
    }

    public void InputUsers()
    {
        Logs = _tcpServer.Logs;
        OpenUsers?.Invoke(this, EventArgs.Empty);
    }
}