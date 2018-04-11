using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberPunkRPG
{
    class Door : InteractiveObject
    {
        Rectangle position;
        public Door(Vector2 pos, Rectangle position) : base(pos)
        {
        this.position=position;
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(AssetManager.doorTex, position, Color.White);
        }
    }
}
