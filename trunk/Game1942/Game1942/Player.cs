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
        protected Rectangle mLivesRectangle;
        protected Vector2 mPosition;
        protected Vector2 mLivesPosition;
        protected SpriteBatch mSpriteBatch;

        protected const int SHIPWIDTH = 32;
        protected const int SHIPHEIGHT = 32;

        protected Rectangle mScreenBounds;
      
        private Animation playerAnimation, explosionAnimation;
        private AnimationPlayer playerAnimationPlayer;
        private bool killed;
        SpriteFont font;
        private int lives, HP, score, currentWeapon, currentMissiles, startLives, maxHP;
        private float lTime, playerSpeed;

        public Player(Game game, ref Texture2D theTexture)
            : base(game)
        {
            mTexture = theTexture;
            mPosition = new Vector2();
            mLivesRectangle = new Rectangle(169, 268, SHIPWIDTH, SHIPHEIGHT);

            playerAnimation = new Animation(game, theTexture, 7);
            explosionAnimation = new Animation(game, theTexture, 6);
           
            playerAnimationPlayer = new AnimationPlayer(game);
            mSpriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
            
            playerAnimationPlayer.PlayAnimation(playerAnimation);

            //sets starting stats for player
            startLives = 1;
            lives = startLives;
            maxHP = 100;
            HP = maxHP;
            score = 0;
            playerSpeed = 2;
            currentWeapon = 1;
            currentMissiles = 0;
           
            mScreenBounds = new Rectangle(0, 0, Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height);

            font = Game.Content.Load<SpriteFont>("font");
        }
        
        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            // sets the position where lives should start to draw from
            mLivesPosition.X = mScreenBounds.Width - SHIPWIDTH*lives;
            // Move the ship with keyboard
            KeyboardState keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(Keys.Up))
            {
                mPosition.Y -= playerSpeed;
            }
            if (keyboard.IsKeyDown(Keys.Down))
            {
                mPosition.Y += playerSpeed;
            }
            if (keyboard.IsKeyDown(Keys.Left))
            {
                mPosition.X -= playerSpeed;
            }
            if (keyboard.IsKeyDown(Keys.Right))
            {
                mPosition.X += playerSpeed;
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

        public override void Draw(GameTime gameTime)
        {
            mSpriteBatch.Begin();
            for (int x = 0; x < lives; x++)
            {
                // draw lives
                mSpriteBatch.Draw(mTexture, mLivesPosition, mLivesRectangle, Color.White);
                mLivesPosition.X += 32;
            }
            //draws hp for player above the player sprite
            for (int x = 0; x < playerSpeed; x++)
            {                
                if (HP >= 0)
                {
                    mSpriteBatch.DrawString(font, "HP: " + HP.ToString(), new Vector2(mPosition.X, mPosition.Y + 32), Color.White);
                }
            }             
            mSpriteBatch.End();
            //plays the player animation
            playerAnimationPlayer.Draw(gameTime, mSpriteBatch, getPosition());
            base.Draw(gameTime);
        }
        // the bounds of the ship
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
        // removes hp from the player and plays sound and animation if the player is dead.
        public void IsHit(int dmg)
        {
            HP -= dmg;
            if (HP <= 0)
            {
                Killed = true;
                AudioManager.Effect("implosion");
                playerAnimationPlayer.PlayAnimation(explosionAnimation);
            }
        }
        // 
        public void IsKilled(GameTime gTime)
        {

            lTime+=(float)gTime.ElapsedGameTime.TotalSeconds;
            if (lTime > explosionAnimation.FrameTime*explosionAnimation.FrameCount)
            {
                lives -= 1;
                Killed = false;
                PutInStartPosition();
                playerAnimationPlayer.PlayAnimation(playerAnimation);
                lTime = 0;
            }
            
            HP = 100;  
        }

        public int GetScore()
        {
            return score;
        }

        public void SetScore(int score)
        {
            this.score += score;
        }

        public int GetLives()
        {
            return lives;
        }

        public void AddLives(int addLives)
        {
            lives += addLives;
        }

        public void AddSpeed(float addSpeed)
        {
            playerSpeed += addSpeed;
        }

        public void FillHealth()
        {
            HP = maxHP;
        }

        public void IncreaseHP(int increase)
        {
            maxHP += increase;
            HP += increase;
        }

        public void ResetLives()
        {
            lives = startLives;
        }

        public int GetCurrentWeapon()
        {
            return currentWeapon;
        }

        public int GetCurrentMissiles()
        {
            return currentMissiles;
        }

        public void SetCurrentWeapon(int weapon)
        {
            currentWeapon = weapon;
        }

        public void SetCurrentMissiles(int missile)
        {
            currentMissiles = missile;
        }

        public void UpgradeWeapon(int upgrade)
        {
            if (currentWeapon + upgrade < 6 && currentWeapon + upgrade > 0)
            {
                currentWeapon += upgrade;
            }
        }

        public void AddMissiles(int upgrade)
        {
            if (currentMissiles + upgrade < 4 && currentMissiles + upgrade > 0)
            {
                currentMissiles += upgrade;
            }
        }
    }
}