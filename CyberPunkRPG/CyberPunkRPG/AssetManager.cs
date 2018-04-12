using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace CyberPunkRPG
{
    class AssetManager
    {
        public static Texture2D playerTex { get; private set; }
        public static Texture2D basicEnemyTex { get; private set; }
        public static Texture2D strongEnemyTex { get; private set; }
        public static Texture2D reloadDisplay { get; private set; }
        public static Texture2D projectileTex { get; private set; }
        public static Texture2D wallTex { get; private set; }
        public static Texture2D coverTex { get; private set; }
        public static Texture2D doorTex { get; private set; }
        public static Texture2D wireTex { get; private set; }
        public static Texture2D spikeTex { get; private set; }
        public static SpriteFont gameText { get; private set; }
        public static Song song { get; private set; }

        public static void LoadContent(ContentManager Content)
        {            
            playerTex = Content.Load<Texture2D>("AgentShoot");
            basicEnemyTex = Content.Load<Texture2D>("Armor2Shoot");
            strongEnemyTex = Content.Load<Texture2D>("ArmorShoot");
            projectileTex = Content.Load<Texture2D>("projectile");
            reloadDisplay = Content.Load<Texture2D>("healthbar");
            wallTex = Content.Load<Texture2D>("Wall1");
            coverTex = Content.Load<Texture2D>("box");
            doorTex = Content.Load<Texture2D>("door");
            wireTex = Content.Load<Texture2D>("BarbedWire");
            spikeTex = Content.Load<Texture2D>("Spikes");
            gameText = Content.Load<SpriteFont>("Gametext");
            song = Content.Load<Song>("Cyberpunk Moonlight Sonata");
        }
    }
}
