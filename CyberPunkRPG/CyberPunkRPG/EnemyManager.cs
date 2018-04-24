using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberPunkRPG
{
    class EnemyManager
    {
        public List<Enemy> enemyList = new List<Enemy>();

        public EnemyManager()
        {

        }

        public void Update(GameTime gameTime)
        {
            foreach (Enemy e in enemyList)
            {
                e.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (Enemy e in enemyList)
            {
                e.Draw(sb);
            }
        }
    }
}
