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
        Rectangle position;
        public Door(Vector2 pos, Rectangle position) : base(pos)
        {
            this.position = position;
            interactiveObjectHitBox = new Rectangle((int)position.X, (int)position.Y, position.Width, position.Height);
            interactHitBox = new Rectangle((int)position.X - 20, (int)position.Y - 15, position.Width + 40, position.Height + 30);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch sb)
        {
            if (!isInteracted)
            {
                sb.Draw(AssetManager.doorTex, interactHitBox, Color.Red);//Ritar ut interactHitbox
                sb.Draw(AssetManager.doorTex, position, Color.White);
            }
            else
            {
                sb.Draw(AssetManager.doorTex, position, Color.Black);
            }
        }
    }
}
