using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberPunkRPG
{
    class InvinciblePickup : InteractiveObject
    {
        Rectangle sourceRect;

        public InvinciblePickup(Vector2 pos) : base(pos)
        {
            sourceRect = new Rectangle(96, 160, 32, 32);
            interactHitBox = new Rectangle((int)pos.X, (int)pos.Y, sourceRect.Width, sourceRect.Height);
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(AssetManager.pickupTex, pos, sourceRect, Color.White);
        }
    }
}
