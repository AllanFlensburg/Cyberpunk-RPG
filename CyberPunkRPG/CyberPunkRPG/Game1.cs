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
        Door door;
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
            player = new Player(Vector2.Zero, new Rectangle(0, 0, 25, 55), camera, this, map, Window);
            enemyManager = new EnemyManager(player, projectileManager, map);
            CreateEnemies();
            door = new Door(Vector2.Zero, new Rectangle (100, 20, 50, 50));
            b = new BarbedWire(Vector2.Zero, new Rectangle(500, 500, 50, 50));
            map.barbedWireList.Add(b);
            //Rectangle hitboxBackup = new Rectangle(20, 10, 25, 60); Backup värden för när vi testade hitbox
            //Rectangle playerBackup = new Rectangle(0, 0, 92, 76); Backup värden för när vi testade hitbox

            currentGameState = GameState.PlayingGame;
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

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

                    break;
                case GameState.PlayingGame:
                    player.Update(gameTime);
                    door.Update(gameTime);
                    enemyManager.Update(gameTime);
                    projectileManager.Update(gameTime);
                    ChangeMusic();
                    camera.SetPosition(player.pos);
                    break;
                case GameState.GameOver:
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
            GraphicsDevice.Clear(Color.DarkSlateGray);

            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, camera.GetTransformation(view));
            Window.Title = "Cyberpunk-RPG";

            switch (currentGameState)
            {
                case GameState.Menu:
                    spriteBatch.DrawString(AssetManager.gameText, "Press ENTER to start game", new Vector2(Window.ClientBounds.Width / 4, Window.ClientBounds.Height / 3), Color.Yellow, 0, Vector2.Zero, 2, SpriteEffects.None, 1);
                    break;
                case GameState.PlayingGame:
                    map.Draw(spriteBatch);
                    player.Draw(spriteBatch);
                    enemyManager.Draw(spriteBatch);
                    projectileManager.Draw(spriteBatch);
                    door.Draw(spriteBatch);
                    break;
                case GameState.GameOver:

                    break;
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private bool ButtonPressed()
        {
            if (currentKeyboardState.IsKeyUp(Keys.Enter) && lastKeyboardState.IsKeyDown(Keys.Enter))
                return true;
            else
                return false;
        }

        public void CreateEnemies()
        {
            for (int i = 0; i < 3; i++)
            {
                int x = 400;
                int y = 100;

                Enemy basic = new BasicEnemy(new Vector2(x * i, 100), player, map, projectileManager);
                enemyManager.enemyList.Add(basic);
                Enemy strong = new StrongEnemy(new Vector2(x * i, 200), player, map, projectileManager);
                enemyManager.enemyList.Add(strong);
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
        }
    }
}
