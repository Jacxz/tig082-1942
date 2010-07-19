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
    public class AnimationTest : Microsoft.Xna.Framework.GameComponent
    {
        private Texture2D texture;
        private float frameTime;
        private bool isLooping;
        private int frameCount, frameWidth, frameHeight, startX, startY;

        public AnimationTest(Game game, Texture2D texture, float frameTime, bool isLooping, int frameCount, int frameWidth, int frameHeight, int startX, int startY)
            : base(game)
        {
            this.texture = texture;
            this.frameTime = frameTime;
            this.isLooping = isLooping;
            this.frameCount = frameCount;
            this.frameWidth = frameWidth;
            this.frameHeight = frameHeight;
            this.startX = startX;
            this.startY = startY;
        }

        public Texture2D Texture
        {
            get { return texture; }
        }
        /// <summary>
        /// Duration of time to show each frame.
        /// </summary>
        public float FrameTime
        {
            get { return frameTime; }
        }
        /// <summary>
        /// When the end of the animation is reached, should it
        /// continue playing from the beginning?
        /// </summary>
        public bool IsLooping
        {
            get { return isLooping; }
        }
        /// <summary>
        /// Gets the number of frames in the animation.
        /// </summary>
        public int FrameCount
        {
            get { return frameCount; }
            
        }

        /// <summary>
        /// Gets the width of a frame in the animation.
        /// </summary>
        public int FrameWidth
        {
            // Assume square frames.
            get { return frameWidth; }
        }

        /// <summary>
        /// Gets the height of a frame in the animation.
        /// </summary>
        public int FrameHeight
        {
            get { return frameWidth; }
        }

        public int StartX
        {
            get { return startX; }
        }

        public int StartY
        {
            get { return startY; }
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
    }
}