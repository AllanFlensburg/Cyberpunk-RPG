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

        List<Enemy> enemyList = new List<Enemy>();

        MapManager map;
        Player player;
        EnemyManager enemyManager;
        ProjectileManager projectileManager;
        Rectangle endPos;

        bool wonTheGame;
        bool lostTheGame;
        
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
            graphics.IsFullScreen = true;
            graphics.ApplyChanges();
            IsMouseVisible = true;

            currentKeyboardState = new KeyboardState();
            lastKeyboardState = new KeyboardState();

            view = GraphicsDevice.Viewport;
            camera = new Camera(view);

            projectileManager = new ProjectileManager();
            map = new MapManager();
            player = new Player(new Vector2(6030, 5350), new Rectangle(0, 0, 25, 55), camera, this, map, Window, projectileManager);
            endPos = new Rectangle(10270, 2370, 70, 70);
            enemyManager = new EnemyManager(player, projectileManager, map);
            wonTheGame = false;
            lostTheGame = false;
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
                    spriteBatch.Draw(AssetManager.tunnelTex, endPos, Color.White);
                    enemyManager.Draw(spriteBatch);
                    foreach (Blind b in map.blindList)
                    {
                        b.Draw(spriteBatch);
                    }
                    projectileManager.Draw(spriteBatch);
                    player.Draw(spriteBatch);
                    spriteBatch.Draw(AssetManager.fenceTex1, new Vector2(4000, 0), Color.White);
                    spriteBatch.Draw(AssetManager.fenceTex2, new Vector2(8000, 0), Color.White);
                    player.DrawHealthbar(spriteBatch);


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
            if (currentKeyboardState.IsKeyDown(Keys.F1) == true && !lastKeyboardState.IsKeyDown(Keys.F1))
            {
                MediaPlayer.Resume();
                MediaPlayer.Stop();
                MediaPlayer.Play(AssetManager.song1);
            }
            else if (currentKeyboardState.IsKeyDown(Keys.F2) == true && !lastKeyboardState.IsKeyDown(Keys.F2))
            {
                MediaPlayer.Stop();
                MediaPlayer.Play(AssetManager.song2);
            }
            else if (currentKeyboardState.IsKeyDown(Keys.F3) == true && !lastKeyboardState.IsKeyDown(Keys.F3))
            {
                MediaPlayer.Stop();
                MediaPlayer.Play(AssetManager.song3);
            }
            else if (currentKeyboardState.IsKeyDown(Keys.F4) == true && !lastKeyboardState.IsKeyDown(Keys.F4))
            {
                MediaPlayer.Stop();
                MediaPlayer.Play(AssetManager.song4);
            }
            else if (currentKeyboardState.IsKeyDown(Keys.F5) == true && !lastKeyboardState.IsKeyDown(Keys.F5))
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
