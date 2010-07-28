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
        protected SpriteBatch        spriteBatch;
        protected Rectangle          pacRect =     new Rectangle(000, 0, 290, 67);
        protected Vector2            pacPosition = new Vector2(300, 0);
        protected KeyboardState oldKeyboardState = Keyboard.GetState(), keyboardState;
        protected HighScoreScene     highscoreScene;
        protected StartScene         startScene;
        protected GameScene          currentScene;
        private string path = (@"..\..\..\Content\highscore.txt");
        protected int elapsedMilliseconds = 0, playerScore;
        private string playerName = "";
        private bool newHighscore;

        public HighScoreScene(Game game, SpriteFont smallFont, SpriteFont largeFont, Texture2D background) 
            : base(game)
        {
            menu = new TextMenuComponent(game, smallFont, largeFont);
            updateScore();
            menu.Position = new Vector2((Game.Window.ClientBounds.Width - menu.WidthMenu) / 2,
                                                Game.Window.ClientBounds.Height - menu.HeightMenu - 100);
            Components.Add(menu);
            gameFont = smallFont;
            newHighscore = false;

            spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
        }

        protected override void LoadContent()
        {
            highscoreLogo = Game.Content.Load<Texture2D>("Highscore");       // sceneSprite
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            if (newHighscore)
            {
                if (GetHighScoreName(gameTime))
                {
                    saveScore();
                    newHighscore = false;
                    updateScore();
                    menu.Visible = true;
                }
            }
            base.Update(gameTime);
        }

        protected void ShowScene(GameScene scene)
        {
            currentScene.Hide();
            currentScene = scene;
            currentScene.Show();
        }

        private bool KeyPressed(Keys key)
        {
            bool result = (oldKeyboardState.IsKeyDown(key) &&
                (keyboardState.IsKeyUp(key)));
            
            return result;
        }

        /// <summary>
        /// Check if the Enter Key ou 'A' button was pressed
        /// </summary>
        /// <returns>true, if enter key ou 'A' button was pressed</returns>
        private void HandleActionInput()
        {
            // Get the Keyboard state

            KeyboardState keyboardState = Keyboard.GetState();

            bool backKey = (oldKeyboardState.IsKeyDown(Keys.Escape) &&
                (keyboardState.IsKeyUp(Keys.Escape)));
            bool enterKey = (oldKeyboardState.IsKeyDown(Keys.Enter) &&
                (keyboardState.IsKeyUp(Keys.Enter)));
            oldKeyboardState = keyboardState;
            if (backKey)
            {
                ShowScene(startScene);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(highscoreLogo, pacPosition, pacRect, Color.White);
            if (newHighscore)
            {
                spriteBatch.DrawString(gameFont, "You got a new highscore!!!\nenter your name:\n\n" + playerName +
                    "\n\n<press return when done>",
                    new Vector2(Game.Window.ClientBounds.Width / 2 - 100, Game.Window.ClientBounds.Height / 2 - 50),
                    Color.White);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
        
        public override void Show()
        {
            if (newHighscore)
            {
                menu.Visible = false;
            }
            else
            {
                updateScore();
            }
            base.Show();
        }

        public override void Hide()
        {
            base.Hide();
        }

        private void updateScore()
        {
            highscoreObject[] temp = Game1942.XmlHandling.ReadFromXML(path);
            temp = XmlHandling.SortHighscore(temp);
            string[] items = new string[temp.Length - 1];
            for (int i = 0; i < temp.Length - 1; i++)
            {
                items[i] = temp[i].PlayerName + " " + temp[i].PlayerScore + "p";
            }
            menu.SetMenuItems(items);
        }

        public bool GetHighScoreName(GameTime gameTime)
        {
            oldKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();

            if (KeyPressed(Keys.Enter))
            {
                return true;
            }
            else if (KeyPressed(Keys.Back) && playerName.Length > 0)
            {
                playerName = playerName.Remove(playerName.Length - 1);
                return false;
            }
            else if (KeyPressed(Keys.A))
            {
                playerName += "A";
                return false;
            }
            else if (KeyPressed(Keys.B))
            {
                playerName += "B";
                return false;
            }
            else if (KeyPressed(Keys.C))
            {
                playerName += "C";
                return false;
            }
            else if (KeyPressed(Keys.D))
            {
                playerName += "D";
                return false;
            }
            else if (KeyPressed(Keys.E))
            {
                playerName += "E";
                return false;
            }
			else if(KeyPressed(Keys.F))
			{
			    playerName += "F";
		        return false;
			}
            else if (KeyPressed(Keys.G))
            {
                playerName += "G";
                return false;
            }
            else if (KeyPressed(Keys.H))
            {
                playerName += "H";
                return false;
            }
            else if (KeyPressed(Keys.I))
            {
                playerName += "I";
                return false;
            }
            else if (KeyPressed(Keys.J))
            {
                playerName += "J";
                return false;
            }
            else if (KeyPressed(Keys.K))
            {
                playerName += "K";
                return false;
            }
            else if (KeyPressed(Keys.L))
            {
                playerName += "L";
                return false;
            }
            else if (KeyPressed(Keys.M))
            {
                playerName += "M";
                return false;
            }
            else if (KeyPressed(Keys.N))
            {
                playerName += "N";
                return false;
            }
            else if (KeyPressed(Keys.O))
            {
                playerName += "O";
                return false;
            }
            else if (KeyPressed(Keys.P))
            {
                playerName += "P";
                return false;
            }
            else if (KeyPressed(Keys.Q))
            {
                playerName += "Q";
                return false;
            }
            else if (KeyPressed(Keys.R))
            {
                playerName += "R";
                return false;
            }
            else if (KeyPressed(Keys.S))
            {
                playerName += "S";
                return false;
            }
            else if (KeyPressed(Keys.T))
            {
                playerName += "T";
                return false;
            }
            else if (KeyPressed(Keys.U))
            {
                playerName += "U";
                return false;
            }
            else if (KeyPressed(Keys.V))
            {
                playerName += "V";
                return false;
            }
            else if (KeyPressed(Keys.W))
            {
                playerName += "W";
                return false;
            }
            else if (KeyPressed(Keys.X))
            {
                playerName += "X";
                return false;
            }
            else if (KeyPressed(Keys.Y))
            {
                playerName += "Y";
                return false;
            }
            else if (KeyPressed(Keys.Z))
            {
                playerName += "Z";
                return false;
            }
            else if (KeyPressed(Keys.Space))
            {
                playerName += " ";
                return false;
            }
            return false;
        }

        private void saveScore()
        {
            highscoreObject[] tmpList = new highscoreObject[10];
            highscoreObject playerObject = new highscoreObject();
            tmpList = XmlHandling.ReadFromXML(path);
            playerObject.PlayerName = playerName;
            playerObject.PlayerScore = playerScore;
            tmpList = XmlHandling.CheckInsertHighscore(tmpList, playerObject);
            XmlHandling.WriteHighScoreToXML(tmpList, path);
            playerScore = 0;
        }

        public void newHighTrue(int newScore)
        {
            playerScore = newScore;
            newHighscore = true;
        }

        public bool getNewHigh()
        {
            return newHighscore;
        }
    }
}

