using System;
using System.Collections.Generic;
using System.Drawing;
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
using Ascendancy.Networking;
using Ascendancy.User_Controls;
using Ascendancy.User_Controls.Multiplayer;
using Brushes = System.Windows.Media.Brushes;
using Image = System.Windows.Controls.Image;
using Panel = System.Windows.Controls.Panel;
using Point = System.Windows.Point;

namespace Ascendancy
{
    /// <summary>
    /// Interaction logic for GameBoardUserControl.xaml
    /// </summary>
    public partial class GameBoardUserControl : UserControl
    {
        private GameEngine engine;

        private readonly Image[,] inactiveImages;
        private Image[,] activeImages;

        private Move[] validMoves;
        private static bool humanTurn;
        private static Move humanMove;
        private readonly List<Move> playedMoves;
        private bool gamePaused;

        readonly Sprite originalPossibleMoveSprite = new Sprite("possibleMoveY", 311, AnimationType.AnimateForever)
        {
            Width = 68,
            Height = 68,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top,
            Stretch = Stretch.Fill,
            Name = "possibleMove"
        };

        public GameBoardUserControl(GameEngine engine)
        {
            InitializeComponent();

            inactiveImages = new[,]
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


            activeImages = new Image[8, 8];

            humanTurn = false;
            humanMove = Move.None;
            validMoves = new Move[0];
            playedMoves = new List<Move>();

            engine.OnPlayerChanged += on_player_changed;
            engine.OnPlayerMove += on_player_moved;
            // todo add on game end to show the game ending screen and update valid moves

            engine.start();
            this.engine = engine;
            StartMusic();
        }

        private void StartMusic()
        {
            //start up the looped media
            VolumeManager.play(@"Resources/Audio/BattleTheme.wav", SoundType.Music, SoundLoop.Loop);
        }

        private void on_player_moved(object gameengine, PlayerMoveEventArgs eventargs)
        {
            BoardState state = eventargs.State;
            Move move = eventargs.Move;

            Dispatcher.Invoke(() =>
            {
                playedMoves.Add(move);

                int row = move.Row;
                int col = move.Col;

                inactiveImages[row, col].Opacity = 0;
                if (activeImages[row, col] != null)
                {
                    Sprite previousSprite = activeImages[row, col] as Sprite;
                    if (previousSprite != null)
                    {
                        previousSprite.Opacity = 0;
                        originalPossibleMoveSprite.RemoveDependent(previousSprite);
                    }
                }
                activeImages[row, col] = addPod(state[move], inactiveImages[row, col], move);

                int redScore, blackScore;
                eventargs.Board.GetScore(state, out redScore, out blackScore);

                //change the score labels
                HumanScoreLabel.Content = redScore;
                RobotScoreLabel.Content = blackScore;

                animateScoreBar(HumanScoreBar, redScore);
                animateScoreBar(RobotScoreBar, blackScore);
            });
        }

        private void on_player_changed(object gameengine, PlayerEventArgs e)
        {
            humanTurn = e.Player is HumanPlayer;
            Dispatcher.Invoke(() =>
            {
                updateValidMoves(e);

                if (e.State.CurrentPlayer == PieceType.Red)
                {
                    HumanScoreLabel.Foreground = Brushes.Red;
                    RobotScoreLabel.Foreground = Brushes.White;

                    GradRed.Color = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FF02D9FB");
                }
                else if (e.State.CurrentPlayer == PieceType.Black)
                {
                    HumanScoreLabel.Foreground = Brushes.White;
                    RobotScoreLabel.Foreground = Brushes.Red;

                    GradRed.Color = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFFB0202");
                }
            });
        }

        private void CancelIdle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // animate the cancel button from the ExitControl, then kill anim object
            //UserControlAnimation.FadeInUserControlButton(CloseHover, false);

            //ContentControlActions.FadeOut();

            engine.kill();


            ContentControlActions.setPopup(new GameCompleteUserControl(GameResult.Loss, on_game_complete_callback));
        }

        #region New Functions

