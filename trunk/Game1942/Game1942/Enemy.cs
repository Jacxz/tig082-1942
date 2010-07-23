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
        protected int Yspeed, Xspeed, error, mHP, mStartX, mStartY, mStartHP, mType, mEnemyWidth, mEnemyHeight, exptype;

        protected bool animationFlag = false;
        protected int score = 5;
    
        protected Random random;
        protected SpriteBatch mSpriteBatch;
        protected SpriteFont gameFont;
       
        private float timePassed, lTime = 0;
        private AnimationPlayer AnimationPlayer;
        private Animation EnemyAnimation, EnemyExplosion;
        private List<Weapon> EnemyBulletList = new List<Weapon>();
         

        public Enemy(Game game, Texture2D theTexture, int HP, int type)
            : base(game)
        {
            mTexture = theTexture;
            mType = type;
            mStartHP = HP;
            mPosition = new Vector2();
            // Get the current spritebatch
            mSpriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));

            // creates the animationplayer
            AnimationPlayer = new AnimationPlayer(game);            
            // creates the animations
            EnemyAnimation = new Animation(game, theTexture, mType);
            EnemyExplosion = new Animation(game, theTexture, EnemyAnimation.Dietype);
            
            // Create the source rectangle.
            // This represents where is the sprite picture in surface
            spriteRectangle = new Rectangle(mStartX, mStartY, mEnemyWidth, mEnemyHeight); 

            // Initialize the random number generator and put the enemy in the start position
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
            mPosition.Y = -20;
            Yspeed = 1 + random.Next(2);
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
                if (mHP >= 0)    
                {
                    mSpriteBatch.DrawString(gameFont, "HP: " + mHP.ToString(), HpPosition, Color.White);
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

            AddBullets(gameTime);

            base.Update(gameTime);
        }

        protected void AddBullets(GameTime gTime)
        {
            lTime += (float)gTime.ElapsedGameTime.TotalSeconds;
            //time left until next shot
            if (mType == 8 && lTime > 1)
            {
                EnemyBulletList.Add(new Weapon(Game, ref mTexture, new Vector2(mPosition.X + 16, mPosition.Y + 64), new Vector2(37, 202), new Vector2(0, 4), 50));
                Game.Components.Add(EnemyBulletList[EnemyBulletList.Count - 1]);
                lTime = 0;
            }
        }

        /// <summary>
        /// Check if the Enemy intersects with the specified rectangle
        /// </summary>
        /// <param name="rect">test rectangle</param>
        /// <returns>true, if has a collision</returns>
        /// 
        public bool checkCollision(Rectangle rect) 
        {
            return getBounds().Intersects(rect);            
        }

        public Rectangle getBounds()
        {            
            return new Rectangle((int)mPosition.X, (int)mPosition.Y, EnemyAnimation.FrameWidth, EnemyAnimation.FrameHeight);
        }

        public void isHit(int dmg) 
        {
            mHP -= dmg;
        }

        private void reset()
        {
            mHP = mStartHP;
            AnimationPlayer.PlayAnimation(EnemyAnimation);
        }

        private void DoMovment()
        {
            // Move Enemy
            mPosition.X += Xspeed;
        }

        public int Score
        {
            get { return score; }
        }

        public int IsDead()
        {
            if ((mHP <= 1) && !animationFlag)
            {
                return 5 ;
            }
            else return 0;
        }

        private void DoChecks(GameTime gTime)
        {
            // Check if the Enemy is dead
            if (mHP <= 1)
            {
                // if dead load the explosion animation
                animationFlag = true;
                AnimationPlayer.PlayAnimation(EnemyExplosion);
                timePassed += (float)gTime.ElapsedGameTime.TotalSeconds;
                //resets the animation to EnemyAnimation when the explosion animation is done playing and puts it in the start position
                if (timePassed > EnemyExplosion.FrameTime * EnemyExplosion.FrameCount)
                {
                    PutinStartPosition();
                    
                    timePassed = 0;
                    animationFlag = false;
                }
            }

            // Check if the enemy still visible
            if ((mPosition.Y >= Game.Window.ClientBounds.Height) ||
                (mPosition.X >= Game.Window.ClientBounds.Width) ||
                (mPosition.X <= -EnemyAnimation.FrameWidth))//|| (mPosition.X >= 780))
            {
                PutinStartPosition();
            }
        }

        public void SetBulletList(List<Weapon> bulletList)
        {
            EnemyBulletList = bulletList;
        }

        public List<Weapon> GetBulletList()
        {
            return EnemyBulletList;
        }
        public void killBullet(int i)
        {
            EnemyBulletList[i].mPosition.Y = -100;
        }
    }
}