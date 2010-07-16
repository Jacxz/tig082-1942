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
        public Vector2 mOrigin;
        private Vector2 mSpritePos;
        private Texture2D mTexture;
        private float mTimePerFrame, mTotalElapsedTime;
        private int mFrameCount, mFrame, mLookDirection, mKind;
        SpriteFont font;
        public int error;

        private bool Paused;

        public Animation(Game game, Texture2D theTexture, Vector2 vec, float rot, float sca, float dep, int FrameCount, float TimePerFrame, int kindOf)
            : base(game)
        {
            mTexture = theTexture;
            mOrigin = vec;
            mRotation = rot;
            mScale = sca;
            mDepth = dep;
            mFrameCount = FrameCount;
            mTimePerFrame = TimePerFrame;
            mKind = kindOf;
            mSpriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));

            font = Game.Content.Load<SpriteFont>("font"); // ska denna ligga här?
        }


        public void UpdateFrame(float elapsed)
        {
            if (Paused)
                return;
            mTotalElapsedTime += elapsed;
            if (mTotalElapsedTime > mTimePerFrame)
            {
                mFrame++;
                // Keep the Frame between 0 and the total frames, minus one.
                mFrame = mFrame % mFrameCount;
                mTotalElapsedTime -= mTimePerFrame;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            Rectangle lSourceRect = new Rectangle(64 * mFrame + mKind * 64+4, mKind * 64+4, 64, 64);
            
            mSpriteBatch.Begin();
            if (!Paused)
            {
                mSpriteBatch.DrawString(font, "Animation Error: " + error, new Vector2(45, 60), Color.White);
                // mSpriteBatch.Draw(mTexture, mSpritePos, lSourceRect, Color.White, mRotation, mOrigin, mScale, SpriteEffects.None, mDepth);
                mSpriteBatch.Draw(mTexture, new Vector2(300, 300), new Rectangle(64, 64, 128, 128), Color.White, mRotation, mOrigin, mScale, SpriteEffects.None, 0.5f);
            }
            error++;
            
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
        public int LookDirection
        {
            get { return mLookDirection; }
            set { mLookDirection = value; }
        }
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
            Paused = false;
        }
        public void Pause()
        {
            Paused = true;
        }

        public Vector2 SpritePos
        {
            get { return mSpritePos; }
            set { mSpritePos = value; }
        }
        public int getError()
        {
            return error;
        }
    }
}