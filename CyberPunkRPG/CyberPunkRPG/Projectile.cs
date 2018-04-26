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
        int maxDistance;
        float scale = 0.15f;
        Vector2 startPosition;
        public bool Visible = false;
        public int damage;
        //bool explosion = false;
        //Vector2 explosionPos;

        //private float explosionTimer;
        //bool explosionStart = false;
        //protected double explosionFrameTimer;
        //protected double explosionFrameInterval;
        //protected int explosionFrame;
        //protected int numberOfExplosionFrames;
        //protected int explosionFrameWidth;
        //protected Rectangle explosionRect;
        public Rectangle hitBox;
        MapManager map;

        public Projectile(Vector2 pos, Vector2 speed, Vector2 direction, int maxDistance, int damage, MapManager map) : base(pos)
        {
            this.pos = pos;
            this.speed = speed;
            this.damage = damage;
            this.direction = direction;
            this.maxDistance = maxDistance;
            this.map = map;

            //explosionFrameTimer = 120;
            //explosionFrameInterval = 120;
            //explosionFrame = 0;
            //numberOfExplosionFrames = 3;
        }

        public override void Update(GameTime gameTime)
        {
            //if (explosionStart)
            //{
            //    explosionTimer += gameTime.ElapsedGameTime.Milliseconds;
            //}

            //if (explosionTimer >= 100)
            //{

            //}

            projectileWallCollision();
            projectileCoverCollision();
            projectileDoorCollision();

            if (Visible)
            {
                pos += speed * direction * (float)gameTime.ElapsedGameTime.TotalSeconds;
                hitBox = new Rectangle((int)pos.X, (int)pos.Y, 9, 7);
            }

            if (Vector2.Distance(startPosition, pos) > maxDistance)
            {
                Visible = false;
                //explosion = true;
            }

            //if (explosion)
            //{
            //    hitBox = explosionRect;
            //}
        }

        //Animation för explosion Kommer senare

        //private void ExplosionAnimation(GameTime gameTime)
        //{
        //    if (explosionFrameTimer <= 0)
        //    {
        //        explosionFrameTimer = explosionFrameInterval;
        //        explosionFrame++;
        //        if (explosionFrame == 1)
        //        {
        //            explosionRect = new Rectangle(25, 33, 80, 72);
        //            explosionFrameWidth = 80;
        //        }
        //        else if (explosionFrame == 2)
        //        {
        //            explosionRect = new Rectangle(127, 30, 105, 88);
        //            explosionFrameWidth = 105;
        //        }
        //        else if (explosionFrame == 3)
        //        {
        //            explosionRect = new Rectangle(240, 17, 127, 113);
        //            explosionFrameWidth = 127;
        //        }
        //        explosionRect.X = (explosionFrame % numberOfExplosionFrames) * explosionFrameWidth;
        //    }
        //}
        public void projectileWallCollision()
        {
            foreach (Wall w in map.wallList)
            {
                if (hitBox.Intersects(w.hitBox))
                {
                    Visible = false;
                }
            }
        }

        public void projectileCoverCollision()
        {
            foreach (Cover c in map.coverList)
            {
                if (hitBox.Intersects(c.hitBox))
                {
                    Visible = false;
                }
            }
        }

        public void projectileDoorCollision()
        {
            foreach (Door d in map.doorList)
            {
                if (hitBox.Intersects(d.interactiveObjectHitBox) && !d.isInteracted)
                {
                    Visible = false;
                }
            }
        }


        public override void Draw(SpriteBatch sb)
        {
            if (Visible)
            {
                sb.Draw(AssetManager.projectileTex, pos, null, Color.White, 0, new Vector2((AssetManager.projectileTex.Width / 2) * scale, (AssetManager.projectileTex.Height / 2) * scale), scale, SpriteEffects.None, 1);
            }
            //if (explosion)
            //{
            //    sb.Draw(AssetManager.explosionTex, pos, null, Color.White, 0, new Vector2((AssetManager.explosionTex.Width / 2) * scale, (AssetManager.explosionTex.Height / 2) * scale), scale, SpriteEffects.None, 1);
            //}
        }

        public void distanceCheck(Vector2 theStartPosition) // Metod för en ny projektil
        {
            pos = theStartPosition;
            startPosition = theStartPosition;
            Visible = true;
        }
    }
}
