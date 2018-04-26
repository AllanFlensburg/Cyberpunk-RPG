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

        public StrongEnemy(Vector2 pos, Player player, MapManager map, ProjectileManager projectileManager) : base(pos, player, map, projectileManager)
        {
            speed = new Vector2(100, 100);
            damage = 20;
            lives = 30;
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
            if (lives == 30)
            {
                {
                    sb.Draw(AssetManager.doorTex, hitBox, hitBox, Color.Red); //Ritar ut enemy hitbox för testning
                    sb.Draw(AssetManager.strongEnemyTex, pos, sourceRect, Color.White);
                    sb.Draw(AssetManager.assaultRifleTex, pos, sourceRect, Color.White, 0, new Vector2(), 1, SpriteEffects.None, 1);
                }
            }
            else if (lives < 30 && lives > 10)
            {
                {
                    sb.Draw(AssetManager.strongEnemyTex, pos, sourceRect, Color.Yellow);
                    sb.Draw(AssetManager.assaultRifleTex, pos, sourceRect, Color.Yellow, 0, new Vector2(), 1, SpriteEffects.None, 1);
                }
            }
            else if (lives < 20 && lives > 0)
            {
                sb.Draw(AssetManager.strongEnemyTex, pos, sourceRect, Color.Orange);
                sb.Draw(AssetManager.assaultRifleTex, pos, sourceRect, Color.Orange, 0, new Vector2(), 1, SpriteEffects.None, 1);
            }
            else if (lives <= 0)
            {
                sb.Draw(AssetManager.strongEnemyTex, pos, sourceRect, Color.Red);
                sb.Draw(AssetManager.assaultRifleTex, pos, sourceRect, Color.Red, 0, new Vector2(), 1, SpriteEffects.None, 1);
            }
        }
    }
}
