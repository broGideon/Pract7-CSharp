using Server.ViewModel;

namespace Server.View;

public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();
        MainViewModel mainViewModel = new MainViewModel();
        mainViewModel.StartChat += (_, _) => StartServer();
        mainViewModel.StartConnect += (_, _) => StartClient();
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