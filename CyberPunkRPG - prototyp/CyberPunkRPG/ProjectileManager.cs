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
        public List<Projectile> projectileList = new List<Projectile>();

        public ProjectileManager()
        {

        }

        public void Update(GameTime gameTime)
        {
            UpdateProjectiles(gameTime);
        }

        private void UpdateProjectiles(GameTime gameTime)
        {
            foreach (Projectile p in projectileList)
            {
                p.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (Projectile p in projectileList)
            {
                p.Draw(sb);
            }
        }
    }
}