        private void PodPositionListener_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!humanTurn) return;

            //Move move = findMoveFromPosition(e.GetPosition(GameBoardGrid));
            Move move = findMoveFromPosition(sender);
            if (validMove(move))
            {
                humanMove = move;
            }
        }

        private void animateScoreBar(FrameworkElement barType, int score)
        {
            //scale the scorebar
            score = score * 10 + 30;

            Storyboard animateBar = new Storyboard();
            DoubleAnimationUsingKeyFrames changeWidth = new DoubleAnimationUsingKeyFrames();

            changeWidth.KeyFrames.Add(new EasingDoubleKeyFrame(score,
                KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(500)), new SineEase()));

            animateBar.Children.Add(changeWidth);
            Storyboard.SetTarget(changeWidth, barType);
            Storyboard.SetTargetProperty(changeWidth, new PropertyPath("Width"));
            animateBar.Begin(barType);
        }

        #endregion

        private void UserControlButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // todo This doesn't seem correct, check it later
            VolumeManager.play(@"Resources/Audio/hover.wav");
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

        private void updateValidMoves(PlayerEventArgs e)
        {
            Board board = e.Board;
            BoardState state = e.State;
            Player player = e.Player;

            // Reset all current valid moves excluding the just played move
            foreach (Move move in validMoves.Where(move => !playedMoves.Contains(move)))
            {
                PlayableGameBoardGridEventListener.Children.Remove(activeImages[move.Row, move.Col]);

                inactiveImages[move.Row, move.Col].Opacity = 1;
                activeImages[move.Row, move.Col] = null;
            }

            if (player is HumanPlayer)
            {
                validMoves = board.GetPossibleMoves(state);
            }
            else
            {
                validMoves = new Move[0];
            }

            foreach (Move move in validMoves)
            {
                Sprite duplicate = originalPossibleMoveSprite.Duplicate();
                duplicate.Margin = inactiveImages[move.Row, move.Col].Margin;
                inactiveImages[move.Row, move.Col].Opacity = 0;

                activeImages[move.Row, move.Col] = duplicate;
                PlayableGameBoardGridEventListener.Children.Add(duplicate);
                Panel.SetZIndex(duplicate, 2);
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

        //finalize this event listener format
        // todo Make this function more robust
        private static Move findMoveFromPosition(object sender)
        {
            //var row = ((int) point.Y - 10) / 113;
            //var col = ((int) point.X - 290) / 113;

            DependencyObject position = sender as DependencyObject;
            string name = position.GetValue(FrameworkElement.NameProperty) as string;

            int row = Convert.ToInt32(name.ToString().Substring(1, 1));
            int col = Convert.ToInt32(name.ToString().Substring(2, 1));

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

        //credit goes to infiniteLoop, the creators of Locomotion
        private Sprite addPod(PieceType playerMoved, Image flyToImage, Move move)
        {
            string podType;
            Storyboard dropPod;
            string sound;

            //determines the relative size of the sprite depending on row position
            int wid;
            int hit;

            if (move.Row > 6)
            {
                wid = 125;
                hit = 125;
            }
            else if (move.Row > 4)
            {
                wid = 110;
                hit = 110;
            }
            else if (move.Row > 2)
            {
                wid = 98;
                hit = 98;
            }
            else
            {
                wid = 80;
                hit = 80;
            }


            if (playerMoved == PieceType.Red)
            {
                podType = "dropPodHuman";
                dropPod = FindResource("DropHumanPod") as Storyboard;
                sound = "Resources/Audio/HumanPodDown.wav";
            }
            else
            {
                podType = "robotTest1";
                dropPod = FindResource("DropRobotPod") as Storyboard;
                sound = "Resources/Audio/RobotPodDown.wav";
            }

            // Sprite resource name and width
            Sprite podImage = new Sprite(podType, 311)
            {
                Width = wid,
                Height = hit,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Stretch = Stretch.Fill,
                Name = podType
            };

            //attempt to resolve the location of the piece
            Vector offset = VisualTreeHelper.GetOffset(flyToImage);

            const double sizeIsOffBecauseThePiecesWereDesignedHastily = 40;

            double left = offset.X + 15 - sizeIsOffBecauseThePiecesWereDesignedHastily;          //getPegLeft(row, col) - 6 + (col - row);
            double top = offset.Y - sizeIsOffBecauseThePiecesWereDesignedHastily;           //getPegTop(row, col) + 109 + (row + col);

            podImage.Margin = new Thickness(left, top, 0, 0);
            //podImage.Visibility = Visibility.Hidden;

            PlayableGameBoardGridEventListener.Children.Add(podImage);
            Panel.SetZIndex(podImage, 3);

            if (podType.Equals("dropPodHuman"))
            {
                podImage.BeginStoryboard(dropPod);
                MoveTo(podImage, offset.X, offset.Y);
            }

            VolumeManager.play(sound);

            return podImage;
        }



        private void InGameMenuPopUpButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Canvas sss = (Canvas)sender;
            Storyboard localStoryboard = App.Current.FindResource("ButtonUpStoryboard") as Storyboard;
            Storyboard.SetTarget(localStoryboard, sss.Children[1]);
            localStoryboard.Begin();

            gamePaused = true;
            ContentControlActions.setPopup(new InGameMenuUserControl(on_menu_callback));
        }

        private void InGameMenuPopUpButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Canvas sss = (Canvas)sender;
            Storyboard localStoryboard = App.Current.FindResource("ButtonDownStoryboard") as Storyboard;
            Storyboard.SetTarget(localStoryboard, sss.Children[1]);
            localStoryboard.Begin();
        }

        private void on_menu_callback(object sender, MenuOptionEventArgs eventargs)
        {
            switch (eventargs.Option)
            {
                case MenuOption.Resume:
                    ContentControlActions.FadeOut();
                    gamePaused = false;
                    break;
                case MenuOption.Restart:
                    break;
                case MenuOption.Exit:
                    // Fade out the popup
                    ContentControlActions.FadeOut();
                    // Fade out the user control
                    ContentControlActions.FadeOut();
                    break;
            }
        }

        private void on_game_complete_callback(object sender, GameCompleteMenuOptionEventArgs eventArgs)
        {
            switch (eventArgs.Option)
            {
                case GameCompleteMenuOption.NewGame:
                    ContentControlActions.FadeOut();
                    gamePaused = false;
                    //todo: figure out which type of game user was playing and start a new one
                    break;
                case GameCompleteMenuOption.Restart:
                    ContentControlActions.FadeOut();

                    //for testing purposes
                    //todo: put this in correct spot
                    ContentControlActions.setPopup(new NetworkGameDisconnectUserControl(new KulamiPeer(), on_network_game_disconnect_callback));

                    //todo: put restart functionality here, plus transition music/sounds
                    break;
                case GameCompleteMenuOption.MainMenu:
                    engine.kill();
                    // Fade out the popup
                    ContentControlActions.FadeOut();
                    // Fade out the user control
                    ContentControlActions.FadeOut();

                    //todo: gracefully fade out the music/sound (ie don't let the fanfare play to completion)
                    break;
            }
        }

        private void on_network_game_disconnect_callback(object sender, NetworkGameDisconnectMenuOptionEventArgs eventArgs)
        {
            switch (eventArgs.Option)
            {
                case NetworkGameDisconnectMenuOption.BackToLobby:
                    engine.kill();
                    //fade out the pop up
                    ContentControlActions.FadeOut();
                    //fade out the user control
                    ContentControlActions.FadeOut();
                    gamePaused = false;

                    //todo: there is probably a much better way to do this
                    ContentControlActions.setPopup(new MultiplayerStarterUserControl());
                    ContentControlActions.setPopup(new OnlineNamePromptUserControl());
                    ContentControlActions.setPopup(new OnlineLobbyUserControl());
                    break;

                case NetworkGameDisconnectMenuOption.MainMenu:
                    engine.kill();
                    // Fade out the popup
                    ContentControlActions.FadeOut();
                    // Fade out the user control
                    ContentControlActions.FadeOut();

                    //todo: gracefully fade out the music/sound
                    break;
            }
        }

        private void InGameMenuPopUpButton_MouseEnter(object sender, MouseEventArgs e)
        {
            Canvas animateThisCanvas = (Canvas)sender;
            UserControlAnimation.FadeInUserControlButton(animateThisCanvas.Children[0], true);
            //added sound effect for the button
            VolumeManager.play(@"Resources/Audio/UserControlButtonHover.wav");
        }

        private void InGameMenuPopUpButton_MouseLeave(object sender, MouseEventArgs e)
        {
            //todo get mouse down working with this
            Canvas animateThisCanvas = (Canvas)sender;
            UserControlAnimation.FadeInUserControlButton(animateThisCanvas.Children[0], false);
        }

        private void HelpIdle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //HomeScreenContentControl.Content = new User_Controls.HelpUserControl(this, HomeScreenContentControl);
        }

        private void SoundIdle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
