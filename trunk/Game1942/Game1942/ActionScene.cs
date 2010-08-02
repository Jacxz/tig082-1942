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
        private PowerUpManager powerUpManager;
     
        private int screenheight, screenwidth, oldLives;
        private ScrollingBackground currentBackground;

        private float weaponCycle1, weaponCycle2, shootRate1 = 0.15f, shootRate2 = 1.5f, timeOnLevel = 0, ePos, pPos;

        private WeaponManager weaponManager;
	    private int score = 0;
        private bool mGameOver = false, stillValid = true;
        private Vector2 mePos, mpPos, mClosePos;

        //font
        private SpriteFont gameFont;

        private List<Enemy> enemies = new List<Enemy>(), chosenTargets = new List<Enemy>();
        private Enemy closestEnemy;
        private List<Weapon> bulletList = new List<Weapon>(), enemyBulletList = new List<Weapon>();

        private KeyboardState oldKeyboardState, keyboard;

        public ActionScene(Game game, Texture2D theTexture, Texture2D backGroundTexture, 
            SpriteFont smallFont)
            : base(game)
        {
            gameFont = smallFont;
            actionTexture = theTexture;
            mBackgroundTexture = backGroundTexture;
          
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
            if(GameOverState())
            {
                SetGameOver();
                Start();
            }
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
           
            if (oldLives > player.GetLives())
            {
                ResetScene();
                oldLives = player.GetLives();
            }

            if (player.GetLives() < 0)
            {
                player.ResetLives();
                mGameOver = true;
            }

            
            
            powerUpManager.Update(gameTime);
            enemies = level.GetCurrentEnemys();
            for (int x = 0; x < enemies.Count; x++)
            {
                if (enemies[x].GetLevelWon())
                {
                    SetGameOver();
                }

                if (enemies.Count > 0)
                {
                    // forwards the players position, so that enemies can shoot in its direction
                    enemies[x].SetPlayerPosition(player.getPosition());

                    // This checks all enemies and adds a powerup if the enemy has not yet created one
                    if (enemies[x].GetIfPowerUpDropped())
                    {
                        if (!enemies[x].GetHasCreatedPowerUp())
                        {
                            enemies[x].SetHasCreatedPowerUp();
                            if (enemies[x].GetPowerUpType() != 0)
                            {
                                powerUpManager.AddPowerUp(enemies[x].GetPowerUpType(), 0, 1, enemies[x].GetPosition());
                            }
                        }
                    }
                }
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
            else
            {                
                player.ResetLives();
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
                mSpriteBatch.DrawString(gameFont, "Player Score: " + collisionDetection.GetScore() +
                    "\nActionScene EnemyCounts: " + level.GetCurrentEnemys().Count +
                    "\nBullet count: " + weaponManager.GetWeaponList().Count +
                    "\nCurrent Weapon: " + player.GetCurrentWeapon() +
                    "\nLevel Time: " + (int)level.GetTime(), new Vector2(15, 15), Color.White, 0, new Vector2(0,0), 0.5f, SpriteEffects.None, 0);
            mSpriteBatch.End();
            base.Draw(gameTime);
        }

        public void AddBullet(GameTime gTime)
        {
            weaponCycle1 += (float)gTime.ElapsedGameTime.TotalSeconds;
            weaponCycle2 += (float)gTime.ElapsedGameTime.TotalSeconds;
            if (keyboard.IsKeyDown(Keys.LeftControl) && !player.Killed)
            {
                if (weaponCycle1 > shootRate1)
                {
                    weaponManager.AddBullet(player.GetCurrentWeapon(), player.getPosition());
                    AudioManager.Effect("luger");
                    weaponCycle1 = 0;
                }
                if (weaponCycle2 > shootRate2)
                {
                    findClosestEnemies();
                    if (chosenTargets.Count > 0)
                    {
                        for (int z = 0; z < Math.Min(chosenTargets.Count, player.GetCurrentMissiles()); z++)
                        {
                            weaponManager.AddBullet(50, player.getPosition(), chosenTargets[z].GetMiddlePosition(), closestEnemy.GetSpeed(), gTime);
                        }
                    }
                    weaponCycle2 = 0;
                }
            }
        }

        public void findClosestEnemies()
        {
            enemies = level.GetCurrentEnemys();
            chosenTargets = new List<Enemy>();
            closestEnemy = null;
            for (int i = 0; i < player.GetCurrentMissiles(); i++)
            {
                for (int x = 0; x < enemies.Count; x++)
                {
                    if (closestEnemy != null)
                    {
                        mClosePos = closestEnemy.GetMiddlePosition();
                        mpPos = player.getPosition();
                        mpPos.Y -= 200; // favors targets slightly in front of the player
                        mePos = enemies[x].GetPosition();
                        if ((float)Math.Sqrt((mpPos.X - mePos.X) * (mpPos.X - mePos.X) + (mpPos.Y - mePos.Y) * (mpPos.Y - mePos.Y))
                            <
                            (float)Math.Sqrt((mpPos.X - mClosePos.X) * (mpPos.X - mClosePos.X) + (mpPos.Y - mClosePos.Y) * (mpPos.Y - mClosePos.Y)))
                        {
                            stillValid = true;
                            for (int z = 0; z < chosenTargets.Count; z++)
                            {
                                if (mePos == chosenTargets[z].GetPosition())
                                {
                                    stillValid = false;
                                }
                            }
                            if (stillValid)
                            {
                                closestEnemy = enemies[x];
                                chosenTargets.Add(closestEnemy);
                            }
                        }
                    }
                    else
                    {
                        closestEnemy = enemies[x];
                        chosenTargets.Add(closestEnemy);
                    }
                }
            }
        }

        public void ResetScene()
        {
            player.UpgradeWeapon(-1);
            level.Reset();
            RemoveBullets();
        }

        public int ResultScore
        {
            get { return collisionDetection.GetScore() + 1000; }
        }

        public void SetGameOver()
        {
            RemoveBullets();
            level.TotalReset();
            player.SetCurrentWeapon(1);
            mGameOver = !mGameOver;
        }

        public bool GameOverState()
        {
            return mGameOver;
        }

        public void RemoveBullets()
        {
            weaponManager.RemoveBullets();            
        }
    }
}
