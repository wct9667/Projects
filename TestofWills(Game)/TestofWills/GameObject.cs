using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace TestofWills
{
    /// <summary>
    /// This class is the parent for Boss, Player, and the background. Basically every game object
    /// Fields: Texture2D for the image, Rectangle for the position, health, as both the boss and player will have health
    /// Methods: abstract Draw, Update and LoadData, checkPosition ensures that the children will not go over the bounds of the window
    /// Center() centers the gameObject in respect to the x axis
    /// 
    /// </summary>
    abstract class GameObject
    {
        //fields
        protected Texture2D asset;
        protected Rectangle position;
        protected int health;
        protected bool isAlive;
        protected Vector2 velocity;
        protected int screenHeight;
        protected int screenWidth;
        protected int initialHealth;

        //properties,
        //collisionManager will need access to the rectangle
        public Rectangle Position
        {
            get { return position; }
        }
        //health will also need to be accessed to change and display it on the board
        public int Health
        {
            get { return health; }
            set 
            { 
                //check if the health is negative
                health -= value; 
                if  (health <= 0)
                {
                    isAlive = false;
                }  
            }
        }
        public int SetHealth
        {
            set { health = value; }
        }

        //this gets the initial health so the health can be reset
        public int InitialHealth
        {
            get { return initialHealth; }
            set { initialHealth = value; }
        }

        //constructor
        public GameObject(Rectangle position, int health, Vector2 velocity, int height, int width)
        {
            this.position = position;
            this.health = health;
            this.velocity = velocity;
            isAlive = true;
            screenHeight = height;
            screenWidth = width;
            initialHealth = health;

        }
        //overload to take loaded data
        public GameObject(Rectangle position,int height, int width, string filename)
        {
            string[] data = LoadData(filename);
            this.position = position;
            //parse the loaded data in
            this.health = int.Parse(data[2]);
            this.velocity.X = int.Parse(data[0]);
            this.velocity.Y = int.Parse(data[1]);
            isAlive = true;
            screenHeight = height;
            screenWidth = width;
            initialHealth = health;
        }

        //methods

        //update
        public abstract void Update(GameTime gameTime);

        //load data
        /// <summary>
        /// This method will load in the data for the player and the boss, the velocity and the health for each.
        /// </summary>
        public virtual string[] LoadData(string filename)
        {
            try
            {
                //open the streamreader
                StreamReader reader = new StreamReader(filename);
                //read in the first line of useless stuff
                reader.ReadLine();
                //read in the rest
                string[] data = reader.ReadLine().Split(",");
                reader.Close();
                return data;
            }
            catch (Exception e) 
            {
                Console.WriteLine(e.Message);
                return new string[0];
            } 
        }

        //draw, this can be overwritten in child classes, likely will to actually animate the objects
        public virtual void Draw(SpriteBatch sb)
        {
            //draws the object
            sb.Draw(asset,
                position,
                Color.White);
        }

        //checkPosition
        public abstract void CheckPosition();

        //center the GameObject, this will be used to ensure the boss and the player start in the middle 
        public void Center()
        {
            position.X = screenWidth / 2 - position.Width / 2;
        }

        //health reset, makes sure to reset without needing to change the property
        public void HealthReset()
        {
            health = initialHealth;
        }
    }
}
