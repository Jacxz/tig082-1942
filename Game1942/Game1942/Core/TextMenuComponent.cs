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


namespace Game1942.Core
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class TextMenuComponent : Microsoft.Xna.Framework.DrawableGameComponent
    {

        protected SpriteBatch sBatch;
        //font declerations and color
        protected readonly SpriteFont regularFont, selectedFont;
        protected Color regularColor = Color.White, selectedColor = Color.Red;

        protected KeyboardState oldKeyboardState;

        //position of menu
        private Vector2 position = new Vector2();


        //menu items 
        private int selectedIndex = 0;
        private readonly List<string> menuItems;

        protected int widthMenu, heightMenu;



        public void SetMenuItems(string[] items)
        {
            menuItems.Clear();
            menuItems.AddRange(items);
            CalculateBounds();
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Color SelectedColor
        {
            get { return selectedColor; }
            set { selectedColor = value; }
        }

        public Color RegularColor
        {
            get { return regularColor; }
            set { regularColor = value; }
        }

        public int HeightMenu
        {
            get { return heightMenu; }
        }

        public int WidthMenu
        {
            get { return widthMenu; }
        }

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set { selectedIndex = value; }
        }

        protected void CalculateBounds()
        {
            widthMenu = 0;
            heightMenu = 0;

            foreach (string item in menuItems)
            {
                Vector2 size = selectedFont.MeasureString(item);
                if (size.X > widthMenu)
                {
                    widthMenu = (int)size.X;
                }
                heightMenu += selectedFont.LineSpacing;
            }
        }

        public TextMenuComponent(Game game, SpriteFont normalFont, SpriteFont selectedFont)
            : base(game)
        {
            regularFont = normalFont;
            this.selectedFont = selectedFont;
            menuItems = new List<string>();

            sBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));

            oldKeyboardState = Keyboard.GetState();


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
            KeyboardState keyboardState = Keyboard.GetState();

            bool up, down, enter;
            down = (oldKeyboardState.IsKeyDown(Keys.Down) && (keyboardState.IsKeyUp(Keys.Down)));
            up = (oldKeyboardState.IsKeyDown(Keys.Up) && (keyboardState.IsKeyUp(Keys.Up)));
            enter = (oldKeyboardState.IsKeyDown(Keys.Enter) && (keyboardState.IsKeyUp(Keys.Enter)));
            if (down)
            {
                selectedIndex++;
                if (selectedIndex == menuItems.Count)
                {
                    selectedIndex = 0;
                }
                AudioManager.Effect("menu");
            }

            if (up)
            {
                selectedIndex--;
                if (selectedIndex == -1)
                {
                    selectedIndex = menuItems.Count - 1;
                }
                AudioManager.Effect("menu");
            }

          
            oldKeyboardState = keyboardState;
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            sBatch.Begin();
            float y = position.Y;
            for (int i = 0; i < menuItems.Count; i++)
            {
                SpriteFont font;
                Color theColor;
                if (i == selectedIndex)
                {
                    font = selectedFont;
                    theColor = selectedColor;
                }
                else
                {
                    font = regularFont;
                    theColor = regularColor;
                }

                // for shadowing
                sBatch.DrawString(font, menuItems[i], new Vector2(position.X + 1, y + 1), Color.Yellow);
                //the string item
                sBatch.DrawString(font, menuItems[i], new Vector2(position.X + 1, y + 1), theColor);
                y += font.LineSpacing;

            }
            sBatch.End();
            base.Draw(gameTime);
        }
    }
}