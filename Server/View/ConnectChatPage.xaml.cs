using System.Windows.Controls;
using Server.ViewModel;

namespace Server.View;

public partial class ConnectChatPage : Page
{
    private readonly MainWindow _window;
    public ConnectChatPage(MainViewModel mainViewModel, MainWindow window)
    {
        InitializeComponent();
        this._window = window;
        mainViewModel.StartChat += (_, _) => StartClient();
        DataContext = mainViewModel;
    }
    
    private void StartClient()
    {
        var clientWindow = new ClientWindow(NameTextBox.Text, IpTextBox.Text);
        clientWindow.Show();
        _window.Close();
    }
}