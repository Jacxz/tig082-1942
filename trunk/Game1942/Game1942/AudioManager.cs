using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;

namespace Game1942
{
    public static class AudioManager
    {
        static Dictionary<string, SoundEffect> soundLib = new Dictionary<string,SoundEffect>();

        static ContentManager soundContent;


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
    }
}
