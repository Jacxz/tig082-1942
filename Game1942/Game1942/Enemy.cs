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
        protected int error, mHP, mStartX, mStartY, mStartHP, mType, mEnemyWidth, mEnemyHeight, exptype;

        protected bool animationFlag = false, bossMode = false, canBeRemoved = false;
        protected int score = 5;
    
        protected Random random;
        protected SpriteBatch mSpriteBatch;
        protected SpriteFont gameFont;

        private float timePassed, lTime = 0, lTime2 = 0, moveTime = 0, Yspeed, Xspeed;
        private AnimationPlayer AnimationPlayer;
        private Animation EnemyAnimation, EnemyExplosion;
        private List<Weapon> EnemyBulletList = new List<Weapon>();
        private WeaponManager weaponManager;

        public Enemy(Game game, Texture2D theTexture, int HP, int type, float Xspe, float Yspe, int xpos, int ypos)
            : base(game)
        {
            mTexture = theTexture;
            mType = type;
            mStartHP = HP;

            mPosition = new Vector2();
            mPosition.X = xpos;
            mPosition.Y = ypos;
            Yspeed = Yspe;
            Xspeed = Xspe;
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

            weaponManager = new WeaponManager(Game, mTexture);
            weaponManager.Initialize();
        }

        /// <summary>
        /// Initialize Enemy Position and Velocity
        /// </summary>
        public void PutinStartPosition()
        {
          //  mPosition.X = random.Next(Game.Window.ClientBounds.Width - mEnemyWidth); 
           // mPosition.Y = -20;

           // Yspeed = 1 + random.Next(2);
           // Xspeed = random.Next(2) - 1;
            reset();		
		    if (mType == 10)
            {
		       mPosition.X = 350;
               moveTime = 0;
		       Yspeed = 2;
		       Xspeed = 0;
		    }
        }

        /// <summary>
        /// Allows the game component draw your content in game screen
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            mSpriteBatch.Begin();
            if (mType != 10 || mPosition.Y < 50)
		{
              for (int x = 0; x < Yspeed; x++)
              {
                mPosition.Y += 1;
                if (mHP >= 0)    
                {
                    mSpriteBatch.DrawString(gameFont, "HP: " + mHP.ToString() + " Bullet count: " + weaponManager.GetWeaponList().Count, HpPosition, Color.White);
                }           
              }
		}
		else if (mType == 10)
        {
        	moveTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
        	mPosition.X = (float)(350.0f + 300.0f * Math.Sin(moveTime / 2000.0f));
			if (mHP <= 500 && mPosition.Y < 100)
			{
				mPosition.Y += 1;
			}
        	if (mHP >= 0)
        	{
        		mSpriteBatch.DrawString(gameFont, "HP: " + mHP.ToString() + " Bullet count: " + weaponManager.GetWeaponList().Count, HpPosition, Color.White);
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
            weaponManager.Update(gameTime);

            base.Update(gameTime);
        }

        protected void AddBullets(GameTime gTime)
        {
            lTime += (float)gTime.ElapsedGameTime.TotalSeconds;
            lTime2 += (float)gTime.ElapsedGameTime.TotalSeconds;
            //time left until next shot
            if (mType == 8 && lTime > 1)
            {
                weaponManager.AddBullet(10, mPosition);
                lTime = 0;
            }
            if (mType == 10 && moveTime > 0)
            {
			    if (lTime > 0.5)
                {
                    weaponManager.AddBullet(11, mPosition);
                    lTime = 0;
                    if (lTime2 > 1 && mHP <= 500)
                    {
                        weaponManager.AddBullet(12, mPosition);
                        weaponManager.AddBullet(13, mPosition);
                        lTime2 = 0;
                    }
                }
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
        //keeps track of the animation and if it finished playing or not and if the enemy left the screen. Is set to true in DoChecks(). Used to start the Ifdead() in enemymanager.
        public bool IsDone()
        {
            return canBeRemoved;
        }

        public bool GetDead()
        {
            return animationFlag;
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
			        if ( bossMode == false)
			        {
                        PutinStartPosition();
                    }
                    timePassed = 0;
                    animationFlag = false;
                    canBeRemoved = true;
                }
            }

            // Check if the enemy still visible
            if ((mPosition.Y >= Game.Window.ClientBounds.Height) ||
                (mPosition.X >= Game.Window.ClientBounds.Width) ||
                (mPosition.X <= -EnemyAnimation.FrameWidth))
            {
                if (bossMode == false)
                {
                    PutinStartPosition();
                }
                canBeRemoved = true;
            }
        }

        public List<Weapon> GetBulletList()
        {
            return weaponManager.GetWeaponList();
        }
        public void SetBossMode()
        {
            bossMode = true;
        }
    }
}