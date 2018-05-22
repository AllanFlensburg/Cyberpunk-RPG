using Microsoft.Xna.Framework;
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
        public Rectangle hitBox;
        protected bool blind;
        public bool isBlind
        {
            get { return blind; }
        }

        public Blind(Vector2 pos, Rectangle position, bool blind) : base(pos)
        {
            this.position = position;
            this.blind = blind;
            hitBox = position;
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(AssetManager.blindTex, position, Color.White);
        }
    }
}