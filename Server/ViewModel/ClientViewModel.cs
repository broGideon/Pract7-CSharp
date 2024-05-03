using System.Collections.ObjectModel;
using Server.ViewModel.Helper;

namespace Server.ViewModel;

public class ClientViewModel : BindingHelper
{
    private readonly TcpClient _tcpClient;
    private string _message = string.Empty;

    private ObservableCollection<string> _messages;

    private ObservableCollection<string> _users;

    public ClientViewModel(string name, string ip)
    {
        _tcpClient = new TcpClient(name, ip, this);
        Messages = _tcpClient.Message;
        Logs = _tcpClient.Users;
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

    public ObservableCollection<string> Logs
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

    public void CloseWindowAtError()
    {
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

    /*public async void SendMessageKB(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
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
    }*/
}