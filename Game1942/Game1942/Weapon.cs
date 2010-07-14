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

        protected Texture2D texture;
        protected Rectangle spriteRectangle;
        public Vector2 position;
        protected int speed = 4;
        private int mKind;
       
        protected SpriteBatch mSpriteBatch;

        protected const int BULLETWIDTH = 32;
        protected const int BULLETLENGTH = 32;

        public Weapon(Game game, ref Texture2D theTexture, Vector2 newPosition, int kindOf)
            : base(game)
        {
            texture = theTexture;
            position = newPosition;
            mKind = kindOf;
            if (mKind == 1)
            {
                spriteRectangle = new Rectangle(37, 169, BULLETWIDTH, BULLETLENGTH);
            }
            else if (mKind == 2)
            {
                spriteRectangle = new Rectangle(4, 169, BULLETWIDTH, BULLETLENGTH);
            }
            else if (mKind == 3)
            {
                spriteRectangle = new Rectangle(37, 169, BULLETWIDTH, BULLETLENGTH);
            }
            

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
            mSpriteBatch.Begin();          
            mSpriteBatch.Draw(texture, position, spriteRectangle, Color.White);
            mSpriteBatch.End();
            base.Draw(gameTime);
            
        }


        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            if (mKind == 1)
            {
                position.Y -= speed;
                position.X -= speed / 2;
            }
            else if (mKind == 2)
            {
                // updates bullet position
                position.Y -= speed;                
            }
            else if (mKind == 3)
            {
                position.Y -= speed;
                position.X += speed / 2;
            }
            
            base.Update(gameTime);
        }
    }
}