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

        protected Texture2D texture;
        protected Rectangle spriteRectangle;
        protected Vector2 position;
        protected int Yspeed;
        protected int Xspeed;
        protected Random random;
        protected SpriteBatch mSpriteBatch;
        protected SpriteFont gameFont;

        protected const int ENEMYWIDTH = 32; // ändrat kod
        protected const int ENEMYHEIGHT = 32; // ändrat kod

        public Enemy(Game game, ref Texture2D theTexture)
            : base(game)
        {
            texture = theTexture;
            position = new Vector2();
            // Get the current spritebatch
            mSpriteBatch =
                (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));

            // Create the source rectangle.
            // This represents where is the sprite picture in surface
            spriteRectangle = new Rectangle(4, 4, ENEMYWIDTH, ENEMYHEIGHT); //// ändrat kod

            // Initialize the random number generator and put the enemy in 
            // your start position
            random = new Random(this.GetHashCode());
            PutinStartPosition();
            gameFont = Game.Content.Load<SpriteFont>("font");
        }

        /// <summary>
        /// Initialize Meteor Position and Velocity
        /// </summary>
        public void PutinStartPosition()
        {
            position.X = random.Next(Game.Window.ClientBounds.Width - ENEMYWIDTH); // ändrat kod
            position.Y = 0;
            Yspeed = 1 + random.Next(3);
            Xspeed = random.Next(3) - 1;
        }

        /// <summary>
        /// Allows the game component draw your content in game screen
        /// </summary>
        public override void Draw(GameTime gameTime)
        {

            mSpriteBatch.Begin();

            for (int x = 0; x < Yspeed; x++)
            {
                position.Y += 1;
                mSpriteBatch.DrawString(gameFont, "X: " + position.X.ToString() + " Y: " + position.Y.ToString(), position, Color.White);
                mSpriteBatch.Draw(texture, position, spriteRectangle, Color.White);
            }
            mSpriteBatch.End();
            base.Draw(gameTime);
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // Check if the enemy still visible
            if ((position.Y >= Game.Window.ClientBounds.Height) ||
                (position.X >= Game.Window.ClientBounds.Width) || (position.X <= 0) || (position.X >= 780))
            {
                PutinStartPosition();
            }

            // Move Enemy
            //  position.Y += Yspeed;
            position.X += Xspeed;

            base.Update(gameTime);
        }

        /// <summary>
        /// Check if the meteor intersects with the specified rectangle
        /// </summary>
        /// <param name="rect">test rectangle</param>
        /// <returns>true, if has a collision</returns>
        /// 
        public bool checkCollision(Rectangle rect) 
        {
            Rectangle spriterect = new Rectangle((int)position.X, (int)position.Y, ENEMYWIDTH, ENEMYHEIGHT);
            return spriterect.Intersects(rect);

        }

        public Rectangle getBounds()
        {
            return new Rectangle((int)position.X, (int)position.Y, ENEMYWIDTH, ENEMYHEIGHT);
        }
    }
}