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
        //local variables
        private HomeScreenAnimation localStoryboard;
        private OptionsUserControl globalOptions;
        private static Storyboard playIntro;
        private Storyboard fadeout;
        private Storyboard playThemeSong;
        private Storyboard playHomeScreenBackground;

        public MainWindow()
        {
            InitializeComponent();

            SoundManager.MusicVolume = 50;
            SoundManager.SoundVolume = 50;

            //Nik added this in to try making a global options controller,
            //accessible from any context (even during gameplay)
            globalOptions = new OptionsUserControl();


            //boot up other local variables
            localStoryboard = new HomeScreenAnimation();

            //i'm loading the content controller upon finishing the intro animation
            ContentControlActionsWrapper.baseContentControl = HomeScreenContentControl;

            playIntro = FindResource("IntroStoryboard") as Storyboard;
            HomeScreenIntro.BeginStoryboard(playIntro);
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
            //use the global options controller
            setContent(globalOptions);
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
            base.OnClosed(e);
            Application.Current.Shutdown();
        }


        #region Intro Functions

        private void IntroStoryboard_OnCompleted(object sender, EventArgs e)
        {
            //throw new NotImplementedException();

            //stop all the things
            playIntro.Remove();
            playIntro.Stop();
            HomeScreenIntro.Stop();
            HomeScreenIntro.Volume = 0;

            //send the intro materials to the back
            Panel.SetZIndex(HomeScreenIntro, 1);
            Panel.SetZIndex(IntroSkipText, 1);
            UserControlAnimation.FadeInContentControl(HomeScreenIntro, false);


            //instantiate content controller
            ContentControlActionsWrapper.baseContentControl = HomeScreenContentControl;

            //start up the looped media
            fadeout = FindResource("FadeTransitionHandler") as Storyboard;
            TransitionHandler.BeginStoryboard(fadeout);
            playThemeSong = FindResource("HomeScreenThemeSongStoryboard") as Storyboard;
            HomeScreenThemeSong.BeginStoryboard(playThemeSong);
            playHomeScreenBackground = FindResource("HomeScreenBackgroundVideoStoryboard") as Storyboard;
            HomeScreenVideo.BeginStoryboard(playHomeScreenBackground);
        }

        private void HomeScreenIntro_MouseDown(object sender, MouseButtonEventArgs e)
        {
            IntroStoryboard_OnCompleted(sender, e);
        }

        private void HomeScreenIntro_KeyDown(object sender, KeyEventArgs e)
        {
            IntroStoryboard_OnCompleted(sender, e);
        }

        private void FadeTransitionHandler_OnCompleted(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            Panel.SetZIndex(TransitionHandler, 1);
        }

        #endregion

        /*************************** Animations ************************/
        private void HomeScreen_MouseEnter(object sender, MouseEventArgs e)
        {
            localStoryboard.FadeInHomeScreenButton(sender, true);

            //todo: do all sound effects have to interrupt each other?

            //sound effect style 1
            System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"Resources/Audio/HomeScreenButtonHover.wav");
            player.Play();
            

            //sound effect style 2
            //MediaElement m = new MediaElement();
            //HomeScreenGrid.Children.Add(m);
            //m.BeginStoryboard(FindResource("PlayHomeButtonSoundEffect") as Storyboard);
        }

        private void HomeScreen_MouseLeave(object sender, MouseEventArgs e)
        {
            localStoryboard.FadeInHomeScreenButton(sender, false);
            localStoryboard = new HomeScreenAnimation();
        }



        private void MenuLogo_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            localStoryboard.HomeScreenLogo_MouseEnter(sender);
            System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"Resources/Audio/LogoHover.wav");
            player.Play();
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
