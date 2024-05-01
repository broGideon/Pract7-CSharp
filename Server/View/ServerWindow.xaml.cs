using Server.ViewModel;

namespace Server.View;

public partial class ServerWindow
{
    public ServerWindow(string name)
    {
        InitializeComponent();
        ServerViewModel serverViewModel = new ServerViewModel(name);
        serverViewModel.Close += (_, _) => CloseThisWindow();
        DataContext = serverViewModel;
    }

    private void CloseThisWindow()
    {
        MainWindow window = new MainWindow();
        window.Show();
        Close();
    }
}