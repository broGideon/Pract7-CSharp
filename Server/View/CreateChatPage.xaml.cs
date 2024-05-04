using System.Windows;
using System.Windows.Controls;
using Server.ViewModel;

namespace Server.View;

public partial class CreateChatPage : Page
{
    private readonly MainWindow _window;
    public CreateChatPage(MainViewModel mainViewModel, MainWindow window)
    {
        InitializeComponent();
        this._window = window;
        mainViewModel.StartChat += (_, _) => StartServer();
        DataContext = mainViewModel;
    }
    
    private void StartServer()
    {
        var serverWindow = new ServerWindow(NameTextBox.Text);
        serverWindow.Show();
        _window.Close();
    }
}