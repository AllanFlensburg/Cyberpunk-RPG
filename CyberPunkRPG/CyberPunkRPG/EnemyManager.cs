using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberPunkRPG
{
    class EnemyManager
    {
        Player player;
        List<string> strings = new List<string>();
        ProjectileManager projectileManager;
        MapManager map;
        public List<Enemy> enemyList = new List<Enemy>();
        public List<Blind> blindList = new List<Blind>();

        public EnemyManager(Player player, ProjectileManager projectileManager, MapManager map)
        {
            this.player = player;
            this.projectileManager = projectileManager;
            this.map = map;
            CreateAllEnemies();
        }

        void CreateAllEnemies()
        {
            StreamReader sr = new StreamReader("../../../../Content/MyMap.txt");
            while (!sr.EndOfStream)
            {
                strings.Add(sr.ReadLine());
            }
            sr.Close();

            string BasicEnemies = strings[4];
            string[] allBasicEnemyDesntinations = BasicEnemies.Split(';');

            for (int i = 0; i < allBasicEnemyDesntinations.Length; i++)
            {
                string[] allBasicEnemies = allBasicEnemyDesntinations[i].Split(',');

                try
                {
                    int x = Convert.ToInt32(allBasicEnemies[0]);
                    int y = Convert.ToInt32(allBasicEnemies[1]);
                    Vector2 basicEnemyPos = new Vector2(x, y);
                    BasicEnemy basicEnemy = new BasicEnemy(basicEnemyPos, player, map, projectileManager);
                    enemyList.Add(basicEnemy);
                }
                catch (FormatException)
                {
                    Console.WriteLine("Someting went wrong");
                }
            }

            string StrongEnemies = strings[5];
            string[] allStrongEnemyDesntinations = StrongEnemies.Split(';');

            for (int i = 0; i < allStrongEnemyDesntinations.Length; i++)
            {
                string[] allStrongEnemies = allStrongEnemyDesntinations[i].Split(',');

                try
                {
                    int x = Convert.ToInt32(allStrongEnemies[0]);
                    int y = Convert.ToInt32(allStrongEnemies[1]);
                    Vector2 strongEnemyPos = new Vector2(x, y);
                    StrongEnemy strongEnemy = new StrongEnemy(strongEnemyPos, player, map, projectileManager);
                    enemyList.Add(strongEnemy);
                }
                catch (FormatException)
                {
                    Console.WriteLine("Someting went wrong");
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (Enemy e in enemyList)
            {
                e.Update(gameTime);
                if (e.lives <= 0)
                {
                    e.isHit = true;
                }
            }
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (Enemy e in enemyList)
            {
                e.Draw(sb);
            }

            foreach (Blind b in blindList)
            {
                b.Draw(sb);
            }
        }       
    }   
}
