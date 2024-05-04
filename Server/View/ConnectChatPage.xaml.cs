using System.Windows.Controls;
using Server.ViewModel;

namespace Server.View;

public partial class ConnectChatPage : Page
{
    public ConnectChatPage(MainViewModel mainViewModel)
    {
        InitializeComponent();
        DataContext = mainViewModel;
    }
}