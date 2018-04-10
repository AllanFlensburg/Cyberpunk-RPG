using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CyberPunkRPG
{
    class AssetManager
    {
        public static Texture2D playerTex { get; private set; }
        public static Texture2D enemyTex { get; private set; }
        public static Texture2D reloadDisplay { get; private set; }
        public static Texture2D projectileTex { get; private set; }
        public static Texture2D wallTex { get; private set; }
        public static Texture2D coverTex { get; private set; }
        public static Texture2D doorTex { get; private set; }
        public static SpriteFont gameText { get; private set; }

        public static void LoadContent(ContentManager Content)
        {            
            playerTex = Content.Load<Texture2D>("player");
            enemyTex = Content.Load<Texture2D>("enemy");
            projectileTex = Content.Load<Texture2D>("projectile");
            reloadDisplay = Content.Load<Texture2D>("healthbar");
            wallTex = Content.Load<Texture2D>("Wall");
            coverTex = Content.Load<Texture2D>("Cover");
            doorTex = Content.Load<Texture2D>("door");
            gameText = Content.Load<SpriteFont>("Gametext");
        }
    }
}
