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
    enum gunState
    {
        pistol,
        assaultRifle,
        sniperRifle,
        rocketLauncher,
    }
    class Player : GameObject
    {
        Camera camera;
        Game1 game;
        MapManager map;
        Door door;
        Vector2 mousePos;
        Vector2 projectileStart;
        Vector2 projectileSpeed;
        Vector2 dashSpeed;
        Vector2 dashDistance = new Vector2(400, 400);
        Vector2 startingPosition = Vector2.Zero;
        Vector2 endPosition = Vector2.Zero;
        Vector2 worldPosition;
        Rectangle hitBox;
        int playerSpeed;
        int ammoCount;
        int maxDistance;
        bool reloading;
        float reloadTimer;
        float reloadTime;
        bool jumping = false;
        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;
        MouseState currentMouseState;
        MouseState previousMouseState;
        public List<Projectile> projectileList;
        gunState currentGunState;

        protected double frameTimer;
        protected double frameInterval;
        protected int frame;
        protected int numberOfFrames;
        protected int frameWidth;
        protected Rectangle sourceRect;

        public Player(Vector2 pos, Rectangle hitBox, Camera camera, Game1 game, MapManager map) : base(pos)
        {
            this.pos = pos;
            this.camera = camera;
            this.game = game;
            this.map = map;
            playerSpeed = 100;
            ammoCount = 8;
            reloadTimer = 1.5f;
            reloadTime = 1.5f;
            reloading = false;
            dashSpeed = new Vector2(300, 300);
            projectileSpeed = new Vector2(500, 500);
            projectileList = new List<Projectile>();
            this.hitBox = hitBox;
            currentGunState = gunState.assaultRifle;

            frameTimer = 60;
            frameInterval = 60;
            frame = 0;
            numberOfFrames = 9;
            frameWidth = 64;
            sourceRect = new Rectangle(0, 192, 64, 64);

            if (currentGunState == gunState.assaultRifle)
            {
                ammoCount = 30;
                reloadTime = 2.0f;
                reloadTimer = 2.0f;
                maxDistance = 600;
            }
            else if (currentGunState == gunState.sniperRifle)
            {
                ammoCount = 5;
                reloadTime = 3.0f;
                reloadTimer = 3.0f;
                projectileSpeed = new Vector2(1000, 1000);
                maxDistance = 2000;
                
            }
            else if (currentGunState == gunState.pistol)
            {
                ammoCount = 8;
                reloadTime = 1.5f;
                reloadTimer = 1.5f;
                maxDistance = 500;

            }

            //Kommer senare
            //else if (currentGunState == gunState.rocketLauncher)
            //{
            //    ammoCount = 1;
            //    reloadTime = 5.0f;
            //    reloadTimer = 5.0f;
            //    maxDistance = 1000;
            //}
        }

        public override void Update(GameTime gameTime)
        {
            projectileStart = pos;
            hitBox.X = (int)pos.X;
            hitBox.Y = (int)pos.Y;

            currentKeyboardState = Keyboard.GetState();
            currentMouseState = Mouse.GetState();
            mousePos.X = currentMouseState.X;
            mousePos.Y = currentMouseState.Y;
            worldPosition = Vector2.Transform(mousePos, Matrix.Invert(camera.GetTransformation(camera.view)));

            UpdateMovement(currentKeyboardState, gameTime);
            WallColision();
            CoverColision();
            Animation(gameTime);
            //InteractCollision(); //Programmet går att köras när denna rad är struken, men borde behövas för att checka kollision med player
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

            //Kommer senare
            //if (currentGunState == gunState.rocketLauncher)
            //{
            //    if ()
            //}

            previousKeyboardState = currentKeyboardState;
            previousMouseState = currentMouseState;
        }

        private void Animation(GameTime gameTime)
        {
            if (frameTimer <= 0)
            {
                frameTimer = frameInterval;
                frame++;
                sourceRect.X = (frame % numberOfFrames) * frameWidth;
            }
        }

        private void UpdateMovement(KeyboardState currentKeyboardState, GameTime gameTime)
        {
            if (jumping == false)
            {
                if (currentKeyboardState.IsKeyDown(Keys.A) == true)
                {
                    sourceRect.Y = 64;
                    pos.X -= playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    frameTimer -= gameTime.ElapsedGameTime.TotalMilliseconds;
                }

                if (currentKeyboardState.IsKeyDown(Keys.D) == true)
                {
                    sourceRect.Y = 192;
                    pos.X += playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    frameTimer -= gameTime.ElapsedGameTime.TotalMilliseconds;
                }

                if (currentKeyboardState.IsKeyDown(Keys.S) == true)
                {
                    sourceRect.Y = 128;
                    pos.Y += playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    frameTimer -= gameTime.ElapsedGameTime.TotalMilliseconds;
                }

                if (currentKeyboardState.IsKeyDown(Keys.W) == true)
                {
                    sourceRect.Y = 0;
                    pos.Y -= playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    frameTimer -= gameTime.ElapsedGameTime.TotalMilliseconds;
                }

                if (currentKeyboardState.IsKeyDown(Keys.Space) == true)
                {
                    startingPosition = pos;
                    endPosition = pos += GetDirection(worldPosition - startingPosition) * (float)gameTime.ElapsedGameTime.TotalSeconds;
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
                    if (currentGunState == gunState.pistol)
                    {
                        reloading = false;
                        ammoCount = 8;
                        reloadTimer = reloadTime;
                    }
                    else if (currentGunState == gunState.assaultRifle)
                    {
                        reloading = false;
                        ammoCount = 30;
                        reloadTimer = reloadTime;
                    }
                    else if (currentGunState == gunState.sniperRifle)
                    {
                        reloading = false;
                        ammoCount = 5;
                        reloadTimer = reloadTime;
                    }
                    else if (currentGunState == gunState.rocketLauncher)
                    {
                        reloading = false;
                        ammoCount = 1;
                        reloadTimer = reloadTime;
                    }
                }
            }
        }

        private void ShootProjectile(KeyboardState currentKeyboardState)
        {
            // Kommer senare

            //if (currentGunState == gunState.assaultRifle)
            //{
            //    if (currentKeyboardState.IsKeyDown(Keys.Q) == true && ammoCount >= 1 && reloading == false)
            //    {
            //        ammoCount -= 1;
            //        createNewProjectile(GetDirection(worldPosition - pos));
            //    }
            //}
            if (currentKeyboardState.IsKeyDown(Keys.Q) == true && previousKeyboardState.IsKeyDown(Keys.Q) == false & ammoCount >= 1 & reloading == false)
            {
                ammoCount -= 1;
                createNewProjectile(GetDirection(worldPosition - pos));
            }
        }

        private void createNewProjectile(Vector2 direction)
        {
            Projectile projectile = new Projectile(projectileStart, projectileSpeed, direction, maxDistance);
            projectile.distanceCheck(pos);
            projectileList.Add(projectile);
        }

        public Vector2 GetDirection(Vector2 dir)
        {
            Vector2 newDirection = dir;
            return Vector2.Normalize(newDirection);
        }

        public void WallColision()
        {
            foreach (Wall w in map.wallList)
            {
                if (hitBox.Intersects(w.hitBox))
                {
                    if (hitBox.X > w.hitBox.X && currentKeyboardState.IsKeyDown(Keys.A))
                    {
                        pos.X += w.hitBox.Width - hitBox.Width;
                    }
                    else if (hitBox.X > w.hitBox.X - hitBox.Width && currentKeyboardState.IsKeyDown(Keys.D))
                    {
                        pos.X -= w.hitBox.Width - hitBox.Width;
                    }
                    else if (hitBox.Y > w.hitBox.Y && currentKeyboardState.IsKeyDown(Keys.W))
                    {
                        pos.Y += hitBox.Height;
                    }
                    else if (hitBox.Y < w.hitBox.Y && currentKeyboardState.IsKeyDown(Keys.S))
                    {
                        pos.Y -= hitBox.Height;   
                    }
                }
            }
        }

        public void CoverColision()
        {
            foreach (Cover c in map.coverList)
            {
                if (hitBox.Intersects(c.hitBox))
                {
                    if (hitBox.X > c.hitBox.X && currentKeyboardState.IsKeyDown(Keys.A))
                    {
                        pos.X += hitBox.Width;
                    }
                    else if (hitBox.X < c.hitBox.X && currentKeyboardState.IsKeyDown(Keys.D))
                    {
                        pos.X -= hitBox.Width;
                    }
                    else if (hitBox.Y > c.hitBox.Y && currentKeyboardState.IsKeyDown(Keys.W))
                    {
                        pos.Y += hitBox.Height;
                    }
                    else if (hitBox.Y < c.hitBox.Y && currentKeyboardState.IsKeyDown(Keys.S))
                    {
                        pos.Y -= hitBox.Height;
                    }
                }
            }
        }

        //Checkar kollision mellan dörr och spelare
        public void InteractCollision()
        {
            if (hitBox.Intersects(door.interactHitBox) && door.isInteracted == false && currentKeyboardState.IsKeyDown(Keys.E) && previousKeyboardState.IsKeyDown(Keys.E))
            {
                door.isInteracted = true;
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            
            sb.Draw(AssetManager.doorTex, pos, hitBox, Color.Red); //ritar ut karaktärens hitbox för att testa kollision
            sb.Draw(AssetManager.playerTex, pos, sourceRect, Color.White, 0, new Vector2(), 1, SpriteEffects.None, 1);
            sb.DrawString(AssetManager.gameText, ammoCount.ToString(), pos - new Vector2(46, 72), Color.Yellow);

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
