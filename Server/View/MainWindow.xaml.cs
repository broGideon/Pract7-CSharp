using Server.ViewModel;

namespace Server.View;

public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();
        MainViewModel mainViewModel = new MainViewModel();
        mainViewModel.StartChat += (sender, args) => StartServer();
        mainViewModel.StartConnect += (sender, args) => StartClient();
        DataContext = mainViewModel;
    }

    private void StartServer()
    {
        ServerWindow window = new ServerWindow(NameTextBox.Text);
        window.Show();
        Close();
    }

    private void StartClient()
    {
        ClientWindow window = new ClientWindow(NameTextBox.Text, IpTextBox.Text);
        window.Show();
        Close();
    }
}