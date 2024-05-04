using System.Windows;
using System.Windows.Controls;
using Server.ViewModel;

namespace Server.View;

public partial class CreateChatPage : Page
{
    public CreateChatPage(MainViewModel mainViewModel)
    {
        InitializeComponent();
        DataContext = mainViewModel;
    }
}