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
    public class Animation : Microsoft.Xna.Framework.DrawableGameComponent
    {
        SpriteBatch mSpriteBatch;

        public float mRotation, mScale, mDepth;
        
        private Vector2 mSpritePos;
        private Texture2D mTexture;
        private float mTimePerFrame, mTotalElapsedTime, mElapsed;
        private int mFrameCount, mFrame=1, mLookDirection, mKind, mXpos, mYpos, mJump;
        SpriteFont font;
        public float error, error2;

        private bool Paused;

        public Animation(Game game, Texture2D theTexture, int FrameCount, float TimePerFrame, int X, int Y, int jump)
            : base(game)
        {
            mTexture = theTexture;
            mXpos = X;
            mYpos = Y;
            mJump = jump;
           // mRotation = rot;
           // mScale = sca;
            //mDepth = dep;
            mFrameCount = FrameCount;
            mTimePerFrame = TimePerFrame;
            //mKind = kindOf;
            mSpriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));

            font = Game.Content.Load<SpriteFont>("font"); // ska denna ligga här?
        }


        public void UpdateFrame(float elapsed)
        {
            if (IsPaused)
            {
                // return;
                mTotalElapsedTime += elapsed;
                if (mTotalElapsedTime > mTimePerFrame)
                {
                    mFrame++;
                    mTotalElapsedTime -= mTimePerFrame;
                    error++;
                }
                if (mFrame >= mFrameCount)
                {
                    
                    Pause();
                    Reset();
                    
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {

            UpdateFrame((float)gameTime.ElapsedGameTime.TotalSeconds);
            mSpriteBatch.Begin();
            //mSpriteBatch.DrawString(font, "Animation Error: " +error+ "\nAnimation Time: " + mTotalElapsedTime + "\nmElapsed: " + mElapsed + "\nmFrame:" + mFrame + "\nFramCount: " + mFrameCount, new Vector2(30, 120), Color.White);
                Rectangle lSource = new Rectangle(mXpos + (mJump * mFrame), mYpos, 32, 32);
                if (Paused)
                {
                    mSpriteBatch.Draw(mTexture, mSpritePos, lSource, Color.White);
                }
            mSpriteBatch.End();
            
            base.Draw(gameTime);
        }
        /*
        public int getWidth()
        {
            return mTexture.Width / 14;
        }

        public int getHeight()
        {
            return mTexture.Height / 4;
        }
        */
      
        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {

           
            base.Update(gameTime);
        }

        public bool IsPaused
        {
            get { return Paused; }
        }
        public void Reset()
        {
            mFrame = 0;
            mTotalElapsedTime = 0f;
        }
        public void Stop()
        {
            Pause();
            Reset();
        }
        public void Play()
        {
            Paused = true;
        }
        public void Pause()
        {
            Paused = false;
        }

        public Vector2 SpritePos
        {
            get { return mSpritePos; }
            set { mSpritePos = value; }
        }
        public float getError()
        {
            return error;
        }
    }
}