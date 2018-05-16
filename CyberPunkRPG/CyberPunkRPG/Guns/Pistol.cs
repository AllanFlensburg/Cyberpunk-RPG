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
            sourceRect = new Rectangle(30, 228, 32, 12);
            interactHitBox = new Rectangle((int)pos.X - 6, (int)pos.Y - 6, sourceRect.Width + 12, sourceRect.Height + 12);
            weaponID = 3;
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(AssetManager.doorTex, interactHitBox, Color.Red);
            sb.Draw(AssetManager.pistolTex, pos, sourceRect, Color.White);
        }
    }
}
