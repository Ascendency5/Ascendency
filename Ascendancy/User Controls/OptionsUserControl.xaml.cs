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
            UserControlAnimation.StartButtonGradientSpin(Buttons);
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

        private void OkayButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Canvas sss = (Canvas)sender;
            Storyboard localStoryboard = App.Current.FindResource("ButtonUpStoryboard") as Storyboard;
            Storyboard.SetTarget(localStoryboard, sss.Children[1]);
            localStoryboard.Begin();

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


        private void OkayButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Canvas sss = (Canvas)sender;
            Storyboard localStoryboard = App.Current.FindResource("ButtonDownStoryboard") as Storyboard;
            Storyboard.SetTarget(localStoryboard, sss.Children[1]);
            localStoryboard.Begin();

        }

        private void OkayButton_MouseEnter(object sender, MouseEventArgs e)
        {
            Canvas animateThisCanvas = (Canvas)sender;
            UserControlAnimation.FadeInUserControlButton(animateThisCanvas.Children[0], true);
            //added sound effect for the button
            VolumeManager.play(@"Resources/Audio/UserControlButtonHover.wav");
        }

        private void OkayButton_MouseLeave(object sender, MouseEventArgs e)
        {
            //todo get mouse down working with this
            Canvas animateThisCanvas = (Canvas)sender;
            UserControlAnimation.FadeInUserControlButton(animateThisCanvas.Children[0], false);
        }
    }
}
