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
using Ascendancy.Game_Engine;

namespace Ascendancy.User_Controls.Multiplayer
{
    /// <summary>
    /// Interaction logic for LocalMultiplayerUserControl.xaml
    /// </summary>
    public partial class LocalMultiplayerUserControl : UserControl
    {
        public LocalMultiplayerUserControl()
        {
            InitializeComponent();
        }

        private void Back_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ContentControlActions.FadeOut();
        }

        private void Play_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Set up game engine
            HumanPlayer humanPlayerRed = new HumanPlayer(GameBoardUserControl.human_move_handler);
            HumanPlayer humanPlayerBlack = new HumanPlayer(GameBoardUserControl.human_move_handler);

            Board board = BoardSetup.board_team5;

            GameEngine engine = new GameEngine(board, humanPlayerRed, humanPlayerBlack, PieceType.Red);

            // todo Make this transition better
            ContentControlActions.setUpControl(new GameBoardUserControl(engine));
        }
    }
}
