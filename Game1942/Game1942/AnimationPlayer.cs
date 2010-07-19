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
    public class AnimationPlayer : Microsoft.Xna.Framework.DrawableGameComponent
    {

        private AnimationTest mCurrentAnimation;
        private int frameIndex;
        private float time;
        private SpriteFont gameFont;

        public AnimationPlayer(Game game)
            : base(game)
        {
            gameFont = game.Content.Load<SpriteFont>("font");
        }

        public AnimationTest CurrentAnimation
        {
            get { return mCurrentAnimation; }
        }

        public int FrameIndex
        {
            get { return frameIndex; }
        }

        public void PlayAnimation(AnimationTest animation)
        {
            // If this animation is already running, do not restart it.
            if (mCurrentAnimation == animation)
                return;

            // Start the new animation.
            mCurrentAnimation = animation;
            this.frameIndex = 0;
            this.time = 0.0f;
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

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            base.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position)
        {
            if (mCurrentAnimation == null)
                throw new NotSupportedException("No animation is currently playing.");

            // Process passing time.
            time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            while (time > mCurrentAnimation.FrameTime)
            {
                time -= mCurrentAnimation.FrameTime;

                // Advance the frame index; looping or clamping as appropriate.
                if (mCurrentAnimation.IsLooping)
                {
                    frameIndex = (frameIndex + 1) % mCurrentAnimation.FrameCount;
                }
                else
                {
                    frameIndex = Math.Min(frameIndex + 1, mCurrentAnimation.FrameCount - 1);
                }
            }

            // Calculate the source rectangle of the current frame.
            Rectangle source = new Rectangle(FrameIndex * (mCurrentAnimation.FrameWidth+1) + mCurrentAnimation.StartX , mCurrentAnimation.StartY, mCurrentAnimation.FrameHeight, mCurrentAnimation.FrameWidth);
            spriteBatch.Begin();
            spriteBatch.DrawString(gameFont, "Error AnPlayer: "+(FrameIndex * (mCurrentAnimation.Texture.Width+1) + mCurrentAnimation.StartX), new Vector2(150, 150), Color.White);
            // Draw the current frame.
            spriteBatch.Draw(mCurrentAnimation.Texture, position, source, Color.White);
            spriteBatch.End();
        }
    }
}