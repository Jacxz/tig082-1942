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
        public Vector2 mPosition, mMovement, initPos;

        protected SpriteBatch mSpriteBatch;
        private int dmg;

        private float xBoundary, xSpeed;
        private bool xMovement = true;

        // constructor for weapons with only Y-axis movement
        public Weapon(Game game, ref Texture2D theTexture, Vector2 newPosition,
            Vector2 texturePos, int width, int height, float y, int dmg)
            : base(game)
        {
            mTexture = theTexture;
            mPosition = newPosition;

            mSpriteRectangle = new Rectangle((int)texturePos.X, (int)texturePos.Y, width, height);
            mMovement = new Vector2(0, y);
            this.dmg = dmg;

            mSpriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
        }

        // constructor for weapons with basic movements
        public Weapon(Game game, ref Texture2D theTexture, Vector2 newPosition,
            Vector2 texturePos, int width, int height, Vector2 movement, int dmg)
            : base(game)
        {
            mTexture = theTexture;
            mPosition = newPosition;
            initPos = mPosition;

            mSpriteRectangle = new Rectangle((int)texturePos.X, (int)texturePos.Y, width, height);
            mMovement = movement;
            this.dmg = dmg;

            xSpeed = 1;
            xBoundary = 300;

            mSpriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
        }

        // constructor for weapons with sinus movement in X-axis
        public Weapon(Game game, ref Texture2D theTexture, Vector2 newPosition,
            Vector2 texturePos, int width, int height, Vector2 movement, int dmg,
            double sinValue, float xBoundary)
            : base(game)
        {
            mTexture = theTexture;
            mPosition = newPosition;
            initPos = mPosition;
            this.xBoundary = xBoundary;

            mSpriteRectangle = new Rectangle((int)texturePos.X, (int)texturePos.Y, width, height);
            mMovement = movement;
            this.dmg = dmg;

            xSpeed = (float)Math.Sin(sinValue);
            
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
            if (xMovement)
            {
                mPosition.X += mMovement.X * xSpeed;
                if (mPosition.X > initPos.X + xBoundary || mPosition.X < initPos.X - xBoundary)
                    xMovement = false;
            }
            else if (!xMovement)
            {
                mPosition.X -= mMovement.X * xSpeed;
                if (mPosition.X > initPos.X + xBoundary || mPosition.X < initPos.X - xBoundary)
                    xMovement = true;
            }

            mPosition.Y += mMovement.Y;
            base.Update(gameTime);
        }

        public Rectangle GetBounds()
        {
            return new Rectangle((int)mPosition.X, (int)mPosition.Y, mSpriteRectangle.Width, mSpriteRectangle.Height);
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