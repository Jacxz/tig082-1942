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
    public class PowerUpManager : Microsoft.Xna.Framework.GameComponent
    {
        private Texture2D mTexture;
        private PowerUp mPowerUp;
        private List<PowerUp> mPowerUpList;


        public PowerUpManager(Game game, Texture2D texture)
            : base(game)
        {
            mPowerUpList = new List<PowerUp>();
            mTexture = texture;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }



        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
		// checks all the powerups for the CanBeRemoved flag, and removes 
            for (int x = 0; x < mPowerUpList.Count; x++)
            {
                if (mPowerUpList[x].CanBeRemoved())
                {
                    Game.Components.Remove(mPowerUpList.ElementAt(x));
                    mPowerUpList.RemoveAt(x);
                }
            }
            base.Update(gameTime);
        }

        public void AddPowerUp(int type, float xSpeed, float ySpeed, Vector2 mPosition)
        {

            // read and set the variables from the xml with the type variabel as a identifier.
            mPowerUp = new PowerUp(Game, mTexture, type, xSpeed, ySpeed, mPosition);
            mPowerUpList.Add(mPowerUp);
            Game.Components.Add(mPowerUp);
        }

        public List<PowerUp> getPowerUpList()
        {
            return mPowerUpList;
        }
    }
}

