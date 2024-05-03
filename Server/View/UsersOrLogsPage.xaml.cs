using System.Windows.Controls;

namespace Server.View;

public partial class UsersOrLogsPage : Page
{
    public UsersOrLogsPage(object viewModel, bool IsUsers)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}