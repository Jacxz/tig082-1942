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

        private CollisionDetection collisionDetection;
        private Level level;
        private EnemyManager enemyManager;
        private PowerUpManager powerUpManager;
     
        private int screenheight, screenwidth, oldLives;
        private ScrollingBackground currentBackground;

        private float lTime, shootRate = 0.15f, timeOnLevel = 0;

        private WeaponManager weaponManager;
	    private int score = 0;
        private bool mGameOver = false, bossMode = false;

        //font
        private SpriteFont gameFont;

        private List<Enemy> enemies = new List<Enemy>();
        private List<Weapon> bulletList = new List<Weapon>(), enemyBulletList = new List<Weapon>();

        private KeyboardState oldKeyboardState, keyboard;

        public ActionScene(Game game, Texture2D theTexture, Texture2D backGroundTexture, 
            SpriteFont smallFont)
            : base(game)
        {
            gameFont = smallFont;
            actionTexture = theTexture;
            mBackgroundTexture = backGroundTexture;
          
            enemyManager = new EnemyManager(game, actionTexture);
            weaponManager = new WeaponManager(game, actionTexture);
            powerUpManager = new PowerUpManager(game, actionTexture);
            collisionDetection = new CollisionDetection(game);

            //starts the level script
            level = new Level(game);
            Components.Add(level);

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
            base.Show();
        }

        public override void Hide()
        {
            MediaPlayer.Stop();
            ResetScene();
            base.Hide();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            CheckCollisions(); // checks collition with everything, everything!!! okey not the edges.
            weaponManager.Update(gameTime);
            AddBullet(gameTime);
            currentBackground.Update(gameTime);

            keyboard = Keyboard.GetState();
           
            if (oldLives != player.GetLives())
            {
                ResetScene();
                oldLives = player.GetLives();
            }

            if (player.GetLives() < 0)
            {
                mGameOver = true;
                player.ResetLives();
            }

		    if (timeOnLevel > 1 && bossMode == false)
		    {
		      //  BossMode();
               // bossMode = true;
		    }

            oldKeyboardState = keyboard;
            timeOnLevel += (float)gameTime.ElapsedGameTime.TotalSeconds;
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
            
              
            player.PutInStartPosition();

		    timeOnLevel = 0.0f;
        }

        public void CheckCollisions()
        {
            collisionDetection.CheckPlayerVSEnemy(player, level.GetCurrentEnemys());
            collisionDetection.CheckPlayerBulletVSEnemy(weaponManager.GetWeaponList(), level.GetCurrentEnemys());
            collisionDetection.CheckPlayerVSEnemyBullet(player, level.GetCurrentEnemys());
            collisionDetection.CheckPlayerVSPowerUp(player, powerUpManager.getPowerUpList()); 
           
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            mSpriteBatch.Begin();

            currentBackground.Draw(mSpriteBatch);

            mSpriteBatch.DrawString(gameFont, "Player Score: " + score +
                "\nActionScene EnemyCounts: " + level.GetCurrentEnemys().Count +                
                "\nBullet count: " + weaponManager.GetWeaponList().Count +
                "\nLevel Time: "+ level.GetTime(), new Vector2(15, 15), Color.White);
            
            mSpriteBatch.End();
            base.Draw(gameTime);
        }

        public void AddBullet(GameTime gTime)
        {
            lTime += (float)gTime.ElapsedGameTime.TotalSeconds;
            if (keyboard.IsKeyDown(Keys.LeftControl) && !player.Killed)
            {
                if (lTime > shootRate)
                {
                    weaponManager.AddBullet(player.GetCurrentWeapon(), player.getPosition());
                    AudioManager.Effect("luger");
                    lTime = 0;
                }
            }
        }

      /*  public void BossMode()
        {
            for (int x = 0; x < enemies.Count; x++)
            {
                enemies[x].SetBossMode();
            }
            enemyManager.AddEnemy(10, 1);
            enemies = enemyManager.GetEnemyList();
            Components.Add(enemies[enemies.Count-1]);
        }*/

        public void ResetScene()
        {
            int x;
            for (x = 0; x < enemies.Count; x++)
            {
                enemies[x].PutinStartPosition();
            }
            for (x = 0; x < bulletList.Count; x++)
            {
                bulletList[x].mPosition.Y = -10;
            }
            for (x = 0; x < enemyBulletList.Count; x++)
            {
                enemyBulletList[x].mPosition.Y = 900;
            }
        }

        public int ResultScore
        {
            get { return score; }
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
