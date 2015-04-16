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
using Panel = System.Windows.Controls.Panel;

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
            InsertSprites();
        }
        private void InsertSprites()
        {
            // Sprite resource name and width
            Sprite hardRobotSprite = new Sprite("HardRobotIdle", 311, AnimationType.AnimateForever)
            {
                Width = 310,
                Height = 310,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Stretch = Stretch.Fill,
                Name = "HardRobotIdle",
                Margin = new Thickness(0, 0, 0, 0)
            };

            //Easy or Hard AI
            PlayerTwoSpriteCanvas.Children.Add(hardRobotSprite);
            Panel.SetZIndex(hardRobotSprite, 3);

            VolumeManager.play(@"Resources/Audio/RobotPodDown.wav");
        }

        private void Back_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ContentControlActions.FadeOut();
        }

        private void Play_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Set up game engine
            HumanPlayer humanPlayerRed = new HumanPlayer(PlayerOneNamePrompt.Text, GameBoardUserControl.human_move_handler);
            HumanPlayer humanPlayerBlack = new HumanPlayer(PlayerTwoNamePrompt.Text, GameBoardUserControl.human_move_handler);

            Board board = BoardSetup.Random();

            GameBoardUserControl control = new GameBoardUserControl(
                board,
                humanPlayerRed,
                humanPlayerBlack,
                PieceType.Red
                );

            ContentControlActions.setBaseContentControl(control);
        }

        private void NamePromptText_Loaded(object sender, RoutedEventArgs e)
        {
            PlayerOneNamePrompt.Focus();
            PlayerOneNamePrompt.SelectAll();
        }

        private void NamePromptText_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox == null) return;

            if (textBox.Text != "Player One" && textBox.Text != "Player Two") return;

            textBox.Text = "";
        }
    }
}
