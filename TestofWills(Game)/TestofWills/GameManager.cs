 using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

// Enum for finite state machines
public enum GameState 
{
menu,
quit,
settings,
gamePlay,
pause,
boosted, //second half of gameplay, will trigger when bosses health gets low
gameEnd,
gameWin,
gameWinFinal,
getPowerUp,
credits,
}


namespace TestofWills
{
    /// <summary>
    /// This class manages the positions and the collisions of the projectiles, and the boss and the player.
    /// It will delete projectiles as needed
    /// Fields
    /// List of projectiles, boss, player, timePassed, width, height (screen), dataManager
    /// Methods:
    ///UpdateProjectiles(): this updates all of the proectiles in the list by calling their update methods
    ///this will check for collision using the checkcollision method. It will delete any off-screen projectiles
    ///BossAttack(): This will take a attack from the boss's attack pattern and add the related projectiles to the list
    ///PlayerShoot(): Does the same thing, except the player only has on attack possible
    ///CheckCollision(): This will check to see if the projectile from the player or boss have hit the player or boss, using the enumeration
    ///to differentiate projectile types, it will remove the projectile if it hits and will apply damage
    ///Update(): this method will contain the finite state machine for the game...work in progress!!
    /// </summary>
    class GameManager
    {
        //---------------FIELDS-------------
        private List<Projectile> projectiles = new List<Projectile>();
        private Boss[] bossList;
        private Player player;
        private double timePassed;
        private double playerTimePassed; //need to limit shooting of the player
        private int width;
        private int height;
        private DataManager dataManager;
        private UserInterface userInterface;
        private double fireRate = 1;
        private SoundEffect playerShoot1;
        private SoundEffect playerShoot2;
        private SoundEffect playerShoot3;
        private SoundEffect bossShoot1;
        private SoundEffect bossShoot2;
        private SoundEffect bossShoot3;
        private SoundEffect bossHit;
        private int currentLevel;
        Random rng;

        private float volumeGame;

        //-------------PROPERTIES-----------
        public List<Projectile> Projectiles
        {
            get { return projectiles; }
        }

        public double FireRate
        {
            get { return fireRate; }
            set { fireRate = value; }
        }

        public float VolumeGame
        {
            set { volumeGame = value; }
        }

        public int CurrentLevel
        {
            get { return currentLevel; }
            set { currentLevel = value; }
        }

        public Boss[] BossList
        {
            get { return bossList; }
        }

        //-------------CONSTRUCTORS---------//
        public GameManager(Boss[] boss, Player player, DataManager dataManager, int width, int height, UserInterface userInterface, SoundEffect playerShoot1, SoundEffect playerShoot2, SoundEffect playerShoot3, SoundEffect bossShoot1, SoundEffect bossShoot2, SoundEffect bossShoot3, SoundEffect bossHit, float volumeGame)
        {
            this.bossList = boss;
            this.player = player;
            this.dataManager = dataManager;
            this.width = width;
            this.height = height;
            this.userInterface = userInterface;
            this.playerShoot1 = playerShoot1;
            this.playerShoot2 = playerShoot2;
            this.playerShoot3 = playerShoot3;
            this.bossShoot1 = bossShoot1;
            this.bossShoot2 = bossShoot2;
            this.bossShoot3 = bossShoot3;
            this.volumeGame = volumeGame;
            this.bossHit = bossHit;
            this.currentLevel = 0;
        }


        //---------------METHODS------------

        //Update will be called in Game1
        //Will contain all of our update methods
        public void Update(GameTime gameTime)
        {

            //update the player
            player.Update(gameTime);

            //code for final boss
            if (currentLevel == 3)
            {
                int velocityX = 0;
                int velocityY;
                if (bossList[3].Position.X + bossList[3].Position.Width / 2 > player.Position.X + player.Position.Width / 2)
                {
                    if (Game1.gameState == GameState.boosted)
                    {
                        velocityX = 2;
                    }
                    else
                    {
                        velocityX = 1;
                    }

                }
                else if (bossList[3].Position.X + bossList[3].Position.Width / 2 < player.Position.X + player.Position.Width/2)
                {
                    if (Game1.gameState == GameState.boosted)
                    {
                        velocityX = -2;
                    }
                    else
                    {
                        velocityX = -1;
                    }
                }
                if (Game1.gameState == GameState.boosted)
                {
                    velocityY = -4;
                }
                else
                {
                    velocityY = -3;
                }
                player.MovePlayer(velocityX, velocityY);
            }

            bossList[currentLevel].UpdateAnimation(gameTime);
            //total up the amount of time passed
            timePassed += gameTime.ElapsedGameTime.TotalSeconds;
            playerTimePassed += gameTime.ElapsedGameTime.TotalSeconds;
            //If the time passed is greater than 1, update
            if (playerTimePassed > .25)
            {
                PlayerShoot();
                playerTimePassed = 0;
            }

            if (timePassed > fireRate)
            {
                bossList[currentLevel].Update(gameTime);

                BossAttack();

                if (!bossList[currentLevel].Attack0)
                {
                    rng = new Random();
                    int rand = rng.Next(1, 4);
                    if (rand == 1)
                    {
                        bossShoot1.Play(volumeGame * 0.8f, 0.0f, 0.0f);
                    }
                    else if (rand == 2)
                    {

                        bossShoot2.Play(volumeGame * 0.8f, 0.0f, 0.0f);
                    }
                    else
                    {
                        bossShoot3.Play(volumeGame * 0.8f, 0.0f, 0.0f);
                    }
                }
                timePassed = 0;
            }

            //Check these every time they update only checks first two as second two are stationary
            if (currentLevel != 3 && currentLevel != 2)
                bossList[currentLevel].CheckPosition();
            else if(Game1.gameState == GameState.boosted )
            //&& currentLevel != 3
            {
                bossList[currentLevel].CheckPosition();
            }
            //update the position of all the projectiles
            UpdateProjectiles(gameTime);
            //update ui
            userInterface.Update(gameTime, bossList[currentLevel]);
        }



