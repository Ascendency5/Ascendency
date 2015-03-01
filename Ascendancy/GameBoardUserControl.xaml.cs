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
using Ascendancy.Game_Engine;
using Panel = System.Windows.Controls.Panel;

namespace Ascendancy
{
    /// <summary>
    /// Interaction logic for GameBoardUserControl.xaml
    /// </summary>
    public partial class GameBoardUserControl : UserControl
    {
        private GameEngine engine;

        private Image[,] images;
        private ImageSource blackDot;
        private ImageSource redDot;
        private Sprite currentSprite;
        private Image currentImageInPlay;

        private Move[] validMoves;
        private static bool humanTurn;
        private static Move humanMove;

        public GameBoardUserControl(GameEngine engine)
        {
            InitializeComponent();

            images = new[,]
            {
                {p00, p01, p02, p03, p04, p05, p06, p07},
                {p10, p11, p12, p13, p14, p15, p16, p17},
                {p20, p21, p22, p23, p24, p25, p26, p27},
                {p30, p31, p32, p33, p34, p35, p36, p37},
                {p40, p41, p42, p43, p44, p45, p46, p47},
                {p50, p51, p52, p53, p54, p55, p56, p57},
                {p60, p61, p62, p63, p64, p65, p66, p67},
                {p70, p71, p72, p73, p74, p75, p76, p77}
            };

            blackDot = new BitmapImage(new Uri(@"/Resources/images/greenDot.png", UriKind.Relative));
            redDot = new BitmapImage(new Uri(@"/Resources/images/redDot.png", UriKind.Relative));

            humanTurn = false;
            humanMove = Move.None;

            engine.OnPlayerChanged += on_player_changed;
            engine.OnPlayerMove += on_player_moved;

            engine.start();
        }

        private void on_player_moved(object gameengine, PlayerMoveEventArgs eventargs)
        {
            BoardState state = eventargs.State;
            Move move = eventargs.Move;

            Dispatcher.Invoke(() =>
            {
                int row = move.Row;
                int col = move.Col;

                images[row, col].Opacity = 1;
                if (state[move] == PieceType.Red)
                {
                    images[row, col].Source = redDot;
                    images[row, col].Opacity = 0;
                    currentSprite = addPod("droptest1", images[row, col], row, col);
                }
                else if (state[move] == PieceType.Black)
                {
                    images[row, col].Source = blackDot;
                    images[row, col].Opacity = 0;
                    currentSprite = addPod("droptestRobot", images[row, col], row, col);
                }
            });
        }

        private void on_player_changed(object gameengine, PlayerEventArgs e)
        {
            validMoves = e.Board.GetPossibleMoves(e.State);
            humanTurn = e.Player is HumanPlayer;
        }

        private void CancelIdle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // animate the cancel button from the ExitControl, then kill anim object
            UserControlAnimation.FadeInUserControlButton(CancelHover, false);
            
            ContentControlActionsWrapper.FadeOut();

            //engine.kill(); Yes, this does indeed kill the engine.
        }

