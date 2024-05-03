using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Windows;
using Server.ViewModel.Helper;

namespace Server.ViewModel;

public class MainViewModel : BindingHelper
{
    private string? _ip;

    private string? _name;

    public string? Name
    {
        get => _name;
        set => SetField(ref _name, value);
    }

    public string? Ip
    {
        get => _ip;
        set => SetField(ref _ip, value);
    }

    public event EventHandler StartChat;
    public event EventHandler StartConnect;

    public void CreateChat()
    {
        var ipAddress = new IPEndPoint(IPAddress.Any, 9999);
        TcpListener listener = new TcpListener(ipAddress);


        try
        {
            listener.Start();
        }
        catch
        {
            ShowMessage("Создание чата невозможно", "Ошибка подключения");
            return;
        }
        listener.Stop();
        
        if (!string.IsNullOrEmpty(Name)) StartChat?.Invoke(this, EventArgs.Empty);
        else ShowMessage("Поле имя пользователя не заполнено", "Ошибка валидации");
    }

    public void ConnectChat()
    {
        PingReply reply = null;
        try
        {
            if (!string.IsNullOrEmpty(Ip))
            {
                var ping = new Ping();
                reply = ping.Send(Ip);
            }
        }
        catch (Exception e)
        {
            ShowMessage("Неверный IP", "Ошибка подключения");
            return;
        }
        
        if (!string.IsNullOrEmpty(Ip) && !string.IsNullOrEmpty(Name) && reply.Status == IPStatus.Success) StartConnect?.Invoke(this, EventArgs.Empty);
        else if (string.IsNullOrEmpty(Ip)) ShowMessage("Поле IP чата не заполнено", "Ошибка валидации");
        else if (string.IsNullOrEmpty(Name)) ShowMessage("Поле имя пользователя не заполнено", "Ошибка валидации");
        else if (reply.Status != IPStatus.Success) ShowMessage("Данный IP адрес недоступен", "Ошибка подключения");
    }

    private void ShowMessage(string message, string caption)
    {
        MessageBox.Show(message, caption, MessageBoxButton.OK,MessageBoxImage.Error);
    }
}