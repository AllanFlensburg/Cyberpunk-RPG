using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberPunkRPG
{
    class RocketLauncher : InteractiveObject
    {
        //Vector2 speed;
        //Vector2 direction;
        //int maxDistance;
        //float scale = 0.4f;
        //Vector2 startPosition;
        //public bool Visible = false;
        //bool explosion = false;
        //public Rectangle hitBox;

        Rectangle sourceRect;

        public RocketLauncher(Vector2 pos) : base(pos)
        {
            sourceRect = new Rectangle(0, 192, 64, 64);
            interactHitBox = new Rectangle((int)pos.X, (int)pos.Y, sourceRect.Width, sourceRect.Height);
            identify = 4;
            //this.direction = direction;
            //this.speed = speed;
            //this.pos = pos;
        }

        public override void Update(GameTime gameTime)
        {
            //if (Visible)
            //{
            //    pos += speed * direction * (float)gameTime.ElapsedGameTime.TotalSeconds;
            //    hitBox = new Rectangle((int)pos.X, (int)pos.Y, 9, 7);
            //}

            //if (Vector2.Distance(startPosition, pos) > maxDistance)
            //{
            //    Visible = false;
            //    explosion = true;
            //}
        }

        public override void Draw(SpriteBatch sb)
        {
            //if (Visible)
            //{
            //    sb.Draw(AssetManager.projectileTex, pos, null, Color.White, 0, new Vector2((AssetManager.projectileTex.Width / 2 * scale), (AssetManager.projectileTex.Height / 2) * scale), scale, SpriteEffects.None, 1);
            //}
            //if (explosion)
            //{
            //    sb.Draw(AssetManager.explosionTex, pos, null, Color.White, 0, new Vector2((AssetManager.explosionTex.Width / 2 * scale), (AssetManager.explosionTex.Height / 2) * scale), scale, SpriteEffects.None, 1);
            //}
        }

    //    public void distanceCheckRocket(Vector2 theStartPosition) // Metod för en ny projektil
    //    {
    //        pos = theStartPosition;
    //        startPosition = theStartPosition;
    //        Visible = true;
    //    }
    }
}
