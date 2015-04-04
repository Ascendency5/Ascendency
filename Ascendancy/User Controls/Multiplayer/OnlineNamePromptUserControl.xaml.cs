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
            UserControlAnimation.StartButtonGradientSpin(Buttons);
        }

        private void EventFilter(object sender)
        {
            if (sender == Cancel)
            {
                ContentControlActions.FadeOut();
            }
            else if (sender == EnterLobby || sender == NamePromptText)
            {

                // todo Have this reject it if it's not a valid name
                Networkmanager.ClientName = NamePromptText.Text;

                ContentControlActions.setPopup(new OnlineLobbyUserControl());
            }
        }
        
        private void OnlineNamePromptButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Canvas sss = (Canvas)sender;
            Storyboard localStoryboard = App.Current.FindResource("ButtonUpStoryboard") as Storyboard;
            Storyboard.SetTarget(localStoryboard, sss.Children[1]);
            localStoryboard.Begin();

            EventFilter(sender);
        }

        private void OnlineNamePromptButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Canvas sss = (Canvas)sender;
            Storyboard localStoryboard = App.Current.FindResource("ButtonDownStoryboard") as Storyboard;
            Storyboard.SetTarget(localStoryboard, sss.Children[1]);
            localStoryboard.Begin();

        }

        private void OnlineNamePromptButton_MouseEnter(object sender, MouseEventArgs e)
        {
            Canvas animateThisCanvas = (Canvas)sender;
            UserControlAnimation.FadeInUserControlButton(animateThisCanvas.Children[0], true);

            //added sound effect for the button
            VolumeManager.play(@"Resources/Audio/UserControlButtonHover.wav");
        }

        private void OnlineNamePromptButton_MouseLeave(object sender, MouseEventArgs e)
        {
            //todo get mouse down working with this
            Canvas animateThisCanvas = (Canvas)sender;
            UserControlAnimation.FadeInUserControlButton(animateThisCanvas.Children[0], false);
        }

        private void NamePromptText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                EventFilter(sender);
            }
        }

        private void NamePromptText_GotFocus(object sender, RoutedEventArgs e)
        {
            NamePromptText.Text = "";
        }

    }
}
