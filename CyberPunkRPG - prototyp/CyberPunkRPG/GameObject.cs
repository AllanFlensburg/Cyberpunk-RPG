using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberPunkRPG
{
    abstract class GameObject
    {
        public Vector2 pos;

        public GameObject(Vector2 pos)
        {
            this.pos = pos;
        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public abstract void Draw(SpriteBatch sb);
    }
}
