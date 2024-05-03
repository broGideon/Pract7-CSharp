using Server.ViewModel;

namespace Server.View;

public partial class ClientWindow
{
    private bool _isOpen = true;

    public ClientWindow(string name, string ip)
    {
        InitializeComponent();
        var viewModel = new ClientViewModel(name, ip);
        viewModel.Close += (_, _) => CloseThisWindow();
        DataContext = viewModel;
    }

    private void CloseThisWindow()
    {
        if (!_isOpen) return;

        _isOpen = false;
        var window = new MainWindow();
        window.Show();
        Close();
    }
}