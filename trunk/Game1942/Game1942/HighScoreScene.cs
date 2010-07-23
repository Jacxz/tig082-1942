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
using System.Xml;

namespace Game1942
{

    public class HighScoreScene : GameScene
    {
        protected Texture2D          highscoreLogo;                  // textures    
        protected TextMenuComponent  menu;
        private   SpriteFont         gameFont;                                  // font
        protected SpriteBatch        mSpriteBatch;
        protected Rectangle          pacRect =     new Rectangle(000, 0, 290, 67);
        protected Vector2            pacPosition = new Vector2(300, 0);
        protected KeyboardState      oldKeyboardState = Keyboard.GetState();
        protected HighScoreScene     highscoreScene;
        protected StartScene         startScene;
        protected GameScene          currentScene;
        

        public HighScoreScene(Game game, SpriteFont smallFont, SpriteFont largeFont, Texture2D background) 
            : base(game)
        {
            string path = (@"..\..\..\Content/Highscore.txt");

            Project1.highscoreObject[] temp = Project1.XmlHandling.ReadFromXML(path);
            temp = Project1.XmlHandling.SortHighscore(temp);
            string[] items = new string[temp.Length - 1];
            for (int i = 0; i < temp.Length - 1; i++)
            {
                items[i] = temp[i].PlayerName + " " + temp[i].PlayerScore + "p";
            }
            string[] itemsEnd = { "", "Press <ESC> to leave" };
            items = MergeArrays(items, itemsEnd);

            menu = new TextMenuComponent(game, smallFont, largeFont);
            menu.SetMenuItems(items);
            Components.Add(menu);

            mSpriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
        }

        private string[] MergeArrays(string[] array1, string[] array2)
        {
            int index;
            int array1Length = array1.Length;
            if (array2.Length > 0)
                Array.Resize(ref array1, array1.Length + array2.Length);
            for (index = 0; index <= array2.Length-1; index++)
            {
                array1[array1Length + index] = array2[index];
            }
            return array1;
        }

        protected override void LoadContent()
        {
            // loads the xml file
            
            highscoreLogo = Game.Content.Load<Texture2D>("Highscore");       // sceneSprite
        } 


        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        { base.Initialize(); }


        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        { base.Update(gameTime); }



        protected void ShowScene(GameScene scene)
        {
            currentScene.Hide();
            currentScene = scene;
            currentScene.Show();
        }



        public override void Draw(GameTime gameTime)
        {
            mSpriteBatch.Begin();
            mSpriteBatch.Draw(highscoreLogo, pacPosition, pacRect, Color.White);
            mSpriteBatch.End();
            base.Draw(gameTime);
        }

        
        public override void Show()
        {
            menu.Position = new Vector2((Game.Window.ClientBounds.Width - menu.WidthMenu) / 2, 
                                            Game.Window.ClientBounds.Height - menu.HeightMenu - 10);
            base.Show();
        }

        public override void Hide()
        {
            base.Hide();
        }
 
    }
}

