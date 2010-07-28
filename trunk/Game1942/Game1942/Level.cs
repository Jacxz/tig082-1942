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
using System.IO;
using System.Xml;

namespace Game1942
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Level : Microsoft.Xna.Framework.GameComponent
    {

        private int NumOfEnemies, Amount, KindOfEnemy, xPos, yPos;
        private float ElapsedTime, TestTime, xSpeed, ySpeed;
        private List<Enemy> EnemyList;
        private Texture2D mTexture;
        private EnemyManager enemyManager;
    
        float currentTimeInXML;

        public Level(Game game)
            : base(game)
        {
            EnemyList = new List<Enemy>();
            mTexture = Game.Content.Load<Texture2D>("1945");
            enemyManager = new EnemyManager(game, mTexture);
      
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
            ElapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            ReadXML(ElapsedTime);

            //ska det va så?
            enemyManager.IfDead();

            base.Update(gameTime);
        }

        public List<Enemy> GetCurrentEnemys()
        {
            return EnemyList = enemyManager.GetEnemyList();
        }
        // remove i think
        public float GetTime()
        {
            return ElapsedTime;
        }

        // reads from XML File
        public void ReadXML(float time)
        {


            // loads the xml file
            XmlTextReader txtRead = new XmlTextReader(@"..\..\..\Content/Level.xml");
            //while it reads
            while (txtRead.Read())
            {
                // if the type matches the node "time" in the xml
                if (txtRead.Name == "time")
                {
                    currentTimeInXML = txtRead.ReadElementContentAsFloat();
                    // checks if the testtime is smaller then the current xml read time and if the time elapsed is bigger then the current xml read time. This makes it possible to add the enemies in the xml just once. Pretty logical after 1 hour of bashing the keyboard against the the monitor.
                    if (TestTime < currentTimeInXML && time > currentTimeInXML)
                    {
                        // sets the testtime to the latest xml read file (if(TestTime < currentTimeInXML && time > currentTimeInXML)) 
                        TestTime = currentTimeInXML;
                        
                        //moves one element and reads the value, does so for all the elements in the node.
                        txtRead.Read();
                        NumOfEnemies = txtRead.ReadElementContentAsInt();

                        for (int x = 0; x < NumOfEnemies; x++)
                        {
                            
                            txtRead.Read();
                            KindOfEnemy = txtRead.ReadElementContentAsInt();
                            txtRead.Read();
                            Amount = txtRead.ReadElementContentAsInt();
                            txtRead.Read();
                            xSpeed = txtRead.ReadElementContentAsFloat();
                            txtRead.Read();
                            ySpeed = txtRead.ReadElementContentAsFloat();
                            txtRead.Read();
                            xPos = txtRead.ReadElementContentAsInt();
                            txtRead.Read();
                            yPos = txtRead.ReadElementContentAsInt();
                            enemyManager.AddEnemy(KindOfEnemy, Amount, xSpeed, ySpeed, xPos, yPos);
                        }
                        break;
                    }
                }
            }
        }
    }
}