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


namespace Game1942
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class EnemyManager : Microsoft.Xna.Framework.GameComponent
    {
        private Texture2D mTexture;
        private Enemy mEnemy;
        private List<Enemy> mEnemyList;
        private int mWidth, mHeight, mStartX, mStartY;

        // ska kanske användas
        //public int StartY
        //{
        //    get { return mStartY; }
        //    set { mStartY = value; }
        //}

        //public int StartX
        //{
        //    get { return mStartX; }
        //    set { mStartX = value; }
        //}

        //public int Height
        //{
        //    get { return mHeight; }
        //    set { mHeight = value; }
        //}

        //public int Width
        //{
        //    get { return mWidth; }
        //    set { mWidth = value; }
        //} 


        public EnemyManager(Game game, Texture2D texture)
            : base(game)
        {
            mTexture = texture;
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

            base.Update(gameTime);
        }

        public void AddEnemy(int kind, int amount)
        {

            // read and set the variables from the xml with the kind variabel as a identifier.

            for (int x = 0; x < amount; x++)
            {
                mEnemy = new Enemy(Game, ref mTexture, mWidth, mHeight, mStartX, mStartY);
                Game.Components.Add(mEnemy);
                mEnemyList.Add(mEnemy);
            }
        }



        public List<Enemy> EnemyList()
        {
            return mEnemyList;
        }

        public int StartY
        {            
            set { mStartY = value; }
        }

        public int StartX
        {            
            set { mStartX = value; }
        }

        public int Height
        {         
            set { mHeight = value; }
        }

        public int Width
        {         
            set { mWidth = value; }
        }
    }
}