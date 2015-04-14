using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Ascendancy.User_Controls;

namespace Ascendancy
{
    public static class VolumeManager
    {
        private static double _soundVolume = .60;
        private static double _menuMusicVolume = .60;
        private static double _battleMusicVolume = 0;
        private static bool _battleThemeIsPlaying = false;

        private static MediaPlayer battleThemePlayer;

        //justin's code
        //public static double SoundVolume
        //{
        //    get { return _soundVolume; }
        //    set
        //    {
        //        _soundVolume = value;
        //        if(OnVolumeChanged != null)
        //            OnVolumeChanged(null, new VolumeChangeEventArgs(SoundType.SoundEffect, value));
        //    }
        //}
        //public static double MusicVolume
        //{
        //    get { return _musicVolume; }
        //    set
        //    {
        //        _musicVolume = value;
        //        if (OnVolumeChanged != null)
        //            OnVolumeChanged(null, new VolumeChangeEventArgs(SoundType.Music, value));
        //    }
        //}

        public static double SoundVolume
        {
            get { return _soundVolume; }
            set
            {
                _soundVolume = value;
                if (OnVolumeChanged != null)
                    OnVolumeChanged(null, new VolumeChangeEventArgs(SoundType.SoundEffect, value));
            }
        }

        public static double MainThemeVolume
        {
            get { return _menuMusicVolume; }
            set
            {
                _menuMusicVolume = value;
                if (OnVolumeChanged != null)
                    OnVolumeChanged(null, new VolumeChangeEventArgs(SoundType.MenuMusic, value));
            }
        }

        public static double BattleThemeVolume
        {
            get { return _battleMusicVolume; }
            set
            {
                _battleMusicVolume = value;
                //if (OnVolumeChanged != null)
                //    OnVolumeChanged(null, new VolumeChangeEventArgs(SoundType.BattleMusic, value));
                if (battleThemePlayer != null)
                    battleThemePlayer.Volume = value;
            }
        }

        public static double GetMusicVolume
        {
            get
            {
                if(_battleThemeIsPlaying)
                    return _battleMusicVolume;

                return _menuMusicVolume;
            }
        }


        public static bool BattleThemeTransition
        {
            get { return _battleThemeIsPlaying; }
            set
            {
                //swap the values of the battle music and the menu music
                //should swap a zero with the current global volume

                _battleThemeIsPlaying = value;
                if (_battleThemeIsPlaying)
                {
                    //OnVolumeChanged(null, new VolumeChangeEventArgs(SoundType.BattleMusic, GetMusicVolume));

                    if (battleThemePlayer != null)
                        battleThemePlayer.Volume = GetMusicVolume;
                    OnVolumeChanged(null, new VolumeChangeEventArgs(SoundType.MenuMusic, 0));
                }
                else
                {
                    OnVolumeChanged(null, new VolumeChangeEventArgs(SoundType.MenuMusic, GetMusicVolume));
                    if(battleThemePlayer != null)
                        battleThemePlayer.Volume = 0;
                }
            }
        }

        private static EventHandler<VolumeChangeEventArgs> OnVolumeChanged = on_volume_changed;

        private static readonly ConcurrentDictionary<MediaPlayer, SoundConfiguration> currentMediaPlayers = new ConcurrentDictionary<MediaPlayer, SoundConfiguration>();

        public static void play(string uriString, SoundType type = SoundType.SoundEffect, SoundLoop loop = SoundLoop.None)
        {
            if (type == SoundType.BattleMusic)
            {
                //if it's never been created before
                if (battleThemePlayer == null)
                {
                    battleThemePlayer = new MediaPlayer();
                    battleThemePlayer.Open(new Uri(uriString, UriKind.Relative));
                    battleThemePlayer.Volume = BattleThemeVolume;
                    battleThemePlayer.MediaEnded += on_battle_player_end;
                    battleThemePlayer.Play();
                }
            }
            else
            {
                play(new Uri(uriString, UriKind.Relative), type, loop);
            }
        }

        public static void play(Uri uri, SoundType type = SoundType.SoundEffect, SoundLoop loop = SoundLoop.None)
        {
            MediaPlayer player = new MediaPlayer();
            player.Open(uri);
            play(player, type, loop);
        }

        public static void play(MediaPlayer player, SoundType type = SoundType.SoundEffect, SoundLoop loop = SoundLoop.None)
        {
            SoundConfiguration configuration = new SoundConfiguration(type, loop);
            currentMediaPlayers[player] = configuration;

            switch (type)
            {
                //todo make music transition flow correctly
                case SoundType.BattleMusic:
                    player.Volume = BattleThemeVolume;
                    break;
                case SoundType.MenuMusic:
                    player.Volume = MainThemeVolume;
                    break;
                case SoundType.SoundEffect:
                   player.Volume = SoundVolume;
                   break;
            }

            player.MediaEnded += on_player_end;
            player.Play();
        }

        private static void on_battle_player_end(object sender, EventArgs e)
        {
            MediaPlayer player = (MediaPlayer)sender;
            player.Position = TimeSpan.FromMilliseconds(1);
            player.Play();
        }



        private static void on_player_end(object sender, EventArgs e)
        {
            MediaPlayer player = (MediaPlayer) sender;
            if (!currentMediaPlayers.ContainsKey(player)) return;

            SoundConfiguration configuration = currentMediaPlayers[player];
            if (configuration.Loop == SoundLoop.Loop)
            {
                player.Position = TimeSpan.FromMilliseconds(1);
                player.Play();
            }
            else
            {
                currentMediaPlayers.TryRemove(player, out configuration);
            }
        }

        private static void on_volume_changed(object sender, VolumeChangeEventArgs e)
        {
            currentMediaPlayers
                .Where(x => x.Value.Type == e.Type)
                .Select(x => x.Key)
                .ToList()
                .ForEach(x => x.Volume = e.Volume);
        }
    }

    class VolumeChangeEventArgs : EventArgs
    {
        public SoundType Type { get; private set; }
        public double Volume { get; private set; }

        public VolumeChangeEventArgs(SoundType type, double volume)
        {
            Type = type;
            Volume = volume;
        }
    }

    class SoundConfiguration
    {
        public SoundType Type { get; private set; }
        public SoundLoop Loop { get; private set; }

        public SoundConfiguration(SoundType soundType, SoundLoop soundLoop)
        {
            Type = soundType;
            Loop = soundLoop;
        }
    }


    public enum SoundType
    {
        BattleMusic,
        MenuMusic,
        SoundEffect
    }

    public enum SoundLoop
    {
        None,
        Loop,
        Battle
    }
}
