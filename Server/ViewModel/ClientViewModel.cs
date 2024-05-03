using System.Collections.ObjectModel;
using Server.ViewModel.Helper;

namespace Server.ViewModel;

public class ClientViewModel : BindingHelper
{
    private string _message = string.Empty;

    private ObservableCollection<string> _messages;
    private readonly TcpClient _tcpClient;

    private ObservableCollection<string> _users;

    public ClientViewModel(string name, string ip)
    {
        _tcpClient = new TcpClient(name, ip);
        Messages = _tcpClient.Message;
        Users = _tcpClient.Users;
    }

    public string Message
    {
        get => _message;
        set => SetField(ref _message, value);
    }

    public ObservableCollection<string> Messages
    {
        get => _messages;
        set => SetField(ref _messages, value);
    }

    public ObservableCollection<string> Users
    {
        get => _users;
        set => SetField(ref _users, value);
    }

    public event EventHandler Close;

    public void CloseWindow()
    {
        _ = _tcpClient.SendMessage("/disconnect");
        _tcpClient.TokenClient.Cancel();

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
}