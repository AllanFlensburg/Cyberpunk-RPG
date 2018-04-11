using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberPunkRPG
{
    class Spikes : GameObject
    {
        Rectangle position;
        public Rectangle hitBox;
        private int damage;

        public Spikes(Vector2 pos, Rectangle position) : base(pos)
        {
            this.position = position;
            hitBox = position;
            damage = 1;
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(AssetManager.spikeTex, position, Color.White);
        }
    }
}
