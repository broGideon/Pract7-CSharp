using System.Globalization;
using System.Windows;
using System.Windows.Controls;
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
        if (!string.IsNullOrEmpty(Name)) StartChat?.Invoke(this, EventArgs.Empty);
        else MessageBox.Show("Поле имя пользователя не заполнено", "Ошибка валидации", MessageBoxButton.OK,MessageBoxImage.Error);
    }

    public void ConnectChat()
    {
        if (!string.IsNullOrEmpty(Ip) && !string.IsNullOrEmpty(Ip)) StartConnect?.Invoke(this, EventArgs.Empty);
        else if (!string.IsNullOrEmpty(Ip)) MessageBox.Show("Поле IP чата не заполнено", "Ошибка валидации", MessageBoxButton.OK,MessageBoxImage.Error);
        else MessageBox.Show("Поле имя пользователя не заполнено", "Ошибка валидации", MessageBoxButton.OK,MessageBoxImage.Error);
    }
}