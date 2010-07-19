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
    public class StartScene : GameScene
    {
        protected SpriteBatch mSpriteBatch;

        protected TextMenuComponent menu;
        protected readonly Texture2D elements;

        protected Texture2D bGround;

        protected Rectangle pacRect = new Rectangle(0, 0, 290, 67);
        protected Vector2 pacPosition = new Vector2(300, 0);


        public StartScene(Game game, SpriteFont smallFont, SpriteFont largeFont, Texture2D background, Texture2D elements)
            : base(game)
        {
            this.elements = elements;
            bGround = background;

            string[] items = { "Start", "Highscore", "Options", "Exit" };
            menu = new TextMenuComponent(game, smallFont, largeFont);
            menu.SetMenuItems(items);
            Components.Add(menu);

            mSpriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));


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

        /// <summary>
        /// Gets the selected menu option
        /// </summary>
        public int SelectedMenuIndex
        {
            get { return menu.SelectedIndex; }
        }

        public override void Draw(GameTime gameTime)
        {

            mSpriteBatch.Begin();
            mSpriteBatch.Draw(elements, pacPosition, pacRect, Color.White);
            mSpriteBatch.End();
            base.Draw(gameTime);

        }

        public override void Show()
        {
            menu.Position = new Vector2((Game.Window.ClientBounds.Width - menu.WidthMenu) / 2, 330);
            base.Show();

        }

        public override void Hide()
        {
            base.Hide();
        }
    }
}