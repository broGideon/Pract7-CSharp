using Server.ViewModel;

namespace Server.View;

public partial class ClientWindow
{
    public ClientWindow(string name, string ip)
    {
        InitializeComponent();
        ClientViewModel viewModel = new ClientViewModel(name, ip);
        viewModel.Close += (_, _) => CloseThisWindow();
        DataContext = viewModel;
    }
    
    private void CloseThisWindow()
    {
        MainWindow window = new MainWindow();
        window.Show();
        Close();
    }
}