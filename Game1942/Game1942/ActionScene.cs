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
        protected Texture2D mBackgroundTexture, actionTexture, blockTextureData, actionTextures;
        protected SpriteBatch mSpriteBatch;
        protected Player player;
        private CollisionDetection mCollison;
        private int screenheight, screenwidth, deltaY, i, j, changeY;
        

        List<Weapon> bulletList = new List<Weapon>();

        public bool mGameOver = false;

        //font
        private SpriteFont gameFont;

        private List<Enemy> Enemies = new List<Enemy>();
     
        // error variables
        int error;

        private KeyboardState oldKeyboardState, keyboard;

        public ActionScene(Game game, Texture2D theTexture, Texture2D backGroundTexture, 
            SpriteFont smallFont)
            : base(game)
        {
            gameFont = smallFont;
            actionTexture = theTexture;
            mBackgroundTexture = backGroundTexture;
            deltaY = 2;

            // creates and puts the player in start position
            Start();

            oldKeyboardState = Keyboard.GetState();
            mSpriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
        }

        protected override void LoadContent()
        {
            AudioManager.LoadEffect("luger");
            AudioManager.LoadEffect("implosion");
            actionTextures = Game.Content.Load<Texture2D>("1945");
            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            screenheight = GraphicsDevice.Viewport.Height;
            screenwidth = GraphicsDevice.Viewport.Width;
        }

        public override void Show()
        {
            AudioManager.PlayMusic("song");
            base.Show();
        }

        public override void Hide()
        {
            MediaPlayer.Stop();
            base.Hide();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            keyboard = Keyboard.GetState();
            AddBullet();

            changeY += deltaY;
            changeY = changeY % 32;

            // removes the bullet when it reached the end of the screen
            for (int i = 0; i < bulletList.Count; i++)
            {
                if (bulletList[i].mPosition.Y < (0))
                {
                    bulletList.RemoveAt(i);
                }
            }
            CheckCollisions(); // checks collition with enemy List

            oldKeyboardState = keyboard;
            base.Update(gameTime);
        }

        private void Start() 
        {
            if (player == null)
            {
                player = new Player(Game, ref actionTexture);
                player.Initialize();
                Components.Add(player);
            }
            for (int x = 0; x < 10; x++)
            {
                Enemies.Add(new Enemy(Game, ref actionTexture));
                Components.Add(Enemies[Enemies.Count -1]);
            }

            player.PutInStartPosition();
        }

        public void CheckCollisions()
        {
            for (int x = 0; x < Enemies.Count - 1; x++)
            {
                if (Enemies[x].checkCollision(player.GetBounds()))
                {                   
                    player.IsHit();                   
                }
            }
            for (int x = 0; x < bulletList.Count - 1; x++)
            {
                for (int y = 0; y < Enemies.Count - 1; y++)
                {
                    if (Enemies[y].checkCollision(bulletList[x].GetBounds()))
                    {
                        error++;
                        Enemies[y].PutinStartPosition();
                        bulletList.RemoveAt(x);
                    }
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            mSpriteBatch.Begin();
            mSpriteBatch.DrawString(gameFont, "ActionScene Bullets: " + bulletList.Count.ToString()+ "\nActionScene Player killed Enemy: "+ error.ToString(), new Vector2(15, 15), Color.White);

            if (player.GetLives() < 0)
                GameOver();

            // Here the scrolling background is printed
            for (i = 0; i < screenwidth/32; i++)
            {
                for (j = 0; j < (screenheight / 32)+2; j++)
                {
                    mSpriteBatch.Draw(actionTextures, new Vector2(32 * i,32 * j + changeY)
                        , new Rectangle(268, 367, 32, 32),
                         Color.White, 0, new Vector2(0, 32), 1, SpriteEffects.None, 0f);
                }
            }

            mSpriteBatch.End();
        
            base.Draw(gameTime);
        }

        public void AddBullet()
        {
            if (keyboard.IsKeyDown(Keys.Space) && !oldKeyboardState.Equals(keyboard))
            {
                bulletList.Add(new Weapon(Game, ref actionTexture, player.getPosition(), 1));
                Components.Add(bulletList[bulletList.Count - 1]);
                bulletList.Add(new Weapon(Game, ref actionTexture, player.getPosition(), 2));
                Components.Add(bulletList[bulletList.Count - 1]);
                bulletList.Add(new Weapon(Game, ref actionTexture, player.getPosition(), 3));
                Components.Add(bulletList[bulletList.Count - 1]);
                AudioManager.Effect("luger");
            }
        }

        private void GameOver()
        {
            mSpriteBatch.DrawString(gameFont, "Game Over", new Vector2(75, 75), Color.White);
            AudioManager.GameOver();
            while (MediaPlayer.State != MediaState.Stopped)
            { }
            mGameOver = true;
        }
    }
}