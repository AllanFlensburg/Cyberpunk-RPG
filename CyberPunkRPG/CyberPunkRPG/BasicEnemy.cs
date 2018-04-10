using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberPunkRPG
{
    class BasicEnemy : Enemy
    {

        public BasicEnemy(Vector2 pos) : base(pos)
        {
            speed = new Vector2(150, 150);
            damage = 1;
            lives = 1;
            frameTimer = 0;
            frameInterval = 0;
            frame = 0;
            numberOfFrames = 0;
            frameWidth = 0;
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(AssetManager.enemyTex, pos, Color.White);
        }
    }
}
