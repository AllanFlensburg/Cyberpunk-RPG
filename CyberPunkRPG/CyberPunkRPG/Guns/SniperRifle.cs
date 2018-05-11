﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberPunkRPG
{
    class SniperRifle : InteractiveObject
    {
        //Vector2 speed;
        //Vector2 direction;
        //int maxDistance;
        //float scale = 0.15f;
        //Vector2 startPosition;
        //public bool Visible = false;
        //public Rectangle hitBox;
        Rectangle sourceRect;

        public SniperRifle(Vector2 pos) : base(pos)
        {
            sourceRect = new Rectangle(30, 230, 32, 12);
            interactHitBox = new Rectangle((int)pos.X - 6, (int)pos.Y - 6 , sourceRect.Width + 12, sourceRect.Height + 12);
            identify = 2;
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
            //}
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(AssetManager.doorTex, interactHitBox, Color.Red);
            sb.Draw(AssetManager.sniperRifleTex, sourceRect, Color.White);
        //    if (Visible)
        //    {
        //        sb.Draw(AssetManager.projectileTex, pos, null, Color.White, 0, new Vector2((AssetManager.projectileTex.Width / 2 * scale), (AssetManager.projectileTex.Height / 2) * scale), scale, SpriteEffects.None, 1);
        //    }
        }

        //public void distanceCheckAssault(Vector2 theStartPosition) // Metod för en ny projektil
        //{
        //    //pos = theStartPosition;
        //    //startPosition = theStartPosition;
        //    //Visible = true;
        //}
    }
}

