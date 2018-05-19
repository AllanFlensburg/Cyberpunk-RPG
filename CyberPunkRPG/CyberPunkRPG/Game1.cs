using CyberPunkRPG.Guns;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System.Media;

namespace CyberPunkRPG
{
    enum GameState
    {
        Menu,
        PlayingGame,
        GameOver,
        Restart,
    }

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        KeyboardState currentKeyboardState;
        KeyboardState lastKeyboardState;
        GameState currentGameState;
        Viewport view;
        Camera camera;
        HealthPickup h;
        InvinciblePickup i;
        Speedpickup s;
        InteractiveObject ar;
        InteractiveObject p;
        InteractiveObject sr;

        List<Enemy> enemyList = new List<Enemy>();

        MapManager map;
        Player player;
        EnemyManager enemyManager;
        ProjectileManager projectileManager;
        Rectangle endPos;
        bool wonTheGame;
        bool lostTheGame;
        BarbedWire b;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            MediaPlayer.Stop();
            AssetManager.LoadContent(Content);
            graphics.PreferredBackBufferWidth = Constants.ScreenWidth;
            graphics.PreferredBackBufferHeight = Constants.ScreenHeight;
            graphics.ApplyChanges();
            IsMouseVisible = true;

            currentKeyboardState = new KeyboardState();
            lastKeyboardState = new KeyboardState();

            view = GraphicsDevice.Viewport;
            camera = new Camera(view);

            projectileManager = new ProjectileManager();
            map = new MapManager();
            player = new Player(new Vector2(3000, 1000), new Rectangle(0, 0, 25, 55), camera, this, map, Window, projectileManager);
            enemyManager = new EnemyManager(player, projectileManager, map);
            b = new BarbedWire(Vector2.Zero, new Rectangle(3500, 1500, 50, 50));
            map.barbedWireList.Add(b);
            wonTheGame = false;
            lostTheGame = false;
            endPos = new Rectangle(4900, 3600, 50, 50);
            s = new Speedpickup(new Vector2(3200, 1200));
            map.powerUpList.Add(s);
            i = new InvinciblePickup(new Vector2(3100, 1100));
            map.powerUpList.Add(i);
            h = new HealthPickup(new Vector2(3000, 1000));
            map.powerUpList.Add(h);
            ar = new AssaultRifle(new Vector2(2900, 1000));
            map.weaponList.Add(ar);
            p = new Pistol(new Vector2(2850, 1000));
            map.weaponList.Add(p);
            sr = new SniperRifle(new Vector2(2800, 1000));
            map.weaponList.Add(sr);
            //rl = new RocketLauncher(new Vector2(2800, 920));
            //map.weaponList.Add(rl);

            //Rectangle hitboxBackup = new Rectangle(20, 10, 25, 60); Backup värden för när vi testade hitbox
            //Rectangle playerBackup = new Rectangle(0, 0, 92, 76); Backup värden för när vi testade hitbox

            currentGameState = GameState.Menu;
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }
            currentKeyboardState = Keyboard.GetState();

            switch (currentGameState)
            {
                case GameState.Menu:
                    if (ButtonPressed())
                        currentGameState = GameState.PlayingGame;
                    break;
                case GameState.PlayingGame:
                    if (ButtonPressed())
                        currentGameState = GameState.GameOver;
                    break;
                case GameState.GameOver:
                    if (ButtonPressed())
                        currentGameState = GameState.Restart;
                    break;
            }

            switch (currentGameState)
            {
                case GameState.Menu:
                    camera.SetPosition(new Vector2(Window.ClientBounds.Width / 2 + 200, Window.ClientBounds.Height / 2));
                    break;
                case GameState.PlayingGame:
                    EndGame();
                    player.Update(gameTime);
                    enemyManager.Update(gameTime);
                    projectileManager.Update(gameTime);
                    ChangeMusic();
                    camera.SetPosition(player.pos);
                    map.Update(gameTime);
                    break;
                case GameState.GameOver:
                    camera.SetPosition(new Vector2 (Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 2));
                    break;
                case GameState.Restart: // Man kan starta om när en spelrunda tar slut, loadcontent ändrar automatiskt till meny igen
                    LoadContent();
                    break;
            }

            lastKeyboardState = currentKeyboardState;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, camera.GetTransformation(view));
            Window.Title = "Chromium Wars";

            switch (currentGameState)
            {
                case GameState.Menu:
                    spriteBatch.Draw(AssetManager.titleTex, new Vector2(200, 0), Color.White);
                    break;
                case GameState.PlayingGame:

                    map.Draw(spriteBatch);
                    spriteBatch.Draw(AssetManager.doorTex, endPos, Color.White);
                    enemyManager.Draw(spriteBatch);
                    projectileManager.Draw(spriteBatch);
                    player.Draw(spriteBatch);
                    break;
                case GameState.GameOver:
                    if (wonTheGame)
                    {
                        spriteBatch.Draw(AssetManager.winTex, new Vector2(0, 0), Color.White);
                    }
                    else if (lostTheGame)
                    {
                        spriteBatch.Draw(AssetManager.endTex, new Vector2(0, 0), Color.White);
                    }
                    break;
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private bool ButtonPressed()
        {
            if (currentKeyboardState.IsKeyUp(Keys.Enter) && lastKeyboardState.IsKeyDown(Keys.Enter))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Vector2 GetDirection(Vector2 dir)
        {
            Vector2 newDirection = dir;
            return Vector2.Normalize(newDirection);
        }

        public Vector2 GetCameraPosition()
        {
            return camera.position;
        }

        public void EndGame()
        {
            if (player.hitBox.Intersects(endPos))
            {
                wonTheGame = true;
                currentGameState = GameState.GameOver;
            }
            else if (player.gameOver)
            {
                lostTheGame = true;
                currentGameState = GameState.GameOver;
                PlayDeathSong();
            }
        }

        public void ChangeMusic()
        {
            if (currentKeyboardState.IsKeyDown(Keys.F1) == true)
            {
                MediaPlayer.Stop();
                MediaPlayer.Play(AssetManager.song1);
            }
            else if (currentKeyboardState.IsKeyDown(Keys.F2) == true)
            {
                MediaPlayer.Stop();
                MediaPlayer.Play(AssetManager.song2);
            }
            else if (currentKeyboardState.IsKeyDown(Keys.F3) == true)
            {
                MediaPlayer.Stop();
                MediaPlayer.Play(AssetManager.song3);
            }
            else if (currentKeyboardState.IsKeyDown(Keys.F4) == true)
            {
                MediaPlayer.Stop();
                MediaPlayer.Play(AssetManager.song4);
            }
            else if (currentKeyboardState.IsKeyDown(Keys.F5) == true)
            {
                MediaPlayer.Stop();
                MediaPlayer.Play(AssetManager.song5);
            }
        }

        public void PlayDeathSong()
        {
            MediaPlayer.Stop();
            MediaPlayer.Play(AssetManager.deathSong);
        }
    }
}
