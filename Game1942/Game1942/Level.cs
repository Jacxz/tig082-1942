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

        private int NumOfEnemies, Amount, KindOfEnemy;
        private float ElapsedTime, TestTime, OldTime;
        private List<Enemy> EnemyList;
        private Texture2D mTexture;
        private EnemyManager enemyManager;
        private List<float> TestList;
        float currentTimeInXML;

        public Level(Game game)
            : base(game)
        {
            mTexture = Game.Content.Load<Texture2D>("1945");
            enemyManager = new EnemyManager(game, mTexture);
            TestList = new List<float>();
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

            base.Update(gameTime);
        }

        public List<Enemy> GetCurrentEnemys()
        {
            return EnemyList = enemyManager.GetEnemyList();
        }

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
                            enemyManager.AddEnemy(KindOfEnemy, Amount);
                        }
                        break;
                    }
                }
            }
        }
    }
}