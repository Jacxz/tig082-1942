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
using System.IO;

namespace Game1942
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class ActionScene : GameScene
    {
        protected Texture2D mBackgroundTexture, actionTexture, blockTextureData;
        protected SpriteBatch mSpriteBatch;
        protected Player player;
        private Enemy mEnemy1;
        private EnemyManager enemyManager;
        private CollisionDetection mCollison;
        private int screenheight, screenwidth, deltaY, i, j, changeY, oldLives;
        private ScrollingBackground currentBackground;

        private float lTime, shootRate = 0.15f;

        

        private bool mGameOver = false;

        //font
        private SpriteFont gameFont;

        private List<Enemy> Enemies = new List<Enemy>();
        private List<Weapon> BulletList = new List<Weapon>();
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
            enemyManager = new EnemyManager(game, actionTexture);

            oldKeyboardState = Keyboard.GetState();
            mSpriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
        }

        protected override void LoadContent()
        {
            AudioManager.LoadEffect("luger");
            AudioManager.LoadEffect("implosion");
            Texture2D background = Game.Content.Load<Texture2D>("starfield");
            currentBackground.Load(GraphicsDevice, background);
            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            currentBackground = new ScrollingBackground("water", actionTexture);
            Start();
            base.Initialize();
            screenheight = GraphicsDevice.Viewport.Height;
            screenwidth = GraphicsDevice.Viewport.Width;
            
        }

        public override void Show()
        {
            // starts the background music
            AudioManager.PlayMusic("song");
            // creates and puts the player in start position
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
            CheckCollisions(); // checks collition with enemy List
            keyboard = Keyboard.GetState();

            AddBullet(gameTime);

            currentBackground.Update(gameTime);
           
            if (oldLives != player.GetLives())
            {
                ResetScene();
                oldLives = player.GetLives();
            }
            // removes the bullet when it reached the end of the screen
            for (int i = 0; i <= BulletList.Count-1; i++)
            {
                if (BulletList[i].mPosition.Y < (0))
                {
                    BulletList.RemoveAt(i);
                }
            }
            

            if (player.GetLives() < 0)
            {
                mGameOver = true;
                player.ResetLives();
            }

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
                oldLives = player.GetLives();
            }
            enemyManager.AddEnemy(1, 1);
            enemyManager.AddEnemy(2, 1);
            enemyManager.AddEnemy(3, 1);
            enemyManager.AddEnemy(4, 1);
            enemyManager.AddEnemy(5, 1);
            Enemies = enemyManager.GetEnemyList();

            for (int x = 0; x < Enemies.Count; x++)
            {
               
                Components.Add(Enemies[x]);
            }
           
            player.PutInStartPosition();
        }
        // check collision enemys vs player and subtracts 5 hp from player each hit.
        public void CheckCollisions()
        {
            for (int x = 0; x <= Enemies.Count - 1; x++)
            {
                if (Enemies[x].checkCollision(player.GetBounds()))
                {                   
                    player.IsHit();
                    Enemies[x].isHit();
                    
                }
            }

            // check if any enemy has a collision with a weapon, if it collides the position of the weapon is put outside the screen and will be removed in the update.
            for (int x = 0; x <= BulletList.Count - 1; x++)
            {
                for (int y = 0; y <= Enemies.Count - 1; y++)
                {
                    if (Enemies[y].checkCollision(BulletList[x].GetBounds()))
                    {                        
                        Enemies[y].isHit();
                        BulletList[x].mPosition.Y = -10;
                    }
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            mSpriteBatch.Begin();

            currentBackground.Draw(mSpriteBatch);
            
            mSpriteBatch.DrawString(gameFont, "ActionScene EnemyCounts: " + (Enemies.Count-1) + "\nActionScene : " + enemyManager.getError(), new Vector2(15, 15), Color.White);
            mSpriteBatch.End();
            base.Draw(gameTime);
        }

        public void AddBullet(GameTime gTime)
        {
            lTime += (float)gTime.ElapsedGameTime.TotalSeconds;
            if (keyboard.IsKeyDown(Keys.Space) && !oldKeyboardState.IsKeyDown(Keys.Space))
            {
                if (lTime > shootRate)
                {
                    BulletList.Add(new Weapon(Game, ref actionTexture, player.getPosition(), 1));
                    Components.Add(BulletList[BulletList.Count - 1]);
                    BulletList.Add(new Weapon(Game, ref actionTexture, player.getPosition(), 2));
                    Components.Add(BulletList[BulletList.Count - 1]);
                    BulletList.Add(new Weapon(Game, ref actionTexture, player.getPosition(), 3));
                    Components.Add(BulletList[BulletList.Count - 1]);
                    AudioManager.Effect("luger");
                    lTime = 0;
                }
            }
        }

        public void ResetScene()
        {
            for (int x = 0; x <= Enemies.Count - 1; x++)
            {
                Enemies[x].PutinStartPosition();
            }
            for (int x = 0; x <= BulletList.Count - 1; x++)
            {
                BulletList[x].mPosition.Y = -10;
            } 
        }

        public void SetGameOver()
        {
            mGameOver = !mGameOver;
        }

        public bool GameOverState()
        {
            return mGameOver;
        }

       
    }
}
