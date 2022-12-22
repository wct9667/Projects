using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TestofWills
{
    /// <summary>
    /// this class will store a list of all the possible projectiles
    /// will load them into a list
    /// Note: 
    /// Methods: LoadData
    /// </summary>
    class DataManager
    {
        //fields
        List<Projectile> projectileData = new List<Projectile>();
        private Texture2D playerBullets;
        private Texture2D bossBullets1;
        private Texture2D bossBullets2;
        private Texture2D bossBullets3;
        private Texture2D testRectangle;


        //attack patterns
        List<List<int>> attackPatterns = new List<List<int>>();


        //properties
        public List<Projectile> ProjectileData
        {
            get { return projectileData; }
        }

        //attack pattern data get
        public List<List<int>> AttackPatternData
        {
            get { return attackPatterns; }
        }


        //constructor
        public DataManager(ContentManager Content, string filename)
        {
            bossBullets1 = bossBullets1 = Content.Load<Texture2D>("boss bullets1");
            bossBullets2 = Content.Load<Texture2D>("boss bullets2");
            bossBullets3 = Content.Load<Texture2D>("boss bullets3");
            playerBullets = Content.Load<Texture2D>("new player Bullets");
            testRectangle = Content.Load<Texture2D>("Grey rectangle");
            LoadDataProj(filename);
            LoadDataAttacks("../../../AttackPatterns.txt");
        }

        /// <summary>
        /// This method reads in a text file, that contains all the projectile data
        /// Notes: the position is set to 0,0, load the position when copying the data
        /// </summary>
        /// <param name="filename"></param>
        private void LoadDataProj(string filename)
        {
            try
            {
                //open the streamreader
                StreamReader reader = new StreamReader(filename);
                //read in the first line of useless stuff
                string line = reader.ReadLine();
                while ((line = reader.ReadLine()) != null)
                {
                    string[] data = line.Split(",");
                    Texture2D local;
                    ProjectileType proj;
                    //the 5th spot holds the data for the type of projectile, if 1 the attack is a player, if anything else it is a boss,
                    //will also assign a texture based on this number, the textures will be loaded in game1 and passed to the dataManager
                    switch (data[5])
                    {
                        case "1":
                            local = playerBullets;
                            proj = ProjectileType.player;
                            break;
                        case "2":
                            local = bossBullets1;
                            proj = ProjectileType.boss;
                            break;
                        case "3":
                            local = bossBullets2;
                            proj = ProjectileType.boss;
                            break;
                        case "4":
                            //more cases can be added with more types of projectiles
                            local = bossBullets3;
                            proj = ProjectileType.boss;
                            break;
                        default:
                            local = bossBullets3;
                            proj = ProjectileType.boss;
                            break;

                    }

                    //the projectile will have a default value of 0,0 for the position, as the position needs to be modified based on the boss/player pos.
                    projectileData.Add(new Projectile(int.Parse(data[4]), new Vector2(float.Parse(data[0]), float.Parse(data[1])),
                        new Rectangle(0, 0, int.Parse(data[2]), int.Parse(data[3])),
                        local, proj, testRectangle));
                }
                reader.Close();
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
        }


        /// <summary>
        /// This methd loads in attack patterns and stores them in a list of lists
        /// </summary>
        /// <param name="filename"></param>
        private void LoadDataAttacks(string filename)
        {
            try
            {
                //open the streamreader
                StreamReader reader = new StreamReader(filename);
                //read in the first line of useless stuff
                string line = reader.ReadLine();
                while ((line = reader.ReadLine()) != null)
                {
                    string[] data = line.Split(",");
                    List<int> temp = new List<int>();
                    foreach (string attack in data)
                    {
                        temp.Add(int.Parse(attack));
                    }
                    attackPatterns.Add(temp);
                }
                reader.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

    }
}
