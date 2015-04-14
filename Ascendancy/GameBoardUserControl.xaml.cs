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
        private bool difficultyIsHard = true;

        private Image[,] inactiveImages;
        private Image[,] activeImages;

        private Move[] validMoves;
        private static bool humanTurn;
        private static Move humanMove;
        private List<Move> playedMoves;

        readonly Sprite originalPossibleMoveSprite = new Sprite("PossibleMoveSprite", 311, AnimationType.AnimateForever)
        {
            Width = 68,
            Height = 68,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top,
            Stretch = Stretch.Fill,
            Name = "PossibleMoveSprite"
        };

        public GameBoardUserControl(Board board, Player playerOne, Player playerTwo, PieceType firstType)
        {
            InitializeComponent();

            HumanPlayerName.Content = playerOne.GetName();
            RobotPlayerName.Content = playerTwo.GetName();

            engine = new GameEngine(board, playerOne, playerTwo, firstType);

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
            engine.OnGameEnd += on_game_end;
            // todo add on game end to show the game ending screen and update valid moves

            setUpPlayerCustom(playerOne);
            setUpPlayerCustom(playerTwo);
            difficultyIsHard = !(playerTwo is EasyAiPlayer);

            engine.start();

            //StartMusic();

            //this gets the board configuration and shows the proper mask on the gameboard
            setGameboardConfig(BoardSetup.FromBoard(board));
        }

        private void setUpPlayerCustom(Player player)
        {
            var networkPlayer = player as NetworkPlayer;
            if (networkPlayer != null)
            {
                engine.OnPlayerMove += networkPlayer.on_player_move;
            }
        }

        private void on_game_end(object sender, GameBoardEventArgs e)
        {
            // animate the cancel button from the ExitControl, then kill anim object
            //ContentControlActions.FadeOut();
            //get the proper variables to find out who won
            BoardState state = e.State;
            int redScore, blackScore;
            e.Board.GetScore(state, out redScore, out blackScore);
            GameResult finalGameState;

            Dispatcher.Invoke(() =>
            {
                //change the score labels
                HumanScoreLabel.Content = redScore;
                RobotScoreLabel.Content = blackScore;
            });

            //set the final state
            if (redScore == blackScore)
                finalGameState = GameResult.Tie;
            else if (redScore < blackScore)
                finalGameState = GameResult.Loss;
            else
                finalGameState = GameResult.Win;

            ContentControlActions.setPopup(new GameCompleteUserControl(finalGameState, on_game_complete_callback));

            //VolumeManager.BattleThemeTransition = false;
            engine.kill();
        }

        //private void StartMusic()
        //{
        //    //start up the looped media
        //    VolumeManager.play(@"Resources/Audio/BattleTheme.wav", SoundType.BattleMusic, SoundLoop.Loop);
        //    VolumeManager.BattleThemeTransition = true;
        //}

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
                HumanPodsLabel.Content = state.RedMovesLeft;
                RobotPodsLabel.Content = state.BlackMovesLeft;

                HumanScoreLabel.Content = redScore;
                RobotScoreLabel.Content = blackScore;
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
            score = score * 7 + 30;

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

        private void updateValidMoves(PlayerEventArgs e)
        {
            Board board = e.Board;
            BoardState state = e.State;
            Player player = e.Player;

            // Reset all current valid moves excluding the just played move
            foreach (Move move in validMoves.Where(move => !playedMoves.Contains(move)))
            {
                PlayableGameBoardGridEventListener.Children.Remove(activeImages[move.Row, move.Col]);

                inactiveImages[move.Row, move.Col].Opacity = .2;
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

        //finalize this event listener format
        // todo Make this function more robust
        private static Move findMoveFromPosition(object sender)
        {
            //var row = ((int) point.Y - 10) / 113;
            //var col = ((int) point.X - 290) / 113;

            DependencyObject position = sender as DependencyObject;
            string name = position.GetValue(FrameworkElement.NameProperty) as string;

            int row = Convert.ToInt32(name.Substring(1, 1));
            int col = Convert.ToInt32(name.Substring(2, 1));

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
                if (difficultyIsHard)
                {
                    podType = "HardRobotSprite";
                    dropPod = FindResource("DropRobotPod") as Storyboard;
                    //todo change sound effect per robot type
                    sound = "Resources/Audio/RobotPodDown.wav";
                }
                else
                {
                    podType = "EasyRobotSprite";
                    dropPod = FindResource("DropRobotPod") as Storyboard;
                    sound = "Resources/Audio/RobotPodDown.wav";
                }
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

            double left = offset.X;     //getPegLeft(row, col) - 6 + (col - row);
            double top = offset.Y-20;           //getPegTop(row, col) + 109 + (row + col);

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


        #region InGameMenu

        private void on_menu_callback(object sender, MenuOptionEventArgs eventargs)
        {
            switch (eventargs.Option)
            {
                case MenuOption.Resume:
                    ContentControlActions.FadeOut();
                    break;
                case MenuOption.Restart:
                    //ContentControlActions.setPopup(new ChatboxUserControl());
                    break;
                case MenuOption.Exit:
                    // Fade out the popup
                    ContentControlActions.FadeOut();
                    //// Fade out the user control
                    ContentControlActions.FadeOut();

                    engine.kill();

                    break;
            }
        }

        private void on_game_complete_callback(object sender, GameCompleteMenuOptionEventArgs eventArgs)
        {
            switch (eventArgs.Option)
            {
                case GameCompleteMenuOption.NewGame:
                    ContentControlActions.FadeOut();
                    //todo: figure out which type of game user was playing and start a new one
                    break;
                //case GameCompleteMenuOption.Restart:
                //    ContentControlActions.FadeOut();

                //    //for testing purposes
                //    //todo: put this in correct spot
                //    ContentControlActions.setPopup(new NetworkGameDisconnectUserControl(new KulamiPeer(), on_network_game_disconnect_callback));

                //    //todo: put restart functionality here, plus transition music/sounds
                //    break;
                case GameCompleteMenuOption.MainMenu:
                    Dispatcher.Invoke(() =>
                    {
                        engine.kill();
                    });
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

        #endregion

        private void Menu_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ContentControlActions.setPopup(new InGameMenuUserControl(on_menu_callback));
        }

        //sets what gameboard is being played for the current game
        private void setGameboardConfig(int boardConfig)
        {
            string filename = @"/Resources/Images/GameBoard/Team" + boardConfig + ".png";

            //set the gameboard as a mask to the main scenery
            GameConfigMask.Source = new BitmapImage(
                new Uri(filename, UriKind.Relative));
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ContentControlActions.setPopup(new GameCompleteUserControl(GameResult.Win, on_game_complete_callback));
        }

        private void Position_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!humanTurn) return;
            Move move = findMoveFromPosition(sender);
            if (validMove(move))
            {
                FrameworkElement element = sender as FrameworkElement;
                element.Opacity = 1;
            }
        }
        private void Position_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!humanTurn) return;
            Move move = findMoveFromPosition(sender);
            if (validMove(move))
            {
                FrameworkElement element = sender as FrameworkElement;
                element.Opacity = 0;
            }
        }
    }
}
