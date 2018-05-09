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
            sourceRect = new Rectangle(0, 192, 32, 32);
            interactHitBox = new Rectangle((int)pos.X, (int)pos.Y, sourceRect.Width, sourceRect.Height);
            identify = 1;
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(AssetManager.doorTex, interactHitBox, Color.Red);
            sb.Draw(AssetManager.assaultRifleTex, pos, sourceRect, Color.White);
        }
    }
}
