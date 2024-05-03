using Server.ViewModel;

namespace Server.View;

public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();
        var mainViewModel = new MainViewModel();
        mainViewModel.StartChat += (_, _) => StartServer();
        mainViewModel.StartConnect += (_, _) => StartClient();
        DataContext = mainViewModel;
    }

    private void StartServer()
    {
        var window = new ServerWindow(NameTextBox.Text);
        window.Show();
        Close();
    }

    private void StartClient()
    {
        var window = new ClientWindow(NameTextBox.Text, IpTextBox.Text);
        window.Show();
        Close();
    }
}