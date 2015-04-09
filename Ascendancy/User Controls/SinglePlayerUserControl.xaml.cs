using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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
using Ascendancy.Game_Engine;
using Panel = System.Windows.Forms.Panel;

namespace Ascendancy.User_Controls
{
    /// <summary>
    /// Interaction logic for SinglePlayerUserControl.xaml
    /// </summary>
    public partial class SinglePlayerUserControl : UserControl
    {
        private bool gameModeIsEasy;
        private bool playerGoesFirst;

        public SinglePlayerUserControl()
        {
            InitializeComponent();

            //set the default difficulty and playerGoesFirst/second state
            gameModeIsEasy = true;
            playerGoesFirst = true;
        }

        private void startGame()
        {
            ContentControlActions.FadeOut();

            // Set up game engine
            // todo Add code for gameModeIsEasy/hard AI
            HumanPlayer humanPlayer = new HumanPlayer(GameBoardUserControl.human_move_handler);
            AIPlayer aiPlayer = new AIPlayer();

            Board board = BoardSetup.board_team5;

            GameEngine engine = new GameEngine(board, humanPlayer, aiPlayer,
                playerGoesFirst ? PieceType.Red : PieceType.Black
                );

            ContentControlActions.setUpControl(new GameBoardUserControl(engine));
        }

        private void Play_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            startGame();
        }

        private void Cancel_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ContentControlActions.FadeOut();
        }

        private void Easy_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            gameModeIsEasy = true;
            Easy.Selected = true;
            Hard.Selected = false;
        }

        private void Hard_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            gameModeIsEasy = false;
            Easy.Selected = false;
            Hard.Selected = true;
        }

        private void GoFirst_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            playerGoesFirst = true;
            GoFirst.Selected = true;
            GoSecond.Selected = false;
        }

        private void GoSecond_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            playerGoesFirst = false;
            GoFirst.Selected = false;
            GoSecond.Selected = true;
        }
    }
}
