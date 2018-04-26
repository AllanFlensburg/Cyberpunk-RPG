using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberPunkRPG
{
    class ProjectileManager
    {
        public List<Projectile> playerProjectileList = new List<Projectile>();
        public List<Projectile> enemyProjectileList = new List<Projectile>();

        public ProjectileManager()
        {

        }

        public void Update(GameTime gameTime)
        {
            foreach (Projectile p in playerProjectileList)
            {
                p.Update(gameTime);
                if (p.Visible == false)
                {
                    playerProjectileList.Remove(p);
                    break;
                }
            }

            foreach (Projectile e in enemyProjectileList)
            {
                e.Update(gameTime);
                if (e.Visible == false)
                {
                    enemyProjectileList.Remove(e);
                    break;
                }
            }

            UpdateProjectiles(gameTime);
        }

        private void UpdateProjectiles(GameTime gameTime)
        {
            //foreach (Projectile p in playerProjectileList)
            //{
            //    p.Update(gameTime);
            //    if (p.Visible == false)
            //    {
            //        playerProjectileList.Remove(p);
            //        break;
            //    }
            //}
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (Projectile p in playerProjectileList)
            {
                p.Draw(sb);
            }

            foreach (Projectile e in enemyProjectileList)
            {
                e.Draw(sb);
            }
        }
    }
}
