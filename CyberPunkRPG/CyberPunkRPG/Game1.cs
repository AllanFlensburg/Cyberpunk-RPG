﻿ using Microsoft.Xna.Framework;
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
        Door door;

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
            CreateEnemies();

            currentKeyboardState = new KeyboardState();
            lastKeyboardState = new KeyboardState();

            view = GraphicsDevice.Viewport;
            camera = new Camera(view);

            map = new MapManager();
            player = new Player(Vector2.Zero, new Rectangle(20, 10, 25, 60), camera, this, map);
            door = new Door(Vector2.Zero, new Rectangle (100, 20, 50, 50));
            Rectangle hitboxBackup = new Rectangle(20, 10, 25, 60);
            Rectangle playerBackup = new Rectangle(0, 0, 92, 76);
            MediaPlayer.Play(AssetManager.song);

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
                    foreach (Enemy e in enemyList)
                    {
                        e.Update(gameTime);
                        if (Vector2.Distance(e.pos, player.pos) < 300 & e.isHit == false)
                        {
                            e.pos += e.speed * GetDirection(player.pos - e.pos) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        }
                        foreach (Projectile p in player.projectileList)
                        {
                            if (e.hitBox.Intersects(p.hitBox) && e.isHit == false)
                            {
                                e.isHit = true;
                                p.Visible = false;
                            }
                        }

                    }
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
                    foreach (Enemy e in enemyList)
                    {
                        e.Draw(spriteBatch);
                    }

                    map.Draw(spriteBatch);
                    player.Draw(spriteBatch);
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

                Enemy basic = new BasicEnemy(new Vector2(x * i, 100));
                enemyList.Add(basic);
                Enemy strong = new StrongEnemy(new Vector2(x * i, 200));
                enemyList.Add(strong);
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
    }
}
