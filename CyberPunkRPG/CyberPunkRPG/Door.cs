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
            if (interactiveObjectHitBox.Width < interactiveObjectHitBox.Height)
            {
                interactHitBox = new Rectangle((int)position.X - 50, (int)position.Y, position.Width + 100, position.Height);
            }
            if (interactiveObjectHitBox.Width > interactiveObjectHitBox.Height)
            {
                interactHitBox = new Rectangle((int)position.X, (int)position.Y - 50, position.Width, position.Height + 100);
            }
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
