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
using System.IO;
using System.Xml;

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
        private int frameCount, frameWidth, frameHeight, startX, startY, dietype;

        public int Dietype
        {
            get { return dietype; }
            
        }

        public AnimationTest(Game game, Texture2D texture, int type)
            : base(game)
        {
            this.texture = texture;
            ReadXML(type);         
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
            base.Update(gameTime);
        }
        // reads from XML File
        public void ReadXML(int type)
        {
            // loads the xml file
            XmlTextReader txtRead = new XmlTextReader(@"..\..\..\Content/Animations.xml");

            while (txtRead.Read())
            {
                // if the type matches the node "type" in the xml
                if (txtRead.Name == "type")
                {
                    // if the value type is holding equals the type we want
                    if (txtRead.ReadElementContentAsInt() == type)
                    {
                        //moves one element and reads the value, does so for all the elements in the node.
                        txtRead.Read();
                        frameTime = txtRead.ReadElementContentAsFloat();
                        txtRead.Read();
                        frameCount = txtRead.ReadElementContentAsInt();
                        txtRead.Read();
                        dietype = txtRead.ReadElementContentAsInt();
                        txtRead.Read();
                        isLooping = txtRead.ReadElementContentAsBoolean();
                        txtRead.Read();
                        frameWidth = txtRead.ReadElementContentAsInt();
                        txtRead.Read();
                        frameHeight = txtRead.ReadElementContentAsInt();
                        txtRead.Read();
                        startX = txtRead.ReadElementContentAsInt();
                        txtRead.Read();
                        startY = txtRead.ReadElementContentAsInt();
                        break;
                    }
                }
            }
        }
    }
}