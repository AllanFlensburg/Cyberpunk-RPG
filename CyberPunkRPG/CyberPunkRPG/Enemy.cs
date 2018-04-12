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
        protected Rectangle sourceRect;
        public Vector2 speed; // Lägg till i konstruktor // Kollisionen för spelaren mot väggar är fel på grund av draw-metoden (Player andvänder origin)
        public Vector2 direction; // Lägg till
        public bool isHit = false;
        public Rectangle hitBox;

        protected int damage;
        protected int lives;
        protected double frameTimer;
        protected double frameInterval;
        protected int frame;
        protected int numberOfFrames;
        protected int frameWidth;

        public Enemy(Vector2 pos) : base(pos)
        {
            this.pos = pos;
            sourceRect = new Rectangle(0, 64, 64, 64);
            speed = new Vector2(100, 100);
        }

        public override void Update(GameTime gameTime)
        {
            hitBox = new Rectangle((int)pos.X, (int)pos.Y, 64, 64);
            //frameTimer -= gameTime.ElapsedGameTime.TotalMilliseconds;
            //Animation(gameTime);
            //UpdateMovement(gameTime);
        }

        //private void UpdateMovement(GameTime gameTime)
        //{
        //    pos += speed * direction * (float)gameTime.ElapsedGameTime.TotalSeconds;
        //}

        //private void Animation(GameTime gameTime)
        //{
        //    if (frameTimer <= 0)
        //    {
        //        frameTimer = frameInterval;
        //        frame++;
        //        sourceRect.X = (frame % numberOfFrames) * frameWidth;
        //    }
        //}

        public override void Draw(SpriteBatch sb)
        {
            if (!isHit)
            {
                {
                    sb.Draw(AssetManager.basicEnemyTex, pos, sourceRect, Color.White, 0f, new Vector2(), 1f, SpriteEffects.None, 1);
                }
            }
            else
            {
                {
                    sb.Draw(AssetManager.basicEnemyTex, pos, sourceRect, Color.Black, 0f, new Vector2(), 1f, SpriteEffects.None, 1);
                }
            }
        }
    }
}
