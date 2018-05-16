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
        Rectangle positionOrigin;
        public Rectangle doorHitBox;
        int doorTimer;
        public Door(Vector2 pos, Rectangle position) : base(pos)
        {
            this.position = position;
            doorHitBox = new Rectangle((int)position.X, (int)position.Y, position.Width, position.Height);
            if (doorHitBox.Width < doorHitBox.Height)
            {
                interactHitBox = new Rectangle((int)position.X - 50, (int)position.Y, position.Width + 100, position.Height);
            }
            if (doorHitBox.Width > doorHitBox.Height)
            {
                interactHitBox = new Rectangle((int)position.X, (int)position.Y - 50, position.Width, position.Height + 100);
            }
            positionOrigin = position;
        }


        public override void Update(GameTime gameTime)
        {
            MoveDoor();
            if (isInteracted || !isInteracted)
            {
                doorTimer += gameTime.ElapsedGameTime.Milliseconds;
            }
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch sb)
        {
            if (!isInteracted)
            {
                sb.Draw(AssetManager.doorTex, position, Color.White);
            }
            else
            {
                sb.Draw(AssetManager.doorTex, position, Color.Black);
            }
        }

        public void MoveDoor()
        {
            if (positionOrigin.Width > positionOrigin.Height)
            {
                if (isInteracted)
                {
                    if (position.X < positionOrigin.X + 101)
                    {
                        position.X += doorTimer;
                    }
                    doorTimer = 0;
                }
                else if (!isInteracted)
                {
                    if (position.X > positionOrigin.X)
                    {
                        position.X -= doorTimer;
                    }
                    doorTimer = 0;
                }
            }

            if (positionOrigin.Width < positionOrigin.Height)
            {
                if (isInteracted)
                {
                    if (position.Y < positionOrigin.Y + 101)
                    {
                        position.Y += doorTimer;
                    }
                    doorTimer = 0;
                }
                else if (!isInteracted)
                {
                    if (position.Y > positionOrigin.Y)
                    {
                        position.Y -= doorTimer;
                    }
                    doorTimer = 0;
                }
            }
        }
    }
}
