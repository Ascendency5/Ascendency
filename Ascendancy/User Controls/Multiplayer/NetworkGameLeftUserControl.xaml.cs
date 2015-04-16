using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Ascendancy.Networking;

namespace Ascendancy.User_Controls.Multiplayer
{
    /// <summary>
    /// Interaction logic for NetworkGameLeftUserControl.xaml
    /// </summary>
    public partial class NetworkGameLeftUserControl : UserControl
    {
        private readonly VoidFunctionTemplate.Function Callback;

        public NetworkGameLeftUserControl(KulamiPeer peer, VoidFunctionTemplate.Function callback)
        {
            InitializeComponent();
            this.Callback = callback;

            LeftGameNotificationLabel.Content = peer.Name + " has left the game.";
            LeftGameNotificationLabelGlow.Content = peer.Name + " has left the game.";
        }

        private void MainMenu_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ContentControlActions.FadeOut();
            Callback();
        }
    }
}
