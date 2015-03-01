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

namespace Ascendancy.User_Controls
{
    /// <summary>
    /// Interaction logic for ExitConfirmationUserControl.xaml
    /// </summary>
    public partial class OptionsUserControl : UserControl
    {
        private int loadedCounterSoundSlider;
        private int loadedCounterMusicSlider;

        public OptionsUserControl()
        {
            InitializeComponent();

            loadedCounterSoundSlider = 0;
            loadedCounterMusicSlider = 0;
            SoundPercentageLabel.Content = SoundManager.SoundVolume + "%";
            MusicPercentageLabel.Content = SoundManager.MusicVolume + "%";
            SoundSlider.Value = SoundManager.SoundVolume;
            MusicSlider.Value = SoundManager.MusicVolume;
        }

        private void OptionsUserControlView_Loaded(object sender, RoutedEventArgs e)
        {
            //SoundSlider.Value = SoundManager.SoundVolume;
            //MusicSlider.Value = SoundManager.MusicVolume;
        }

        private void OkayIdle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //animate the cancel button from the ExitControl, then kill anim object
            UserControlAnimation.FadeInUserControlButton(OkayHover, true);

            ContentControlActionsWrapper.FadeOut();
        }

        private void SoundSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (loadedCounterSoundSlider <= 1)
            {
                loadedCounterSoundSlider++;
                SoundSlider.Value = 50;
            }
            else
            {
                SoundManager.SoundVolume = SoundSlider.Value;
                SoundPercentageLabel.Content = SoundManager.SoundVolume + "%";
            }
        }

        private void MusicSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (loadedCounterMusicSlider <= 1)
            {
                loadedCounterMusicSlider++;
                MusicSlider.Value = 50;
            }
            else
            {
                SoundManager.MusicVolume = MusicSlider.Value;
                MusicPercentageLabel.Content = SoundManager.MusicVolume + "%";
            }
        }


        private void UserControlButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            UserControlAnimation.FadeInUserControlButton(OkayHover, false);

            //added sound effect for the button
            System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"Resources/Audio/UserControlButtonHover.wav");
            player.Play();
        }

        private void UserControlButton_MouseEnter(object sender, MouseEventArgs e)
        {
            UserControlAnimation.FadeInUserControlButton(sender, false);
        }

        private void UserControlButton_MouseLeave(object sender, MouseEventArgs e)
        {
            if (OkayHover.Opacity < 1)
                UserControlAnimation.FadeInUserControlButton(OkayHover, true);

            UserControlAnimation.FadeInUserControlButton(sender, true);
        }
    }
}
