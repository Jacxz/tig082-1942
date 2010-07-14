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
    public class ActionScene : GameScene
    {
        protected Texture2D mBackgroundTexture, actionTexture;
        protected SpriteBatch mSpriteBatch;
        GraphicsDeviceManager graphics;
        protected Player player;

        List<Weapon> bulletList = new List<Weapon>();
        SpriteFont font;

        private Enemy enemy;

        private const float Rotation = 0;
        private const float Scale = 2.0f;
        private const float Depth = 0.5f;

        private Viewport viewport;
        private Vector2 shipPos;
        private const int Frames = 4;
        private const int FramesPerSec = 2;

        //font
        private SpriteFont gameFont;
           
        private const int addDropTime = 1000;       

        private const int startDrops = 1;
        private KeyboardState oldKeyboardState;

        int k = 4;

        public ActionScene(Game game, Texture2D theTexture, Texture2D backGroundTexture, SpriteFont smallFont)
            : base(game)
        {
            font = smallFont;
            actionTexture = theTexture;
            mBackgroundTexture = backGroundTexture;

            // creates and puts the player in start position
            Start();
            
            oldKeyboardState = Keyboard.GetState();
            mSpriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
        }

        protected override void LoadContent()
        {
            font = (SpriteFont)Game.Services.GetService(typeof(SpriteFont));
            viewport = graphics.GraphicsDevice.Viewport;
            shipPos = new Vector2(viewport.Width / 2, viewport.Height / 2);
            
            gameFont = Game.Content.Load<SpriteFont>("font");

            base.LoadContent();
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
            base.Show();
        }

        public override void Hide()
        {
            base.Hide();
        }


        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(Keys.Space) && !oldKeyboardState.Equals(keyboard))
            {
                bulletList.Add(new Weapon(Game, ref actionTexture, player.getPosition(), 1));
                Components.Add(bulletList[bulletList.Count - 1]);
                bulletList.Add(new Weapon(Game, ref actionTexture, player.getPosition(), 2));
                Components.Add(bulletList[bulletList.Count - 1]);
                bulletList.Add(new Weapon(Game, ref actionTexture, player.getPosition(), 3));
                Components.Add(bulletList[bulletList.Count - 1]);
            }

            
          
            // removes the bullet when it reached the end of the screen
            for (int i = 0; i < bulletList.Count; i++)
            {
                if (bulletList[i].position.Y < (0))
                {
                    bulletList.RemoveAt(i);
                }
            }

      
            oldKeyboardState = keyboard;
            base.Update(gameTime);
        }
        private void Start()
        {
            if (player == null)
            {
                player = new Player(Game, ref actionTexture);
                Components.Add(player);
            }
            for (int x = 0; x < 11; x++)
            {
                enemy = new Enemy(Game, ref actionTexture);
                Components.Add(enemy);
            }

            player.PutInStartPosition();
        }

        public override void Draw(GameTime gameTime)
        {
            mSpriteBatch.Begin();
            mSpriteBatch.DrawString(font, "Bullets: " + bulletList.Count.ToString(), new Vector2(15, 15), Color.White);           
            mSpriteBatch.End();
            base.Draw(gameTime);
            
        }
    }
}