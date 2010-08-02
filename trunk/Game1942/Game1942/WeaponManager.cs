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
    public class WeaponManager : Microsoft.Xna.Framework.GameComponent
    {
        private Texture2D mTexture;
        private List<Weapon> weaponList;
        int weaponDmg;
        new float xValue, yValue, weaponSpeed; 

        public WeaponManager(Game game, Texture2D texture)
            : base(game)
        {
            mTexture = texture;
            weaponList = new List<Weapon>();
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < weaponList.Count; i++)
            {
                if (weaponList[i].mPosition.Y < 0 || weaponList[i].mPosition.Y > 800)
                {
                    Game.Components.Remove(weaponList.ElementAt(i));
                    weaponList.RemoveAt(i);
                }
            }
            base.Update(gameTime);
        }

        // Adds a bullet
        // @position: the position of the ship that shoots
        public void AddBullet(int type, Vector2 position)
        {
            switch (type)
            {
                case 1: // one single bullet straight doing 5dmg
                    weaponList.Add(new Weapon(Game, ref mTexture, new Vector2(position.X + 11, position.Y - 16), new Vector2(48, 176), 9, 20, -4f, 5));
                    Game.Components.Add(weaponList.ElementAt(weaponList.Count - 1));
                    break;
                case 2: // one single bullet straight with 10dmg using a texture visually displaying two bullets.
                    weaponList.Add(new Weapon(Game, ref mTexture, new Vector2(position.X + 7, position.Y - 16), new Vector2(11, 177), 17, 16, -4f, 10));
                    Game.Components.Add(weaponList.ElementAt(weaponList.Count - 1));
                    break;
                case 3: // two bullets straight with sinus movement in the x-axis doing 5dmg each
                    weaponList.Add(new Weapon(Game, ref mTexture, new Vector2(position.X + 4, position.Y - 16), new Vector2(48, 176), 9, 20, new Vector2(4, -4), 5, 25, 8));
                    Game.Components.Add(weaponList.ElementAt(weaponList.Count - 1));
                    weaponList.Add(new Weapon(Game, ref mTexture, new Vector2(position.X + 17, position.Y - 16), new Vector2(48, 176), 9, 20, new Vector2(-4, -4), 5, 25, 8));
                    Game.Components.Add(weaponList.ElementAt(weaponList.Count - 1));
                    break;
                case 4: // three bullets two single on the sides moving outwards on the x-axis doing 5dmg each and one single going straight in the middle doing 5dmg
                    weaponList.Add(new Weapon(Game, ref mTexture, new Vector2(position.X - 1, position.Y - 10), new Vector2(13, 245), 13, 13, new Vector2(-2, -4), 5));
                    Game.Components.Add(weaponList.ElementAt(weaponList.Count - 1));
                    weaponList.Add(new Weapon(Game, ref mTexture, new Vector2(position.X + 11, position.Y - 16), new Vector2(48, 176), 9, 20, -4f, 5));
                    Game.Components.Add(weaponList.ElementAt(weaponList.Count - 1));
                    weaponList.Add(new Weapon(Game, ref mTexture, new Vector2(position.X + 20, position.Y - 10), new Vector2(47, 245), 13, 13, new Vector2(2, -4), 5));
                    Game.Components.Add(weaponList.ElementAt(weaponList.Count - 1));
                    break;
                case 5: // three bullets two single on the sides moving outwards on the x-axis doing 5dmg each and one single going straight in the middle doing 10dmg using a texture visually displaying two bullets.
                    weaponList.Add(new Weapon(Game, ref mTexture, new Vector2(position.X - 4, position.Y - 10), new Vector2(13, 245), 13, 13, new Vector2(-1, -4), 5));
                    Game.Components.Add(weaponList.ElementAt(weaponList.Count - 1));
                    weaponList.Add(new Weapon(Game, ref mTexture, new Vector2(position.X + 7, position.Y - 16), new Vector2(11, 177), 17, 16, -4f, 10));
                    Game.Components.Add(weaponList.ElementAt(weaponList.Count - 1));
                    weaponList.Add(new Weapon(Game, ref mTexture, new Vector2(position.X + 23, position.Y - 10), new Vector2(47, 245), 13, 13, new Vector2(1, -4), 5));
                    Game.Components.Add(weaponList.ElementAt(weaponList.Count - 1));
                    break;
                case 6: // four bullets, two single on the sides moving outwards on the x-axis doing 5dmg each and two bullets straight with sinus movement in the x-axis doing 10dmg each
                    weaponList.Add(new Weapon(Game, ref mTexture, new Vector2(position.X - 4, position.Y - 10), new Vector2(13, 245), 13, 13, new Vector2(-1, -4), 5));
                    Game.Components.Add(weaponList.ElementAt(weaponList.Count - 1));
                    weaponList.Add(new Weapon(Game, ref mTexture, new Vector2(position.X + 6, position.Y - 20), new Vector2(48, 176), 9, 20, new Vector2(4, -4), 10, 25, 10));
                    Game.Components.Add(weaponList.ElementAt(weaponList.Count - 1));
                    weaponList.Add(new Weapon(Game, ref mTexture, new Vector2(position.X + 15, position.Y - 20), new Vector2(48, 176), 9, 20, new Vector2(-4, -4), 10, 25, 10));
                    Game.Components.Add(weaponList.ElementAt(weaponList.Count - 1));
                    weaponList.Add(new Weapon(Game, ref mTexture, new Vector2(position.X + 23, position.Y - 10), new Vector2(47, 245), 13, 13, new Vector2(1, -4), 5));
                    Game.Components.Add(weaponList.ElementAt(weaponList.Count - 1));
                    break;
                case 10: // normal enemy bullet (for 64 pixel enemys)
                    weaponList.Add(new Weapon(Game, ref mTexture, new Vector2(position.X + 27, position.Y + 53), new Vector2(49, 214), 9, 9, 4f, 20));
                    Game.Components.Add(weaponList.ElementAt(weaponList.Count - 1));
                    break;
                case 11: // straight bullet (boss)
                    weaponList.Add(new Weapon(Game, ref mTexture, new Vector2(position.X + 43, position.Y + 87), new Vector2(11, 209), 17, 17, 4f, 40));
                    Game.Components.Add(weaponList.ElementAt(weaponList.Count - 1));
                    break;
                case 12: // side bullet left (boss)
                    weaponList.Add(new Weapon(Game, ref mTexture, new Vector2(position.X + 27, position.Y + 79), new Vector2(79, 243), 15, 15, new Vector2(-3, 3.5f), 25));
                    Game.Components.Add(weaponList.ElementAt(weaponList.Count - 1));
                    break;
                case 13: // side bullet right (boss)
                    weaponList.Add(new Weapon(Game, ref mTexture, new Vector2(position.X + 59, position.Y + 79), new Vector2(113, 243), 15, 15, new Vector2(3, 3.5f), 25));
                    Game.Components.Add(weaponList.ElementAt(weaponList.Count - 1));
                    break;
                case 14: // small enemy side bullet left (for 64 pixel enemys)
                    weaponList.Add(new Weapon(Game, ref mTexture, new Vector2(position.X + 16, position.Y + 50), new Vector2(86, 216), 7, 7, new Vector2(-2, 4), 10));
                    Game.Components.Add(weaponList.ElementAt(weaponList.Count - 1));
                    break;
                case 15: // small enemy side bullet right (for 64 pixel enemys)
                    weaponList.Add(new Weapon(Game, ref mTexture, new Vector2(position.X + 42, position.Y + 53), new Vector2(86, 216), 7, 7, new Vector2(2, 4), 10));
                    Game.Components.Add(weaponList.ElementAt(weaponList.Count - 1));
                    break;
                case 16: // small enemy bullet (for 64 pixel enemys)
                    weaponList.Add(new Weapon(Game, ref mTexture, new Vector2(position.X + 29, position.Y + 50), new Vector2(86, 216), 7, 7, new Vector2(0, 4), 10));
                    Game.Components.Add(weaponList.ElementAt(weaponList.Count - 1));
                    break;
                case 17: // small enemy bullet (for 32 pixel enemys)
                    weaponList.Add(new Weapon(Game, ref mTexture, new Vector2(position.X + 12, position.Y + 28), new Vector2(86, 216), 7, 7, new Vector2(0, 4), 10));
                    Game.Components.Add(weaponList.ElementAt(weaponList.Count - 1));
                    break;
            }
        }

        // Adds a bullet targeted at the player
        // @position: the position of the ship that shoots
        public void AddBullet(int type, Vector2 ePos, Vector2 pPos)
        {
            pPos.X += 16;
            pPos.Y += 16;
            switch (type)
            {
                case 40: // small targeted bullet (boss)
                    weaponSpeed = 5;
                    weaponDmg = 10;

                    ePos.X += 50;
                    ePos.Y += 50;
                    xValue += (pPos.X - ePos.X) / (float)Math.Sqrt((pPos.X - ePos.X) * (pPos.X - ePos.X) + (pPos.Y - ePos.Y) * (pPos.Y - ePos.Y));
                    yValue += (pPos.Y - ePos.Y) / (float)Math.Sqrt((pPos.X - ePos.X) * (pPos.X - ePos.X) + (pPos.Y - ePos.Y) * (pPos.Y - ePos.Y));
                    ePos.X += xValue * 25 - 4;
                    ePos.Y += yValue * 25 - 4;
                    xValue = (pPos.X - ePos.X) / (float)Math.Sqrt((pPos.X - ePos.X) * (pPos.X - ePos.X) + (pPos.Y - ePos.Y) * (pPos.Y - ePos.Y));
                    yValue = (pPos.Y - ePos.Y) / (float)Math.Sqrt((pPos.X - ePos.X) * (pPos.X - ePos.X) + (pPos.Y - ePos.Y) * (pPos.Y - ePos.Y));

                    weaponList.Add(new Weapon(Game, ref mTexture,
                        new Vector2(ePos.X, ePos.Y), 
                        new Vector2(86, 216), 7, 7,
                        new Vector2(xValue * weaponSpeed, yValue * weaponSpeed),weaponDmg));
                    Game.Components.Add(weaponList.ElementAt(weaponList.Count - 1));
                    break;
                case 41: // small targeted bullet (for 64 pixel enemys)
                    weaponSpeed = 4.5f;
                    weaponDmg = 10;

                    ePos.X += 32;
                    ePos.Y += 32;
                    xValue += (pPos.X - ePos.X) / (float)Math.Sqrt((pPos.X - ePos.X) * (pPos.X - ePos.X) + (pPos.Y - ePos.Y) * (pPos.Y - ePos.Y));
                    yValue += (pPos.Y - ePos.Y) / (float)Math.Sqrt((pPos.X - ePos.X) * (pPos.X - ePos.X) + (pPos.Y - ePos.Y) * (pPos.Y - ePos.Y));
                    ePos.X += xValue * 15 - 4;
                    ePos.Y += yValue * 15 - 4;
                    xValue = (pPos.X - ePos.X) / (float)Math.Sqrt((pPos.X - ePos.X) * (pPos.X - ePos.X) + (pPos.Y - ePos.Y) * (pPos.Y - ePos.Y));
                    yValue = (pPos.Y - ePos.Y) / (float)Math.Sqrt((pPos.X - ePos.X) * (pPos.X - ePos.X) + (pPos.Y - ePos.Y) * (pPos.Y - ePos.Y));

                    weaponList.Add(new Weapon(Game, ref mTexture,
                        new Vector2(ePos.X, ePos.Y),
                        new Vector2(86, 216), 7, 7,
                        new Vector2(xValue * weaponSpeed, yValue * weaponSpeed), weaponDmg));
                    Game.Components.Add(weaponList.ElementAt(weaponList.Count - 1));
                    break;
            }
        }
        
        // Adds a missile targeted at a enemy
        // @position: the position of the ship that shoots
        public void AddBullet(int type, Vector2 pPos, Vector2 ePos, Vector2 eSpeed, GameTime gTime)
        {
            switch (type)
            {
                case 50: // seeking missile from player to enemy
                    weaponList.Add(new Weapon(Game, ref mTexture, new Vector2(pPos.X + 11, pPos.Y - 16),
                        new Vector2(4, 532), 18, 12, 30, 6f, 15, ePos, eSpeed, gTime));
                    Game.Components.Add(weaponList.ElementAt(weaponList.Count - 1));
                    break;
            }
        }

        public List<Weapon> GetWeaponList()
        {
            return weaponList;
        }

        public void RemoveBullets()
        {
            
            for(int x = 0; x < weaponList.Count; x++)  
            {
                Game.Components.Remove(weaponList.ElementAt(x));
            }
            weaponList.Clear();
        }
    }
}

