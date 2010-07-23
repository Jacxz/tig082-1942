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
                    weaponList.RemoveAt(i);
                }
            }
            base.Update(gameTime);
        }

        public void AddBullet(int type, Vector2 position)
        {
            switch (type)
            {
                case 1:
                    weaponList.Add(new Weapon(Game, ref mTexture, position, new Vector2(37, 169), new Vector2(0, -4), 5));
                    Game.Components.Add(weaponList.ElementAt(weaponList.Count - 1));
                    break;
                case 2:
                    weaponList.Add(new Weapon(Game, ref mTexture, position, new Vector2(4, 169), new Vector2(0, -4), 10));
                    Game.Components.Add(weaponList.ElementAt(weaponList.Count - 1));
                    break;
                case 3:
                    weaponList.Add(new Weapon(Game, ref mTexture, new Vector2(position.X - 10, position.Y), new Vector2(4, 235), new Vector2(-2, -4), 5));
                    Game.Components.Add(weaponList.ElementAt(weaponList.Count - 1));
                    weaponList.Add(new Weapon(Game, ref mTexture, position, new Vector2(37, 169), new Vector2(0, -4), 5));
                    Game.Components.Add(weaponList.ElementAt(weaponList.Count - 1));
                    weaponList.Add(new Weapon(Game, ref mTexture, new Vector2(position.X + 10, position.Y), new Vector2(37, 235), new Vector2(2, -4), 5));
                    Game.Components.Add(weaponList.ElementAt(weaponList.Count - 1));
                    break;
                case 4:
                    weaponList.Add(new Weapon(Game, ref mTexture, new Vector2(position.X - 10, position.Y), new Vector2(4, 235), new Vector2(-2, -4), 5));
                    Game.Components.Add(weaponList.ElementAt(weaponList.Count - 1));
                    weaponList.Add(new Weapon(Game, ref mTexture, position, new Vector2(4, 169), new Vector2(0, -4), 10));
                    Game.Components.Add(weaponList.ElementAt(weaponList.Count - 1));
                    weaponList.Add(new Weapon(Game, ref mTexture, new Vector2(position.X + 10, position.Y), new Vector2(37, 235), new Vector2(2, -4), 5));
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

