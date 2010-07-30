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
    
    public class CollisionDetection : Microsoft.Xna.Framework.GameComponent
    {

        private List<Weapon> enemyBulletList;
        private int TotalScore;

        public CollisionDetection(Game game)
            : base(game)
        {


        }

        public int GetScore()
        {
            return TotalScore;
        }

        public void CheckPlayerBulletVSEnemy(List<Weapon> bulletList, List<Enemy> enemies)
        {
            // check if enemy collides with a player bullet, if it collides move the weapon outside of the screen.

            for (int x = 0; x <= bulletList.Count - 1; x++)
            {
                for (int y = 0; y <= enemies.Count - 1; y++)
                {
                    if (enemies[y].checkCollision(bulletList[x].GetBounds()))
                    {
                        enemies[y].isHit(bulletList[x].GetDmg());
                        bulletList[x].mPosition.Y = -100;
                        TotalScore += enemies[y].IsDead();
                    }
                }
            }
        }

        public void CheckPlayerVSEnemyBullet(Player player, List<Enemy> enemies)
        {
            // check collision between player and enemybullets
            for (int x = 0; x < enemies.Count; x++)
            {
                enemyBulletList = enemies[x].GetBulletList();
                if (enemyBulletList != null)
                {
                    for (int y = 0; y < enemyBulletList.Count; y++)
                    {
                        if (enemyBulletList[y].checkCollision(player.GetBounds()))
                        {
                            player.IsHit(enemyBulletList[y].GetDmg());
                            enemyBulletList[y].mPosition.Y = 900;
                        }
                    }
                }
            }
        }

        public void CheckPlayerVSEnemy(Player player, List<Enemy> enemies)
        {
            // check collision enemys vs player and subtracts 5 hp from player each hit.
            for (int x = 0; x < enemies.Count; x++)
            {
                if (enemies[x].checkCollision(player.GetBounds()))
                {
                    if (enemies[x].GetIsDead())
                    {
                        player.IsHit(5);
                    }
                    enemies[x].isHit(10);
                }
            }
        }

        public void CheckPlayerVSPowerUp(Player player, List<PowerUp> powerUps)
        {
            // check collision enemys vs player and subtracts 5 hp from player each hit.
            for (int x = 0; x < powerUps.Count; x++)
            {
                if (powerUps[x].checkCollision(player.GetBounds()))
                {
                    //Game.Components.Remove(powerUps.ElementAt(x));
                    if (!powerUps[x].GetUsed())
                    {
                        player.UpgradeWeapon(1);
                        powerUps[x].SetUsed();
                    }
                }
            }
        }


    }
}