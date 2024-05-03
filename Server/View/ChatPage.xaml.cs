using System.Windows.Controls;
using Server.ViewModel;

namespace Server.View;

public partial class ChatPage : Page
{
    private readonly ServerViewModel serverViewModel;

    public ChatPage(ServerViewModel serverViewModel)
    {
        InitializeComponent();
        this.serverViewModel = serverViewModel;
        DataContext = this.serverViewModel;
    }
}