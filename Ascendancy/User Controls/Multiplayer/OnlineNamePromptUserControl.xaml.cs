using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Ascendancy.Networking;

namespace Ascendancy.User_Controls.Multiplayer
{
    /// <summary>
    /// Interaction logic for OnlineNamePromptUserControl.xaml
    /// </summary>
    public partial class OnlineNamePromptUserControl : UserControl
    {
        public OnlineNamePromptUserControl()
        {
            InitializeComponent();
        }

        private void NamePromptText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                CheckNamePrompt();
            }
        }

        private void NamePromptText_Loaded(object sender, RoutedEventArgs e)
        {
            NamePromptText.Focus();
            NamePromptText.SelectAll();
        }

        private void NamePromptText_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (NamePromptText.Text == "Enter your name here")
            {
                NamePromptText.Text = "";
            }
        }

        private void Back_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ContentControlActions.FadeOut();
        }

        private void EnterLobby_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            CheckNamePrompt();
        }

        private void CheckNamePrompt()
        {
            // todo strip spaces for check
            // todo Disable button if text is empty
            if (NamePromptText.Text == "") return;

            Networkmanager.ClientName = NamePromptText.Text;

            ContentControlActions.setPopup(new OnlineLobbyUserControl());
        }
    }
}
