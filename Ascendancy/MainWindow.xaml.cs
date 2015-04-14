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
using Ascendancy.User_Control;
using Ascendancy.User_Controls;
using Ascendancy.User_Controls.Multiplayer;

namespace Ascendancy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            ContentControlActions.baseContentControl = HomeScreenContentControl;
            ContentControlActions.popupContentControl = PopupContentControl;

            VolumeManager.MainThemeVolume = .5;
            VolumeManager.BattleThemeVolume = .5;
            VolumeManager.SoundVolume = .5;

            IntroUserControl intro = new IntroUserControl();
            intro.OnComplete += on_intro_completed;
            //setBaseContent(new HomeScreenUserControl());
            setBaseContent(intro);

            Networkmanager.OnDiscovery += PeerHolder.on_peer_discovery;
            Networkmanager.OnDisconnect += PeerHolder.on_peer_disconnect;

            Networkmanager.Start();
        }

        private void on_intro_completed(object sender, EventArgs e)
        {
            setBaseContent(new HomeScreenUserControl());
        }

        private void setBaseContent(UserControl control)
        {
            ContentControlActions.setBaseContentControl(control);
        }

        /********** Misc Event Functions *********/

        private void HomeScreenWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //this.Topmost = true;
            this.WindowState = WindowState.Maximized;
            this.WindowStyle = WindowStyle.None;
            this.Focus();
        }

        private void AscendancyHomeScreen_KeyDown(object sender, KeyEventArgs e)
        {
            //if the intro isn't in progress, and no other UserControl is active...
            ContentControl baseControl = ContentControlActions.baseContentControl;
            if (baseControl.Content is IntroUserControl)
            {
                if (e.Key == Key.Space || e.Key == Key.Escape)
                {
                    ((IntroUserControl)baseControl.Content).stopIntro();
                    return;
                }
            }

            if (ContentControlActions.IsPopupVisible)
            {
                if (e.Key == Key.Escape)
                {
                    ContentControlActions.FadeOut();
                    return;
                }
            }

            if (baseControl.Content is HelpPopUpUserControl)
            {
                if (e.Key == Key.Escape)
                {
                    ContentControlActions.FadeOut();
                    return;
                }
            }

            if (baseControl.Content is HomeScreenUserControl)
            {
                if (e.Key == Key.Escape)
                {
                    ContentControlActions.setPopup(new ExitConfirmationUserControl());
                }
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Networkmanager.Shutdown();
            Application.Current.Shutdown();
        }
    }
}