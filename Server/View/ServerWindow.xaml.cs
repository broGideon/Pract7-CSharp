using System.Windows;
using System.Windows.Media.Animation;
using Server.ViewModel;

namespace Server.View;

public partial class ServerWindow
{
    private bool _isOpen = true;
    private readonly ServerViewModel serverViewModel;

    public ServerWindow(string name)
    {
        InitializeComponent();
        serverViewModel = new ServerViewModel(name);
        serverViewModel.Close += (_, _) => CloseThisWindow();
        DataContext = serverViewModel;
        BeginAnimation();
        MainFrame.Content = new ChatPage(serverViewModel);
    }

    private void CloseThisWindow()
    {
        if (!_isOpen) return;

        _isOpen = false;
        var window = new MainWindow();
        window.Show();
        Close();
    }

    private void LogsButton_onClick(object sender, RoutedEventArgs e)
    {
        BeginAnimation();
        MainFrame.Content = new UsersOrLogsPage(serverViewModel, false);
    }


    private void UsersButton_onClick(object sender, RoutedEventArgs e)
    {
        BeginAnimation();
        MainFrame.Content = new UsersOrLogsPage(serverViewModel, true);
    }

    private void ChatButton_onClick(object sender, RoutedEventArgs e)
    {
        BeginAnimation();
        MainFrame.Content = new ChatPage(serverViewModel);
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