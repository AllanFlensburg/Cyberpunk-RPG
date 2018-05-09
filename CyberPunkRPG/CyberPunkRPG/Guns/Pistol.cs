using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CyberPunkRPG.Guns
{
    class Pistol : InteractiveObject
    {
        Rectangle sourceRect;

        public Pistol(Vector2 pos) : base(pos)
        {
            sourceRect = new Rectangle(0, 192, 64, 64);
            interactHitBox = new Rectangle((int)pos.X, (int)pos.Y, sourceRect.Width, sourceRect.Height);
            identify = 3;
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(SpriteBatch sb)
        {

        }
    }
}
