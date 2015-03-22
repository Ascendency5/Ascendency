using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace Ascendancy
{
    public class Sprite : System.Windows.Controls.Image
    {
        private readonly int numSprites;
        private int currentFrame;
        private readonly Timer animationTimer = new Timer();
        private readonly CroppedBitmap[] croppedImages;
        private readonly AnimationType animationType;

        private List<Sprite> dependents;
        private Sprite original;

        public Sprite(string name, int spriteWidth, AnimationType type)
            : this(name, spriteWidth, AnimationSpeed.Normal, type) { }

        public Sprite(string name, int spriteWidth, AnimationSpeed speed = AnimationSpeed.Normal, AnimationType type = AnimationType.AnimateOnce)
        {
            BitmapImage spriteSheet =
                new BitmapImage(UriFromImageName(name)) { BaseUri = BaseUriHelper.GetBaseUri(this) };

            numSprites = (int)((spriteSheet.Width / spriteWidth) - 1);
            int spriteHeight = (int)spriteSheet.Height;

            croppedImages = new CroppedBitmap[numSprites];

            for (int i = 0; i < numSprites; i++)
            {
                Int32Rect cropWindow = new Int32Rect(spriteWidth * i, 0, spriteWidth, spriteHeight);
                croppedImages[i] = new CroppedBitmap(spriteSheet, cropWindow);
            }

            animationType = type;

            this.Source = croppedImages[0];

            animationTimer.Interval = (int)speed;
            animationTimer.Tick += animationTimer_Tick;

            animationTimer.Start();
        }

        public Sprite(Sprite sprite)
        {
            numSprites = sprite.numSprites;
            croppedImages = sprite.croppedImages;
            animationType = sprite.animationType;

            Width = sprite.Width;
            Height = sprite.Height;
            HorizontalAlignment = sprite.HorizontalAlignment;
            VerticalAlignment = sprite.VerticalAlignment;
            Stretch = sprite.Stretch;
            Name = sprite.Name;

            if (sprite.original == null)
            {
                // The sprite we're copying from is the original
                if (sprite.dependents == null)
                {
                    sprite.dependents = new List<Sprite>();
                }
                sprite.dependents.Add(this);
                original = sprite;
            }
            else
            {
                original = sprite.original;
                original.dependents.Add(this);
            }
        }

        public Sprite Duplicate()
        {
            return new Sprite(this);
        }

        public void RemoveDependent(Sprite sprite)
        {
            dependents.Remove(sprite);
        }

        private static Uri UriFromImageName(string name)
        {
            return new Uri(@"/Ascendancy;component/Resources/Images/Sprites/" + name + @".png", UriKind.Relative);
        }

        void animationTimer_Tick(object sender, EventArgs e)
        {
            currentFrame++;
            if (currentFrame >= numSprites)
            {
                if (animationType != AnimationType.AnimateForever)
                {
                    StopAnimation();
                    if (animationType == AnimationType.AnimateOnceThenHide)
                    {
                        // todo apply this to all dependents, but see if it works first
                        this.Visibility = Visibility.Hidden;
                    }
                    return;
                }
                currentFrame = 0;
            }

            this.Source = croppedImages[currentFrame];

            if (dependents == null) return;
            foreach (Sprite dependent in dependents)
            {
                dependent.Source = dependent.croppedImages[currentFrame];
            }
        }

        public void StopAnimation()
        {
            if (original != null)
            {
                original.StopAnimation();
            }
            else
            {
                animationTimer.Stop();
            }
        }

        public void ChangeAnimationSpeed(int speed)
        {
            if (original != null)
            {
                original.ChangeAnimationSpeed(speed);
            }
            else
            {
                animationTimer.Interval = speed;
            }
        }

        public void ResetAnimation()
        {
            if (original != null)
            {
                original.ResetAnimation();
            }
            else
            {
                currentFrame = 0;
                animationTimer.Start();
            }
        }
    }

    public enum AnimationType
    {
        AnimateOnce,
        AnimateOnceThenHide,
        AnimateForever
    }

    public enum AnimationSpeed
    {
        Normal = 29
    }
}
