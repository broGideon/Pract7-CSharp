using System.Windows;
using System.Windows.Media.Animation;
using Server.ViewModel;

namespace Server.View;

public partial class ClientWindow
{
    private readonly ClientViewModel clientViewModel;
    private bool _isOpen = true;

    public ClientWindow(string name, string ip)
    {
        InitializeComponent();
        clientViewModel = new ClientViewModel(name, ip);
        clientViewModel.Close += (_, _) => CloseThisWindow();
        DataContext = clientViewModel;
        MainFrame.Content = new ChatPage(clientViewModel);
    }

    private void CloseThisWindow()
    {
        if (!_isOpen) return;

        _isOpen = false;
        var window = new MainWindow();
        window.Show();
        Close();
    }

    private void OpenUsers(object sender, RoutedEventArgs e)
    {
        BeginAnimation();
        MainFrame.Content = new UsersOrLogsPage(clientViewModel, true);
    }

    private void ChatButton_onClick(object sender, RoutedEventArgs e)
    {
        BeginAnimation();
        MainFrame.Content = new ChatPage(clientViewModel);
    }

    private void BeginAnimation()
    {
        var OpacityAnim = new DoubleAnimation();
        OpacityAnim.From = 0;
        OpacityAnim.To = 1;
        OpacityAnim.Duration = TimeSpan.FromSeconds(0.5);
        MainFrame.BeginAnimation(OpacityProperty, OpacityAnim);
    }
}