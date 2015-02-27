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
        private Image[,] images;
        private ImageSource blackDot;
        private ImageSource redDot;

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
                }
                else if (state[move] == PieceType.Black)
                {
                    images[row, col].Source = blackDot;
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
            //animate the cancel button from the ExitControl, then kill anim object
            UserControlAnimation.FadeInUserControlButton(CancelHover, false);
            
            ContentControlActionsWrapper.FadeOut();
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
    }
}
