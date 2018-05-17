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
        public int damage;
        public bool isHidden;

        public Spikes(Vector2 pos/*, Rectangle position*/) : base(pos)
        {
            position.X = (int)pos.X;
            position.Y = (int)pos.Y;
            position.Width = AssetManager.spikeTex.Width;
            position.Height = AssetManager.spikeTex.Height;
            hitBox = position;

            damage = 2;
            isHidden = true;
        }

        public override void Draw(SpriteBatch sb)
        {
            if (!isHidden)
            {
                sb.Draw(AssetManager.spikeTex, pos, Color.White);
            }
        }
    }
}
