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
        Vector2 prevPos;
        bool noCollision;
        public bool isHit = false;
        public Rectangle hitBox;
        Player player;
        MapManager map;

        protected int damage;
        protected int lives;
        protected double frameTimer;
        protected double frameInterval;
        protected int frame;
        protected int numberOfFrames;
        protected int frameWidth;

        public Enemy(Vector2 pos, Player player, MapManager map) : base(pos)
        {
            this.pos = pos;
            this.player = player;
            this.map = map;
            sourceRect = new Rectangle(0, 192, 64, 64);
            direction = Vector2.Zero;
            speed = Vector2.Zero;
        }

        public override void Update(GameTime gameTime)
        {
            foreach (Wall w in map.wallList)
            {
                if (hitBox.Intersects(w.hitBox))
                {
                    noCollision = false;
                }
            }
            if (noCollision)
            {
                prevPos = pos;
            }

            hitBox = new Rectangle((int)pos.X + 20, (int)pos.Y + 10, 25, 55);
            UpdateMovement(gameTime);
            Animation(gameTime);
            EnemyWallCollision();

            foreach (Projectile p in player.projectileList)
            {
                if (hitBox.Intersects(p.hitBox) && isHit == false)
                {
                    isHit = true;
                    p.Visible = false;
                    speed = Vector2.Zero;
                }
            }
        }

        private void UpdateMovement(GameTime gameTime)
        {
            pos += speed * direction * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Vector2.Distance(pos, player.pos) < 300 & isHit == false)
            {
                speed = new Vector2(100, 100);
                direction = GetDirection(player.pos - pos);
            }
        }

        public Vector2 GetDirection(Vector2 dir)
        {
            Vector2 newDirection = dir;
            return Vector2.Normalize(newDirection);
        }

        private void Animation(GameTime gameTime)
        {
            if (frameTimer <= 0)
            {
                frameTimer = frameInterval;
                frame++;
                sourceRect.X = (frame % numberOfFrames) * frameWidth;
            }

            if (direction.X < 0 && direction.X < direction.Y)
            {
                sourceRect.Y = 64;
                frameTimer -= gameTime.ElapsedGameTime.TotalMilliseconds;
            }
            else if (direction.X > 0 && direction.X > direction.Y)
            {
                sourceRect.Y = 192;
                frameTimer -= gameTime.ElapsedGameTime.TotalMilliseconds;
            }
            else if (direction.Y > 0 && direction.Y > direction.X)
            {
                sourceRect.Y = 128;
                frameTimer -= gameTime.ElapsedGameTime.TotalMilliseconds;
            }
            else if (direction.Y < 0 && direction.Y < direction.X)
            {
                sourceRect.Y = 0;
                frameTimer -= gameTime.ElapsedGameTime.TotalMilliseconds;
            }
        }

        public void EnemyWallCollision()
        {
            foreach (Wall w in map.wallList)
            {
                if (hitBox.Intersects(w.hitBox))
                {
                    pos = prevPos;
                    //hitBox.X = (int)pos.X + 20;
                    //hitBox.Y = (int)pos.Y + 10;

                    if (hitBox.X > w.hitBox.Right - 3)
                    {
                        pos.X += 2;
                    }
                    if (hitBox.X < w.hitBox.Left)
                    {
                        pos.X -= 2;
                    }
                    if (hitBox.Y < w.hitBox.Top)
                    {
                        pos.Y -= 2;
                    }
                    if (hitBox.Y > w.hitBox.Bottom - 3)
                    {
                        pos.Y += 2;
                    }
                }
            }
        }

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
