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
        public Vector2 speed;
        public Vector2 direction;
        Vector2 prevPos;
        bool noCollision;
        public bool isHit = false;
        public Rectangle hitBox;
        Player player;
        MapManager map;
        ProjectileManager projectileManager;

        Vector2 projectileStart;
        Vector2 projectileSpeed;
        int maxDistance;
        protected float reloadTimer;
        protected float reloadTime;
        public bool reloading = false;
        protected bool active = true;

        protected int damage;
        public int lives;
        protected double frameTimer;
        protected double frameInterval;
        protected int frame;
        protected int numberOfFrames;
        protected int frameWidth;

        public Enemy(Vector2 pos, Player player, MapManager map, ProjectileManager projectileManager) : base(pos)
        {
            this.pos = pos;
            this.player = player;
            this.map = map;
            reloading = false;
            noCollision = true;
            this.projectileManager = projectileManager;
            sourceRect = new Rectangle(0, 192, 64, 64);
            direction = Vector2.Zero;
            speed = Vector2.Zero;
            projectileSpeed = new Vector2(500, 500);
            maxDistance = 600;
        }

        public override void Update(GameTime gameTime)
        {
            ShootPlayer(gameTime);

            foreach (Wall w in map.wallList)
            {
                if (hitBox.Intersects(w.hitBox))
                {
                    noCollision = false;
                }
                else
                {
                    noCollision = true;
                }
            }
            foreach (Block w in map.blockList)
            {
                if (hitBox.Intersects(w.hitBox))
                {
                    noCollision = false;
                }
                else
                {
                    noCollision = true;
                }
            }

            foreach (Cover c in map.coverList)
            {
                if (hitBox.Intersects(c.hitBox))
                {
                    noCollision = false;
                }
                else
                {
                    noCollision = true;
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
            EnemyBlockCollision();
            EnemyCoverCollision();
            EnemyDoorCollision();
            MakeEnemyActive();

            foreach (Projectile p in projectileManager.playerProjectileList)
            {
                if (hitBox.Intersects(p.hitBox) && isHit == false)
                {
                    p.Visible = false;
                    speed = Vector2.Zero;
                    direction = Vector2.Zero;
                    lives -= p.damage;
                }
            }
        }

        private void UpdateMovement(GameTime gameTime)
        {
            pos += speed * direction * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (active && Vector2.Distance(pos, player.pos) <= 600 && isHit == false)
            {
                speed = new Vector2(100, 100);
                direction = GetDirection(player.pos - pos);
            }
            else
            {
                speed = Vector2.Zero;
            }

            if (isHit || Vector2.Distance(pos, player.pos) <= 200)
            {
                speed = Vector2.Zero;
                direction = Vector2.Zero;

                if (isHit)
                {
                    active = false;
                }
            }
        }

        public Vector2 GetDirection(Vector2 dir)
        {
            Vector2 newDirection = dir;
            return Vector2.Normalize(newDirection);
        }

        private void ShootPlayer(GameTime gameTime)
        {
            projectileStart = pos;

            if (Vector2.Distance(pos, player.pos) <= 600 && isHit == false && reloading == false && player.CurrentHealth > 0 && active)
            {
                createNewProjectile(GetDirection((player.pos + new Vector2(32, 32)) - pos));
                reloading = true;
            }

            if (reloading == true)
            {
                reloadTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (reloadTimer <= 0)
                {
                    reloadTimer = reloadTime;
                    reloading = false;
                }
            }
        }

        private void createNewProjectile(Vector2 direction)
        {
            Projectile projectile = new Projectile(AssetManager.enemyProjectileTex, projectileStart, projectileSpeed, direction, maxDistance, damage, map);
            projectile.distanceCheck(pos);
            projectileManager.enemyProjectileList.Add(projectile);
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

        public void EnemyBlockCollision()
        {
            foreach (Block w in map.blockList)
            {
                if (hitBox.Intersects(w.hitBox))
                {
                    pos = prevPos;

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

        public void EnemyCoverCollision()
        {
            foreach (Cover c in map.coverList)
            {
                if (hitBox.Intersects(c.hitBox))
                {
                    pos = prevPos;

                    if (hitBox.X > c.hitBox.Right - 3)
                    {
                        pos.X += 2;
                    }
                    if (hitBox.X < c.hitBox.Left)
                    {
                        pos.X -= 2;
                    }
                    if (hitBox.Y < c.hitBox.Top)
                    {
                        pos.Y -= 2;
                    }
                    if (hitBox.Y > c.hitBox.Bottom - 3)
                    {
                        pos.Y += 2;
                    }
                }
            }
        }

        public void EnemyDoorCollision()
        {
            foreach (Door d in map.doorList)
            {
                if (hitBox.Intersects(d.doorHitBox) && !d.isInteracted)
                {
                    pos = prevPos;

                    if (hitBox.X > d.doorHitBox.Right - 3)
                    {
                        pos.X += 2;
                    }
                    if (hitBox.X < d.doorHitBox.Left)
                    {
                        pos.X -= 2;
                    }
                    if (hitBox.Y < d.doorHitBox.Top)
                    {
                        pos.Y -= 2;
                    }
                    if (hitBox.Y > d.doorHitBox.Bottom - 3)
                    {
                        pos.Y += 2;
                    }
                }
            }
        }

        public void MakeEnemyActive()
        {
            foreach (Blind b in map.blindList)
            {
                if (hitBox.Intersects(b.blindHitBox))
                {
                    if (b.isBlind)
                    {
                        active = false;
                    }
                    else
                    {
                        reloadTimer = 0.5f;
                        reloading = true;
                        active = true;
                    }
                }
                //else if (Vector2.Distance(pos, player.pos) <= 600 && isHit == false)
                //{
                //    active = true;
                //}
            }

            if (map.blindList.Count == 0 && Vector2.Distance(pos, player.pos) <= 600 && isHit == false)
            {
                active = true;
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
