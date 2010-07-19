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
    public class Player : Microsoft.Xna.Framework.DrawableGameComponent
    {
        protected Texture2D mTexture;
        protected Rectangle mSpriteRectangle, mLivesRectangle;
        protected Vector2 mPosition;
        protected Vector2 mLivesPosition;
        protected SpriteBatch mSpriteBatch;

        protected const int SHIPWIDTH = 32;
        protected const int SHIPHEIGHT = 32;

        protected Rectangle mScreenBounds;

        private Animation mExplosionAnimation;
        
        private bool killed;
        SpriteFont font;
        private int lives, HP;
        private float error,error2;

        public Player(Game game, ref Texture2D theTexture)
            : base(game)
        {
            mTexture = theTexture;
            mPosition = new Vector2();

            mExplosionAnimation = new Animation(game, mTexture, 6, 0.1f, 70, 169,33);
          
            mSpriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));

            mSpriteRectangle = new Rectangle(136, 202, SHIPWIDTH, SHIPHEIGHT);
            mLivesRectangle = new Rectangle(169, 268, SHIPWIDTH, SHIPHEIGHT);
            
            lives = 3;
            HP = 100;
           
            mScreenBounds = new Rectangle(0, 0, Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height);

            font = Game.Content.Load<SpriteFont>("font");
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

        protected override void LoadContent()
        {
            
            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            mLivesPosition.X = mScreenBounds.Width - SHIPWIDTH*lives;
          
            // Move the ship with keyboard
            
            KeyboardState keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(Keys.Up))
            {
                mPosition.Y -= 3;
            }
            if (keyboard.IsKeyDown(Keys.Down))
            {
                mPosition.Y += 3;
            }
            if (keyboard.IsKeyDown(Keys.Left))
            {
                mPosition.X -= 3;
            }
            if (keyboard.IsKeyDown(Keys.Right))
            {
                mPosition.X += 3;
            }


            // Keep the ship inside the screen
            if (mPosition.X < mScreenBounds.Left)
            {
                mPosition.X = mScreenBounds.Left;
            }
            if (mPosition.X > mScreenBounds.Width - SHIPWIDTH)
            {
                mPosition.X = mScreenBounds.Width - SHIPWIDTH;
            }
            if (mPosition.Y < mScreenBounds.Top)
            {
                mPosition.Y = mScreenBounds.Top;
            }
            if (mPosition.Y > mScreenBounds.Height - SHIPHEIGHT)
            {
                mPosition.Y = mScreenBounds.Height - SHIPHEIGHT;
            }
            if (Killed)
            {
                IsKilled(gameTime);
                
            }
         
            base.Update(gameTime);
        }

        /// <summary>
        /// Draw the ship sprite
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            mSpriteBatch.Begin();
            for (int x = 0; x < lives; x++)
            {
                // draw lives
                mSpriteBatch.Draw(mTexture, mLivesPosition, mLivesRectangle, Color.White);
                mLivesPosition.X += 32;
            }
            // Draw the ship
            mSpriteBatch.Draw(mTexture, mPosition, mSpriteRectangle, Color.White);
            mSpriteBatch.DrawString(font, "Player HP: "+HP.ToString()+"\nPlayer GameTime: "+ (error += (float)gameTime.ElapsedGameTime.TotalSeconds) , new Vector2(15,60), Color.White);
            mSpriteBatch.End();

            // Draw the explosion
            if (mExplosionAnimation.IsPaused)
            {
                mExplosionAnimation.Draw(gameTime);
            }
            base.Draw(gameTime);
            
        }

        /// <summary>
        /// Get the bound rectangle of ship position in screen
        /// </summary>
        public Rectangle GetBounds()
        {
            return new Rectangle((int)mPosition.X, (int)mPosition.Y, SHIPWIDTH, SHIPHEIGHT);
        }
        public void PutInStartPosition()
        { 
                mPosition.X = mScreenBounds.Width / 2;
                mPosition.Y = mScreenBounds.Height - SHIPHEIGHT;
        }
        
        public Vector2 getPosition()
        {
            return mPosition;
        }

        public bool Killed
        {
            get { return killed; }
            set { killed = value; }
        }

        public void IsHit()
        {
            HP -= 5;
            if (HP <= 0)
            {
                Killed = true;
            }
        }

        public void IsKilled(GameTime gTime)
        {
            lives -= 1;
            mExplosionAnimation.SpritePos = mPosition;
            mExplosionAnimation.Play();
           // error2 += (float)gTime.ElapsedGameTime.TotalSeconds;
            //if (error2 > 0.6f)
            //{
                PutInStartPosition();
                Killed = false;
              //  error2 = 0;
            //}
            HP = 100;
        }



    }
}