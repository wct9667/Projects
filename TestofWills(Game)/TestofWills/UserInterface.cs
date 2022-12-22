using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.IO;
//Currently working here
namespace TestofWills
{
    
    class UserInterface
    {
        // FIELDS
        public static bool GodMode = false;

        private double timer;
        private int bossHealth;
        private SpriteFont font;
        private SpriteFont arial30B;
        private int height;
        private int width;
         

        //settings fields
        RolloverButton checkbox;
       
        Texture2D clicked;
        Texture2D notClicked;
        Texture2D hoveredCheck;
        private DataManager dataManager;
        ContentManager content;
        Texture2D slider;

        RolloverButton volumeGamePlus;
        RolloverButton volumeMusicPlus;
        RolloverButton volumeGameMinus;
        RolloverButton volumeMusicMinus;
        Texture2D hovered;
        Texture2D notHovered;


        //settings text
        private Texture2D gameVolumeText;
        private Texture2D musicVolumeText;
        private Texture2D godModeText;

        //score
        private double currentHighScore;

        float volumeGame = .2f;
        float volumeMusic = .5f;

        //sounds-could do static somewhere?
        private SoundEffect buttonClickSound;

        // PROPERTIES

        public double Timer
        {
            get { return timer; }
            set { timer = value; }
        }

        // METHODS

        // Update method tracks the elapsed game time and the boss' health
        public void Update(GameTime gameTime, Boss boss)
        {
            timer += gameTime.ElapsedGameTime.TotalSeconds;
            bossHealth = boss.Health;
        }

        public void UpdateGodMode()
        {
            //CHECKBOX LOGIC
            bool clicked = checkbox.isMouseClicked();
            //checkbox
            if (clicked && !GodMode)
            {
                buttonClickSound.Play(volumeGame, 0.0f, 0.0f);
                GodMode = true;
                //update to god mode(loop through datamanager and set boss projectiles to zero damage;
                for (int i = 0; i < dataManager.ProjectileData.Count; i++)
                {
                    if (dataManager.ProjectileData[i].ProjType == ProjectileType.boss)
                    {
                        Projectile c = dataManager.ProjectileData[i];
                        c.Damage = 0;
                        dataManager.ProjectileData[i] = c;
                    }
                }

            }
            else if (clicked && GodMode)
            {
                buttonClickSound.Play(volumeGame, 0.0f, 0.0f);
                GodMode = false;
                //update to god mode(loop through datamanager and set boss projectiles to zero damage;
                for (int i = 0; i < dataManager.ProjectileData.Count; i++)
                {
                    if (dataManager.ProjectileData[i].ProjType == ProjectileType.boss)
                    {
                        Projectile c = dataManager.ProjectileData[i];
                        c.Damage = 1;
                        dataManager.ProjectileData[i] = c;
                    }
                }

            }
        }
            //LOGIC FOR VOLUME SLIDER
            //check all 4 buttons that will be needed for the slider(+ - for music and game sounds)
        //updates the game volume
        public float UpdateGameVolume()
        {
            if (volumeGamePlus.isMouseClicked())
            {
                buttonClickSound.Play(volumeGame, 0.0f, 0.0f);
                volumeGame += .1f;
                if (volumeGame > 1.0f)
                {
                    volumeGame = 1.0f;
                }
            }
            else if (volumeGameMinus.isMouseClicked())
            {
                buttonClickSound.Play(volumeGame, 0.0f, 0.0f);
                volumeGame -= .1f;
                if(volumeGame < 0)
                {
                    volumeGame = 0;
                }
            }
            return volumeGame;
        }
        //updates the music volume
        public float UpdateMusicVolume()
        {
            if (volumeMusicPlus.isMouseClicked())
            {
                buttonClickSound.Play(volumeGame, 0.0f, 0.0f);
                volumeMusic += .1f;
                if (volumeMusic > 1.0f)
                {
                    volumeMusic = 1.0f;
                }
            }
            else if (volumeMusicMinus.isMouseClicked())
            {
                buttonClickSound.Play(volumeGame, 0.0f, 0.0f);
                volumeMusic -= .1f;
                if(volumeMusic < 0)
                {
                    volumeMusic = 0;
                }
            }
            return volumeMusic;
        }
           
        public void DrawSliders(SpriteBatch sb)
        {
           sb.Draw(slider, new Rectangle(volumeMusicMinus.X + volumeMusicMinus.Width, volumeGamePlus.Y, 200 , volumeGamePlus.Height), Color.Black);
           sb.Draw(slider, new Rectangle(volumeMusicMinus.X + volumeMusicMinus.Width, volumeGamePlus.Y, (int)(volumeGame * 200), volumeGamePlus.Height), Color.Green);

           sb.Draw(slider, new Rectangle(volumeMusicMinus.X + volumeMusicMinus.Width, volumeMusicPlus.Y, 200, volumeMusicPlus.Height), Color.Black);
           sb.Draw(slider, new Rectangle(volumeMusicMinus.X + volumeMusicMinus.Width, volumeMusicPlus.Y, (int)(volumeMusic * 200), volumeMusicPlus.Height), Color.Green);
        }


        // Draws elapsed time and boss' health to the screen

        // FEEL FREE TO MOVE THESE AROUND IF YOU DON'T LIKE THE PLACEMENT


        public void Draw(SpriteBatch sb)
        {
            
            sb.DrawString(font,
                    "Time:" + String.Format("{0:0}", timer),
                    new Vector2(10),
                    Color.White);
            /*sb.DrawString(font,
                    "Boss health: " + bossHealth,
                    new Vector2(10, 30),
                    Color.White);*/
        }

