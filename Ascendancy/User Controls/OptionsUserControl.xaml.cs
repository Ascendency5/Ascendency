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

        public OptionsUserControl()
        {
            InitializeComponent();

            updateMusicSlider(VolumeManager.MusicVolume);
            updateSoundSlider(VolumeManager.SoundVolume);
        }

        private void OptionsUserControlView_Loaded(object sender, RoutedEventArgs e)
        {
            //updateSoundSlider(VolumeManager.SoundVolume);
            //updateMusicSlider(VolumeManager.MusicVolume);
        }

        private void updateSoundSlider(double volume)
        {
            SoundSlider.Value = volume;
            SoundPercentageLabel.Content = volume*100 + "%";
        }

        private void updateMusicSlider(double volume)
        {
            MusicSlider.Value = volume;
            MusicPercentageLabel.Content = volume*100 + "%";
        }

        private void OkayIdle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //animate the cancel button from the ExitControl, then kill anim object
            UserControlAnimation.FadeInUserControlButton(OkayHover, true);

            ContentControlActions.FadeOut();
        }

        private void SoundSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            updateSoundSlider(SoundSlider.Value);
            VolumeManager.SoundVolume = SoundSlider.Value;
        }

        private void MusicSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            updateMusicSlider(MusicSlider.Value);
            VolumeManager.MusicVolume = MusicSlider.Value;
        }


        private void UserControlButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            UserControlAnimation.FadeInUserControlButton(OkayHover, false);

            //added sound effect for the button
            VolumeManager.play(@"Resources/Audio/UserControlButtonHover.wav");
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
