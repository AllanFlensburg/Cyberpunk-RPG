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
        public Door(Vector2 pos) : base(pos)
        {

        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(AssetManager.doorTex, pos, Color.White);
        }
    }
}
