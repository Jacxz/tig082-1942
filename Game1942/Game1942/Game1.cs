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
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch mSpriteBatch;
        ContentManager contentManager;
        //game scenes
        protected StartScene startScene;
        protected ActionScene actionScene;
        protected HighScoreScene highScoreScene;
        protected GameOverScene gameOverScene;
        //current active scene
        protected GameScene currentScene;

        // textures
        protected Texture2D startElementsTexture, startBackgroundTexture, actionTextures;

        //font
        private SpriteFont gameFont;
        // fonts
        protected SpriteFont smallFont, largeFont;

        protected KeyboardState oldKeyboardState = Keyboard.GetState();

        protected int score;

        protected TextMenuComponent gameOverText;
        private string path = (@"..\..\..\Content\highscore.txt");

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            mSpriteBatch = new SpriteBatch(GraphicsDevice);
            
            Services.AddService(typeof(SpriteBatch), mSpriteBatch);
            contentManager = new ContentManager(Services, @"Content\");
            actionTextures = Content.Load<Texture2D>("1945");

            startElementsTexture = Content.Load<Texture2D>("MenuTitle"); // sceneSprite
            
            gameFont = Content.Load<SpriteFont>("font");
            smallFont = Content.Load<SpriteFont>("smallMenuFont");
            largeFont = Content.Load<SpriteFont>("largeMenuFont");

            AudioManager.Initialize(this);
            AudioManager.LoadEffect("menu");

            // start scene
            startScene = new StartScene(this, smallFont, largeFont, startBackgroundTexture, startElementsTexture);
            Components.Add(startScene);
            startScene.Show();
            currentScene = startScene;
            // actionscene
            actionScene = new ActionScene(this, actionTextures, startBackgroundTexture, smallFont);
            actionScene.Initialize();
            Components.Add(actionScene);
            // game over scene
            gameOverScene = new GameOverScene(this, actionTextures);
            gameOverScene.Initialize();
            Components.Add(gameOverScene);
            // highscore scene
            highScoreScene = new HighScoreScene(this, smallFont, largeFont, startBackgroundTexture);
            highScoreScene.Initialize();
            Components.Add(highScoreScene);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here

        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboard = Keyboard.GetState();

            // Handle GameScene Inputs
            HandleScenesInput();
            base.Update(gameTime);
        }

        protected void ShowScene(GameScene scene)
        {
            currentScene.Hide();
            currentScene = scene;
            currentScene.Show();
        }

        private void HandleScenesInput()
        {
            // Handle Start Scene Input
            if (currentScene == startScene)
            {
                HandleStartSceneInput();
            }

            // Handle Action Scene Input
            else if (currentScene == actionScene)
            {
                HandleActionInput();
                if (actionScene.GameOverState())
                {
                    actionScene.ResetGameOver();
                    if (XmlHandling.CheckInsertHighscoreBool(XmlHandling.ReadFromXML(path), actionScene.ResultScore))
                    {
                        gameOverScene.HighScoreTrue();
                    }
                    ShowScene(gameOverScene);
                }
            }

            else if (currentScene == gameOverScene)
            {
                if (XmlHandling.CheckInsertHighscoreBool(XmlHandling.ReadFromXML(path), actionScene.ResultScore))
                {
                    highScoreScene.newHighTrue(actionScene.ResultScore);
                    if (CheckEnter())
                    {
                        ShowScene(highScoreScene);
                    }
                }                    
                    
                else if (CheckEnter())
                {
                    ShowScene(startScene);
                }
            }
            
            else if (currentScene == highScoreScene)
            {
                if (CheckEnter() && !highScoreScene.getNewHigh())
                {
                    ShowScene(startScene);
                }
            }
        }

        private bool CheckEnter()
        {
            // Get the Keyboard state
            KeyboardState keyboardState = Keyboard.GetState();
            bool result = (oldKeyboardState.IsKeyDown(Keys.Enter) &&
                (keyboardState.IsKeyUp(Keys.Enter)));
            oldKeyboardState = keyboardState;
            return result;
        }

        /// <summary>
        /// Check if the Enter Key or 'A' button was pressed
        /// </summary>
        /// <returns>true, if enter key or 'A' button was pressed</returns>
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

        /// <summary>
        /// Handle buttons and keyboard in StartScene
        /// </summary>
        private void HandleStartSceneInput()
        {
            if (CheckEnter())
            {
                switch (startScene.SelectedMenuIndex)
                {
                    case 0:
                        ShowScene(actionScene);
                        break;
                    case 1:
                        ShowScene(highScoreScene);
                        break;
                    case 2:
                        Exit();
                        break;
                }
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            mSpriteBatch.Begin();
            mSpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
