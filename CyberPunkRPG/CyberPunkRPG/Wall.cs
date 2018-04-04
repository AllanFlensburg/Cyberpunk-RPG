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
        Rectangle position;
        public Rectangle hitBox;
        protected bool wall;
        public bool isWall // Får in att det är en vägg när planen skapas
        {
            get { return wall; }
        }

        public Wall(Vector2 pos, Rectangle position, bool wall) : base(pos)
        {
            this.position = position;
            this.wall = wall;
            hitBox = position;
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(AssetManager.wallTex, position, Color.White);
        }
    }
}
