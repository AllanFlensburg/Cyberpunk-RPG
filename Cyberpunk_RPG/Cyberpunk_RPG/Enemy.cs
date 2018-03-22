using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyberpunk_RPG
{
    class Enemy
    {
        Texture2D enemyTex;
        public Vector2 pos;
        public Vector2 speed;
        public bool isHit = false;
        public Rectangle hitBox;

        public Enemy(Texture2D enemyTex, Vector2 pos)
        {
            this.enemyTex = enemyTex;
            this.pos = pos;
            speed = new Vector2(100, 100);
        }

        public void Update(GameTime gameTime)
        {
            hitBox = new Rectangle((int)pos.X, (int)pos.Y, enemyTex.Width, enemyTex.Height);
        }

        public void Draw(SpriteBatch sb)
        {
            if (!isHit)
            {
                sb.Draw(enemyTex, pos, Color.White);
            }
            else
            {
                sb.Draw(enemyTex, pos, Color.Black);
            }
        }
    }
}
