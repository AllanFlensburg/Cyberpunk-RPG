using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberPunkRPG
{
    class StrongEnemy : Enemy
    {

        public StrongEnemy(Vector2 pos) : base(pos)
        {
            speed = new Vector2(100, 100);
            damage = 1;
            lives = 3;
            frameTimer = 100;
            frameInterval = 100;
            frame = 0;
            numberOfFrames = 9;
            frameWidth = 64;
            sourceRect = new Rectangle(0, 64, 64, 64);
        }

        public override void Draw(SpriteBatch sb)
        {
            if (!isHit)
            {
                {
                    sb.Draw(AssetManager.strongEnemyTex, pos, sourceRect, Color.White);
                }
            }
            else
            {
                {
                    sb.Draw(AssetManager.strongEnemyTex, pos, sourceRect, Color.Red);
                }
            }
        }
    }
}
