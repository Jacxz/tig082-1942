using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using Game1942.Core;


namespace Game1942
{
    struct highscoreObject
    {
        public string PlayerName;
        public int PlayerScore;

        public highscoreObject(string playerName, int playerScore)
        {
            this.PlayerName = playerName;
            this.PlayerScore = playerScore;
        }
    }
}
