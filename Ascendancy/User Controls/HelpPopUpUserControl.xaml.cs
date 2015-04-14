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

namespace Ascendancy.User_Control
{
    /// <summary>
    /// Interaction logic for OptionsUserControl.xaml
    /// </summary>
    public partial class HelpPopUpUserControl : UserControl
    {
        private FrameworkElement[] screenShotText;
        private FrameworkElement currentScreenShot;
        private int currentSlideIndex;

        public HelpPopUpUserControl()
        {
            InitializeComponent();
            Storyboard helpVideoStoryboard = FindResource("HelpPage1Storyboard") as Storyboard;
            helpVideoStoryboard.Begin();

            //keep track of the screen shot texts
            screenShotText = new []
            {
                HelpLabel1,HelpLabel2,HelpLabel3,
                HelpLabel4,HelpLabel5,HelpLabel6
            };

            //make previous button inaccessable
            PreviousSlideButton.Visibility = Visibility.Hidden;

            currentSlideIndex = 1;
            screenShotText[currentSlideIndex - 1].Opacity = 1;
        }

        private void closeHelpIdle_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ContentControlActions.FadeOut();
        }

        private void previousIdle_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //hide previous if we're scrolling to the first image
            if (currentSlideIndex == 2)
            {
                PreviousSlideButton.Visibility = Visibility.Hidden;
            }
            //if we're scrolling *from* the last image, show the "next" button
            else if (currentSlideIndex == 6)
            {
                NextSlideButton.Visibility = Visibility.Visible;
            }

            //move the slide index down and go back one slide
            //animate that stuff!
            UserControlAnimation.FadeInElement(screenShotText[currentSlideIndex - 1], false);
            currentSlideIndex--;
            UserControlAnimation.FadeInElement(screenShotText[currentSlideIndex - 1], true);
            string filename = "HelpPage" + currentSlideIndex + "Storyboard";

            Storyboard helpVideoStoryboard = FindResource(filename) as Storyboard;
            helpVideoStoryboard.Begin();
        }

        private void nextIdle_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (currentSlideIndex == 1)
            {
                PreviousSlideButton.Visibility = Visibility.Visible;
            }

            else if (currentSlideIndex == 5)
            {
                NextSlideButton.Visibility = Visibility.Hidden;
            }

            //move the slide index up and go forward one slide
            //animate that stuff!
            UserControlAnimation.FadeInElement(screenShotText[currentSlideIndex - 1], false);
            currentSlideIndex++;
            UserControlAnimation.FadeInElement(screenShotText[currentSlideIndex - 1], true);
            string filename = "HelpPage" + currentSlideIndex + "Storyboard";

            Storyboard helpVideoStoryboard = FindResource(filename) as Storyboard;
            helpVideoStoryboard.Begin();
        }
    }
}


