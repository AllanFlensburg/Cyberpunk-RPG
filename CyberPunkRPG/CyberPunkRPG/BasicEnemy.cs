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

        public BasicEnemy(Vector2 pos, Player player, MapManager map, ProjectileManager projectileManager) : base(pos, player, map, projectileManager)
        {
            speed = new Vector2(150, 150);
            damage = 10;
            lives = 10;
            reloadTimer = 1.2f;
            reloadTime = 1.2f;
            frameTimer = 100;
            frameInterval = 100;
            frame = 0;
            numberOfFrames = 9;
            frameWidth = 64;
            //sourceRect = new Rectangle(0, 64, 64, 64);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch sb)
        {
            if (!isHit)
            {
                {
                    sb.Draw(AssetManager.doorTex, hitBox, hitBox, Color.Red); //Ritar ut enemy hitbox för testning
                    sb.Draw(AssetManager.basicEnemyTex, pos, sourceRect, Color.White);
                    sb.Draw(AssetManager.assaultRifleTex, pos, sourceRect, Color.White, 0, new Vector2(), 1, SpriteEffects.None, 1);
                }
            }
            else
            {
                {
                    sb.Draw(AssetManager.basicEnemyTex, pos, sourceRect, Color.Red);
                    sb.Draw(AssetManager.assaultRifleTex, pos, sourceRect, Color.Red, 0, new Vector2(), 1, SpriteEffects.None, 1);
                }
            }
        }
    }
}
