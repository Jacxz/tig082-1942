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
    public class Enemy : Microsoft.Xna.Framework.DrawableGameComponent
    {

        protected Texture2D mTexture;
        protected Rectangle spriteRectangle;
        protected Vector2 mPosition, HpPosition;
        protected int Yspeed, Xspeed, error, HP=100, mStartX, mStartY;        
        protected Random random;
        protected SpriteBatch mSpriteBatch;
        protected SpriteFont gameFont;
        protected int mEnemyWidth, mEnemyHeight;
        private float timePassed;
        private AnimationPlayer AnimationPlayer;
        private AnimationTest EnemyAnimation, EnemyExplosion;
         

        public Enemy(Game game, Texture2D theTexture, int width, int height, int startX, int startY)
            : base(game)
        {
            mTexture = theTexture;
            mEnemyWidth = width;
            mEnemyHeight = height;
            mStartX = startX;
            mStartY = startY;
            mPosition = new Vector2();
            // Get the current spritebatch
            mSpriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));

            // creates the animationplayer
            AnimationPlayer = new AnimationPlayer(game);
            // creates the animations
            EnemyAnimation = new AnimationTest(game, theTexture, 0.1f, true, 3, mEnemyWidth, mEnemyHeight, mStartX, mStartY);
            EnemyExplosion = new AnimationTest(game, theTexture, 0.1f, false, 6, 32, 32, 70, 169);
            
            // Create the source rectangle.
            // This represents where is the sprite picture in surface
            spriteRectangle = new Rectangle(mStartX, mStartY, mEnemyWidth, mEnemyHeight); 

            // Initialize the random number generator and put the enemy in 
            // your start position
            random = new Random(this.GetHashCode());
            PutinStartPosition();
            AnimationPlayer.PlayAnimation(EnemyAnimation);
            gameFont = Game.Content.Load<SpriteFont>("font");
        }

        /// <summary>
        /// Initialize Enemy Position and Velocity
        /// </summary>
        public void PutinStartPosition()
        {
            mPosition.X = random.Next(Game.Window.ClientBounds.Width - mEnemyWidth); 
            mPosition.Y = 0;
            Yspeed = 1 + random.Next(3);
            Xspeed = random.Next(3) - 1;
            reset();
        }

        /// <summary>
        /// Allows the game component draw your content in game screen
        /// </summary>
        public override void Draw(GameTime gameTime)
        {

            mSpriteBatch.Begin();
            
            
                for (int x = 0; x < Yspeed; x++)
                {
                    mPosition.Y += 1;
                    if (HP >= 0)    
                    {
                    mSpriteBatch.DrawString(gameFont, "HP: " + HP.ToString(), HpPosition, Color.White);
                    //mSpriteBatch.Draw(mTexture, mPosition, spriteRectangle, Color.White);                
                    }           
            }
            mSpriteBatch.End();
            AnimationPlayer.Draw(gameTime, mSpriteBatch, mPosition);
            base.Draw(gameTime);
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            //Sets the position for the HP bar on top of the enemy
            HpPosition = mPosition;
            HpPosition.Y -= 10;

            DoMovment();
            DoChecks(gameTime);
            

            base.Update(gameTime);
        }

        /// <summary>
        /// Check if the Enemy intersects with the specified rectangle
        /// </summary>
        /// <param name="rect">test rectangle</param>
        /// <returns>true, if has a collision</returns>
        /// 
        public bool checkCollision(Rectangle rect) 
        {
            Rectangle spriterect = new Rectangle((int)mPosition.X, (int)mPosition.Y, mEnemyWidth, mEnemyHeight);
            return spriterect.Intersects(rect);            
        }

        public Rectangle getBounds()
        {            
            return new Rectangle((int)mPosition.X, (int)mPosition.Y, mEnemyWidth, mEnemyHeight);
        }

        public void isHit() 
        {
            HP -= 20;
        }

        private void reset()
        {
            HP = 100;
        }

        private void DoMovment()
        {
            // Move Enemy
            mPosition.X += Xspeed;
        }

        private void DoChecks(GameTime gTime)
        {
            // Check if the Enemy is dead
            if (HP <= 1)
            {
                // if dead load the explosion animation
                AnimationPlayer.PlayAnimation(EnemyExplosion);
                timePassed += (float)gTime.ElapsedGameTime.TotalSeconds;
                //resets the animation to EnemyAnimation when the explosion animation is done playing and puts it in the start position
                if (timePassed > EnemyExplosion.FrameTime * EnemyExplosion.FrameCount)
                {
                    PutinStartPosition();
                    AnimationPlayer.PlayAnimation(EnemyAnimation);
                    timePassed = 0;
                }
                
            }

            // Check if the enemy still visible
            if ((mPosition.Y >= Game.Window.ClientBounds.Height) ||
                (mPosition.X >= Game.Window.ClientBounds.Width) || (mPosition.X <= 0) || (mPosition.X >= 780))
            {
                PutinStartPosition();
            }
        }
    }
}