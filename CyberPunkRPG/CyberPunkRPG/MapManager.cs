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
        public List<Wall> wallList = new List<Wall>();
        public List<Cover> coverList = new List<Cover>();
        public List<Door> doorList = new List<Door>();
        public List<BarbedWire> barbedWireList = new List<BarbedWire>();
        public List<Spikes> spikeList = new List<Spikes>();

        public MapManager()
        {
            CreateCurrentLevel();
        }

        void CreateCurrentLevel()
        {
            StreamReader sr = new StreamReader("../../../../Content/MyMap.txt");
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
                    Wall wall = new Wall(Vector2.Zero, wallDestination, true);
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
                    Cover cover = new Cover(Vector2.Zero, coverDestination, true);
                    coverList.Add(cover);
                }
                catch (FormatException e)
                {
                    Console.WriteLine("Someting went wrong");
                }
            }
            
            string Doors = strings[2];
            string[] allDoorDesntinations = Doors.Split(';');

            for (int i = 0; i < allDoorDesntinations.Length; i++)
            {
                string[] allDoors = allDoorDesntinations[i].Split(',');

                try
                {
                    int x = Convert.ToInt32(allDoors[0]);
                    int y = Convert.ToInt32(allDoors[1]);
                    int Width = Convert.ToInt32(allDoors[2]);
                    int Height = Convert.ToInt32(allDoors[3]);
                    Rectangle doorDestination = new Rectangle(x, y, Width, Height);
                    Door door = new Door(Vector2.Zero, doorDestination);
                    doorList.Add(door);
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
                w.Draw(sb);
            }

            foreach (Cover c in coverList)
            {
                c.Draw(sb);
            }

            foreach (Door c in doorList)
            {
                c.Draw(sb);
            }
        }
    }
}
