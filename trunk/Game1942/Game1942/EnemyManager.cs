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
using System.Xml;
using System.IO;


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
        private int mWidth, mHeight, mStartX, mStartY, mHP, score;
        

       
        public EnemyManager(Game game, Texture2D texture)
            : base(game)
        {
            mTexture = texture;
            mEnemyList = new List<Enemy>();
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
            base.Update(gameTime);
        }

        public void Reset()
        {
            for (int x = 0; x < mEnemyList.Count; x++)
            {
                mEnemyList[x].RemoveBullets();
            }
            for (int x = 0; x < mEnemyList.Count; x++)
            {
                Game.Components.Remove(mEnemyList.ElementAt(x));
                
            }
            mEnemyList.Clear();
        }
        //removes the component and removes the object from the list
        public void IfDead()
        {
            for (int x = 0; x < mEnemyList.Count; x++)
            {
                if (mEnemyList[x].IsDone())
                {
                    Game.Components.Remove(mEnemyList.ElementAt(x));
                    mEnemyList.RemoveAt(x);
                }
            }
        }

        public void AddEnemy(int type, int amount, float xSpeed, float ySpeed, int xPos, int yPos, int powerUpType)
        {
            
            // read and set the variables from the xml with the type variabel as a identifier.
            ReadXML(type);
            for (int x = 0; x < amount; x++)
            {
                mEnemy = new Enemy(Game, mTexture, mHP, type, xSpeed, ySpeed, xPos, yPos, powerUpType, score);
                mEnemyList.Add(mEnemy);
                Game.Components.Add(mEnemy);
            }
        }

        

        public List<Enemy> GetEnemyList()
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
        public int HP
        {
            set { mHP = value; }
        }
        
        // reads from XML File
        public void ReadXML(int type)
        {
            // loads the xml file
            XmlTextReader txtRead = new XmlTextReader(@"..\..\..\Content/Enemies.xml");
            //while it reads
            while (txtRead.Read())
            {   
                // if the type matches the node "type" in the xml
                if (txtRead.Name == "type")
                {
                    // if the value type is holding equals the type we want
                    if (txtRead.ReadElementContentAsInt() == type)
                    {
                        //moves one element and reads the value.
                        txtRead.Read();
                        HP = txtRead.ReadElementContentAsInt();
                        txtRead.Read();
                        score = txtRead.ReadElementContentAsInt();
                       
                        break;
                    }
                }
            }
        }
    }
}

