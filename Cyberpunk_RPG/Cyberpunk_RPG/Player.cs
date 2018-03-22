using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyberpunk_RPG
{
    class Player
    {
        Texture2D playerTex;
        Texture2D projectileTex;
        public Vector2 pos;
        Vector2 mousePos;
        Vector2 projectileStart;
        Vector2 projectileSpeed;
        Vector2 dashSpeed;
        int playerSpeed;
        float jumpTime;
        bool jumping = false;
        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;
        MouseState currentMouseState;
        MouseState previousMouseState;
        public List<Projectile> projectileList;

        public Player(Texture2D playerTex, Texture2D projectileTex, Vector2 pos)
        {
            this.playerTex = playerTex;
            this.projectileTex = projectileTex;
            this.pos = pos;
            playerSpeed = 100;
            jumpTime = 0.8f;
            dashSpeed = new Vector2(200, 200);
            projectileSpeed = new Vector2(300, 300);
            projectileList = new List<Projectile>();
        }

        public void Update(GameTime gameTime)
        {
            jumpTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (jumpTime < 0)
            {
                jumpTime = 0.8f;
                jumping = false;
            }
            projectileStart = pos;
            currentKeyboardState = Keyboard.GetState();
            currentMouseState = Mouse.GetState();
            mousePos.X = currentMouseState.X;
            mousePos.Y = currentMouseState.Y;
            
            UpdateMovement(currentKeyboardState, gameTime);
            ShootProjectile(currentKeyboardState);
            foreach (Projectile p in projectileList)
            {
                p.Update(gameTime);
                if (p.Visible == false)
                {
                    projectileList.Remove(p);
                    break;
                }
            }

            previousKeyboardState = currentKeyboardState;
            previousMouseState = currentMouseState;
        }

        private void UpdateMovement(KeyboardState currentKeyboardState, GameTime gameTime)
        {
         
            if (currentKeyboardState.IsKeyDown(Keys.A) == true)
            {
                pos.X -= playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (currentKeyboardState.IsKeyDown(Keys.D) == true)
            {
                pos.X += playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (currentKeyboardState.IsKeyDown(Keys.S) == true)
            {
                pos.Y += playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (currentKeyboardState.IsKeyDown(Keys.W) == true)
            {
                pos.Y -= playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (currentKeyboardState.IsKeyDown(Keys.Space) == true)
            {
                jumpTime = 0.8f;
                jumping = true;
            }

            if (jumping == true)
            {
                pos += dashSpeed * GetDirection(mousePos - pos) * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

        }

        private void ShootProjectile(KeyboardState currentKeyboardState)
        {
            if (currentKeyboardState.IsKeyDown(Keys.Q) == true && previousKeyboardState.IsKeyDown(Keys.Q) == false)
            {
                createNewProjectile(GetDirection(mousePos - pos));
            }
        }

        private void createNewProjectile(Vector2 direction)
        {
            Projectile projectile = new Projectile(projectileTex, projectileStart, projectileSpeed, direction);
            projectile.distanceCheck(pos);
            projectileList.Add(projectile);
        }

        public Vector2 GetDirection(Vector2 dir)
        {
            Vector2 newDirection = dir;
            return Vector2.Normalize(newDirection);
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(playerTex, pos, Color.White);

            foreach (Projectile p in projectileList)
            {
                p.Draw(sb);
            }
        }
    }
}
