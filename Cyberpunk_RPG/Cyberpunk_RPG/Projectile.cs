using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyberpunk_RPG
{
    class Projectile
    {
        Vector2 pos;
        Vector2 speed;
        Vector2 direction;
        const int maxDistance = 500;
        float scale = 0.1f;
        Vector2 startPosition;
        public bool Visible = false;
        public Rectangle hitBox;
        Texture2D projectileTex;

        public Projectile(Texture2D projectileTex, Vector2 pos, Vector2 speed, Vector2 direction)
        {
            this.pos = pos;
            this.projectileTex = projectileTex;
            this.speed = speed;
            this.direction = direction;
        }

        public void Update(GameTime gameTime)
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

        public void Draw(SpriteBatch sb)
        {
            if (Visible)
            {
                sb.Draw(projectileTex, pos, null, Color.White, 0, new Vector2((projectileTex.Width / 2) * scale, (projectileTex.Height / 2) * scale), scale, SpriteEffects.None, 1);
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
