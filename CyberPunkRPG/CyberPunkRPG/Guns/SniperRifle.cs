using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberPunkRPG
{
    class SniperRifle : InteractiveObject
    {
        Rectangle sourceRect;

        public SniperRifle(Vector2 pos) : base(pos)
        {
            sourceRect = new Rectangle(30, 230, 32, 12);
            interactHitBox = new Rectangle((int)pos.X - 6, (int)pos.Y - 6 , sourceRect.Width + 12, sourceRect.Height + 12);
            weaponID = 2;
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(AssetManager.sniperRifleTex, pos, sourceRect, Color.White);
        }
    }
}

