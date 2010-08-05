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

namespace Game1942
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Island : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private Vector2 mPosition;
        private int mType;
        private Texture2D mTexture;
        private float elapsedTime;
        public Island(Game game, int type, float xPos, Texture2D texture)
            : base(game)
        {
            mType = type;
            mTexture = texture;
            mPosition.X = xPos;
            mPosition.Y = -50;
        }
        public bool IsDone()
        {
            return mPosition.Y > 600;
        }

        public void Draw(GameTime gameTime, SpriteBatch batch)
        {
            elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds * 100;
            mPosition.Y += elapsedTime;

            switch (mType)
            {
                case 50:
                    batch.Draw(mTexture, new Vector2(mPosition.X, mPosition.Y)
                        , new Rectangle(103, 499, 64, 64), Color.White);
                    break;
                case 51:
                    batch.Draw(mTexture, new Vector2(mPosition.X, mPosition.Y)
                        , new Rectangle(168, 499, 64, 64), Color.White);
                    break;
                case 52:
                    batch.Draw(mTexture, new Vector2(mPosition.X, mPosition.Y)
                        , new Rectangle(233, 499, 64, 64), Color.White);
                    break;
            }
            base.Draw(gameTime);
        }
    }
}