using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberPunkRPG
{
    class Wall : GameObject
    {
        protected bool wall;
        public bool isWall // Får in att det är en vägg när planen skapas
        {
            get { return wall; }
        }

        public Wall(Vector2 pos, bool wall) : base(pos)
        {
            this.wall = wall;
        }

        public override void Draw(SpriteBatch sb)
        {
            //sb.Draw(tex, pos, Color.White);
        }
    }
}
