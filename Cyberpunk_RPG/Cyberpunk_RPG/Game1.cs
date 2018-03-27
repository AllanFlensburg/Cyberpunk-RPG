using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Cyberpunk_RPG
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

        SpriteFont gameText;
        Texture2D playerTex;
        Texture2D enemyTex;
        Texture2D projectileTex;
        List<Enemy> enemyList = new List<Enemy>();

        Player player;

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

            gameText = Content.Load<SpriteFont>("Gametext");
            playerTex = Content.Load<Texture2D>("player");
            enemyTex = Content.Load<Texture2D>("enemy");
            projectileTex = Content.Load<Texture2D>("projectile");

            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;
            graphics.ApplyChanges();
            IsMouseVisible = true;
            CreateEnemies();

            currentKeyboardState = new KeyboardState();
            lastKeyboardState = new KeyboardState();

            player = new Player(playerTex, projectileTex, Vector2.Zero, gameText);

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
                    foreach (Enemy e in enemyList)
                    {
                        e.Update(gameTime);
                        if (Vector2.Distance(e.pos, player.pos) < 300)
                        {
                            e.pos += e.speed * GetDirection(player.pos - e.pos) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        }
                        foreach (Projectile p in player.projectileList)
                        {
                            if (e.hitBox.Intersects(p.hitBox))
                            {
                                e.isHit = true;
                                p.Visible = false;
                            }
                        }
                    }
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
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            Window.Title = "Cyberpunk-RPG";

            switch (currentGameState)
            {
                case GameState.Menu:
                    spriteBatch.DrawString(gameText, "Press ENTER to start game", new Vector2(Window.ClientBounds.Width / 4, Window.ClientBounds.Height / 3), Color.Yellow, 0, Vector2.Zero, 2, SpriteEffects.None, 1);
                    break;
                case GameState.PlayingGame:
                    foreach (Enemy e in enemyList)
                    {
                        e.Draw(spriteBatch);
                    }
                    player.Draw(spriteBatch);
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

                Enemy enemy = new Enemy(enemyTex, new Vector2(x, y * i));
                enemyList.Add(enemy);
            }
        }

        public Vector2 GetDirection(Vector2 dir)
        {
            Vector2 newDirection = dir;
            return Vector2.Normalize(newDirection);
        }
    }
}
