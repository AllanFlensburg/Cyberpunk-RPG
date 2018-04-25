using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberPunkRPG
{
    class BarbedWire : GameObject
    {
        Rectangle position;
        public Rectangle hitBox;
        public float slowMultiplier;

        public BarbedWire(Vector2 pos, Rectangle position) : base(pos)
        {
            this.position = position;
            hitBox = position;
            slowMultiplier = 50;
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(AssetManager.wireTex, position, Color.White);
        }
    }
}
