using System.Net.Sockets;
using System.Text;
using System.Windows;
using Server.ViewModel;

namespace Server.View;

public partial class ClientWindow
{
    public ClientWindow(string name, string ip)
    {
        InitializeComponent();
        //ServerViewModel serverViewModel = new ServerViewModel(name);
    }
}