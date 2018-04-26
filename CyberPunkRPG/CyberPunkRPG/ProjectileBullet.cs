using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberPunkRPG
{
    class ProjectileBullet : Projectile
    {
        bool explosion = false;
        Vector2 explosionPos;

        private float explosionTimer;
        bool explosionStart = false;

        protected double explosionFrameTimer;
        protected double explosionFrameInterval;
        protected int explosionFrame;
        protected int numberOfExplosionFrames;
        protected int explosionFrameWidth;
        protected Rectangle explosionRect;

        public ProjectileBullet(Vector2 pos, Vector2 speed, Vector2 duration, int maxDistance, int damage, MapManager map) : base(pos, speed, duration, maxDistance, damage, map)
        {
            explosionFrameTimer = 60;
            explosionFrameInterval = 60;
            explosionFrame = 0;
            numberOfExplosionFrames = 13;
            explosionFrameWidth = 128;
            explosionRect = new Rectangle(0, 0, 128, 128);
    }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (explosionStart)
            {
                explosionTimer += gameTime.ElapsedGameTime.Milliseconds;
            }

            if (explosionTimer >= 100)
            {

            }
        }
        public void ExplosionAnimation(GameTime gameTime)
        {
            if (explosionFrameTimer <= 0)
            {
                explosionFrameTimer = explosionFrameInterval;
                explosionFrame++;
                explosionRect.X = (explosionFrame % numberOfExplosionFrames) * explosionFrameWidth;
            }
        }
    }
}
