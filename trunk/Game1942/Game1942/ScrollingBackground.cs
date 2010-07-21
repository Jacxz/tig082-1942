using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game1942
{
    public class ScrollingBackground
    {
        // class ScrollingBackground
        private Vector2 screenpos, origin, texturesize;
        private Texture2D mytexture, actionTexture;
        private int screenheight, screenwidth;
        private string type;
        private float elapsedTime;

        public ScrollingBackground(string type, Texture2D actionTexture)
        {
            this.type = type;
            this.actionTexture = actionTexture;
        }

        public void Load(GraphicsDevice device, Texture2D backgroundTexture)
        {
            mytexture = backgroundTexture;
            screenheight = device.Viewport.Height;
            screenwidth = device.Viewport.Width;
            // Set the origin so that we're drawing from the 
            // center of the top edge.
            origin = new Vector2(mytexture.Width / 2, 0);
            // Set the screen position to the center of the screen.
            screenpos = new Vector2(screenwidth / 2, screenheight / 2);
            // Offset to draw the second texture, when necessary.
            texturesize = new Vector2(0, mytexture.Height);
        }
        // ScrollingBackground.Update
        public void Update(GameTime gameTime)
        {
            // The time since Update was called last.
            elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds * 100;

            if (type.Equals("water"))
            {
                screenpos.Y += elapsedTime;
                screenpos.Y = screenpos.Y % 32;
            }
            else if (type.Equals("space"))
            {
                screenpos.Y += elapsedTime;
                screenpos.Y = screenpos.Y % mytexture.Height;
            }
        }
        // ScrollingBackground.Draw
        public void Draw(SpriteBatch batch)
        {
            if(type.Equals("water"))
            {
                Water(batch);
            }
            else if(type.Equals("space"))
            {
                Space(batch);
            }
        }

        private void Water(SpriteBatch batch)
        {
            int i, j;
            // Here the scrolling background is printed
            for (i = 0; i <= screenwidth / 32; i++)
            {
                for (j = 0; j <= (screenheight / 32) + 2; j++)
                {
                    batch.Draw(actionTexture, new Vector2(32 * i, 32 * j + screenpos.Y)
                        , new Rectangle(268, 367, 32, 32),
                         Color.White, 0, new Vector2(0, 32), 1, SpriteEffects.None, 0f);
                }
            }
        }

        private void Space(SpriteBatch batch)
        {
            // Draw the texture, if it is still onscreen.
            if (screenpos.Y < screenheight)
            {
                batch.Draw(mytexture, screenpos, null,
                     Color.White, 0, origin, 1, SpriteEffects.None, 0f);
            }
            // Draw the texture a second time, behind the first,
            // to create the scrolling illusion.
            batch.Draw(mytexture, screenpos - texturesize, null,
                 Color.White, 0, origin, 1, SpriteEffects.None, 0f);
        }
    }
}
