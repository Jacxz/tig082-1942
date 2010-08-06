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
        private double timer;
        private bool highscore;

        public GameOverScene(Game game, Texture2D texture)
            : base(game)
        {
            spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
            this.texture = texture;
            rectangle = new Rectangle(303, 503, 97, 13);
            highscore = false;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Show()
        {
            if (highscore)
                AudioManager.HighScore();
            else
                AudioManager.GameOver();
            base.Show();
        }

        public override void Hide()
        {
            highscore = false;
            rectangle = new Rectangle(303, 503, 97, 13);
            base.Hide();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            if ((gameTime.TotalGameTime.TotalMilliseconds - timer) > 100)
            {
                if (!highscore)
                {
                    if (rectangle.Y == 503)
                    {
                        rectangle.Y = 520;
                        timer = gameTime.TotalGameTime.TotalMilliseconds;
                    }
                    else
                    {
                        rectangle.Y = 503;
                        timer = gameTime.TotalGameTime.TotalMilliseconds;
                    }
                }
                else
                {
                    if (rectangle.Y == 127)
                    {
                        rectangle.Y = 144;
                        timer = gameTime.TotalGameTime.TotalMilliseconds;
                    }
                    else
                    {
                        rectangle.Y = 127;
                        timer = gameTime.TotalGameTime.TotalMilliseconds;
                    }
                }
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            
            spriteBatch.Begin();
            spriteBatch.Draw(texture, new Vector2(Game.Window.ClientBounds.Width / 2 - 100, Game.Window.ClientBounds.Height / 2), rectangle, Color.White, 0, new Vector2(), 2f, new SpriteEffects(), 0);
            spriteBatch.End();
 
            base.Draw(gameTime);
        }

        public void HighScoreTrue()
        {
            highscore = true;
            rectangle = new Rectangle(574, 127, 103, 13);
        }
    }
}