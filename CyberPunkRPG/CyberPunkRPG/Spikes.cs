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
        public Rectangle hitBox;
        public int damage;
        public bool isHidden;

        public Spikes(Vector2 pos) : base(pos)
        {
            hitBox = new Rectangle((int)pos.X, (int)pos.Y, AssetManager.spikeTex.Width, AssetManager.spikeTex.Height);

            damage = 2;
            isHidden = false;
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
