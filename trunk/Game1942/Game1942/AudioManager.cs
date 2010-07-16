using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace Game1942
{
    public static class AudioManager
    {
        static Dictionary<string, SoundEffect> soundLib = new Dictionary<string,SoundEffect>();

        static ContentManager soundContent;
        static Song currentlyPlaying;

        public static void Initialize(Game game)
        {
            soundContent = game.Content;
        }

        public static void Effect(string effect)
        {
            SoundEffect toPlay;
            if (soundLib.TryGetValue(effect, out toPlay))
                toPlay.Play();
        }

        public static void LoadEffect(string assetName)
        {
            soundLib.Add(assetName, soundContent.Load<SoundEffect>(assetName));
        }

        public static void PlayMusic(string assetName)
        {
            currentlyPlaying = null;
            currentlyPlaying = soundContent.Load<Song>(assetName);
            MediaPlayer.Play(currentlyPlaying);
            MediaPlayer.IsRepeating = true;
        }
    }
}