        private void UserControlButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MediaPlayer player = new MediaPlayer();
            player.Open(new Uri(@"/Resources/Audio/hover.wav", UriKind.Relative));
            player.Volume = 0.97;
            player.Play();
        }

        private void UserControlButton_MouseEnter(object sender, MouseEventArgs e)
        {
            UserControlAnimation.FadeInUserControlButton(sender, true);
        }

        private void UserControlButton_MouseLeave(object sender, MouseEventArgs e)
        {
            UserControlAnimation.FadeInUserControlButton(sender, false);
        }

        private void PlayableGameBoardGridEventListener_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!humanTurn) return;

            Move move = findMoveFromPosition(e.GetPosition(GameBoardGrid));
            if (validMove(move))
            {
                humanMove = move;
            }
        }

        private bool validMove(Move move)
        {
            return validMoves.Any(validMove => validMove == move);
        }

        private void PlayableGameBoardGridEventListener_MouseEnter(object sender, MouseEventArgs e)
        {
        }

        private void PlayableGameBoardGridEventListener_MouseLeave(object sender, MouseEventArgs e)
        {
        }

        private static Move findMoveFromPosition(Point point)
        {
            var row = ((int) point.Y - 10) / 113;
            var col = ((int) point.X - 290) / 113;

            return new Move(row, col);
        }

        public static Move human_move_handler(object humanplayer, GameBoardEventArgs args)
        {
            while (humanMove == Move.None)
                Thread.Yield();
            Move returnMove = humanMove;

            humanMove = Move.None;
            humanTurn = false;

            return returnMove;
        }



        /******************************* Sprite Handler *********************************/

        #region Animations

        private static void MoveTo(Image target, double newX, double newY)
        {
            //var top = Canvas.GetTop(target);
            //var left = Canvas.GetLeft(target);
            TranslateTransform trans = new TranslateTransform();
            target.RenderTransform = trans;
            //DoubleAnimation anim1 = new DoubleAnimation(top, newY - top, TimeSpan.FromSeconds(10));
            //DoubleAnimation anim2 = new DoubleAnimation(left, newX - left, TimeSpan.FromSeconds(10));
            //DoubleAnimation anim1 = new DoubleAnimation(0 - newY,0, TimeSpan.FromMilliseconds(300));
            //DoubleAnimation anim2 = new DoubleAnimation(0 - newX,0, TimeSpan.FromMilliseconds(300));

            //start it from the top of the screen
            DoubleAnimation anim1 = new DoubleAnimation(0 - newY, 0, TimeSpan.FromMilliseconds(300));
            DoubleAnimation anim2 = new DoubleAnimation(0 - newX - 1000, 0, TimeSpan.FromMilliseconds(300));
            trans.BeginAnimation(TranslateTransform.XProperty, anim1);
            trans.BeginAnimation(TranslateTransform.YProperty, anim2);

            //add in a little recoil

        }

        private void animationTest(Image flyToImage)
        {
            //ImageSource giffy = new BitmapImage(new Uri(@"/Resources/images/GameBoard/podTest.gif", UriKind.Relative));
            //humanPods[humanMoveCount].Source = giffy;

            Vector offset = VisualTreeHelper.GetOffset(flyToImage);
            MoveTo(flyToImage, offset.Y, offset.X);

            //MoveTo(humanPods[humanMoveCount], offset.Y, offset.X);
            //humanMoveCount++;
        }

        //credit goes to infiniteLoop, the creators of Locomotion
        private Sprite addPod(string podType, Image flyToImage, int row, int col)
        {
            Sprite img;

            img = new Sprite(podType, 311, 310, 75, 29);

            // Physical attributes
            img.Width = 145;
            img.Height = 145;
            img.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            img.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            img.Stretch = Stretch.Fill;

            // Positional attributes
            //img.RenderTransform = new RotateTransform(-45);
            //img.Name = "pod" + row + col;
            //img.Uid = "pod" + row + col;

            img.Name = "podX";
            img.Uid = "podX";

            //attempt to resolve the location of the piece
            Vector offset = VisualTreeHelper.GetOffset(flyToImage);
            MoveTo(img, offset.Y, offset.X);

            double sizeIsOffBecauseThePiecesWereDesignedHastily = 50;
            double idk = 15;

            double left = offset.X + idk - sizeIsOffBecauseThePiecesWereDesignedHastily;          //getPegLeft(row, col) - 6 + (col - row);
            double top = offset.Y - sizeIsOffBecauseThePiecesWereDesignedHastily;           //getPegTop(row, col) + 109 + (row + col);

            img.Margin = new Thickness(left, top, 0, 0);
            img.Visibility = System.Windows.Visibility.Hidden;
            //img.Opacity = 0;

            Storyboard bounceDisk = FindResource("DropPod") as Storyboard;

            DoubleAnimation temp = (DoubleAnimation)bounceDisk.Children[2];

            temp.From = -150;
            temp.To = 0;

            //bounceDisk.Children[2] = temp;

            //Storyboard.SetTarget(bounceDisk.Children[0], img);  //visibility
            //Storyboard.SetTarget(bounceDisk.Children[1], img);  //opacity
            //Storyboard.SetTarget(bounceDisk.Children[2], img);  //bounciness

            PlayableGameBoardGridEventListener.Children.Add(img);
            //System.Windows.Controls.Panel.SetZIndex(img, 2 * (row + col) + 1);
            System.Windows.Controls.Panel.SetZIndex(img, 3);


            img.BeginStoryboard(bounceDisk);

            //attempt to make pigs fly
            MoveTo(img, offset.Y, offset.X);

            return img;
        }

        private void DropPod_Storyboard_Completed(object sender, EventArgs e)
        {
            //send it to the back
            currentSprite.StopAnimation();

            #region very roundabout way of doing currentSprite.StopAnimation();

            /*********** I guess I solved it with one line of code *********/

            //Panel.SetZIndex(currentSprite,3);
            //Image img = new Image();

            //// Physical attributes
            //currentImageInPlay.Width = 180;
            //img.Height = 112;
            //img.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            //img.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            //img.Stretch = Stretch.Fill;
            //img.Name = "podX";
            //img.Uid = "podX";
            //GridFullOfPods.Children.Add(img);

            ////attempt to resolve the location of the piece
            //Vector offset = VisualTreeHelper.GetOffset((Image)currentSprite);

            //double left = offset.X;          //getPegLeft(row, col) - 6 + (col - row);
            //double top = offset.Y;           //getPegTop(row, col) + 109 + (row + col);

            //img.Margin = new Thickness(left, top, 0, 0);
            //img.Visibility = System.Windows.Visibility.Visible;

            //Logger.Content = "CompletedCalled";
            ////foreach (FrameworkElement element in GridFullOfPods.Children)
            ////{
            ////    //if (uie.Uid != null && uie.Uid.Length > 2 && uie.Uid.Substring(0, 2) == "po")
            ////    if (element.Uid != null && element.Uid.Length > 2 && element.Uid.Substring(0, 2) == "po")
            ////        Panel.SetZIndex(element, 1);
            ////    element.Opacity = 0;
            ////}

            ////ATTEMPT TO FADE OUT THE SPRITE UPON COMPLETION
            //Storyboard buttonStoryboard = new Storyboard();
            //DoubleAnimation woosh;

            ////usage: DoubleAnimation(to, from, new Duration(TimeSpan.FromMilliseconds(TRANSITION_TIME)))
            //woosh = new DoubleAnimation(1, 0, new Duration(TimeSpan.FromMilliseconds(400)));

            ////add to the storyboard
            //buttonStoryboard.Children.Add(woosh);
            //Storyboard.SetTarget(woosh,currentSprite);
            //Storyboard.SetTargetProperty(woosh, new PropertyPath("Opacity"));

            ////animate this object
            //buttonStoryboard.Begin((FrameworkElement)currentSprite);

            ////set the image of the current image in action
            //ImageSource changeSource = new BitmapImage(new Uri(@"/Resources/images/GameBoard/podFinish.png", UriKind.Relative));

            //currentImageInPlay.Source = changeSource;
            //currentImageInPlay.Opacity = 1;
            //Panel.SetZIndex(currentImageInPlay, 3);

            #endregion

        }
        #endregion

        /**************************** End Sprite Handler ********************************/
        /********************************************************************************/
    }
}
