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
        protected Texture2D mBackgroundTexture, actionTexture, blockTextureData;
        protected SpriteBatch mSpriteBatch;
        GraphicsDeviceManager graphics;
        protected Player player;
        private CollisionDetection mCollison;

        protected SoundEffect mExplosion;

        List<Weapon> bulletList = new List<Weapon>();
        SpriteFont font;

        //private Enemy enemy; // ändrat kod
        private List<Enemy> Enemies = new List<Enemy>(); // ändrat kod

     
        // error variables
        int error;

        //font
        private SpriteFont gameFont;

        private KeyboardState oldKeyboardState;

        int k = 4;

        public ActionScene(Game game, Texture2D theTexture, Texture2D backGroundTexture, SpriteFont smallFont, SoundEffect explosion)
            : base(game)
        {
            font = smallFont;
            actionTexture = theTexture;
            mBackgroundTexture = backGroundTexture;
            mExplosion = explosion;

            // creates and puts the player in start position
            Start();

            oldKeyboardState = Keyboard.GetState();
            mSpriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
            mCollison = new CollisionDetection(player, bulletList, Enemies, actionTexture);
        }

        protected override void LoadContent()
        {
           // mExplosion = Game.Content.Load<SoundEffect>("luger");
            gameFont = Game.Content.Load<SpriteFont>("font");
            base.LoadContent();
            error++;
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
                mExplosion.Play();
            }



            // removes the bullet when it reached the end of the screen
            for (int i = 0; i < bulletList.Count; i++)
            {
                if (bulletList[i].mPosition.Y < (0))
                {
                    bulletList.RemoveAt(i);
                }
            }
           // CheckCollisions(); // checks collition with enemy List

            oldKeyboardState = keyboard;
            base.Update(gameTime);
        }
        private void Start() // ändrat kod
        {
            if (player == null)
            {
                player = new Player(Game, ref actionTexture);
                player.Initialize();
                Components.Add(player);
              //  mCollison = new CollisionDetection(player, bulletList, Enemies, actionTexture);
            }
            for (int x = 0; x < 10; x++)// ändrat kod
            {
             
                Enemies.Add(new Enemy(Game, ref actionTexture));
                Components.Add(Enemies[Enemies.Count -1]);
            }

            player.PutInStartPosition();
        }

        public void CheckCollisions()// ändrat kod
        {
           
            
            /*for (int x = 0; x < Enemies.Count - 1; x++)
            {
                if (Enemies[x].checkCollision(player.GetBounds()))
                {
                    error++;
                    player.IsHit();
                }
            }
             */
            
           
            //mCollison.fuckIT();
            mCollison.checkCollision();           


        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            mSpriteBatch.Begin();

            mSpriteBatch.DrawString(gameFont, "ActionScene Bullets: " + bulletList.Count.ToString()+ "\nActionScene Hits: "+ error.ToString(), new Vector2(15, 15), Color.White);
            mSpriteBatch.End();
            base.Draw(gameTime);

        }
    }
}