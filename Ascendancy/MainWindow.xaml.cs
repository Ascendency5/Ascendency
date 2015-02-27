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
using Ascendancy.User_Controls;

namespace Ascendancy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //video player variables
        //private MediaTimeline timeline;
        //private MediaClock clock;
        //private MediaPlayer player;
        //private VideoDrawing drawing;
        //private DrawingBrush brush;

        //local variables
        private HomeScreenAnimation localStoryboard;

        public MainWindow()
        {
            InitializeComponent();

            //boot up other local variables
            localStoryboard = new HomeScreenAnimation();

            ContentControlActionsWrapper.baseContentControl = HomeScreenContentControl;
        }

        #region User Control Callers

        private void SinglePlayerHover_MouseLeftButtonUp(object sender, RoutedEventArgs e)
        {
            setContent(new SinglePlayerUserControl());
        }

        private void MultiplayerHover_MouseLeftButtonUp(object sender, RoutedEventArgs e)
        {
            //HomeScreenContentControl.Content = new User_Controls.MultiplayerUserControl(this, HomeScreenContentControl);
        }

        private void HelpHover_MouseLeftButtonUp(object sender, RoutedEventArgs e)
        {
            //HomeScreenContentControl.Content = new User_Controls.HelpUserControl(this, HomeScreenContentControl);
        }

        private void OptionsHover_MouseLeftButtonUp(object sender, RoutedEventArgs e)
        {
            setContent(new OptionsUserControl());
        }

        private void ExitHover_MouseLeftButtonUp(object sender, RoutedEventArgs e)
        {
            setContent(new ExitConfirmationUserControl());
        }

        private void setContent(UserControl control)
        {
            ContentControlActionsWrapper.setUpControl(control);
        }
        #endregion

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

            if (e.Key == Key.Escape)
            {
                ExitHover_MouseLeftButtonUp(sender, e);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            //Thread.Sleep(new TimeSpan(0, 0, 1));
            base.OnClosed(e);
            Application.Current.Shutdown();
        }


        /*************************** Animations ************************/
        private void HomeScreen_MouseEnter(object sender, MouseEventArgs e)
        {
            localStoryboard.FadeInHomeScreenButton(sender, true);
        }

        private void HomeScreen_MouseLeave(object sender, MouseEventArgs e)
        {
            localStoryboard.FadeInHomeScreenButton(sender, false);
            localStoryboard = new HomeScreenAnimation();
        }



        private void MenuLogo_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            localStoryboard.HomeScreenLogo_MouseEnter(sender);
        }

        private void MenuLogo_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            localStoryboard = new HomeScreenAnimation();
        }





        /********************** End animation Block *********************/
    }
}





//get the video going
//MediaPlayer mp = new MediaPlayer();
//mp.Open(new Uri(@"/Resources/video/menu.mp4", UriKind.Relative));
//VideoDrawing vd = new VideoDrawing();
//vd.Player = mp;
//vd.Rect = new Rect(0, 0, 100, 100);
//DrawingBrush db = new DrawingBrush(vd); 
////foreach (Button b in videoGrid.Children)
////    b.Background = db;
//MainGrid.Background = db;
//mp.Play();



//timeline = new MediaTimeline(new Uri(@"Resources/video/menu.mp4", UriKind.Relative));
//timeline.RepeatBehavior = RepeatBehavior.Forever;

//clock = timeline.CreateClock();

//player = new MediaPlayer();
//player.Clock = clock;

//VideoDrawing drawing = new VideoDrawing();
//drawing.Rect = new Rect(new Size(1440,900));
//drawing.Player = player;

//DrawingBrush brush = new DrawingBrush(drawing);
//this.Background = brush;
