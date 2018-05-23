﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberPunkRPG
{
    class Blind : GameObject
    {
        Rectangle position;
        public Rectangle blindHitBox;
        protected bool blind;
        public bool isBlind = true;
        //{
        //    get { return blind; }
        //}

        public Blind(Vector2 pos, Rectangle position) : base(pos)
        {
            this.position = position;
            blindHitBox = position;
        }

        public override void Draw(SpriteBatch sb)
        {
            if (isBlind == true)
            {
                sb.Draw(AssetManager.blindTex, position, Color.White);
            }
        }
    }
}