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
        public Vector2 mPosition, mMovement, initPos, mTargetPosition, mTargetSpeed;

        protected SpriteBatch mSpriteBatch;
        private AnimationPlayer AnimationPlayer;
        private Animation WeaponAnimation;
        private GameTime gTime;
        private int dmg, movementType; // 0=only y axis, 1=normal movement, 2=sinus movement in X-axis, 3=seeking missile

        private float xBoundary, xSpeed, mBaseSpeed, chaseTime = 0;
        private bool xMovement = true;

        // constructor for weapons with only Y-axis movement
        public Weapon(Game game, ref Texture2D theTexture, Vector2 newPosition,
            Vector2 texturePos, int width, int height, float y, int dmg)
            : base(game)
        {
            mTexture = theTexture;
            mPosition = newPosition;
            movementType = 0;

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
            movementType = 1;

            mSpriteRectangle = new Rectangle((int)texturePos.X, (int)texturePos.Y, width, height);
            mMovement = movement;
            this.dmg = dmg;

            xSpeed = 1;
            xBoundary = 1000;

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
            movementType = 2;
            this.xBoundary = xBoundary;

            mSpriteRectangle = new Rectangle((int)texturePos.X, (int)texturePos.Y, width, height);
            mMovement = movement;
            this.dmg = dmg;

            xSpeed = (float)Math.Sin(sinValue);
            
            mSpriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
        }

        // constructor for seeking missiles
        public Weapon(Game game, ref Texture2D theTexture, Vector2 newPosition,
            Vector2 texturePos, int animationType, int width, int height, float y, int dmg,
            Vector2 targetPosition, Vector2 targetSpeed, GameTime gTime)
            : base(game)
        {
            mTexture = theTexture;
            mPosition = newPosition;
            mTargetPosition = targetPosition;
            mTargetSpeed = targetSpeed;
            movementType = 3;
            this.gTime = gTime;

            // creates the animationplayer
            AnimationPlayer = new AnimationPlayer(game);
            // creates the animations
            WeaponAnimation = new Animation(game, theTexture, animationType);
            AnimationPlayer.PlayAnimation(WeaponAnimation);

            mSpriteRectangle = new Rectangle((int)texturePos.X, (int)texturePos.Y, width, height);
            mBaseSpeed = y;
            mMovement = new Vector2(0, -y);
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
            if (movementType != 3)
            {
                mSpriteBatch.Begin();
                mSpriteBatch.Draw(mTexture, mPosition, mSpriteRectangle, Color.White);
                mSpriteBatch.End();
            }
            else
            {
                AnimationPlayer.Draw(gameTime, mSpriteBatch, mPosition);
            }
            base.Draw(gameTime);
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            switch (movementType)
            {
                case 0:
                    mPosition.Y += mMovement.Y;
                    break;
                case 1:
                    mPosition.Y += mMovement.Y;
                    mPosition.X += mMovement.X;
                    break;
                case 2:
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
                    break;
                case 3:
                    mTargetPosition += mTargetSpeed;
                    chaseTime += (float)gTime.ElapsedGameTime.TotalSeconds;
                    if (chaseTime < 1)
                    {
                        mMovement.X += 0.8f * ((mTargetPosition.X - mPosition.X) / (float)Math.Sqrt((mTargetPosition.X - mPosition.X) *
                            (mTargetPosition.X - mPosition.X) + (mTargetPosition.Y - mPosition.Y) * (mTargetPosition.Y - mPosition.Y)));
                        mMovement.Y += 0.8f * ((mTargetPosition.Y - mPosition.Y) / (float)Math.Sqrt((mTargetPosition.X - mPosition.X) *
                            (mTargetPosition.X - mPosition.X) + (mTargetPosition.Y - mPosition.Y) * (mTargetPosition.Y - mPosition.Y)));
                        mMovement.X = mMovement.X * mBaseSpeed / (float)Math.Sqrt((mMovement.X * mMovement.X) + (mMovement.Y * mMovement.Y));
                        mMovement.Y = mMovement.Y * mBaseSpeed / (float)Math.Sqrt((mMovement.X * mMovement.X) + (mMovement.Y * mMovement.Y));
                        if (mMovement.X > 1)
                        {
                            WeaponAnimation.SetRotation((float)Math.Atan(mMovement.Y / mMovement.X) + 1.5f);
                        }
                        else
                        {
                            WeaponAnimation.SetRotation((float)Math.Atan(mMovement.Y / mMovement.X) + 4.7f);
                        }
                    }
                    mPosition.X += mMovement.X;
                    mPosition.Y += mMovement.Y;
                    break;
            }
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