        //Started the settings screen
        /// <summary>
        /// /////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        /// <param name="sb"></param>
        public void DrawSettings(SpriteBatch sb)
        {
            
            if (!GodMode)
            {
                checkbox.Draw(sb);
            }
            else
            {
                checkbox.Draw(sb);
                sb.Draw(clicked, checkbox.Rectangle, Color.White);
            }

            volumeGamePlus.Draw(sb);
            volumeMusicPlus.Draw(sb);
            volumeGameMinus.DrawReversed(sb);
            volumeMusicMinus.DrawReversed(sb);

            //debug drawing
          //  sb.DrawString(font, $"{volumeGame} game", new Vector2(width - 100, height -100), Color.White);
         //   sb.DrawString(font, $"{volumeMusic} music", new Vector2(width - 200, height - 200), Color.White);

            //text next to the buttons
            sb.Draw(gameVolumeText, new Rectangle(volumeGameMinus.Rectangle.X - gameVolumeText.Width,
                volumeGamePlus.Rectangle.Y, 175, volumeGamePlus.Rectangle.Height), Color.White);
            
            sb.Draw(musicVolumeText, new Rectangle(volumeMusicMinus.Rectangle.X - musicVolumeText.Width,
                volumeMusicPlus.Rectangle.Y, 175, volumeMusicPlus.Rectangle.Height), Color.White);
            
            sb.Draw(godModeText, new Rectangle(volumeMusicMinus.Rectangle.X - musicVolumeText.Width + 8,
               checkbox.Rectangle.Y, 210, checkbox.Rectangle.Height), Color.White);

            DrawSliders(sb);
        }










        // CONSTRUCTORS

        // A new UI will start with timer at zero
        // The font is passed in as a parameter
        public UserInterface(DataManager dataManager, ContentManager content, int width, int height)
        {
            this.width = width;
            this.height = height;
            timer = 0;
            this.dataManager = dataManager;
            this.content = content;
            font = content.Load<SpriteFont>("Arial12");
            arial30B = content.Load<SpriteFont>("Arial30");
            //godmode
            clicked = content.Load<Texture2D>("checkbox");
            notClicked = content.Load<Texture2D>("emptybox");
            hoveredCheck = content.Load<Texture2D>("hoveredbox");
            //volume
            notHovered = content.Load<Texture2D>("positive");
            hovered = content.Load<Texture2D>("positive");

            slider = content.Load<Texture2D>("healthbar2");

            gameVolumeText = content.Load<Texture2D>("GameText");
            musicVolumeText = content.Load<Texture2D>("MusicText");
            godModeText = content.Load<Texture2D>("GodModeText");


            checkbox = new RolloverButton(notClicked, hoveredCheck, new Rectangle(width/5, height/2, 75, 75));
            volumeGameMinus = new RolloverButton(notHovered, hovered, new Rectangle(checkbox.X, checkbox.Y + checkbox.Height + height/15, 75, 75));
            volumeMusicMinus = new RolloverButton(notHovered, hovered, new Rectangle(checkbox.X, volumeGameMinus.Height + volumeGameMinus.Y + height/15, 75, 75));
            volumeGamePlus = new RolloverButton(notHovered, hovered, new Rectangle(volumeGameMinus.X + volumeGameMinus.Width + 200, checkbox.Y + checkbox.Height + height / 15, 75, 75));
            volumeMusicPlus = new RolloverButton(notHovered, hovered, new Rectangle(volumeGameMinus.X + volumeGameMinus.Width + 200, volumeGamePlus.Y + volumeGamePlus.Height + height / 15, 75, 75));

            buttonClickSound = content.Load<SoundEffect>("button Click");
            timeLoad();
        }


        //method for timer resetting, returns a double of the time
        public double TimerReset()
        {
            double time = timer;
            timer = 0;
            return time;
        }


        //////////////////////////////////////////////////////////////score
        ///  //some sort of loading for the time, will load the high score and compare it to the current score, if the score it lower(time) it will resave it
        private void timeLoad()
        {
            try
            {
                StreamReader reader = new StreamReader("../../../HighScore");
                currentHighScore = double.Parse(reader.ReadLine());
                reader.Close();
            }
            catch { }
        }

        public void CompareTime(double time)
        {
            if (!GodMode)
            {
                if (time < currentHighScore && time != 0)
                {
                    currentHighScore = time;
                    TimeSave(time);
                }
            }
        }

        private void TimeSave(double time)
        {
            try
            {
                    StreamWriter writer = new StreamWriter("../../../HighScore");
                    writer.Write(time);
                    writer.Close();
            }
            catch { };
        }


        //Draw the score
        public void DrawScore(SpriteBatch sb, double time)
        {

            sb.DrawString(arial30B, $"{String.Format("Record:")}", new Vector2(width / 3 + 2 * width / 8, height - height * 3 / 4), Color.White);
            sb.DrawString(arial30B, $"{String.Format("{0:0.0}", currentHighScore)}s", new Vector2(width/3 + 2*width/8 + arial30B.MeasureString("Record:").Length(), height - height* 3/4 ), Color.White);
            
            sb.DrawString(arial30B, $"{String.Format("Time:")}", new Vector2(width / 3 -2* width / 8, height - height * 3 / 4), Color.White);
            sb.DrawString(arial30B, $"{String.Format("{0:0.0}", time )}s", new Vector2(width / 3 - 2* width / 8 + arial30B.MeasureString("Time:").Length(), height - height * 3 / 4), Color.White);
        }
    }
}
