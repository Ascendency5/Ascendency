using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Ascendancy
{
    public static class UserControlAnimation
    {
        //todo delete the old FadeIUserControlButton method after all other buttons have been replaced
        //all user control objects animate their buttons with this method
        public static void FadeInUserControlButton(object sender, bool fadeIn)
        {
            Storyboard buttonStoryboard = new Storyboard();
            DoubleAnimation changeButtonOpacity;
            DependencyObject currentButton = sender as DependencyObject;
            if (currentButton == null) return;

            if (fadeIn)
                changeButtonOpacity = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromMilliseconds(130)));
            else
                changeButtonOpacity = new DoubleAnimation(1, 0, new Duration(TimeSpan.FromMilliseconds(130)));

            //add to the storyboard
            buttonStoryboard.Children.Add(changeButtonOpacity);
            Storyboard.SetTarget(changeButtonOpacity, currentButton);
            Storyboard.SetTargetProperty(changeButtonOpacity, new PropertyPath("Opacity"));

            //animate this object
            buttonStoryboard.Begin((FrameworkElement)sender);
        }

        //all user control "views" fade in and out the same way
        private static void FadeContentControl(FrameworkElement currentContentControl, bool fadeIn)
        {
            Storyboard userControlStoryboard = new Storyboard();
            DoubleAnimation changeContentOpacity;

            Duration transitionDuration = new Duration(TimeSpan.FromMilliseconds(400));
            if (fadeIn)
                changeContentOpacity = new DoubleAnimation(0, 1, transitionDuration);
            else
                changeContentOpacity = new DoubleAnimation(1, 0, transitionDuration);

            //add to the storyboard
            userControlStoryboard.Children.Add(changeContentOpacity);
            Storyboard.SetTarget(changeContentOpacity, currentContentControl);
            Storyboard.SetTargetProperty(changeContentOpacity, new PropertyPath("Opacity"));

            //animate this object
            userControlStoryboard.Begin(currentContentControl);
        }

        public static void FadeInContentControl(ContentControl currentContentControl)
        {
            FadeContentControl(currentContentControl, true);
        }

        public static void FadeOutContentControl(ContentControl currentContentControl)
        {
            FadeContentControl(currentContentControl, false);
        }

        //Context animation methods

        //todo check code robustness with Justin

        public static void StartButtonGradientSpin(Grid sender)
        {
            foreach (Canvas childCanvas in sender.Children)
            {
                foreach (FrameworkElement element in childCanvas.Children)
                {
                    //if it is a path type element
                    if (element is Path)
                    {
                        //make a storyboard to animate it
                        Storyboard localStoryboard = new Storyboard();
                        localStoryboard = App.Current.FindResource("ButtonHoverStoryboard") as Storyboard;

                        //target e, then animate e
                        Storyboard.SetTarget(localStoryboard, element);
                        localStoryboard.Begin();
                    }
                }
            }
        }
    }
}
