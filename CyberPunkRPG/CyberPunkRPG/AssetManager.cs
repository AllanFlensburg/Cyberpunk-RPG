using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace CyberPunkRPG
{
    class AssetManager
    {   
        public static Texture2D winTex { get; private set; }
        public static Texture2D endTex { get; private set; }
        public static Texture2D titleTex { get; private set; }
        public static Texture2D groundTex1 { get; private set; }
        public static Texture2D groundTex2 { get; private set; }
        public static Texture2D groundTex3 { get; private set; }
        public static Texture2D groundTex4 { get; private set; }
        public static Texture2D playerTex { get; private set; }
        public static Texture2D basicEnemyTex { get; private set; }
        public static Texture2D strongEnemyTex { get; private set; }
        public static Texture2D reloadDisplay { get; private set; }
        public static Texture2D projectileTex { get; private set; }
        public static Texture2D wallTex { get; private set; }
        public static Texture2D blindTex { get; private set; }
        public static Texture2D coverTex { get; private set; }
        public static Texture2D doorTex { get; private set; }
        public static Texture2D wireTex { get; private set; }
        public static Texture2D spikeTex { get; private set; }
        public static Texture2D pistolTex { get; private set; }
        public static Texture2D assaultRifleTex { get; private set; }
        public static Texture2D sniperRifleTex { get; private set; }
        public static Texture2D explosionTex { get; private set; }
        public static Texture2D healthbarTex { get; private set; }
        public static Texture2D pickupTex { get; private set; }
        public static Texture2D playerDeathTex { get; private set; }
        public static SpriteFont gameText { get; private set; }

        public static SoundEffect healthPowerup { get; private set; }
        public static SoundEffect speedPowerup { get; private set; }
        public static SoundEffect invinciblePowerup { get; private set; }
        public static SoundEffect assaultrifleReload { get; private set; }
        public static SoundEffect pistolReload { get; private set; }
        public static SoundEffect sniperReload { get; private set; }
        public static SoundEffect deathSound { get; private set; }

        public static Song song1 { get; private set; }
        public static Song song2 { get; private set; }
        public static Song song3 { get; private set; }
        public static Song song4 { get; private set; }
        public static Song song5 { get; private set; }
        public static Song deathSong { get; private set; }

        public static void LoadContent(ContentManager Content)
        {
            winTex = Content.Load<Texture2D>("Win");
            endTex = Content.Load<Texture2D>("End");
            titleTex = Content.Load<Texture2D>("Title");
            groundTex1 = Content.Load<Texture2D>("Ground1_01");
            groundTex2 = Content.Load<Texture2D>("Ground1_02");
            groundTex3 = Content.Load<Texture2D>("Ground1_03");
            groundTex4 = Content.Load<Texture2D>("Ground1_04");
            playerTex = Content.Load<Texture2D>("AgentShoot");
            basicEnemyTex = Content.Load<Texture2D>("Armor2Shoot");
            strongEnemyTex = Content.Load<Texture2D>("ArmorShoot");
            projectileTex = Content.Load<Texture2D>("projectile");
            reloadDisplay = Content.Load<Texture2D>("healthbar");
            wallTex = Content.Load<Texture2D>("Wall1");
            blindTex = Content.Load<Texture2D>("Wall");
            coverTex = Content.Load<Texture2D>("box");
            doorTex = Content.Load<Texture2D>("door");
            wireTex = Content.Load<Texture2D>("BarbedWire");
            spikeTex = Content.Load<Texture2D>("Spike_Pixel");
            pistolTex = Content.Load<Texture2D>("Pistols");
            assaultRifleTex = Content.Load<Texture2D>("AssaultRifle");
            sniperRifleTex = Content.Load<Texture2D>("Sniper");
            explosionTex = Content.Load<Texture2D>("Explosion");
            healthbarTex = Content.Load<Texture2D>("healthBar");
            pickupTex = Content.Load<Texture2D>("LightsUp_Spritesheet");
            playerDeathTex = Content.Load<Texture2D>("AgentWalkSouth");
            gameText = Content.Load<SpriteFont>("Gametext");

            healthPowerup = Content.Load<SoundEffect>("Powerup");
            speedPowerup = Content.Load<SoundEffect>("Powerup3");
            invinciblePowerup = Content.Load<SoundEffect>("Powerup6");
            assaultrifleReload = Content.Load<SoundEffect>("assaultrifleR");
            pistolReload = Content.Load<SoundEffect>("pistolR");
            sniperReload = Content.Load<SoundEffect>("sniperR");
            deathSound = Content.Load<SoundEffect>("deathSound");

            song1 = Content.Load<Song>("Cyberpunk Moonlight Sonata");
            song2 = Content.Load<Song>("buildy");
            song3 = Content.Load<Song>("CyberPunk_Chronicles");
            song4 = Content.Load<Song>("core_175bpm");
            song5 = Content.Load<Song>("Wonderland_Round_3");
            deathSong = Content.Load<Song>("Death Is Just Another Path");
        }
    }
}
