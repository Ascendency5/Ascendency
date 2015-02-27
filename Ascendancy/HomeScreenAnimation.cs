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
    class HomeScreenAnimation
    {
        private Storyboard menuButtonStoryboard;
        private Storyboard logoStoryboard;
        private MediaPlayer menuButtonSound;
        private MediaPlayer themeSong;

        public HomeScreenAnimation()
        {
            menuButtonStoryboard = new Storyboard();
            logoStoryboard = new Storyboard();
            menuButtonSound = new MediaPlayer();
            themeSong = new MediaPlayer();
        }

        //Accessor methods

        public Storyboard MenuButtonStoryboard
        {
            get { return menuButtonStoryboard; }
            set { menuButtonStoryboard = value; }
        }

        public Storyboard LogoStoryboard 
        {
            get{return logoStoryboard;}
            set { logoStoryboard = value; }
        }

        public MediaPlayer MenuButtonSound
        {
            get { return menuButtonSound; }
            set { menuButtonSound = value; }
        }

        public MediaPlayer ThemeSong
        {
            get { return themeSong; }
            set { themeSong = value; }
        }

        //todo make the "mouse leave" event is smoother
        //Context animation methods

        public void FadeInHomeScreenButton(object sender, bool fadeIn)
        {
            menuButtonStoryboard = new Storyboard();
            DependencyObject currentElement = sender as DependencyObject;
            string item = currentElement.GetValue(FrameworkElement.NameProperty) as string;
            DoubleAnimation woosh;

            //usage: DoubleAnimation(to, from, new Duration(TimeSpan.FromMilliseconds(TRANSITION_TIME)))
            if (fadeIn)
                woosh = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromMilliseconds(200)));
            else
                woosh = new DoubleAnimation(1, 0, new Duration(TimeSpan.FromMilliseconds(200)));

            //add to the storyboard
            menuButtonStoryboard.Children.Add(woosh);
            Storyboard.SetTargetName(woosh, item);
            Storyboard.SetTargetProperty(woosh, new PropertyPath("Opacity"));

            //animate this object
            menuButtonStoryboard.Begin((FrameworkElement)sender);
        }

        public void HomeScreenLogo_MouseEnter(object sender)
        {
            var epicAnimation = new DoubleAnimationUsingKeyFrames();
            //epicAnimation.RepeatBehavior = RepeatBehavior.Forever;

            //TimeSpan(TOTAL)
            epicAnimation.Duration = new Duration(new TimeSpan(0, 0, 0, 0, 900));

            //FromTimeSpan(new TimeSpan(INTERVAL))
            epicAnimation.KeyFrames.Add(new EasingDoubleKeyFrame(0.5, 
                KeyTime.FromTimeSpan(new TimeSpan(0, 0, 0, 0, 00))));
            epicAnimation.KeyFrames.Add(new EasingDoubleKeyFrame(1, 
                KeyTime.FromTimeSpan(new TimeSpan(0, 0, 0, 0, 300))));
            epicAnimation.KeyFrames.Add(new EasingDoubleKeyFrame(0.1,
                KeyTime.FromTimeSpan(new TimeSpan(0, 0, 0, 0, 600))));
            epicAnimation.KeyFrames.Add(new EasingDoubleKeyFrame(0.5,
                KeyTime.FromTimeSpan(new TimeSpan(0, 0, 0, 0, 900))));

            Storyboard.SetTargetName(epicAnimation, "LogoGradient");
            Storyboard.SetTargetProperty(epicAnimation, new PropertyPath("Offset"));

            logoStoryboard.Children.Add(epicAnimation);

            //idk if this will work
            logoStoryboard.Begin((FrameworkElement) sender);
        }
    }
}
