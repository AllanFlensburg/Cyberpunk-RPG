using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberPunkRPG
{
    class Block : GameObject
    {
        Rectangle position;
        public Rectangle hitBox;
        protected bool block;
        public bool isBlock 
        {
            get { return block; }
        }

        public Block(Vector2 pos, Rectangle position, bool block) : base(pos)
        {
            this.position = position;
            this.block = block;
            hitBox = position;
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(AssetManager.blockTex, position, Color.White);
        }
    }
}
