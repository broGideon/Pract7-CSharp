using System.Windows.Controls;
using Server.ViewModel;

namespace Server.View;

public partial class UsersOrLogsPage : Page
{
    public UsersOrLogsPage(ServerViewModel serverViewModel, bool IsUsers)
    {
        InitializeComponent();
        DataContext = serverViewModel;
        if (IsUsers) LogsOrUsers.ItemsSource = serverViewModel.Logs;
        //логи
    }
}