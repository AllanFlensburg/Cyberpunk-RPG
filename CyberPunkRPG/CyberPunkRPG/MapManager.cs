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
    class MapManager
    {
        List<string> strings = new List<string>();
        List<Wall> wallList = new List<Wall>();
        List<Cover> coverList = new List<Cover>();

        public MapManager()
        {
            CreateCurrentLevel();
        }

        void CreateCurrentLevel()
        {
            StreamReader sr = new StreamReader("MyMap.txt");
            while (!sr.EndOfStream)
            {
                strings.Add(sr.ReadLine());
            }
            sr.Close();

            string Walls = strings[0];
            string[] allWallDesntinations = Walls.Split(';');

            for (int i = 0; i < allWallDesntinations.Length; i++)
            {
                string[] allWalls = allWallDesntinations[i].Split(',');

                try
                {
                    int x = Convert.ToInt32(allWalls[0]);
                    int y = Convert.ToInt32(allWalls[1]);
                    int Width = Convert.ToInt32(allWalls[2]);
                    int Height = Convert.ToInt32(allWalls[3]);
                    Rectangle wallDestination = new Rectangle(x, y, Width, Height);
                    Wall wall = new Wall(new Vector2(x, y), true);
                    wallList.Add(wall);
                }
                catch (FormatException e)
                {
                    Console.WriteLine("Someting went wrong");
                }
            }

            string Covers = strings[1];
            string[] allCoverDesntinations = Covers.Split(';');

            for (int i = 0; i < allCoverDesntinations.Length; i++)
            {
                string[] allCovers = allCoverDesntinations[i].Split(',');

                try
                {
                    int x = Convert.ToInt32(allCovers[0]);
                    int y = Convert.ToInt32(allCovers[1]);
                    int Width = Convert.ToInt32(allCovers[2]);
                    int Height = Convert.ToInt32(allCovers[3]);
                    Rectangle coverDestination = new Rectangle(x, y, Width, Height);
                    Cover cover = new Cover();
                    coverList.Add(cover);
                }
                catch (FormatException e)
                {
                    Console.WriteLine("Someting went wrong");
                }
            }
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (Wall w in wallList)
            {
                //w.Draw(sb);
            }

            foreach (Cover c in coverList)
            {
                //c.Draw(sb);
            }
        }
    }
}
