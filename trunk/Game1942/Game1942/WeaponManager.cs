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
                    weaponList.Add(new Weapon(Game, ref mTexture, new Vector2( position.X + 11, position.Y - 16 ), new Vector2(48, 176), 9, 20, -4f, 5));
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
                    weaponList.Add(new Weapon(Game, ref mTexture, new Vector2(position.X + 6, position.Y - 20), new Vector2(48, 176), 9, 20, new Vector2(4, -4), 5, 25, 10));
                    Game.Components.Add(weaponList.ElementAt(weaponList.Count - 1));
                    weaponList.Add(new Weapon(Game, ref mTexture, new Vector2(position.X + 15, position.Y - 20), new Vector2(48, 176), 9, 20, new Vector2(-4, -4), 5, 25, 10));
                    Game.Components.Add(weaponList.ElementAt(weaponList.Count - 1));
                    weaponList.Add(new Weapon(Game, ref mTexture, new Vector2(position.X + 23, position.Y - 10), new Vector2(47, 245), 13, 13, new Vector2(1, -4), 5));
                    Game.Components.Add(weaponList.ElementAt(weaponList.Count - 1));
                    break;
                case 10: // normal enemy bullet
                    weaponList.Add(new Weapon(Game, ref mTexture, new Vector2(position.X + 27, position.Y + 53), new Vector2(49, 214), 9, 9, 4f, 15));
                    Game.Components.Add(weaponList.ElementAt(weaponList.Count - 1));
                    break;
                case 11: // boss straight bullet
                    weaponList.Add(new Weapon(Game, ref mTexture, new Vector2(position.X + 43, position.Y + 87), new Vector2(49, 214), 9, 9, 4f, 30));
                    Game.Components.Add(weaponList.ElementAt(weaponList.Count - 1));
                    break;
                case 12:
                    weaponList.Add(new Weapon(Game, ref mTexture, new Vector2(position.X + 27, position.Y + 79), new Vector2(49, 214), 9, 9, new Vector2(-2, 4), 30));
                    Game.Components.Add(weaponList.ElementAt(weaponList.Count - 1));
                    break;
                case 13:
                    weaponList.Add(new Weapon(Game, ref mTexture, new Vector2(position.X + 59, position.Y + 79), new Vector2(49, 214), 9, 9, new Vector2(2, 4), 30));
                    Game.Components.Add(weaponList.ElementAt(weaponList.Count - 1));
                    break;
            }
        }

        public List<Weapon> GetWeaponList()
        {
            return weaponList;
        }
    }
}

