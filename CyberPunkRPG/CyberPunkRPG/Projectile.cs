using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberPunkRPG
{
    class Projectile : GameObject
    {
        Vector2 speed;
        Vector2 direction;
        const int maxDistance = 500;
        float scale = 0.15f;
        Vector2 startPosition;
        public bool Visible = false;
        public Rectangle hitBox;

        public Projectile(Vector2 pos, Vector2 speed, Vector2 direction) : base(pos)
        {
            this.pos = pos;
            this.speed = speed;
            this.direction = direction;
        }

        public override void Update(GameTime gameTime)
        {
            if (Visible)
            {
                pos += speed * direction * (float)gameTime.ElapsedGameTime.TotalSeconds;
                hitBox = new Rectangle((int)pos.X, (int)pos.Y, 9, 7);
            }

            if (Vector2.Distance(startPosition, pos) > maxDistance)
            {
                Visible = false;
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            if (Visible)
            {
                sb.Draw(AssetManager.projectileTex, pos, null, Color.White, 0, new Vector2((AssetManager.projectileTex.Width / 2) * scale, (AssetManager.projectileTex.Height / 2) * scale), scale, SpriteEffects.None, 1);
            }
        }

        public void distanceCheck(Vector2 theStartPosition) // Metod för en ny projektil
        {
            pos = theStartPosition;
            startPosition = theStartPosition;
            Visible = true;
        }
    }
}