        public void UpdateProjectiles(GameTime gameTime)
        {
            CheckCollision();

            //Update every projectile in the list of projectiles
            for (int i = 0; i < projectiles.Count; i++)
            {
                //Create a new projectile that the old projectile will be set to
                Projectile temp = projectiles[i];
                //Update the new projectile
                temp.Update(gameTime);
                //Set the old projectile
                projectiles[i] = temp;
                //Delete the projectiles once they get off screen
                if(projectiles[i].Position.X < 0 || projectiles[i].Position.X > width || projectiles[i].Position.Y < 0 || projectiles[i].Position.Y > height)
                {
                    projectiles.RemoveAt(i);
                }
            }
        }
        


        //Creates the boss projectiles from the boss and add them to the list of projectiles
        public void BossAttack()
        {
            //get the attack
            Projectile[] array = bossList[currentLevel].AttackPattern();
            fireRate = bossList[currentLevel].AttackTime();
            
            //loop throuth the array and add
            foreach(Projectile p in array)
            {
                projectiles.Add(p);
            }
        }



        /// <summary>
        /// This method manages when the player shoots, and creates a projectile
        /// </summary>
        /// <param name="projectileImage"></param>
        public void PlayerShoot()
        {
            //check to see if the player has shot
            if (player.HasShot)
            {
                //player shoot sound
                Random rng = new Random();
                int rand = rng.Next(1, 8);
                if (rand == 1)
                {
                    playerShoot3.Play(volumeGame * 0.5f, 0.0f, 0.0f);
                }
                else if (rand == 2)
                {

                    playerShoot2.Play(volumeGame * 0.5f, 0.0f, 0.0f);
                }
                else
                {
                    playerShoot1.Play(volumeGame * 0.5f, 0.0f, 0.0f);
                }
                //make the projectiles and update positions
                Projectile playerAttack1 = dataManager.ProjectileData[0];
                Projectile playerAttack2 = dataManager.ProjectileData[0];
                
                playerAttack1.Position = new Rectangle((player.Position.X),
                    (player.Position.Y) - (player.Position.Height / 2),
                    playerAttack1.Position.Width, playerAttack1.Position.Height);

                playerAttack2.Position = new Rectangle((player.Position.X + player.Position.Width - playerAttack2.Position.Width),
                    (player.Position.Y) - (player.Position.Height / 2),
                    playerAttack2.Position.Width, playerAttack2.Position.Height);

                projectiles.Add(playerAttack1);
                projectiles.Add(playerAttack2);
            }
        }


        //Method that check the collision
        //on collision, will subtract the damage from the health by setting the value equal to the damage(-= in the property)
        //It will also delete the projectile, so it won't keep colliding.
        public void CheckCollision()
        {
            for (int i = 0; i < projectiles.Count; i++)
            {
                if (projectiles[i].Position.Intersects(player.InternalHitBox) && projectiles[i].ProjType == ProjectileType.boss)
                {
                    player.Health = projectiles[i].Damage;
                    projectiles.RemoveAt(i);
                }
                else if (projectiles[i].Position.Intersects(bossList[currentLevel].Position) && projectiles[i].ProjType == ProjectileType.player)
                {
                    bossHit.Play(volumeGame * 0.7f, 0.0f, 0.0f);
                    bossList[currentLevel].Health = projectiles[i].Damage;
                    projectiles.RemoveAt(i);
                    bossList[currentLevel].IsHit = true;
                }
            } 
        }


        public void Reset()
        {
            player.HealthReset();
            bossList[currentLevel].HealthReset();
            projectiles.Clear();
            bossList[currentLevel].Center();
            player.Center();
            player.ResetInternal();
            fireRate = 1;         
        }
    }
}
