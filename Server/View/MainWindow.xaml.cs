using System.Windows;
using System.Windows.Media.Animation;
using Server.ViewModel;

namespace Server.View;

public partial class MainWindow
{
    private readonly MainViewModel mainViewModel;
    public MainWindow()
    {
        InitializeComponent();
        this.mainViewModel = new MainViewModel();
        _BeginAnimation();
        MainFrame.Content = new CreateChatPage(mainViewModel);
        mainViewModel.StartChat += (sender, args) => StartServer(sender, args);
        mainViewModel.StartConnect += (sender, args) => StartClient(sender, args);
        DataContext = this.mainViewModel;
    }

    private void CreateChatButton_onClick(object sender, RoutedEventArgs e)
    {
        _BeginAnimation();
        MainFrame.Content = new CreateChatPage(mainViewModel);
    }

    private void ConnectChatButton_onClick(object sender, RoutedEventArgs e)
    {
        _BeginAnimation();
        MainFrame.Content = new ConnectChatPage(mainViewModel);
    }
    
    private void _BeginAnimation()
    {
        var OpacityAnim = new DoubleAnimation();
        OpacityAnim.From = 0;
        OpacityAnim.To = 1;
        OpacityAnim.Duration = TimeSpan.FromSeconds(0.5);
        MainFrame.BeginAnimation(OpacityProperty, OpacityAnim);
    }
    
    private void StartServer(object sender, EventArgs args)
    {
        var serverWindow = new ServerWindow((sender as MainViewModel).Name);
        serverWindow.Show();
        Close();
    }
    
    private void StartClient(object sender, EventArgs args)
    {
        var clientWindow = new ClientWindow((sender as MainViewModel).Name, (sender as MainViewModel).Ip);
        clientWindow.Show();
        Close();
    }
}