using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using Server.ViewModel.Helper;

namespace Server.ViewModel;

public class MainViewModel : BindingHelper
{
    public event EventHandler StartChat;
    public event EventHandler StartConnect;
    
    private string? _name;

    public string? Name
    {
        get => _name;
        set => SetField(ref _name, value);
    }

    private string? _ip;

    public string? Ip
    {
        get => _ip;
        set => SetField(ref _ip, value);
    }

    public void CreateChat()
    {
        if (Name != string.Empty)
        {
            StartChat?.Invoke(this, EventArgs.Empty);
        }
    }

    public void ConnectChat()
    {
        if (Ip != string.Empty && Name != string.Empty)
        {
            StartConnect?.Invoke(this, EventArgs.Empty);
        }
    }
}

public class NotEmptyValidationRule : ValidationRule
{
    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        return string.IsNullOrWhiteSpace((value ?? "").ToString()) ? new ValidationResult(false, "Поле обязательное для заполнения") : ValidationResult.ValidResult;
    }
}