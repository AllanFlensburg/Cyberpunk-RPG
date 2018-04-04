using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberPunkRPG
{
    class Cover : GameObject
    {
        Rectangle position;
        public Rectangle hitBox;
        protected bool cover;
        public bool isCover // Får in att det är ett skydd när planen skapas
        {
            get { return cover; }
        }

        public Cover(Vector2 pos, Rectangle position, bool cover) : base(pos)
        {
            this.position = position;
            this.cover = cover;
            hitBox = position;
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(AssetManager.coverTex, position, Color.White);
        }
    }
}
