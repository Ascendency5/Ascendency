using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Ascendancy
{
    public static class UserControlAnimation
    {
        /**
        private Storyboard buttonStoryboard;
        private Storyboard userControlStoryboard;
        private MediaPlayer menuButtonSound;
        private MediaPlayer themeSong;
         */

        //Context animation methods

        //all user control objects animate their buttons with this method
        public static void FadeInUserControlButton(object sender, bool fadeIn)
        {
            Storyboard buttonStoryboard = new Storyboard();
            DoubleAnimation changeButtonOpacity;
            DependencyObject currentControl = sender as DependencyObject;
            string item = currentControl.GetValue(FrameworkElement.NameProperty) as string;

            //usage: DoubleAnimation(to, from, new Duration(TimeSpan.FromMilliseconds(TRANSITION_TIME)))
            if (fadeIn)
                changeButtonOpacity = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromMilliseconds(150)));
            else
                changeButtonOpacity = new DoubleAnimation(1, 0, new Duration(TimeSpan.FromMilliseconds(170)));

            //add to the storyboard
            buttonStoryboard.Children.Add(changeButtonOpacity);
            Storyboard.SetTargetName(changeButtonOpacity, item);
            Storyboard.SetTargetProperty(changeButtonOpacity, new PropertyPath("Opacity"));

            //animate this object
            buttonStoryboard.Begin((FrameworkElement)sender);
        }

        //all user control "views" fade in and out the same way
        public static void FadeInContentControl(object sender, bool easeIn)
        {
            Storyboard userControlStoryboard = new Storyboard();
            DoubleAnimation changeContentOpacity;
            DependencyObject currentContentControl = sender as DependencyObject;
            string item = currentContentControl.GetValue(FrameworkElement.NameProperty) as string;

            //usage: DoubleAnimation(to, from, new Duration(TimeSpan.FromMilliseconds(TRANSITION_TIME)))
            if (easeIn)
                changeContentOpacity = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromMilliseconds(400)));
            else
                changeContentOpacity = new DoubleAnimation(1, 0, new Duration(TimeSpan.FromMilliseconds(400)));

            //add to the storyboard
            userControlStoryboard.Children.Add(changeContentOpacity);
            Storyboard.SetTargetName(changeContentOpacity, item);
            Storyboard.SetTargetProperty(changeContentOpacity, new PropertyPath("Opacity"));

            //animate this object
            userControlStoryboard.Begin((FrameworkElement)sender);
        }


    }
}
