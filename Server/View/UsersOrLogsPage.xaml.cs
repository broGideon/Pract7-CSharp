using System.Windows.Controls;
using Server.ViewModel;

namespace Server.View;

public partial class UsersOrLogsPage : Page
{
    public UsersOrLogsPage(object viewModel, bool IsUsers)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}