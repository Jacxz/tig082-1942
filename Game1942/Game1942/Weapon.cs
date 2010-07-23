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
    public class Weapon : Microsoft.Xna.Framework.DrawableGameComponent
    {
        protected Texture2D mTexture;
        protected Rectangle mSpriteRectangle;
        public Vector2 mPosition, mMovement;

        protected SpriteBatch mSpriteBatch;
        private int dmg;

        protected const int BULLETWIDTH = 32;
        protected const int BULLETHEIGHT = 32;

        public Weapon(Game game, ref Texture2D theTexture, Vector2 newPosition, Vector2 texturePos, Vector2 movement, int dmg)
            : base(game)
        {
            mTexture = theTexture;
            mPosition = newPosition + new Vector2(0, -22.0f);

            mSpriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));

            mSpriteRectangle = new Rectangle((int)texturePos.X, (int)texturePos.Y, BULLETWIDTH, BULLETHEIGHT);
            mMovement = movement;
            this.dmg = dmg;
            
            mSpriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Draw(GameTime gameTime)
        {
            mSpriteBatch.Begin();
            mSpriteBatch.Draw(mTexture, mPosition, mSpriteRectangle, Color.White);
            mSpriteBatch.End();
            base.Draw(gameTime);
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            mPosition.X += mMovement.X;
            mPosition.Y += mMovement.Y;
            base.Update(gameTime);
        }

        public Rectangle GetBounds()
        {
            return new Rectangle((int)mPosition.X, (int)mPosition.Y, BULLETWIDTH, BULLETHEIGHT);
        }

        public bool checkCollision(Rectangle rect)
        {
            return GetBounds().Intersects(rect);
        }

        public int GetDmg()
        {
            return dmg;
        }
    }
}