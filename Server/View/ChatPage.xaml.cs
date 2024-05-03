using System.Windows.Controls;

namespace Server.View;

public partial class ChatPage : Page
{
    public ChatPage(object viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}