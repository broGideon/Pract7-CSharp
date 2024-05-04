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
        MainFrame.Content = new CreateChatPage(mainViewModel, this);
        DataContext = this.mainViewModel;
    }

    private void CreateChatButton_onClick(object sender, RoutedEventArgs e)
    {
        _BeginAnimation();
        MainFrame.Content = new CreateChatPage(mainViewModel, this);
    }

    private void ConnectChatButton_onClick(object sender, RoutedEventArgs e)
    {
        _BeginAnimation();
        MainFrame.Content = new ConnectChatPage(mainViewModel, this);
    }
    
    private void _BeginAnimation()
    {
        var OpacityAnim = new DoubleAnimation();
        OpacityAnim.From = 0;
        OpacityAnim.To = 1;
        OpacityAnim.Duration = TimeSpan.FromSeconds(0.5);
        MainFrame.BeginAnimation(OpacityProperty, OpacityAnim);
    }
}