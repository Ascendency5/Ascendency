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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Ascendancy.User_Control
{
    /// <summary>
    /// Interaction logic for OptionsUserControl.xaml
    /// </summary>
    public partial class HelpPopUpUserControl : UserControl
    {
        private Image[] screenShots;
        private Image currentScreenShot;
        private int currentSlideIndex;

        public HelpPopUpUserControl()
        {
            InitializeComponent();

            screenShots = new Image[]
            {
                Page1PodPlacement,Page2EnemyTakesPanel,Page3HighlightHoles,Page4HighlightWonPanel,
                Page5WinningScreen,Page6HeadsUpDisplayDescription
            };

            //start the "slideshow" at the first index of the screenShot array
            currentSlideIndex = 0;
            currentScreenShot = screenShots[currentSlideIndex];
            currentScreenShot.Opacity = 1;

            //make previous button inaccessable
            Panel.SetZIndex(previousIdle, 1);
            Panel.SetZIndex(previousHover, 1);
            Panel.SetZIndex(previousClick, 1);
        }


        private void closeHelpIdle_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //animate the cancel button from the ExitControl, then kill anim object
            UserControlAnimation.FadeInUserControlButton(closeHelpIdle, true);

            ContentControlActions.FadeOut();
        }


        /*********************** General Button Animations **************************/

        private void userControlButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //todo: add sound effect for button

            if (sender == closeHelpIdle)
                UserControlAnimation.FadeInUserControlButton(closeHelpHover, false);
            else if (sender == nextIdle)
                UserControlAnimation.FadeInUserControlButton(nextHover, false);
            else if (sender == previousIdle)
                UserControlAnimation.FadeInUserControlButton(previousHover, false);
            
            //added sound effect for the button
            VolumeManager.play(@"Resources/Audio/UserControlButtonHover.wav");
        }

        private void userControlButton_MouseEnter(object sender, MouseEventArgs e)
        {
            UserControlAnimation.FadeInUserControlButton(sender, false);
        }

        private void userControlButton_MouseLeave(object sender, MouseEventArgs e)
        {
            if (closeHelpHover.Opacity < 1)
                UserControlAnimation.FadeInUserControlButton(closeHelpHover, true);
            if (nextHover.Opacity < 1)
                UserControlAnimation.FadeInUserControlButton(nextHover, true);
            if (previousHover.Opacity < 1)
                UserControlAnimation.FadeInUserControlButton(previousHover, true);

            UserControlAnimation.FadeInUserControlButton(sender, true);
        }

        private void previousIdle_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //hide previous if we're scrolling to the first image
            if (currentScreenShot == screenShots[1])
            {
                Panel.SetZIndex(previousIdle, 1);
                Panel.SetZIndex(previousHover, 1);
                Panel.SetZIndex(previousClick, 1);
                UserControlAnimation.FadeInUserControlButton(previousClick, false);
                UserControlAnimation.FadeInUserControlButton(previousHover, false);
                UserControlAnimation.FadeInUserControlButton(previousIdle, false);
            }
            //if we're scrolling *from* the last image, show the "next" button
            else if (currentScreenShot == screenShots[5])
            {
                Panel.SetZIndex(nextIdle, 3);
                Panel.SetZIndex(nextHover, 3);
                Panel.SetZIndex(nextClick, 3);
                UserControlAnimation.FadeInUserControlButton(nextClick, true);
                UserControlAnimation.FadeInUserControlButton(nextClick, true);
                UserControlAnimation.FadeInUserControlButton(nextClick, true);
            }
            
            UserControlAnimation.FadeInUserControlButton(previousHover, true);

            //move the slide index down and go back one slide
            currentSlideIndex--;

            //animate that stuff!
            UserControlAnimation.FadeInUserControlButton(currentScreenShot, false);
            currentScreenShot = screenShots[currentSlideIndex];
            UserControlAnimation.FadeInUserControlButton(currentScreenShot, true);
        }

        private void nextIdle_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (currentScreenShot == screenShots[0])
            {
                Panel.SetZIndex(previousIdle, 3);
                Panel.SetZIndex(previousHover, 3);
                Panel.SetZIndex(previousClick, 3);
                UserControlAnimation.FadeInUserControlButton(previousClick, true);
                UserControlAnimation.FadeInUserControlButton(previousHover, true);
                UserControlAnimation.FadeInUserControlButton(previousIdle, true);
            }

            else if (currentScreenShot == screenShots[4])
            {
                Panel.SetZIndex(nextIdle, 1);
                Panel.SetZIndex(nextHover, 1);
                Panel.SetZIndex(nextClick, 1);
                UserControlAnimation.FadeInUserControlButton(nextClick, false);
                UserControlAnimation.FadeInUserControlButton(nextHover, false);
                UserControlAnimation.FadeInUserControlButton(nextIdle, false);
            }
            UserControlAnimation.FadeInUserControlButton(nextHover, true);


            //move the slide index up and go forward one slide
            currentSlideIndex++;

            //animate that stuff!
            UserControlAnimation.FadeInUserControlButton(currentScreenShot, false);
            currentScreenShot = screenShots[currentSlideIndex];
            UserControlAnimation.FadeInUserControlButton(currentScreenShot, true);
        }
    }
}


