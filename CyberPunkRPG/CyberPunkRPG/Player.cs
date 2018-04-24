﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberPunkRPG
{
    enum weapon
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
        Vector2 mousePos;
        Vector2 projectileStart;
        Vector2 projectileSpeed;
        Vector2 dashSpeed;
        Vector2 dashDistance = new Vector2(400, 400);
        Vector2 startingPosition = Vector2.Zero;
        Vector2 endPosition = Vector2.Zero;
        Vector2 worldPosition;
        Vector2 prevPos;
        Rectangle hitBox;
        private Rectangle healthbarSource;
        private Rectangle healthbarEdgesSource;
        int playerSpeed;
        int ammoCount;
        int maxDistance;
        GameWindow window;
        public int CurrentHealth = 100;
        private int healthbarWidth = 467;
        private int healthbarHeight = 44;
        bool reloading;
        float reloadTimer;
        float reloadTime;
        bool jumping = false;
        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;
        MouseState currentMouseState;
        MouseState previousMouseState;
        public List<Projectile> projectileList;
        weapon activeWeapon;

        protected double frameTimer;
        protected double frameInterval;
        protected int frame;
        protected int numberOfFrames;
        protected int frameWidth;
        protected Rectangle sourceRect;

        public Player(Vector2 pos, Rectangle hitBox, Camera camera, Game1 game, MapManager map, GameWindow window) : base(pos)
        {
            this.window = window;
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
            activeWeapon = weapon.assaultRifle;

            frameTimer = 60;
            frameInterval = 60;
            frame = 0;
            numberOfFrames = 9;
            frameWidth = 64;
            sourceRect = new Rectangle(0, 192, 64, 64);
            healthbarSource = new Rectangle(0, 45, healthbarWidth, healthbarHeight);
            healthbarEdgesSource = new Rectangle(0, 0, healthbarWidth, healthbarHeight);

            if (activeWeapon == weapon.assaultRifle)
            {
                ammoCount = 30;
                reloadTime = 2.0f;
                reloadTimer = 2.0f;
                maxDistance = 600;
            }
            else if (activeWeapon == weapon.sniperRifle)
            {
                ammoCount = 5;
                reloadTime = 3.0f;
                reloadTimer = 3.0f;
                projectileSpeed = new Vector2(1000, 1000);
                maxDistance = 2000;
                
            }
            else if (activeWeapon == weapon.pistol)
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
            bool noCollision = true;
            foreach (Wall w in map.wallList)
            {
                if (hitBox.Intersects(w.hitBox))
                {
                    noCollision = false;
                }
            }
            if (noCollision)
            {
                prevPos = pos;
            }
            projectileStart = pos + new Vector2(32,32);
            hitBox.X = (int)pos.X + 20;
            hitBox.Y = (int)pos.Y + 10;

            currentKeyboardState = Keyboard.GetState();
            currentMouseState = Mouse.GetState();
            mousePos.X = currentMouseState.X;
            mousePos.Y = currentMouseState.Y;
            worldPosition = Vector2.Transform(mousePos, Matrix.Invert(camera.GetTransformation(camera.view)));
            
            UpdateMovement(currentKeyboardState, gameTime);
            WallCollision();
            CoverCollision();
            Animation(gameTime);
            InteractCollision();
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
                    if (activeWeapon == weapon.pistol)
                    {
                        reloading = false;
                        ammoCount = 8;
                        reloadTimer = reloadTime;
                    }
                    else if (activeWeapon == weapon.assaultRifle)
                    {
                        reloading = false;
                        ammoCount = 30;
                        reloadTimer = reloadTime;
                    }
                    else if (activeWeapon == weapon.sniperRifle)
                    {
                        reloading = false;
                        ammoCount = 5;
                        reloadTimer = reloadTime;
                    }
                    else if (activeWeapon == weapon.rocketLauncher)
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
            projectile.distanceCheck(projectileStart);
            projectileList.Add(projectile);
        }

        public Vector2 GetDirection(Vector2 dir)
        {
            Vector2 newDirection = dir;
            return Vector2.Normalize(newDirection);
        }

        public void WallCollision()
        {
            foreach (Wall w in map.wallList)
            {
                if (hitBox.Intersects(w.hitBox))
                {
                    pos = prevPos;
                    hitBox.X = (int)pos.X + 20;
                    hitBox.Y = (int)pos.Y + 10;

                    if (hitBox.X > w.hitBox.Right -3)
                    {
                        pos.X += 2;
                    }
                    if (hitBox.X < w.hitBox.Left)
                    {
                        pos.X -= 2;
                    }
                    if (hitBox.Y < w.hitBox.Top)
                    {
                        pos.Y -= 2;
                    }
                    if (hitBox.Y > w.hitBox.Bottom -3)
                    {
                        pos.Y += 2;
                    }
                }
            }
        }

        public void CoverCollision()
        {
            foreach (Cover c in map.coverList)
            {
                if (hitBox.Intersects(c.hitBox))
                {
                    pos = prevPos;
                    hitBox.X = (int)pos.X + 20;
                    hitBox.Y = (int)pos.Y + 10;

                    if (hitBox.X > c.hitBox.Right - 3)
                    {
                        pos.X += 1;
                    }
                    if (hitBox.X < c.hitBox.Left)
                    {
                        pos.X -= 1;
                    }
                    if (hitBox.Y < c.hitBox.Top)
                    {
                        pos.Y -= 1;
                    }
                    if (hitBox.Y > c.hitBox.Bottom - 3)
                    {
                        pos.Y += 1;
                    }
                }
            }
        }

        //Checkar kollision mellan dörr och spelare
        public void InteractCollision()
        {
            foreach (Door d in map.doorList)
            {
                if (hitBox.Intersects(d.interactHitBox) && d.isInteracted == false && currentKeyboardState.IsKeyDown(Keys.E) && previousKeyboardState.IsKeyDown(Keys.E))
                {
                    d.isInteracted = true;
                }
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            
            sb.Draw(AssetManager.doorTex, hitBox, hitBox, Color.Red); //ritar ut karaktärens hitbox för att testa kollision
            sb.Draw(AssetManager.playerTex, pos, sourceRect, Color.White, 0, new Vector2(), 1, SpriteEffects.None, 1);
            if (activeWeapon == weapon.pistol)
            {
                sb.Draw(AssetManager.pistolTex, pos, sourceRect, Color.White, 0, new Vector2(), 1, SpriteEffects.None, 1);
            }
            else if (activeWeapon == weapon.assaultRifle)
            {
                sb.Draw(AssetManager.assaultRifleTex, pos, sourceRect, Color.White, 0, new Vector2(), 1, SpriteEffects.None, 1);
            }
            else if (activeWeapon == weapon.sniperRifle)
            {
                sb.Draw(AssetManager.sniperRifleTex, pos, sourceRect, Color.White, 0, new Vector2(), 1, SpriteEffects.None, 1);
            }
            sb.DrawString(AssetManager.gameText, ammoCount.ToString(), pos - new Vector2(0, 40), Color.Yellow);

            if (ammoCount == 0 & reloading == false)
            {
                sb.DrawString(AssetManager.gameText, "Press R to Reload", pos - new Vector2(0, 20), Color.Yellow);
            }

            if (reloading == true)
            {
                sb.Draw(AssetManager.reloadDisplay, new Vector2(pos.X, pos.Y- 20), new Rectangle(0, 45,AssetManager.reloadDisplay.Width, 44), Color.White, 0f, new Vector2(), 0.2f, SpriteEffects.None, 1);
                sb.Draw(AssetManager.reloadDisplay, new Rectangle((int)pos.X, (int)pos.Y - 20, (int)((AssetManager.reloadDisplay.Width * 0.2f) * ((double)reloadTimer / reloadTime)), (int)(44 * 0.2f)), new Rectangle(0, 45,AssetManager.reloadDisplay.Width, 44), Color.Green);
                sb.Draw(AssetManager.reloadDisplay, new Vector2(pos.X, pos.Y - 20), new Rectangle(0, 0,AssetManager.reloadDisplay.Width, 44), Color.White, 0f, new Vector2(), 0.2f, SpriteEffects.None, 1);
            }

            foreach (Projectile p in projectileList)
            {
                p.Draw(sb);
            }

            sb.Draw(AssetManager.healthbarTex, new Rectangle((int)pos.X - healthbarWidth / 2, (int)pos.Y - window.ClientBounds.Height/2, healthbarWidth, healthbarHeight), healthbarSource, Color.Gray);
            sb.Draw(AssetManager.healthbarTex, new Rectangle((int)pos.X - healthbarWidth / 2, (int)pos.Y - window.ClientBounds.Height / 2, (int)(healthbarWidth * ((double)CurrentHealth / 100)), healthbarHeight), healthbarSource, Color.Red);
            sb.Draw(AssetManager.healthbarTex, new Rectangle((int)pos.X - healthbarWidth / 2, (int)pos.Y - window.ClientBounds.Height / 2, healthbarWidth, healthbarHeight), healthbarEdgesSource, Color.White);
        }
    }
}
