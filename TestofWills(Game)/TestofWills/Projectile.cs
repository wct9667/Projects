using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TestofWills
{
    //enum for the projectile type
    public enum ProjectileType 
    { 
    player,
    boss,
    }

    /// <summary>
    /// Struct that will be for all projectiles shot by both the player and the enemy
    /// </summary>
    struct Projectile
    {
        //-------------------FIELDS-------------------
        private int damage;
        private Vector2 velocity;
        private Rectangle position;
        Texture2D image;
        ProjectileType projType;
        // Animation reqs
        private int currentFrame;
        private double fps;
        private double secondsPerFrame;
        private double timeCounter;
        private int numSpritesInSheet;
        private int widthOfSingleSprite;
        private Texture2D testRectangle;
        private Vector2 defaultProjectileSpeed;


        //-----------------PROPERTIES-----------------
        public int Damage
        {
            get { return damage; }
            set { damage = value; }
        }

        public Vector2 Velocity
        {
            get { return velocity; } 
            set { velocity = value; }
        }

        public Rectangle Position
        {
            get { return position; }
            set { position = value; }
        }

        public ProjectileType ProjType
        {
            get { return projType; }
        }

        public Vector2 DefaultProjectileSpeed
        {
            get { return defaultProjectileSpeed; }
        }

        //-----------------CONSTRUCTORS---------------
        public Projectile(int damage, Vector2 velocity, Rectangle position, Texture2D image, ProjectileType projType, Texture2D testRectangle)
        {
            this.damage = damage;
            this.velocity = velocity;
            this.position = position;
            this.image = image;
            this.projType = projType;
            currentFrame = 1;
            fps = 10;
            secondsPerFrame = 1.0f / fps;
            timeCounter = 0;
            numSpritesInSheet = 4;
            widthOfSingleSprite = image.Width / numSpritesInSheet;
            this.testRectangle = testRectangle;
            defaultProjectileSpeed = velocity;
        }
        //------------------METHODS-------------------
        /// <summary>
        /// Update method that will be called in Game1 every half second or so
        /// It will chaneg the position of the projectile by the set amount of Velocity
        /// </summary>
        public void Update(GameTime gameTime)
        {
            position.X += (int)velocity.X;
            position.Y += (int)velocity.Y;
            UpdateAnimation(gameTime);
        }
        /// <summary>
		/// Updates the animation time
		/// </summary>
		/// <param name="gameTime">Game time information</param>
        /// Taken from Mario PE
		private void UpdateAnimation(GameTime gameTime)
        {
            // Add to the time counter (need TOTALSECONDS here)
            timeCounter += gameTime.ElapsedGameTime.TotalSeconds;

            // Has enough time gone by to actually flip frames?
            if (timeCounter >= secondsPerFrame)
            {
                // Update the frame and wrap
                currentFrame++;
                if (currentFrame >= 4) currentFrame = 0;

                // Remove one "frame" worth of time
                timeCounter -= secondsPerFrame;
            }
        }
        //draw, this will draw the projectile
        public void Draw(SpriteBatch sb, Color color)
        {
            //draws the object
            if (this.ProjType == ProjectileType.player)
            {
                sb.Draw(image,
                        position,
                        new Rectangle(widthOfSingleSprite * currentFrame, 0, widthOfSingleSprite, image.Height),
                        Color.White,
                        MathHelper.ToRadians(270),
                        Vector2.Zero,
                        SpriteEffects.None,
                        0.0f);
              /*  if (UserInterface.GodMode)
                {
                    sb.Draw(testRectangle,
                   position,
                   Color.Green * .5f);
                }*/


            }
            else
            {
                sb.Draw(image,
                    position,
                    new Rectangle(widthOfSingleSprite * currentFrame, 0, widthOfSingleSprite, image.Height),
                    color,
                    0,
                    Vector2.Zero,
                    SpriteEffects.None,
                    0.0f);
                /*if (UserInterface.GodMode)
                {
                    sb.Draw(testRectangle,
                   position,
                   Color.Red * .5f);
                }*/
            }
          
            //old draw code
            /*
            sb.Draw(image,
                position,
                new Rectangle(widthOfSingleSprite * currentFrame, 0, widthOfSingleSprite, image.Height),
                Color.White);
            */
        }

    }
}
