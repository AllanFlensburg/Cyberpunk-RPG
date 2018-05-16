using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberPunkRPG
{
    class AssaultRifle : InteractiveObject
    {
        Rectangle sourceRect;

        public AssaultRifle(Vector2 pos) : base(pos)
        {
            sourceRect = new Rectangle(6, 98, 32, 12);
            interactHitBox = new Rectangle((int)pos.X - 6, (int)pos.Y - 6, sourceRect.Width + 12, sourceRect.Height + 12);
            weaponID = 1;
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(AssetManager.assaultRifleTex, pos, sourceRect, Color.White);
        }
    }
}
