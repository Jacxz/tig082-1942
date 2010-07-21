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
        public Vector2 mPosition;
        protected int speed = 4;
        private int mKind;

        protected SpriteBatch mSpriteBatch;

        protected const int BULLETWIDTH = 32;
        protected const int BULLETHEIGHT = 32;

        public Weapon(Game game, ref Texture2D theTexture, Vector2 newPosition, int kindOf)
            : base(game)
        {
            mTexture = theTexture;
            mPosition = newPosition + new Vector2(0, -16.0f);
            mKind = kindOf;
            if (mKind == 1)
            {
                mSpriteRectangle = new Rectangle(37, 169, BULLETWIDTH, BULLETHEIGHT);
            }
            else if (mKind == 2)
            {
                mSpriteRectangle = new Rectangle(4, 169, BULLETWIDTH, BULLETHEIGHT);
            }
            else if (mKind == 3)
            {
                mSpriteRectangle = new Rectangle(37, 169, BULLETWIDTH, BULLETHEIGHT);
            }
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
            if (mKind == 1)
            {
                mPosition.Y -= speed;
                mPosition.X -= speed / 2;
            }
            else if (mKind == 2)
            { 
                mPosition.Y -= speed;
            }
            else if (mKind == 3)
            {
                mPosition.Y -= speed;
                mPosition.X += speed / 2;
            }
            base.Update(gameTime);
        }
        public Rectangle GetBounds()
        {
            return new Rectangle((int)mPosition.X, (int)mPosition.Y, BULLETWIDTH, BULLETHEIGHT);
        }
    }
}