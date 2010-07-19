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
using Game1942.Core;

namespace Game1942
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class GameOverScene : GameScene
    {
        private SpriteBatch spriteBatch;
        private Texture2D texture;
        private Rectangle rectangle;
        private TimeSpan timer;

        public GameOverScene(Game game, Texture2D texture)
            : base(game)
        {
            spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
            this.texture = texture;
            rectangle = new Rectangle(303, 503, 96, 12);
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
            if ((gameTime.TotalGameTime - timer).Seconds > 1)
            {
                if (rectangle.Y == 503)
                {
                    rectangle.Y = 520;
                    timer = gameTime.TotalGameTime;
                }
                else
                {
                    rectangle.Y = 503;
                    timer = gameTime.TotalGameTime;
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(texture, new Vector2(Game.Window.ClientBounds.Width / 2 - 50, Game.Window.ClientBounds.Height / 2), rectangle, Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}