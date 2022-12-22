using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TestofWills
{
    /// <summary>
    /// Boss Class
    /// Fields: Random, projectile type 1, 2, 3
    /// Methods: Attacks, AttackPattern
    /// LoadData, Update, Draw
    /// 
    /// </summary>
    class Boss : GameObject
    {
        //----------------FIELDS---------------
        public Random rng;
        DataManager dataManager;
        //other projectiles to be loaded
        bool isHit = false;
       
        // Animation reqs
       // Texture2D bossImage;
        private int currentFrame;
        private double fps;
        private double secondsPerFrame;
        private double timeCounter;
        private int numSpritesInSheet;
        private int widthOfSingleSprite;
        private Texture2D arrow;
        private Texture2D testSquare;

        private ContentManager Content;
        private List<int> attackPattern;
        private int index = -1;
        private Vector2 middlePosition;
        private bool attack0;
        private int intialPattern;

        //---------------PROPERTIES------------
        //will get the intial health to determine states
        public bool IsHit
        {
            get { return isHit; }
            set { isHit = value; }
        }
        public bool Attack0
        {
            get { return attack0; }
        }
      

        //--------------CONSTRUCTORS-----------
        //Boss contructor that doesn't read in Filename
        //This will be used for testing
        public Boss(ContentManager Content, DataManager dataManager, Rectangle position, int health, Vector2 velocity, int height, int width, Random rng, string bossImage) : base( position, health, velocity, height, width)
        {
            this.Content = Content;
            LoadContent(bossImage);
            this.rng = rng;
            this.dataManager = dataManager;
            currentFrame = 1;
            fps = 10;
            secondsPerFrame = 1.0f / fps;
            timeCounter = 0;
            numSpritesInSheet = 3;
            widthOfSingleSprite = asset.Width / numSpritesInSheet;

            //hardcoded to zero for now, will change with mutliple bosses, or can be updated later
            attackPattern = dataManager.AttackPatternData[0];

            
        }

        //Boss contructor that reads in fileName
        //Will be used in the actual game
        public Boss(ContentManager Content, DataManager dataManager, Rectangle position, int height, int width, Random rng, string fileName, int health, string bossImage, int spritesInSheet, int attackNumber) : base(position, height, width, fileName)
        {
            this.Content = Content;
            LoadContent(bossImage);
            this.rng = rng;
            this.dataManager = dataManager;
            currentFrame = 1;
            fps = 10;
            secondsPerFrame = 1.0f / fps;
            timeCounter = 0;
            numSpritesInSheet = spritesInSheet;
            widthOfSingleSprite = asset.Width / numSpritesInSheet;
            this.health = health;
            this.initialHealth = health;
            middlePosition = new Vector2(position.X + position.Width, position.Y + position.Height);
            

            attackPattern = dataManager.AttackPatternData[attackNumber];
            intialPattern = attackNumber;
        }



        //--------------METHODS----------------
        /// <summary>
        /// This will randomly change the boss's position
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            //if the boss is in the middle third
            if (middlePosition.X > screenWidth / 3 && middlePosition.X < screenWidth * (2 / 3))
                velocity.X = rng.Next(-10, 11);

            //boss is off screen
            else if (middlePosition.X < 0)
                velocity.X = rng.Next(10,20);
            else if (middlePosition.X  > screenWidth)
                velocity.X = rng.Next(-21 , -10);

            //boss is in the outer 10ths
            else if (middlePosition.X < screenWidth / 9)
                velocity.X = rng.Next(0, 11);
            else if (middlePosition.X > screenWidth * 8/9)
                velocity.X = rng.Next(-10, 1);

            //if the boss is in the outer thirds
            else if (middlePosition.X < screenWidth / 3)
                velocity.X = rng.Next(-5, 11);
            
            else if (middlePosition.X > screenWidth * (2 / 3))
            {
                velocity.X = rng.Next(-10, 6);
            }
            UpdateAttack();
        }

        /// <summary>
        /// updates the attack pattern
        /// </summary>
        private void UpdateAttack()
        {
            //updates the attack index
            index++;

            //check index length
            if (index > attackPattern.Count - 1)
            {
                index = 0;
            }
        }

        /// <summary>
        /// This checks to make sure the 
        /// </summary>
        public override void CheckPosition()
        {
            position.X += (int)velocity.X;
            position.Y += (int)velocity.Y;

            
            if (position.X + asset.Width < 0)
            {
                position.X = screenWidth - asset.Width ;
            }
            else if (position.X  > screenWidth)
            {
                position.X = 0;
            }
            middlePosition.X = position.X;
        }


        //First attack method for the boss
        //Will load in data and return a array, which will then be used in attack pattern to then push it to GameManager
        private Projectile[] Attack1()
        {
            Projectile[] array;
            
            //load in the projectile
            Projectile bossAttack = dataManager.ProjectileData[1];
            
            //change its position
            bossAttack.Position = new Rectangle(position.X + (position.Width / 2) - (bossAttack.Position.Width / 2), position.Y + position.Height, 
                bossAttack.Position.Width, 
                bossAttack.Position.Height);

            array = new Projectile[] { bossAttack };
            return array;
        }
        /// <summary>
        /// another attack, will spawn 2 other either edge of the boss
        /// </summary>
        /// <returns></returns>
        private Projectile[] Attack2()
        {
            Projectile[] array = new Projectile[2];

            //for now will load position one(basic straight projectile)
            array[0] = dataManager.ProjectileData[1];
            array[1] = dataManager.ProjectileData[1];

            //change the positions of both
            array[0].Position = new Rectangle(position.X, (position.Y + position.Height), array[0].Position.Width, array[0].Position.Height);
            array[1].Position = new Rectangle(position.X + position.Width-array[1].Position.Width, (position.Y + position.Height), array[1].Position.Width, array[1].Position.Height);

            return array;
        }

        private Projectile[] Attack3()
        {
            Projectile[] array = new Projectile[5];

            //load in all 5 projectiles from the list
            array[0] = dataManager.ProjectileData[2];
            array[1] = dataManager.ProjectileData[3];
            array[2] = dataManager.ProjectileData[4];
            array[3] = dataManager.ProjectileData[5];
            array[4] = dataManager.ProjectileData[6];

            //change the positions of all 5
            for (int i = 0; i < array.Length; i++)
            {
                array[i].Position = new Rectangle(position.X + position.Width/2, (position.Y + position.Height), array[i].Position.Width, array[i].Position.Height);
            }
            return array;
        }

        private Projectile[] Attack4()
        {
            Projectile[] array = new Projectile[6];

            //load in all 5 projectiles from the list
            array[0] = dataManager.ProjectileData[1];
            array[1] = dataManager.ProjectileData[1];
            array[2] = dataManager.ProjectileData[1];
            array[3] = dataManager.ProjectileData[1];
            array[4] = dataManager.ProjectileData[1];
            array[5] = dataManager.ProjectileData[1];

            array[0].Position = new Rectangle(screenWidth/5, (position.Y + position.Height), array[0].Position.Width, array[0].Position.Height);
            array[1].Position = new Rectangle(screenWidth * 2/5, (position.Y + position.Height), array[0].Position.Width, array[0].Position.Height);
            array[2].Position = new Rectangle(screenWidth * 3/5, (position.Y + position.Height), array[0].Position.Width, array[0].Position.Height);
            array[3].Position = new Rectangle(screenWidth * 4/5, (position.Y + position.Height), array[0].Position.Width, array[0].Position.Height);
            array[4].Position = new Rectangle(screenWidth - array[0].Position.Width, (position.Y + position.Height), array[0].Position.Width, array[0].Position.Height);
            array[5].Position = new Rectangle(0, position.Y + position.Height, array[0].Position.Width, array[0].Position.Height);

            return array;
        }
        private Projectile[] Attack5()
        {
            Projectile[] array = new Projectile[3];

            array[0] = dataManager.ProjectileData[7];
            array[1] = dataManager.ProjectileData[7];
            array[2] = dataManager.ProjectileData[7];

            array[0].Position = new Rectangle(rng.Next(0,screenWidth - array[0].Position.Width), (position.Y + position.Height), array[0].Position.Width, array[0].Position.Height);
            array[1].Position = new Rectangle(rng.Next(0, screenWidth - array[0].Position.Width), (position.Y + position.Height), array[0].Position.Width, array[0].Position.Height);
            array[2].Position = new Rectangle(rng.Next(0, screenWidth - array[0].Position.Width), (position.Y + position.Height), array[0].Position.Width, array[0].Position.Height);

            return array;
        }

        private Projectile[] Attack6()
        {
            Projectile[] array = new Projectile[12];


            array[0] = dataManager.ProjectileData[2];
            array[1] = dataManager.ProjectileData[3];
            array[2] = dataManager.ProjectileData[4];
            array[3] = dataManager.ProjectileData[5];
            array[4] = dataManager.ProjectileData[2];
            array[5] = dataManager.ProjectileData[3];
            array[6] = dataManager.ProjectileData[4];
            array[7] = dataManager.ProjectileData[5];
            array[8] = dataManager.ProjectileData[2];
            array[9] = dataManager.ProjectileData[3];
            array[10] = dataManager.ProjectileData[4];
            array[11] = dataManager.ProjectileData[5];

            //change the positions of all 5
            for (int i = 0; i < 4; i++)
            {
                array[i].Position = new Rectangle(position.X + position.Width/2, (position.Y + position.Height), array[i].Position.Width, array[i].Position.Height);
            }
            for (int i = 4; i < 8; i++)
            {
                array[i].Position = new Rectangle(position.X + position.Width / 2 - 50, (position.Y + position.Height), array[i].Position.Width, array[i].Position.Height);
            }
            for (int i = 8; i < 12; i++)
            {
                array[i].Position = new Rectangle(position.X + position.Width / 2 + 50, (position.Y + position.Height), array[i].Position.Width, array[i].Position.Height);
            }

            return array;
        }


        /// <summary>
        /// this method returns a random attack from the bosses attack pool
        /// </summary>
        /// <returns></returns>
        public Projectile[] AttackPattern()
        {
            int attackNumber = attackPattern[index];
            

            //switch to randomly determine the attack
            switch (attackNumber)
            {
                case 1:
                    attack0 = false;
                   return Attack1();
                case 2:
                    attack0 = false;
                    return Attack2();
                case 3:
                    attack0 = false;
                    return Attack3();

                ////////////
                //more attacks here
                case 4:
                    attack0= false;
                    return Attack4();
                ///////////
                case 5:
                    attack0 = false;
                    return Attack5();
                case 6:
                    attack0 = false;
                    return Attack6();
                default:
                    attack0 = true;
                    //basic attack
                    return new Projectile[0];
            }
            
        }
        public void UpdateAnimation(GameTime gameTime)
        {
            // Add to the time counter (need TOTALSECONDS here)
            timeCounter += gameTime.ElapsedGameTime.TotalSeconds;

            // Has enough time gone by to actually flip frames?
            if (timeCounter >= secondsPerFrame)
            {
                // Update the frame and wrap
                currentFrame++;
                if (currentFrame >= numSpritesInSheet)
                {
                    currentFrame = 0;
                }
                    

                // Remove one "frame" worth of time
                timeCounter -= secondsPerFrame;
            }
        }
        public override void Draw(SpriteBatch sb)
        {
            if(isHit == false)
            {
                //base.Draw(sb);
                
                sb.Draw(asset,
                position,
                new Rectangle(widthOfSingleSprite * currentFrame, 0, widthOfSingleSprite, asset.Height),
                Color.White);

                //sb.Draw(arrow, new Rectangle(position.X, screenHeight - arrow.Height, arrow.Width, arrow.Height), Color.Red); //taking otu arrow for now
               /* if (UserInterface.GodMode)
                {
                    sb.Draw(testSquare,
                   position,
                   Color.Red * .5f);
                }*/


            }
            else
            {
                sb.Draw(asset,
                position,
                new Rectangle(widthOfSingleSprite * currentFrame, 0, widthOfSingleSprite, asset.Height),
                Color.Red);
                IsHit = false;

               // sb.Draw(arrow, new Rectangle(position.X, screenHeight - arrow.Height, arrow.Width, arrow.Height), Color.Red);
            }
        }

        public void LoadContent(string bossImage)
        {
            asset = Content.Load<Texture2D>(bossImage);
            arrow = Content.Load<Texture2D>("Arrow");
            testSquare = Content.Load<Texture2D>("Grey rectangle");
        }

        /// <summary>
        /// Returns a time based on the attack(time to next attack)
        /// </summary>
        /// <returns></returns>
        public double AttackTime()
        {
            int attackNumber = attackPattern[index];


            //switch to randomly determine the attack
            switch (attackNumber)
            {
                case 1:
                    return 1;
                case 2:
                    return .5;
                case 3:
                    return .25;
                case 4:
                    return .5;
                case 5:
                    return .25;
                case 6:
                    return .5;
                ////////////
                //more attacks here
                ///////////

                default:
                    //basic attack
                    return .25;
            }

        }
        //small attack pattern methods
        public void ChangePattern(int index)
        {
            attackPattern = dataManager.AttackPatternData[index];
        }
        public void ResetAttackPattern()
        {
            attackPattern = dataManager.AttackPatternData[intialPattern];
        }

    }
}
