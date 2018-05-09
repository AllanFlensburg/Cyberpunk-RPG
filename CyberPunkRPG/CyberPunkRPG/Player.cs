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
        ProjectileManager projectileManager;
        Vector2 mousePos;
        Vector2 projectileStart;
        Vector2 projectileSpeed;
        Vector2 dashSpeed;
        Vector2 startingPosition = Vector2.Zero;
        Vector2 endPosition = Vector2.Zero;
        Vector2 worldPosition;
        Vector2 prevPos;
        public Rectangle hitBox;
        private Rectangle healthbarSource;
        private Rectangle healthbarEdgesSource;
        float playerSpeed;
        int ammoCount;
        int maxDistance;
        GameWindow window;
        public int CurrentHealth = 100;
        private int healthbarWidth = 467;
        private int healthbarHeight = 44;
        bool reloading;
        bool speedBoosted;
        bool invincibleBoosted;
        bool doDeathAnimation;
        public bool gameOver;
        float speedBoostTimer;
        float invincibleTimer;
        float reloadTimer;
        float reloadTime;
        float deathAnimationTimer;
        bool jumping = false;
        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;
        MouseState currentMouseState;
        MouseState previousMouseState;
        weapon activeWeapon;

        private float weaponTimer;
        private bool weaponFire = false;
        private int damage = 10;

        protected double frameTimer;
        protected double frameInterval;
        protected int frame;
        protected int numberOfFrames;
        protected int frameWidth;
        protected Rectangle sourceRect;

        public Player(Vector2 pos, Rectangle hitBox, Camera camera, Game1 game, MapManager map, GameWindow window, ProjectileManager projectileManager) : base(pos)
        {
            this.window = window;
            this.pos = pos;
            this.camera = camera;
            this.projectileManager = projectileManager;
            this.game = game;
            this.map = map;
            playerSpeed = 120;
            ammoCount = 8;
            invincibleTimer = 5;
            speedBoostTimer = 5;
            reloadTimer = 1.5f;
            reloadTime = 1.5f;
            deathAnimationTimer = 3.6f;
            reloading = false;
            speedBoosted = false;
            invincibleBoosted = false;
            doDeathAnimation = false;
            gameOver = false;
            dashSpeed = new Vector2(300, 300);
            projectileSpeed = new Vector2(500, 500);
            this.hitBox = hitBox;
            activeWeapon = weapon.assaultRifle;

            frameTimer = 60;
            frameInterval = 60;
            frame = 0;
            numberOfFrames = 9;
            frameWidth = 64;
            sourceRect = new Rectangle(0, 0, 64, 64);
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
                damage = 50;
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

            //Inte klar
            else if (activeWeapon == weapon.rocketLauncher)
            {
                ammoCount = 1;
                reloadTime = 6.0f;
                reloadTimer = 6.0f;
                maxDistance = 1000;
            }
        }

        public override void Update(GameTime gameTime)
        {
            bool noCollision = true;
            int status = 0;
            if (CurrentHealth <= 0)
            {
                if (!doDeathAnimation)
                {
                    doDeathAnimation = true;
                    status = 1;
                }
            }

            if (status == 1)
            {
                frameTimer = 600;
                frameInterval = 600;
                status = 0;
                sourceRect.X = 0;
                sourceRect.Y = 0;
                frame = 0;
            }

            if (doDeathAnimation)
            {
                numberOfFrames = 6;
                frameWidth = 64;
                frameTimer -= gameTime.ElapsedGameTime.TotalMilliseconds;
                deathAnimationTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (deathAnimationTimer < 0)
                {
                    doDeathAnimation = false;
                    gameOver = true;
                }
            }
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
            projectileStart = pos + new Vector2(32, 32);
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
            WireColision();
            EnemyBulletCollision();
            PowerUpCollision();
            HandlePowerUpBoosts(gameTime);
            ShootProjectile(currentKeyboardState);
            Reload(currentKeyboardState, gameTime);

            weaponTimer += (float)gameTime.ElapsedGameTime.Milliseconds;

            if (activeWeapon == weapon.assaultRifle)
            {
                if (weaponTimer >= 300 || weaponTimer == 0)
                {
                    weaponTimer -= 300;
                    weaponFire = true;
                }
                else
                {
                    weaponFire = false;
                }
            }
            if (activeWeapon == weapon.pistol)
            {
                weaponFire = true;

                if (weaponFire)
                {
                    
                }
                else if (weaponFire != true)
                {

                }
            }

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
            if (jumping == false && !doDeathAnimation)
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

        private void WireColision()
        {
            foreach (BarbedWire b in map.barbedWireList)
            {
                bool wireColision = false;
                if (hitBox.Intersects(b.hitBox) && !wireColision && !speedBoosted)
                {
                    playerSpeed = b.slowMultiplier;
                    wireColision = true;
                }
                else
                {
                    playerSpeed = 100;
                    wireColision = false;
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
            if (activeWeapon == weapon.assaultRifle)
            {
                if (currentMouseState.LeftButton == ButtonState.Pressed && ammoCount >= 1 && reloading == false && weaponFire == true) //Ser till att man kan hålla inne "Q" för att skjuta
                {
                    ammoCount -= 1;
                    createNewProjectile(GetDirection(worldPosition - projectileStart));
                }
            }

            if (activeWeapon == weapon.pistol)
            {
                if (currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released && ammoCount >= 1 && reloading == false && weaponFire == true)
                {

                }
            }
            else if (currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released && ammoCount >= 1 && reloading == false)
            {
                ammoCount -= 1;
                createNewProjectile(GetDirection(worldPosition - projectileStart));
            }
        } 

        private void createNewProjectile(Vector2 direction)
        {
            Projectile projectile = new Projectile(projectileStart, projectileSpeed, direction, maxDistance, damage, map);
            projectile.distanceCheck(projectileStart);
            projectileManager.playerProjectileList.Add(projectile);
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

        private void PowerUpCollision()
        {
            foreach (InteractiveObject i in map.powerUpList)
            {
                if (hitBox.Intersects(i.interactHitBox) && i.isInteracted == false && currentKeyboardState.IsKeyDown(Keys.E) && !previousKeyboardState.IsKeyDown(Keys.E))
                {
                    i.isInteracted = true;

                    if (i is HealthPickup)
                    {
                        CurrentHealth += 25;
                        if (CurrentHealth > 100)
                        {
                            CurrentHealth = 100;
                        }
                    }
                    else if (i is Speedpickup)
                    {
                        speedBoosted = true;
                    }
                    else if (i is InvinciblePickup)
                    {
                        invincibleBoosted = true;
                    }
                }
            }
        }

        private void HandlePowerUpBoosts(GameTime gameTime)
        {
            if (speedBoosted)
            {
                speedBoostTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                playerSpeed = 200;

                if (speedBoostTimer <= 0)
                {
                    speedBoosted = false;
                    speedBoostTimer = 5;
                }
            }

            if (invincibleBoosted)
            {
                invincibleTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (invincibleTimer <= 0)
                {
                    invincibleBoosted = false;
                    invincibleTimer = 5;
                }
            }
        }

        private void EnemyBulletCollision()
        {
            foreach (Projectile e in projectileManager.enemyProjectileList)
            {
                if (hitBox.Intersects(e.hitBox) && !invincibleBoosted)
                {
                    e.Visible = false;
                    CurrentHealth -= e.damage;
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
        
        public void InteractCollision()
        {
            foreach (Door d in map.doorList)
            {
                if (hitBox.Intersects(d.interactiveObjectHitBox) && d.isInteracted == false)
                {
                    pos = prevPos;
                    if (hitBox.X > d.interactiveObjectHitBox.Right - 3)
                    {
                        pos.X += 1;
                    }
                    if (hitBox.X < d.interactiveObjectHitBox.Left)
                    {
                        pos.X -= 1;
                    }
                    if (hitBox.Y < d.interactiveObjectHitBox.Top)
                    {
                        pos.Y -= 1;
                    }
                    if (hitBox.Y > d.interactiveObjectHitBox.Bottom - 3)
                    {
                        pos.Y += 1;
                    }
                }

                if (hitBox.Intersects(d.interactHitBox) && d.isInteracted == false && currentKeyboardState.IsKeyDown(Keys.E) && !previousKeyboardState.IsKeyDown(Keys.E))
                {
                    d.isInteracted = true;
                }
                else if (hitBox.Intersects(d.interactHitBox) && d.isInteracted == true && currentKeyboardState.IsKeyDown(Keys.E) && !previousKeyboardState.IsKeyDown(Keys.E))
                {
                    d.isInteracted = false;
                }
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            Rectangle uiWeapon = new Rectangle(0, 192, 64, 64);
            /*sb.Draw(AssetManager.doorTex, hitBox, hitBox, Color.Red);*/ //ritar ut karaktärens hitbox för att testa kollision
            if (CurrentHealth > 0)
            {
                sb.Draw(AssetManager.playerTex, pos, sourceRect, Color.White, 0, new Vector2(), 1, SpriteEffects.None, 1);

                if (speedBoosted)
                {
                    Rectangle sourceRect = new Rectangle(0, 160, 32, 32);
                    sb.Draw(AssetManager.pickupTex, new Vector2(pos.X - 550, pos.Y - 540), sourceRect, Color.White, 0, new Vector2(), 2.2f, SpriteEffects.None, 1);
                    sb.DrawString(AssetManager.gameText, "Speedboost: " + (int)speedBoostTimer, new Vector2(pos.X - 560, pos.Y - 540), Color.Yellow);
                }

                if (invincibleBoosted)
                {
                    Rectangle sourceRect = new Rectangle(96, 160, 32, 32);
                    sb.Draw(AssetManager.pickupTex, new Vector2(pos.X - 450, pos.Y - 540), sourceRect, Color.White, 0, new Vector2(), 2.2f, SpriteEffects.None, 1);
                    sb.DrawString(AssetManager.gameText, "Invincible: " + (int)invincibleTimer, new Vector2(pos.X - 450, pos.Y - 540), Color.Yellow);
                }
            }
            else
            {
                sb.Draw(AssetManager.playerDeathTex, pos, sourceRect, Color.White, 0, new Vector2(), 1, SpriteEffects.None, 1);
            }
            if (activeWeapon == weapon.pistol && CurrentHealth > 0)
            {
                sb.Draw(AssetManager.pistolTex, pos, sourceRect, Color.White, 0, new Vector2(), 1, SpriteEffects.None, 1);
                sb.Draw(AssetManager.pistolTex, new Vector2(pos.X - 420, pos.Y - 600), uiWeapon, Color.White, 0, new Vector2(), 2.5f, SpriteEffects.None, 1);
                sb.DrawString(AssetManager.gameText, "Pistol", new Vector2(pos.X - 330, pos.Y - 530), Color.Yellow);
            }
            else if (activeWeapon == weapon.assaultRifle && CurrentHealth > 0)
            {
                sb.Draw(AssetManager.assaultRifleTex, pos, sourceRect, Color.White, 0, new Vector2(), 1, SpriteEffects.None, 1);
                sb.Draw(AssetManager.assaultRifleTex, new Vector2(pos.X - 420, pos.Y - 600), uiWeapon, Color.White, 0, new Vector2(), 2.5f, SpriteEffects.None, 1);
                sb.DrawString(AssetManager.gameText, "Assaultrifle", new Vector2(pos.X - 350, pos.Y - 540), Color.Yellow);
            }
            else if (activeWeapon == weapon.sniperRifle && CurrentHealth > 0)
            {
                sb.Draw(AssetManager.sniperRifleTex, pos, sourceRect, Color.White, 0, new Vector2(), 1, SpriteEffects.None, 1);
                sb.Draw(AssetManager.sniperRifleTex, new Vector2(pos.X - 420, pos.Y - 600), uiWeapon, Color.White, 0, new Vector2(), 2.5f, SpriteEffects.None, 1);
                sb.DrawString(AssetManager.gameText, "Sniper", new Vector2(pos.X - 330, pos.Y - 530), Color.Yellow);
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

            sb.Draw(AssetManager.healthbarTex, new Rectangle((int)pos.X - healthbarWidth / 2, (int)pos.Y - window.ClientBounds.Height/2, healthbarWidth, healthbarHeight), healthbarSource, Color.Gray);
            sb.Draw(AssetManager.healthbarTex, new Rectangle((int)pos.X - healthbarWidth / 2, (int)pos.Y - window.ClientBounds.Height / 2, (int)(healthbarWidth * ((double)CurrentHealth / 100)), healthbarHeight), healthbarSource, Color.Red);
            sb.Draw(AssetManager.healthbarTex, new Rectangle((int)pos.X - healthbarWidth / 2, (int)pos.Y - window.ClientBounds.Height / 2, healthbarWidth, healthbarHeight), healthbarEdgesSource, Color.White);
        }
    }
}
