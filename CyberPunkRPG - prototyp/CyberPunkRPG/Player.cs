using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberPunkRPG
{
    class Player : GameObject
    {
        //Camera camera;
        //Game1 game;

        Vector2 mousePos;
        Vector2 projectileStart;
        Vector2 projectileSpeed;
        Vector2 dashSpeed;
        Vector2 dashDistance = new Vector2(400, 400);
        Vector2 startingPosition = Vector2.Zero;
        Vector2 endPosition = Vector2.Zero;
        int playerSpeed;
        int ammoCount;
        bool reloading;
        float reloadTimer;
        float reloadTime;
        bool jumping = false;
        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;
        MouseState currentMouseState;
        MouseState previousMouseState;
        public List<Projectile> projectileList;
        Vector2 aim;

        public Player(Vector2 pos/*, Camera camera, Game1 game*/) : base(pos)
        {
            this.pos = pos;
            //this.camera = camera;
            //this.game = game;
            playerSpeed = 100;
            ammoCount = 8;
            reloadTimer = 1.5f;
            reloadTime = 1.5f;
            reloading = false;
            dashSpeed = new Vector2(300, 300);
            projectileSpeed = new Vector2(500, 500);
            projectileList = new List<Projectile>();
        }

        public override void Update(GameTime gameTime)
        {
            projectileStart = pos;
            currentKeyboardState = Keyboard.GetState();
            currentMouseState = Mouse.GetState();
            mousePos.X = currentMouseState.X;
            mousePos.Y = currentMouseState.Y;

            //aim = game.GetCameraPosition();

            UpdateMovement(currentKeyboardState, gameTime);
            ShootProjectile(currentKeyboardState);
            Reload(currentKeyboardState, gameTime);
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
            if (jumping == false)
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
                    startingPosition = pos;
                    endPosition = pos += GetDirection(mousePos - startingPosition) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    jumping = true;
                }
            }

            if (jumping == true)
            {
                pos += dashSpeed * GetDirection(endPosition - startingPosition) * (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (Vector2.Distance(startingPosition, pos) > 200)
                {
                    jumping = false;
                    startingPosition = Vector2.Zero;
                    endPosition = Vector2.Zero;
                }
            }

        }

        private void Reload(KeyboardState currentKeyboardState, GameTime gameTime)
        {
            if (currentKeyboardState.IsKeyDown(Keys.R) == true)
            {
                reloading = true;
            }

            if (reloading == true)
            {
                reloadTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (reloadTimer <= 0)
                {
                    reloading = false;
                    ammoCount = 8;
                    reloadTimer = reloadTime;
                }
            }
        }

        private void ShootProjectile(KeyboardState currentKeyboardState)
        {
            if (currentKeyboardState.IsKeyDown(Keys.Q) == true && previousKeyboardState.IsKeyDown(Keys.Q) == false & ammoCount >= 1 & reloading == false)
            {
                ammoCount -= 1;
                createNewProjectile(GetDirection(mousePos - pos));
            }
        }

        private void createNewProjectile(Vector2 direction)
        {
            Projectile projectile = new Projectile(projectileStart, projectileSpeed, direction);
            projectile.distanceCheck(pos);
            projectileList.Add(projectile);
        }

        public Vector2 GetDirection(Vector2 dir)
        {
            Vector2 newDirection = dir;
            return Vector2.Normalize(newDirection);
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(AssetManager.playerTex, pos, null, Color.White, 0, new Vector2(AssetManager.playerTex.Width / 2, AssetManager.playerTex.Height / 2), 1, SpriteEffects.None, 1);

            sb.DrawString(AssetManager.gameText, ammoCount.ToString(), pos - new Vector2(46, 70), Color.Yellow);

            if (ammoCount == 0 & reloading == false)
            {
                sb.DrawString(AssetManager.gameText, "Press R to Reload", pos - new Vector2(46, 90), Color.Yellow);
            }

            if (reloading == true)
            {
                sb.Draw(AssetManager.reloadDisplay, new Vector2(pos.X - 46, pos.Y - 90), new Rectangle(0, 45,AssetManager.reloadDisplay.Width, 44), Color.White, 0f, new Vector2(), 0.2f, SpriteEffects.None, 1);
                sb.Draw(AssetManager.reloadDisplay, new Rectangle((int)pos.X - 46, (int)pos.Y - 90, (int)((AssetManager.reloadDisplay.Width * 0.2f) * ((double)reloadTimer / reloadTime)), (int)(44 * 0.2f)), new Rectangle(0, 45,AssetManager.reloadDisplay.Width, 44), Color.Green);
                sb.Draw(AssetManager.reloadDisplay, new Vector2(pos.X - 46, pos.Y - 90), new Rectangle(0, 0,AssetManager.reloadDisplay.Width, 44), Color.White, 0f, new Vector2(), 0.2f, SpriteEffects.None, 1);
            }

            foreach (Projectile p in projectileList)
            {
                p.Draw(sb);
            }
        }
    }
}
