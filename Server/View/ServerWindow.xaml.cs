using Server.ViewModel;

namespace Server.View;

public partial class ServerWindow
{
    private bool _isOpen = true;
    public ServerWindow(string name)
    {
        InitializeComponent();
        ServerViewModel serverViewModel = new ServerViewModel(name);
        serverViewModel.Close += (_, _) => CloseThisWindow();
        DataContext = serverViewModel;
    }

    private void CloseThisWindow()
    {
        if (_isOpen)
        {
            _isOpen = false;
            MainWindow window = new MainWindow();
            window.Show();
            Close();
        }
    }
}