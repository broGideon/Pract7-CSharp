using System.Collections.ObjectModel;
using System.Windows.Input;
using Server.ViewModel.Helper;
using Wpf.Ui.Controls;

namespace Server.ViewModel;

public class ServerViewModel : BindingHelper
{
    private const string IP = "127.0.0.1";
    private readonly TcpClient _tcpClient;
    private readonly TcpServer _tcpServer;
    private ObservableCollection<string> _logs;
    private string _message;
    private ObservableCollection<string> _messages;

    public ServerViewModel(string name)
    {
        _tcpServer = new TcpServer();
        _tcpClient = new TcpClient(name, IP, this);
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
        var tcpClient = new TcpClient("/disconnect", IP, this);
        await tcpClient.TokenClient.CancelAsync();
        await _tcpClient.TokenClient.CancelAsync();

        foreach (var item in _tcpServer.Clients.Values) await item.CancelAsync();
        Close(this, EventArgs.Empty);
        _tcpServer._socket.Close();
    }

    public async void SendMessage()
    {
        if (Message == "/disconnect")
        {
            CloseWindow();
            return;
        }

        if (!string.IsNullOrEmpty(Message))
            await _tcpClient.SendMessage(Message);

        Message = string.Empty;
    }
    
    public async void SendMessageKB(object sender, KeyEventArgs e)
    {
        if (e.Key != Key.Enter) return;
        
        string message = (sender as TextBox).Text;
        if (message == "/disconnect")
        {
            CloseWindow();
            return;
        }

        if (!string.IsNullOrEmpty(message))
            await _tcpClient.SendMessage(message);

        (sender as TextBox).Text = string.Empty;
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