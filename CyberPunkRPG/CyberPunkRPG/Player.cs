using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
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
        float standardPlayerSpeed;
        int ammoCount;
        int ammoCapacity;
        int maxDistance;
        GameWindow window;
        public float CurrentHealth = 100f;
        private int healthbarWidth = 467;
        private int healthbarHeight = 44;
        bool reloading;
        bool speedBoosted;
        bool invincibleBoosted;
        bool doDeathAnimation;
        bool readyToDash;
        bool isMoving;
        public bool gameOver;
        float speedBoostTimer;
        float invincibleTimer;
        float dashTimer;
        float dashCooldown;
        float reloadTimer;
        float reloadTime;
        float deathAnimationTimer;
        bool jumping = false;
        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;
        MouseState currentMouseState;
        MouseState previousMouseState;
        public weapon activeWeapon;

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
            standardPlayerSpeed = 125;
            invincibleTimer = 5;
            speedBoostTimer = 5;
            dashTimer = 2f;
            dashCooldown = 2f;
            reloadTimer = 1.5f;
            reloadTime = 1.5f;
            deathAnimationTimer = 3.8f;
            readyToDash = true;
            reloading = false;
            speedBoosted = false;
            invincibleBoosted = false;
            doDeathAnimation = false;
            gameOver = false;
            
            dashSpeed = new Vector2(200, 200);
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

            CheckActiveWeapon();
            if (activeWeapon == weapon.assaultRifle)
            {
                ammoCapacity = 30;
                reloadTime = 2.0f;
                reloadTimer = 2.0f;
                maxDistance = 600;
            }
            else if (activeWeapon == weapon.sniperRifle)
            {
                damage = 50;
                ammoCapacity = 5;
                reloadTime = 3.0f;
                reloadTimer = 3.0f;
                projectileSpeed = new Vector2(1000, 1000);
                maxDistance = 2000;

            }
            else if (activeWeapon == weapon.pistol)
            {
                CheckActiveWeapon();
                ammoCapacity = 8;
                reloadTime = 0.1f;
                reloadTimer = 1.5f;
                maxDistance = 500;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (!readyToDash)
            {
                dashTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (dashTimer <= 0)
            {
                dashTimer = dashCooldown;
                readyToDash = true;
            }
            bool noCollision = true;
            int status = 0;
            if (CurrentHealth <= 0)
            {
                if (!doDeathAnimation && !gameOver)
                {
                    MediaPlayer.Stop();
                    AssetManager.deathSound.Play(0.5f, 0.0f, 0.0f);
                    doDeathAnimation = true;
                    status = 1;
                }
            }

            if (status == 1)
            {
                frameTimer = 650;
                frameInterval = 650;
                status = 0;
                sourceRect.X = 0;
                sourceRect.Y = 0;
                frame = 0;
                status += 1;
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
            SpikeColision(gameTime);
            EnemyBulletCollision();
            PowerUpCollision();
            WeaponCollision();
            blindCollision();
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
                    pos.X -= standardPlayerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    frameTimer -= gameTime.ElapsedGameTime.TotalMilliseconds;
                    isMoving = true;
                }

                if (currentKeyboardState.IsKeyDown(Keys.D) == true)
                {
                    sourceRect.Y = 192;
                    pos.X += standardPlayerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    frameTimer -= gameTime.ElapsedGameTime.TotalMilliseconds;
                    isMoving = true;
                }

                if (currentKeyboardState.IsKeyDown(Keys.S) == true)
                {
                    sourceRect.Y = 128;
                    pos.Y += standardPlayerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    frameTimer -= gameTime.ElapsedGameTime.TotalMilliseconds;
                    isMoving = true;
                }

                if (currentKeyboardState.IsKeyDown(Keys.W) == true)
                {
                    sourceRect.Y = 0;
                    pos.Y -= standardPlayerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    frameTimer -= gameTime.ElapsedGameTime.TotalMilliseconds;
                    isMoving = true;
                }

                if (currentKeyboardState.IsKeyDown(Keys.Space) == true && readyToDash && isMoving && prevPos != pos)
                {
                    readyToDash = false;
                    startingPosition = pos;
                    endPosition = pos += GetDirection(pos - prevPos) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    jumping = true;
                }
            }

            if (jumping == true)
            {
                pos += dashSpeed * GetDirection(endPosition - startingPosition) * (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (Vector2.Distance(startingPosition, pos) > 120)
                {
                    jumping = false;
                    startingPosition = Vector2.Zero;
                    endPosition = Vector2.Zero;
                }
            }

            isMoving = false;
        }

        public void CheckActiveWeapon()
        {
            if (activeWeapon == weapon.assaultRifle)
            {
                reloadTime = 3f;
                reloadTimer = reloadTime;
                ammoCapacity = 30;
                ammoCount = ammoCapacity;
            }

            else if (activeWeapon == weapon.sniperRifle)
            {
                damage = 50;
                reloadTime = 5f;
                reloadTimer = reloadTime;
                ammoCapacity = 5;
                ammoCount = ammoCapacity;
            }
            else if (activeWeapon == weapon.pistol)
            {
                reloadTime = 2f;
                reloadTimer = reloadTime;
                ammoCapacity = 8;
                ammoCount = ammoCapacity;
            }
        }

        private void WireColision()
        {
            foreach (BarbedWire b in map.barbedWireList)
            {
                bool wireColision = false;
                if (hitBox.Intersects(b.hitBox) && !wireColision && !speedBoosted)
                {
                    standardPlayerSpeed = b.slowMultiplier;
                    wireColision = true;
                }
                else
                {
                    standardPlayerSpeed = 125;
                    wireColision = false;
                }
            }
        }

        private void SpikeColision(GameTime gameTime)
        {
            foreach (Spikes s in map.spikeList)
            {
                if (hitBox.Intersects(s.hitBox) && s.isHidden == true)
                {
                    s.isHidden = false;
                    CurrentHealth -= 10;
                }

                if (hitBox.Intersects(s.hitBox) && CurrentHealth > 0)
                {
                    CurrentHealth = CurrentHealth - (s.damage * (float)gameTime.ElapsedGameTime.TotalSeconds);
                }
            }
        }

        private void Reload(KeyboardState currentKeyboardState, GameTime gameTime)
        {
            if (currentKeyboardState.IsKeyDown(Keys.R) == true && ammoCapacity != ammoCount && CurrentHealth > 0)
            {
                CheckActiveWeapon();
                reloading = true;
                PlayReloadSound();
            }

            if (reloading == true)
            {
                reloadTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (reloadTimer <= 0)
                {
                    reloading = false;
                    reloadTimer = reloadTime;
                }
            }
        }

        private void PlayReloadSound()
        {
            if (activeWeapon == weapon.pistol)
            {
                AssetManager.pistolReload.Play(0.4f, 0.0f, 0.0f);
            }
            else if (activeWeapon == weapon.assaultRifle)
            {
                AssetManager.assaultrifleReload.Play(0.4f, 0.0f, 0.0f);
            }
            else if (activeWeapon == weapon.sniperRifle)
            {
                AssetManager.sniperReload.Play(0.4f, 0.0f, 0.0f);
            }
        }

        private void ShootProjectile(KeyboardState currentKeyboardState)
        {
            if (activeWeapon == weapon.assaultRifle)
            {
                if (currentMouseState.LeftButton == ButtonState.Pressed && ammoCount >= 1 && reloading == false && weaponFire == true) //Ser till att man kan hålla inne "Left Mouse Click" för att skjuta
                {
                    ammoCount -= 1;
                    createNewProjectile(GetDirection(worldPosition - projectileStart));
                }
            }

            if (activeWeapon == weapon.pistol)
            {
                if (currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released && ammoCount >= 1 && reloading == false && weaponFire == true)
                {
                    ammoCount -= 1;
                    createNewProjectile(GetDirection(worldPosition - projectileStart));
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
            Projectile projectile = new Projectile(AssetManager.playerProjectileTex, projectileStart, projectileSpeed, direction, maxDistance, damage, map);
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
                    if (!jumping)
                    {
                        pos = prevPos;
                    }
                    hitBox.X = (int)pos.X + 20;
                    hitBox.Y = (int)pos.Y + 10;

                    if (hitBox.X > w.hitBox.Right -10)
                    {
                        pos.X += 2;
                        if (jumping)
                        {
                            startingPosition.X = pos.X += 20;
                            JumpReset();
                        }
                    }
                    if (hitBox.X < w.hitBox.Left)
                    {
                        pos.X -= 2;
                        if (jumping)
                        {
                            startingPosition.X = pos.X -= 20;
                            JumpReset();
                        }
                    }
                    if (hitBox.Y < w.hitBox.Top)
                    {
                        pos.Y -= 2;
                        if (jumping)
                        {
                            startingPosition.Y = pos.Y -= 20;
                            JumpReset();
                        }
                    }
                    if (hitBox.Y > w.hitBox.Bottom -6)
                    {
                        pos.Y += 2;
                        if (jumping)
                        {
                            startingPosition.Y = pos.Y += 20;
                            JumpReset();
                        }
                    }
                }
            }
        }

        private void JumpReset()
        {
            endPosition = endPosition * -1;
            jumping = false;
            startingPosition = Vector2.Zero;
            endPosition = Vector2.Zero;
        }

        private void PowerUpCollision()
        {
            foreach (InteractiveObject i in map.powerUpList)
            {
                if (hitBox.Intersects(i.interactHitBox) && !i.isInteracted && currentKeyboardState.IsKeyDown(Keys.E) && !previousKeyboardState.IsKeyDown(Keys.E))
                {
                    i.isInteracted = true;

                    if (i is HealthPickup)
                    {
                        CurrentHealth += 25;
                        if (CurrentHealth > 100)
                        {
                            CurrentHealth = 100;
                        }
                        AssetManager.healthPowerup.Play(0.4f, 0.0f, 0.0f);
                    }
                    else if (i is Speedpickup)
                    {
                        speedBoosted = true;
                        AssetManager.speedPowerup.Play(0.4f, 0.0f, 0.0f);
                    }
                    else if (i is InvinciblePickup)
                    {
                        invincibleBoosted = true;
                        AssetManager.invinciblePowerup.Play(0.4f, 0.0f, 0.0f);
                    }
                }
            }
        }

        private void WeaponCollision()
        {
            foreach (InteractiveObject w in map.weaponList)
            {
                if (hitBox.Intersects(w.interactHitBox) && !w.isInteracted && currentKeyboardState.IsKeyDown(Keys.E) && !previousKeyboardState.IsKeyDown(Keys.E))
                {
                    w.isInteracted = true;
                    int weaponType = w.myIdentifier();
                    DropCurrentWeapon();

                    if (weaponType == 1)
                    {
                        activeWeapon = weapon.assaultRifle;
                        ammoCount = 30;
                    }
                    if (weaponType == 2)
                    {
                        activeWeapon = weapon.sniperRifle;
                        ammoCount = 5;
                    }
                    if (weaponType == 3)
                    {
                        activeWeapon = weapon.pistol;
                        ammoCount = 8;
                    }

                    CheckActiveWeapon();
                    break;
                }
            }
        }

        private void DropCurrentWeapon()
        {
            if (activeWeapon == weapon.assaultRifle)
            {
                InteractiveObject ar = new AssaultRifle(pos);
                map.weaponList.Add(ar);
            }
            else if (activeWeapon == weapon.pistol)
            {
                InteractiveObject pi = new Guns.Pistol(pos);
                map.weaponList.Add(pi);
            }
            else if (activeWeapon == weapon.sniperRifle)
            {
                InteractiveObject sn = new SniperRifle(pos);
                map.weaponList.Add(sn);
            }
        }

        private void HandlePowerUpBoosts(GameTime gameTime)
        {
            if (speedBoosted)
            {
                speedBoostTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                standardPlayerSpeed = 200;
                dashSpeed = new Vector2(300, 300);

                if (speedBoostTimer <= 0)
                {
                    speedBoosted = false;
                    speedBoostTimer = 5;
                    standardPlayerSpeed = 125;
                    dashSpeed = new Vector2(200, 200);
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
                    if (!jumping)
                    {
                        pos = prevPos;
                    }
                    hitBox.X = (int)pos.X + 20;
                    hitBox.Y = (int)pos.Y + 10;

                    if (hitBox.X > c.hitBox.Right - 3)
                    {
                        pos.X += 1;
                        if (jumping)
                        {
                            startingPosition.X = pos.X += 20;
                            JumpReset();
                        }
                    }
                    if (hitBox.X < c.hitBox.Left)
                    {
                        pos.X -= 1;
                        if (jumping)
                        {
                            startingPosition.X = pos.X -= 20;
                            JumpReset();
                        }
                    }
                    if (hitBox.Y < c.hitBox.Top)
                    {
                        pos.Y -= 1;
                        if (jumping)
                        {
                            startingPosition.Y = pos.Y -= 20;
                            JumpReset();
                        }
                    }
                    if (hitBox.Y > c.hitBox.Bottom - 3)
                    {
                        pos.Y += 1;
                        if (jumping)
                        {
                            startingPosition.Y = pos.Y += 20;
                            JumpReset();
                        }
                    }
                }
            }
        }
        
        public void InteractCollision()
        {
            foreach (Door d in map.doorList)
            {
                if (hitBox.Intersects(d.doorHitBox) && d.isInteracted == false)
                {
                    if (!jumping)
                    {
                        pos = prevPos;
                    }

                    if (hitBox.X > d.doorHitBox.Right - 3)
                    {
                        pos.X += 1;
                        if (jumping)
                        {
                            startingPosition.X = pos.X += 20;
                            JumpReset();
                        }
                    }
                    if (hitBox.X < d.doorHitBox.Left)
                    {
                        pos.X -= 1;
                        if (jumping)
                        {
                            startingPosition.X = pos.X -= 20;
                            JumpReset();
                        }
                    }
                    if (hitBox.Y < d.doorHitBox.Top)
                    {
                        pos.Y -= 1;
                        if (jumping)
                        {
                            startingPosition.Y = pos.Y -= 20;
                            JumpReset();
                        }
                    }
                    if (hitBox.Y > d.doorHitBox.Bottom - 3)
                    {
                        pos.Y += 1;
                        if (jumping)
                        {
                            startingPosition.Y = pos.Y += 20;
                            JumpReset();
                        }
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

        public void blindCollision()
        {
            foreach (Blind b in map.blindList)
            {
                if (hitBox.Intersects(b.blindHitBox))
                {
                    b.isBlind = false;
                }
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            Rectangle uiWeapon = new Rectangle(0, 192, 64, 64);
            //sb.Draw(AssetManager.doorTex, hitBox, hitBox, Color.Red);  //ritar ut karaktärens hitbox för att testa kollision
            if (CurrentHealth > 0)
            {
                sb.Draw(AssetManager.playerTex, pos, sourceRect, Color.White, 0, new Vector2(), 1, SpriteEffects.None, 1);

                if (speedBoosted)
                {
                    Rectangle sourceRect = new Rectangle(0, 160, 32, 32);
                    sb.Draw(AssetManager.pickupTex, new Vector2(pos.X - 550, pos.Y - 540), sourceRect, Color.White, 0, new Vector2(), 2.2f, SpriteEffects.None, 1);
                    sb.DrawString(AssetManager.gameText, "Speedboost: " + (int)speedBoostTimer, new Vector2(pos.X - 560, pos.Y - 540), Color.Green);
                }

                if (invincibleBoosted)
                {
                    Rectangle sourceRect = new Rectangle(96, 160, 32, 32);
                    sb.Draw(AssetManager.pickupTex, new Vector2(pos.X - 450, pos.Y - 540), sourceRect, Color.White, 0, new Vector2(), 2.2f, SpriteEffects.None, 1);
                    sb.DrawString(AssetManager.gameText, "Invincible: " + (int)invincibleTimer, new Vector2(pos.X - 450, pos.Y - 540), Color.Green);
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
                sb.DrawString(AssetManager.gameText, "Pistol", new Vector2(pos.X - 330, pos.Y - 530), Color.Green);
            }
            else if (activeWeapon == weapon.assaultRifle && CurrentHealth > 0)
            {
                sb.Draw(AssetManager.assaultRifleTex, pos, sourceRect, Color.White, 0, new Vector2(), 1, SpriteEffects.None, 1);
                sb.Draw(AssetManager.assaultRifleTex, new Vector2(pos.X - 420, pos.Y - 600), uiWeapon, Color.White, 0, new Vector2(), 2.5f, SpriteEffects.None, 1);
                sb.DrawString(AssetManager.gameText, "Assault Rifle", new Vector2(pos.X - 350, pos.Y - 540), Color.Green);
            }
            else if (activeWeapon == weapon.sniperRifle && CurrentHealth > 0)
            {
                sb.Draw(AssetManager.sniperRifleTex, pos, sourceRect, Color.White, 0, new Vector2(), 1, SpriteEffects.None, 1);
                sb.Draw(AssetManager.sniperRifleTex, new Vector2(pos.X - 420, pos.Y - 600), uiWeapon, Color.White, 0, new Vector2(), 2.5f, SpriteEffects.None, 1);
                sb.DrawString(AssetManager.gameText, "Sniper", new Vector2(pos.X - 330, pos.Y - 530), Color.Green);
            }
            sb.DrawString(AssetManager.gameText, ammoCount.ToString(), pos - new Vector2(0, 40), Color.Green);
            sb.DrawString(AssetManager.gameText, new Vector2(pos.X, pos.Y).ToString(), pos - new Vector2(0, 10), Color.Blue); //Tillfälliga koordianter

            if (ammoCount == 0 & reloading == false)
            {
                sb.DrawString(AssetManager.gameText, "Press R to Reload", pos - new Vector2(0, 20), Color.Green);
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
