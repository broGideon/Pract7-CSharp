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
        if (!_isOpen) return;
        
        _isOpen = false;
        var window = new MainWindow();
        window.Show();
        this.Close();
    }
}