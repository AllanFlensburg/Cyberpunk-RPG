using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberPunkRPG
{
    class Enemy : GameObject
    {
        public Vector2 speed;
        public bool isHit = false;
        public Rectangle hitBox;

        public Enemy(Vector2 pos) : base(pos)
        {
            this.pos = pos;
            speed = new Vector2(100, 100);
        }

        public override void Update(GameTime gameTime)
        {
            hitBox = new Rectangle((int)pos.X, (int)pos.Y, AssetManager.enemyTex.Width, AssetManager.enemyTex.Height);
        }

        public override void Draw(SpriteBatch sb)
        {
            if (!isHit)
            {
                sb.Draw(AssetManager.enemyTex, pos, Color.White);
            }
            else
            {
                sb.Draw(AssetManager.enemyTex, pos, Color.Black);
            }
        }
    }
}
