using System;
using System.Collections.Generic;
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
using System.Xml;
using System.IO;


namespace Game1942
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class IslandManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private Texture2D mTexture;
        private List<Island> islandList;
        private Island island;
        private Game game;

        public IslandManager(Game game, Texture2D texture)
            : base(game)
        {
            this.game = game;
            islandList = new List<Island>();
            mTexture = texture;
        }
        public void Draw(GameTime gameTime, SpriteBatch batch)
        {
            for (int i = 0; i < islandList.Count; i++)
            {
                islandList[i].Draw(gameTime, batch);
                if (islandList[i].IsDone())
                {
                    Game.Components.Remove(islandList.ElementAt(i));
                    islandList.RemoveAt(i);
                }
            }
        }
        public void Reset()
        {
            for (int i = 0; i < islandList.Count; i++)
            {
                Game.Components.Remove(islandList.ElementAt(i));
            }
            islandList.Clear();
        }
        public void AddIsland(int type, int xPos)
        {
            island = new Island(game, type, xPos, mTexture);
            islandList.Add(island);
            Game.Components.Add(island);
        }
    }
}

