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

namespace Ascendancy
{
    public class ImageButton : Control
    {
        private readonly Path glowPath;
        private readonly Path selectedPath;
        private readonly Image image;

        public ImageButton()
        {
            this.DefaultStyleKey = typeof (ImageButton);

            this.MouseEnter += on_mouse_enter;
            this.MouseLeave += on_mouse_leave;
            this.MouseLeftButtonDown += on_mouse_down;
            this.MouseLeftButtonUp += on_mouse_up;

            this.ApplyTemplate();

            image = (Image)GetTemplateChild("Image");
            glowPath = (Path) GetTemplateChild("GlowPath");
            selectedPath = (Path) GetTemplateChild("SelectedPath");

            // todo Figure out what's wrong with uncommeting these lines
            // Something about a 'read-only' state
            //applyAnimation(glowPath);
            //applyAnimation(selectedPath);
        }

        private void applyAnimation(Path path)
        {
            Storyboard gradientStoryboard = (Storyboard) Application.Current.FindResource("ButtonHoverStoryboard");

            Storyboard.SetTarget(gradientStoryboard, path);
            gradientStoryboard.Begin();
        }

        #region Dependency properties

        public bool Selected
        {
            get { return (bool)GetValue(SelectedProperty); }
            set { SetValue(SelectedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Selected.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedProperty =
            DependencyProperty.Register("Selected", typeof (bool), typeof (ImageButton),
                new PropertyMetadata(false));

        public bool Enabled
        {
            get { return (bool)GetValue(EnabledProperty); }
            set { SetValue(EnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Enabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EnabledProperty =
            DependencyProperty.Register("Enabled", typeof(bool), typeof(ImageButton),
            new PropertyMetadata(true));

        public ImageSource Source
        {
            get { return (ImageSource)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Source.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(ImageSource), typeof(ImageButton), new PropertyMetadata(null));

        #endregion

        private void on_mouse_up(object sender, MouseButtonEventArgs e)
        {
            Storyboard localStoryboard = FindResource("ButtonUpStoryboard") as Storyboard;
            Storyboard.SetTarget(localStoryboard, image);
            localStoryboard.Begin();
        }

        private void on_mouse_down(object sender, MouseButtonEventArgs e)
        {
            Storyboard localStoryboard = FindResource("ButtonDownStoryboard") as Storyboard;
            Storyboard.SetTarget(localStoryboard, image);
            localStoryboard.Begin();
        }

        private void on_mouse_leave(object sender, MouseEventArgs e)
        {
            if (!Enabled) return;

            UserControlAnimation.FadeInElement(glowPath, false);
        }

        private void on_mouse_enter(object sender, MouseEventArgs e)
        {
            if (!Enabled || Selected) return;

            UserControlAnimation.FadeInElement(glowPath, true);

            //added sound effect for the button
            VolumeManager.play(@"Resources/Audio/UserControlButtonHover.wav");
        }
    }
}
