using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberPunkRPG
{
    class InteractiveObject : GameObject
    {
        public Rectangle interactHitBox;
        public bool isInteracted = false;

        public InteractiveObject(Vector2 pos) : base(pos)
        {
            this.pos = pos;
        }

        public override void Update(GameTime gameTime)
        {
            interactHitBox = new Rectangle((int)pos.X, (int)pos.Y, AssetManager.doorTex.Width, AssetManager.doorTex.Height);
        }
        public override void Draw(SpriteBatch sb)
        {
        }
    }
}
