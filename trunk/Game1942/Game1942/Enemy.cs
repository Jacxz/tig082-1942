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
        protected Vector2 mPosition, HpPosition, mPlayerPosition;
        protected int error, mHP, mStartX, mStartY, mStartHP, mType, mEnemyWidth, mEnemyHeight, mPowerUpType;

        protected bool animationFlag = false, canBeRemoved = false, 
            hasDroppedPowerUp = false, hasCreatedPowerUp = false, levelWon = false;
        protected int score;
    
        protected Random random;
        protected SpriteBatch mSpriteBatch;
        protected SpriteFont gameFont;

        private float timePassed, weaponCycle1 = 0, weaponCycle2 = 0, weaponCycle3 = 0, moveTime = 0, Yspeed, Xspeed;
        private AnimationPlayer AnimationPlayer;
        private Animation EnemyAnimation, EnemyExplosion;
        private List<Weapon> EnemyBulletList = new List<Weapon>();
        private WeaponManager weaponManager;

        public Enemy(Game game, Texture2D theTexture, int HP, int type, float Xspe, float Yspe, int xpos, int ypos, int powerUpType, int score)
            : base(game)
        {
            this.score = score;
            mTexture = theTexture;
            mType = type;
            mStartHP = HP;
            mPowerUpType = powerUpType;

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

  
        public void PutinStartPosition()
        {

            Reset();		
		    if (mType == 10)
            {
               moveTime = 0;
		    }
        }

        /// <summary>
        /// Allows the game component draw your content in game screen
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            mSpriteBatch.Begin();
            if (mType != 10 || mPosition.Y < 30)
		{
            for (int x = 0; x < Yspeed; x++)
            {
                mPosition.Y += 1;
                if (mHP >= 0)    
                {
                mSpriteBatch.DrawString(gameFont, "HP: " + mHP.ToString() + " Bullet: " + weaponManager.GetWeaponList().Count, HpPosition, Color.White);
                }
            }
		}
		else if (mType == 10)
        {
        	moveTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
        	mPosition.X = (float)(350.0f + 300.0f * Math.Sin(moveTime / 2200.0f));
			if (mHP <= 500 && mPosition.Y < 80)
			{
				mPosition.Y += 1;
			}
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
            weaponManager.Update(gameTime);
            base.Update(gameTime);
        }

        protected void AddBullets(GameTime gTime)
        {
            weaponCycle1 += (float)gTime.ElapsedGameTime.TotalSeconds;
            weaponCycle2 += (float)gTime.ElapsedGameTime.TotalSeconds;
            weaponCycle3 += (float)gTime.ElapsedGameTime.TotalSeconds;
            //time left until next shot
            if (mType == 3 && weaponCycle1 > 1.5)
            {
                weaponManager.AddBullet(17, mPosition);
                weaponCycle1 = 0;
            }
            if (mType == 8 && weaponCycle1 > 1)
            {
                weaponManager.AddBullet(10, mPosition);
                weaponCycle1 = 0;
            }
            #region boss 1
            if (mType == 10 && moveTime > 0)
            {
                // the first mode of the boss, when it has above 500 hp
                if (mHP > 500)
                {
                    if (weaponCycle1 < 2.3 && weaponCycle2 > 0.5)
                    {
                        weaponManager.AddBullet(11, mPosition);
                        weaponCycle2 = 0;
                    }
                    if (weaponCycle1 > 2.5 && weaponCycle3 > 0.15)
                    {
                        weaponManager.AddBullet(40, mPosition, mPlayerPosition);
                        weaponCycle3 = 0;
                    }
                    // weaponCycle1, the primare cycle, is 3 seconds long
                    if (weaponCycle1 > 3)
                    {
                        weaponCycle1 = 0;
                        weaponCycle2 = 0;
                        weaponCycle3 = 0;
                    }
                }
                // the second mode of the boss, when it has below 500 hp
                if (mHP <= 500)
                {
                    if (weaponCycle1 < 2.3 && weaponCycle2 > 0.5)
                    {
                        weaponManager.AddBullet(11, mPosition);
                        weaponManager.AddBullet(12, mPosition);
                        weaponManager.AddBullet(13, mPosition);
                        weaponCycle2 = 0;
                    }
                    if (weaponCycle1 > 2.5 && weaponCycle3 > 0.08)
                    {
                        weaponManager.AddBullet(40, mPosition, mPlayerPosition);
                        weaponCycle3 = 0;
                    }
                    // weaponCycle1, the primare cycle, is 3 seconds long
                    if (weaponCycle1 > 3)
                    {
                        weaponCycle1 = 0;
                        weaponCycle2 = 0;
                        weaponCycle3 = 0;
                    }
                }
            }
            #endregion
            if (mType == 12 && weaponCycle1 > 1.5)
            {
                weaponManager.AddBullet(14, mPosition);
                weaponManager.AddBullet(15, mPosition);
                weaponManager.AddBullet(16, mPosition);
                weaponCycle1 = 0;
            }
            if (mType == 13 && weaponCycle1 > 1.5)
            {
                weaponManager.AddBullet(41, mPosition, mPlayerPosition);
                weaponCycle1 = 0;
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

        private void Reset()
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
                return Score ;
            }
            else return 0;
        }

        public bool GetIsDead()
        {
            return (mHP < 1);
        }

        public int GetPowerUpType()
        {
            return mPowerUpType;
        }

        public bool GetIfPowerUpDropped()
        {
            return hasDroppedPowerUp;
        }

        public bool GetHasCreatedPowerUp()
        {
            return hasCreatedPowerUp;
        }

        public void SetHasCreatedPowerUp()
        {
            hasCreatedPowerUp = true;
        }

        public Vector2 GetPosition()
        {
            return mPosition;
        }

        public void SetPlayerPosition(Vector2 pos)
        {
            mPlayerPosition = pos;
        }

        public bool GetLevelWon()
        {
            return levelWon;
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
                // set the hasDroppedPowerUp flag if not set, this will be read by actionScene, which creates a powerup
                if (!hasDroppedPowerUp)
                {
                    hasDroppedPowerUp = true;
                }
                //resets the animation to EnemyAnimation when the explosion animation is done playing and puts it in the start position
                if (timePassed > EnemyExplosion.FrameTime * EnemyExplosion.FrameCount)
                {
                    if (mType == 10)
                    {
                        levelWon = true;
                    }
                    PutinStartPosition();
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
                PutinStartPosition();
                canBeRemoved = true;
            }
        }

        public List<Weapon> GetBulletList()
        {
            return weaponManager.GetWeaponList();
        }

        public void RemoveBullets()
        {
            weaponManager.RemoveBullets();            
        }
    }
}