using System.Windows.Controls;
using System.Windows.Input;
using Server.ViewModel;

namespace Server.View;

public partial class ChatPage : Page 
{
    public ChatPage(object viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}