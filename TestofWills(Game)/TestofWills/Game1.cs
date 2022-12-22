using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
//Force Of Wills


namespace TestofWills
{
    
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private int screenHeight;
        private int screenWidth;
        private Random rng = new Random();        //random
        Player player;
        Boss boss;
        GameManager manager;
        DataManager dataManager;
        UserInterface userInterface;
        ButtonManager buttonManager;
        private HealthBar healthBar;
        private Texture2D healthTexture;

        private Texture2D TitleScreen;
        private Texture2D GameOver;
        private Texture2D VictoryScreen;
        private Texture2D OptionsScreen;
        private double timeScore;

        private Texture2D powerUpText;
        private Texture2D creditsScreen;

       



        private Texture2D GreyRectangle;

        //Array that will contain all of the bosses
        Boss[] bossList = new Boss[4];

      
        
       
        
       //list for backgrounds
        private List<Background> Backgrounds;
        private int backgroundIndex = 0;
       
        //sound effects
        private SoundEffect buttonClickSound;
        private SoundEffect bossHitSound;
        private SoundEffect bossDeathSound;
        private SoundEffect playerDeathSound;
        private SoundEffect playerShootSound1;
        private SoundEffect playerShootSound2;
        private SoundEffect playerShootSound3;
        private SoundEffect bossShootSound1;
        private SoundEffect bossShootSound2;
        private SoundEffect bossShootSound3;
        private SoundEffect menuMusic;
        private SoundEffectInstance menuMusicInstance;
        private SoundEffect gameMusic;
        private SoundEffectInstance gameMusicInstance;
        private SoundEffect boostedMusic;
        private SoundEffectInstance boostedMusicInstance;
        private SoundEffect gameOverMusic;
        private SoundEffectInstance gameOverMusicInstance;
        private SoundEffect gameWinMusic; 
        private SoundEffectInstance gameWinMusicInstance;

        //volume fields
        private float volumeMusic = .8f;
        private float volumeGame = .8f;

        public static GameState gameState = GameState.menu;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferWidth = 1000;  
           _graphics.PreferredBackBufferHeight = 1000;  

            _graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            screenHeight = _graphics.GraphicsDevice.Viewport.Height;
            screenWidth = _graphics.GraphicsDevice.Viewport.Width;



            //loading sound effects
            buttonClickSound = Content.Load<SoundEffect>("Button Click");
            bossHitSound = Content.Load<SoundEffect>("Hit_Hurt14");
            playerDeathSound = Content.Load<SoundEffect>("Player Death");
            bossDeathSound = Content.Load<SoundEffect>("Boss Death");
            playerShootSound1 = Content.Load<SoundEffect>("Player Shoot1");
            playerShootSound2 = Content.Load<SoundEffect>("Player Shoot2");
            playerShootSound3 = Content.Load<SoundEffect>("Player Shoot3");
            bossShootSound1 = Content.Load<SoundEffect>("Boss Shoot1");
            bossShootSound2 = Content.Load<SoundEffect>("Boss Shoot2");
            bossShootSound3 = Content.Load<SoundEffect>("Boss Shoot3");
            menuMusic = Content.Load<SoundEffect>("Menu Music");
            menuMusicInstance = menuMusic.CreateInstance();
            gameMusic = Content.Load<SoundEffect>("Gameplay Music");
            gameMusicInstance = gameMusic.CreateInstance();
            boostedMusic = Content.Load<SoundEffect>("Boosted Music");
            boostedMusicInstance = boostedMusic.CreateInstance();
            gameOverMusic = Content.Load<SoundEffect>("Game Over");
            gameOverMusicInstance = gameOverMusic.CreateInstance();
            gameWinMusic = Content.Load<SoundEffect>("Game Win");
            gameWinMusicInstance = gameWinMusic.CreateInstance();
            creditsScreen = Content.Load<Texture2D>("creditsScreen");


            GreyRectangle = Content.Load<Texture2D>("Grey rectangle");








            /////////////////////////////////////////////////////////
            healthTexture = Content.Load<Texture2D>("healthbar");
            dataManager = new DataManager(Content, "../../../projdata.txt");
            buttonManager = new ButtonManager(Content, screenWidth, screenHeight);

            //create the player and the boss 
            player = new Player(Content, new Rectangle(0, screenHeight, 3 * 26, 3 * 24), screenHeight, screenWidth, "../../../Player.txt");
            bossList[0] = new Boss(Content, dataManager, new Rectangle(0, buttonManager.Pause.Height, 120, 120), screenHeight, screenWidth, rng, "../../../Boss.txt", 500, "boss animation", 3, 0);
            bossList[1] = new Boss(Content, dataManager, new Rectangle(0, buttonManager.Pause.Height, 2*234/3 , 2*237/3), screenHeight, screenWidth, rng, "../../../Boss.txt", 750, "boss_2_spritesheet", 48, 1);
            bossList[2] = new Boss(Content, dataManager, new Rectangle(0, buttonManager.Pause.Height, 200, 200), screenHeight, screenWidth, rng, "../../../Boss.txt", 1250, "RedStar", 50, 4);
            bossList[3] = new Boss(Content, dataManager, new Rectangle(0, buttonManager.Pause.Height, 2*(12420/60), 2 *79), screenHeight, screenWidth, rng, "../../../Boss.txt", 2500, "black_hole_horizontal", 60, 6);
            userInterface = new UserInterface(dataManager, Content, screenWidth, screenHeight);
            manager = new GameManager(bossList, player, dataManager, screenWidth, screenHeight, userInterface, playerShootSound1, playerShootSound2, playerShootSound3, bossShootSound1, bossShootSound2, bossShootSound3, bossHitSound, volumeGame);



            // Screen states for menus

            TitleScreen = Content.Load<Texture2D>("TitleScreen");
            GameOver = Content.Load<Texture2D>("GameOver");
            VictoryScreen = Content.Load<Texture2D>("VictoryScreen");
            OptionsScreen = Content.Load<Texture2D>("OptionsScreen2");

            //creates list for backgrounds and rotates them
            Backgrounds = new List<Background>();
            Backgrounds.Add(new Background(new Vector2(50, 50), 1.0f, Content, "SpaceBackground"));
            Backgrounds.Add(new Background(new Vector2(50, 50), 1.0f, Content, "background2"));
            Backgrounds.Add(new Background(new Vector2(50, 50), 1.0f, Content, "Background3"));
            Backgrounds.Add(new Background(new Vector2(100, 100), 1.0f, Content, "SpaceBackground4"));


            boss = manager.BossList[manager.CurrentLevel];

            //healthbar
            healthBar = new HealthBar(healthTexture, new Rectangle(0, boss.Position.Y - 30, boss.Health, 20), boss);

            //Power Up Text
            powerUpText = Content.Load<Texture2D>("powerUpText");

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            KeyboardState kbState;
            switch (gameState)
            {

                //------------------------------------------------------Menu
                case GameState.menu:
                    kbState = Keyboard.GetState();
                    menuMusicInstance.IsLooped = true;
                    menuMusicInstance.Volume = volumeMusic;
                    menuMusicInstance.Play();

                    //change the state based on the click
                    if (buttonManager.Setting.isMouseClicked())
                    {
                        //stop menu music
                        //menuMusicInstance.Stop();
                        //button click
                        buttonClickSound.Play(volumeGame, 0.0f, 0.0f);
                        gameState = GameState.settings;
                    }
                    else if (buttonManager.Start.isMouseClicked() || kbState.IsKeyDown(Keys.S))
                    {
                        //stop menu music
                        menuMusicInstance.Stop();
                        //button click
                        buttonClickSound.Play(volumeGame, 0.0f, 0.0f);
                        gameState = GameState.gamePlay;
                    }
                    else if (buttonManager.Quit.isMouseClicked())
                    {
                        //stop menu music
                        menuMusicInstance.Stop();
                        //button click
                        buttonClickSound.Play(volumeGame, 0.0f, 0.0f);
                        gameState = GameState.quit;
                    }
                    else if(buttonManager.Credits.isMouseClicked())
                    {
                        //button click
                        buttonClickSound.Play(volumeGame, 0.0f, 0.0f);
                        gameState = GameState.credits;
                    }
                    break;

                //------------------------------------------------------Quit
                case GameState.quit:
                    //exit the game
                    Exit();
                    break;

                //------------------------------------------------------Setting
                case GameState.settings:
                    //update the settings
                    userInterface.UpdateGodMode();
                    volumeGame = userInterface.UpdateGameVolume();
                    manager.VolumeGame = volumeGame;
                    volumeMusic = userInterface.UpdateMusicVolume();
                    menuMusicInstance.Volume = volumeMusic;
                    menuMusicInstance.Play();

                    kbState = Keyboard.GetState();
                    if (buttonManager.MenuSet.isMouseClicked() || kbState.IsKeyDown(Keys.M))
                    {
                        //button click
                        buttonClickSound.Play(volumeGame, 0.0f, 0.0f);
                        gameState = GameState.menu;
                    }
                    break;

                //----------------------------------------------------Credits
                case GameState.credits:
                    kbState = Keyboard.GetState();
                    if (buttonManager.MenuSet.isMouseClicked())
                    {
                        //button click
                        buttonClickSound.Play(volumeGame, 0.0f, 0.0f);
                        gameState = GameState.menu;
                    }
                    break;
                //------------------------------------------------------Gameplay
                case GameState.gamePlay:


                    //play different shit for different bosses
                    if (manager.CurrentLevel == 3)
                    {
                        boostedMusicInstance.IsLooped = true;
                        boostedMusicInstance.Volume = volumeMusic;
                        boostedMusicInstance.Play();
                    }
                    else
                    {
                        //play gameplay song
                        gameMusicInstance.IsLooped = true;
                        gameMusicInstance.Volume = volumeMusic;
                        gameMusicInstance.Play();
                    }
                    //moves the wallpaper
                    kbState = Keyboard.GetState();
                    Vector2 direction = Vector2.Zero;
                    direction = new Vector2(0, -1);
                    if (kbState.IsKeyDown(Keys.A) || kbState.IsKeyDown(Keys.Left)) /////This will need to be changed when player states are added.
                    {
                        direction = new Vector2(-1, -1);
                    }
                    if (kbState.IsKeyDown(Keys.D) || kbState.IsKeyDown(Keys.Right))
                    {
                        direction = new Vector2(1, -1);
                    }
                    if ((kbState.IsKeyDown(Keys.A) && kbState.IsKeyDown(Keys.D)) || (kbState.IsKeyDown(Keys.Left) && kbState.IsKeyDown(Keys.Right)) || (kbState.IsKeyDown(Keys.A) && kbState.IsKeyDown(Keys.Right)) || (kbState.IsKeyDown(Keys.D) && kbState.IsKeyDown(Keys.Left)))
                    {
                        direction = new Vector2(0, -1);
                    }
                    //Update backgrounds
                    Backgrounds[backgroundIndex].Update(gameTime, direction, GraphicsDevice.Viewport);


                    //update everything else aka the userint, gameManager
                    manager.Update(gameTime);
                    healthBar.Update();

                    //break the state based on health
                    if (player.Health <= 0)
                    {
                        //userInterface.TimerReset();
                        //stop gameplay
                        if(manager.CurrentLevel == 3)
                        {
                            boostedMusicInstance.Stop();
                        }
                        else
                        {
                            gameMusicInstance.Stop();
                        }
                        
                        playerDeathSound.Play(volumeGame, 0.0f, 0.0f);
                        //player death sound
                        gameState = GameState.gameEnd;
                    }
                    if (boss.Health <= boss.InitialHealth * 7 / 24)
                    {
                        if (manager.CurrentLevel != 3)
                        {
                            //stop normal song and start boosted
                            gameMusicInstance.Stop();
                            boostedMusicInstance.IsLooped = true;
                            boostedMusicInstance.Volume = volumeMusic;
                            boostedMusicInstance.Play();
                        }

                        gameState = GameState.boosted;
                        manager.FireRate = .5;

                        //change attack pattern in boosted
                        if (manager.CurrentLevel == 0)
                        {
                            boss.ChangePattern(3);
                        }
                        else if (manager.CurrentLevel == 1)
                        {
                            boss.ChangePattern(2);
                        }
                        else if (manager.CurrentLevel == 2)
                        {
                            boss.ChangePattern(5);
                        }
                        else if (manager.CurrentLevel == 3)
                        {
                            boss.ChangePattern(7);
                        }

                    }
                    if (buttonManager.Pause.isMouseClicked() || kbState.IsKeyDown(Keys.P))
                    {
                        //half song volume
                        if(manager.CurrentLevel == 3)
                        {
                            boostedMusicInstance.Volume = 0.25f * volumeMusic;
                        }
                        else
                        {
                            gameMusicInstance.Volume = 0.25f * volumeMusic;
                        }
                        //button click
                        buttonClickSound.Play(volumeGame, 0.0f, 0.0f);
                        gameState = GameState.pause;
                    }

                    break;

                //----------------------------------------------Pause
                case GameState.pause:

                    kbState = Keyboard.GetState();

                    if (buttonManager.Play.isMouseClicked() || (kbState.IsKeyDown(Keys.Space)))
                    {
                        //restore song volume
                        if(manager.CurrentLevel == 3)
                        {
                            boostedMusicInstance.Volume = volumeMusic;
                        }
                        else
                        {
                            gameMusicInstance.Volume = volumeMusic;
                        }
                        //button click
                        buttonClickSound.Play(volumeGame, 0.0f, 0.0f);
                        gameState = GameState.gamePlay;
                    }
                    if (buttonManager.Quit.isMouseClicked())
                    {

                        //end music
                        if (manager.CurrentLevel == 3)
                        {
                            boostedMusicInstance.Stop();
                        }
                        else
                        {
                            gameMusicInstance.Stop();
                        }
                        //button click
                        buttonClickSound.Play(volumeGame, 0.0f, 0.0f);
                        gameState = GameState.quit;
                    }
                    if (buttonManager.Menu.isMouseClicked() || kbState.IsKeyDown(Keys.M))
                    {
                        

                        //end music
                        if (manager.CurrentLevel == 3)
                        {
                            boostedMusicInstance.Stop();
                        }
                        else
                        {
                            gameMusicInstance.Stop();
                        }
                        TotalReset();
                        //button click
                        buttonClickSound.Play(volumeGame, 0.0f, 0.0f);
                        gameState = GameState.menu;
                    }
                    break;

                //----------------------------------------Boosted
                case GameState.boosted:

                    //moves the wallpaper
                    kbState = Keyboard.GetState();
                    direction = Vector2.Zero;
                    direction = new Vector2(0, -4);
                    if (kbState.IsKeyDown(Keys.A) || kbState.IsKeyDown(Keys.Left))
                    {
                        direction = new Vector2(-4, -4);
                    }
                    if (kbState.IsKeyDown(Keys.D) || kbState.IsKeyDown(Keys.Right))
                    {
                        direction = new Vector2(4, -4);
                    }
                    if ((kbState.IsKeyDown(Keys.A) && kbState.IsKeyDown(Keys.D)) || (kbState.IsKeyDown(Keys.Left) && kbState.IsKeyDown(Keys.Right)) || (kbState.IsKeyDown(Keys.A) && kbState.IsKeyDown(Keys.Right)) || (kbState.IsKeyDown(Keys.D) && kbState.IsKeyDown(Keys.Left)))
                    {
                        direction = new Vector2(0, -4);
                    }
                    //Update backgrounds
                    Backgrounds[backgroundIndex].Update(gameTime, direction, GraphicsDevice.Viewport);

                    //update everything else aka the userint, gameManager
                    manager.Update(gameTime);
                    healthBar.Update();

                    //break the state based on health
                    if (player.Health <= 0)
                    {
                        //userInterface.TimerReset();
                        //stop boosted song
                        boostedMusicInstance.Stop();
                        //player death sound
                        playerDeathSound.Play(volumeGame, 0.0f, 0.0f);
                        gameState = GameState.gameEnd;

                    }
                    if (boss.Health <= 0)
                    {
                        //stop boosted song
                        boostedMusicInstance.Stop();
                        //boss death sound
                        bossDeathSound.Play(volumeGame, 0.0f, 0.0f);

                        //perhaps we do a boolean to see what is drawn if you win
                        if (manager.CurrentLevel == bossList.Length - 1)
                        {
                            gameState = GameState.gameWinFinal;
                            timeScore = userInterface.TimerReset();
                            userInterface.CompareTime(timeScore);
                        }
                        else
                        {
                            gameState = GameState.getPowerUp;
                        }
                    }

                    break;

                //---------------------------------------------Power Up Selection-------------------------------
                case GameState.getPowerUp:
                    gameWinMusicInstance.IsLooped = true;
                    gameWinMusicInstance.Volume = volumeMusic;
                    gameWinMusicInstance.Play();
                    kbState = Keyboard.GetState();
                    if(buttonManager.DamageUp.isMouseClicked())
                    {
                        Projectile temp = dataManager.ProjectileData[0];
                        temp.Damage += 1;
                        dataManager.ProjectileData[0] = temp;
                        //button click
                        buttonClickSound.Play(volumeGame, 0.0f, 0.0f);
                        gameState = GameState.gameWin;
                    }
                    if (buttonManager.SpeedUP.isMouseClicked())
                    {
                        Projectile temp = dataManager.ProjectileData[0];
                        temp.Velocity = new Vector2(temp.Velocity.X, temp.Velocity.Y - 7);
                        dataManager.ProjectileData[0] = temp;
                        //button click
                        buttonClickSound.Play(volumeGame, 0.0f, 0.0f);
                        gameState = GameState.gameWin;
                    }
                    if (buttonManager.HealthUp.isMouseClicked())
                    {
                        player.InitialHealth += 1;
                        player.Health = player.InitialHealth;
                        //button click
                        buttonClickSound.Play(volumeGame, 0.0f, 0.0f);
                        gameState = GameState.gameWin;
                    }

                    break;
                //states for win or lose
                case GameState.gameWin:

                    //win song
                   //gameWinMusicInstance.IsLooped = true;
                   //gameWinMusicInstance.Volume = volumeMusic;
                   //gameWinMusicInstance.Play();

                    kbState = Keyboard.GetState();
                    if (buttonManager.Next.isMouseClicked()|| kbState.IsKeyDown(Keys.N) || (kbState.IsKeyDown(Keys.Space)))
                    {
                        //end win song
                        gameWinMusicInstance.Stop();
                        //button click
                        buttonClickSound.Play(volumeGame, 0.0f, 0.0f);
                        gameState = GameState.gamePlay;

                        backgroundIndex++;
                        manager.CurrentLevel++;
                        resetGame();

                    }
                    if (buttonManager.Quit.isMouseClicked())
                    {
                        //end win song
                        gameWinMusicInstance.Stop();
                        //button click
                        buttonClickSound.Play(volumeGame, 0.0f, 0.0f);
                        gameState = GameState.quit;
                    }
                    if (buttonManager.Menu.isMouseClicked() || kbState.IsKeyDown(Keys.M))
                    {
                        TotalReset();
                        //end win song
                        gameWinMusicInstance.Stop();
                        //button click
                        buttonClickSound.Play(volumeGame, 0.0f, 0.0f);

                        gameState = GameState.menu;

                    }
                    break;
                //------------------------------------------------------EndMenu
                case GameState.gameEnd:
                    //lose music
                    gameOverMusicInstance.IsLooped = true;
                    gameOverMusicInstance.Volume = volumeMusic;
                    gameOverMusicInstance.Play();
                    kbState = Keyboard.GetState();
                    if (buttonManager.Replay.isMouseClicked() || kbState.IsKeyDown(Keys.R) || (kbState.IsKeyDown(Keys.Space)))
                    {
                        
                        foreach (Boss x in manager.BossList)
                        {
                            x.ResetAttackPattern();
                        }
                        manager.Reset();

                        //end lose music
                        gameOverMusicInstance.Stop();
                        //button click
                        buttonClickSound.Play(volumeGame, 0.0f, 0.0f);

                        gameState = GameState.gamePlay;

                    }
                    if (buttonManager.Quit.isMouseClicked())
                    {
                        //end lose music
                        gameOverMusicInstance.Stop();
                        //button click
                        buttonClickSound.Play(volumeGame, 0.0f, 0.0f);
                        gameState = GameState.quit;
                    }
                    if (buttonManager.Menu.isMouseClicked() || kbState.IsKeyDown(Keys.M))
                    {
                        TotalReset();
                        //end lose music
                        gameOverMusicInstance.Stop();
                        //button click
                        buttonClickSound.Play(volumeGame, 0.0f, 0.0f);
                        gameState = GameState.menu;
                    }
                    break;

                case GameState.gameWinFinal:
                    //win song
                    gameWinMusicInstance.IsLooped = true;
                    gameWinMusicInstance.Volume = volumeMusic;
                    gameWinMusicInstance.Play();


                    kbState = Keyboard.GetState();
                    if (buttonManager.Replay.isMouseClicked()||kbState.IsKeyDown(Keys.R))
                    {
                        TotalReset();
                        //end lose music
                        gameWinMusicInstance.Stop();
                        //button click
                        buttonClickSound.Play(volumeGame, 0.0f, 0.0f);
                        gameState = GameState.gamePlay;

                    }
                    if (buttonManager.Quit.isMouseClicked())
                    {
                        TotalReset();
                        //end win song
                        gameWinMusicInstance.Stop();
                        //button click
                        buttonClickSound.Play(volumeGame, 0.0f, 0.0f);
                        gameState = GameState.quit;
                    }
                    if (buttonManager.Menu.isMouseClicked() || kbState.IsKeyDown(Keys.M))
                    {
                        TotalReset();
                        
                        //end win song
                        gameWinMusicInstance.Stop();
                        //button click
                        buttonClickSound.Play(volumeGame, 0.0f, 0.0f);
                        gameState = GameState.menu;
                    }
                    break;
            }
            base.Update(gameTime);
        }

        
        
        
   //////////////////////////////////////////////////////////////////////////////////////////////////////     
        
        
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);

            // TODO: Add your drawing code here
            //testing draw
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearWrap, null, null);//added to make background scroll
           
            switch (gameState)
            {
                //------------------------------------------------------Menu
                case GameState.menu:

                    _spriteBatch.Draw(TitleScreen, new Vector2(0), Color.White);
                    buttonManager.Setting.Draw(_spriteBatch);
                    buttonManager.Start.Draw(_spriteBatch);
                    buttonManager.Quit.Draw(_spriteBatch);
                    buttonManager.Credits.Draw(_spriteBatch);

                    break;
                //------------------------------------------------------Quit
                case GameState.quit:
                    break;

                //------------------------------------------------------Setting
                case GameState.settings:
                    _spriteBatch.Draw(OptionsScreen, new Vector2(0), Color.White);
                    buttonManager.MenuSet.Draw(_spriteBatch);
                    userInterface.DrawSettings(_spriteBatch);
                    break;

                //-------------------------------------------------------Credits
                case GameState.credits:
                    _spriteBatch.Draw(creditsScreen, new Vector2(0), Color.White);
                    buttonManager.MenuSet.Draw(_spriteBatch);
                    break;
                //------------------------------------------------------Gameplay
                case GameState.gamePlay:

                    //draws and scrolls background
                    Backgrounds[backgroundIndex].Draw(_spriteBatch, Color.White);

                    buttonManager.Pause.Draw(_spriteBatch);
                    Color currentColor;
                    if (manager.CurrentLevel == 0)
                    {
                        currentColor = Color.Red;
                    }
                    else if (manager.CurrentLevel == 1)
                    {
                        currentColor = Color.LimeGreen;
                    }
                    else if(manager.CurrentLevel == 2)
                    {
                        currentColor = Color.White;
                    }
                    else
                    {
                        currentColor = Color.Orange;   
                    }
                            
                    //player, boss and ui
                    foreach (Projectile attack in manager.Projectiles)
                    {
                        attack.Draw(_spriteBatch, currentColor);
                    }
                    player.Draw(_spriteBatch);
                    boss.Draw(_spriteBatch);
                    healthBar.Draw(_spriteBatch);
                    userInterface.Draw(_spriteBatch);
                    break;

                //----------------------------------------------Pause
                case GameState.pause:

                    Color halfOpacityWhite = Color.White * 0.5f;



                    //draw the background
                    Backgrounds[backgroundIndex].Draw(_spriteBatch, Color.White * .5f);


                    //player, boss and ui
                    player.Draw(_spriteBatch);
                  
                    if (manager.CurrentLevel == 0)
                    {
                        currentColor = Color.Red;
                    }
                    else if (manager.CurrentLevel == 1)
                    {
                        currentColor = Color.LimeGreen;
                    }
                    else if (manager.CurrentLevel == 2)
                    {
                        currentColor = Color.White;
                    }
                    else
                    {
                        currentColor = Color.Orange;
                    }

                    foreach (Projectile attack in manager.Projectiles)
                    {
                        attack.Draw(_spriteBatch, currentColor);
                    }
                    boss.Draw(_spriteBatch);
                    healthBar.Draw(_spriteBatch);
                    _spriteBatch.Draw(GreyRectangle, new Rectangle(0, 0, screenWidth, screenHeight), halfOpacityWhite);

                    userInterface.Draw(_spriteBatch);
                    buttonManager.Play.Draw(_spriteBatch);
                    buttonManager.Menu.Draw(_spriteBatch);
                    buttonManager.Quit.Draw(_spriteBatch);

                    break;

                //----------------------------------------Boosted
                case GameState.boosted:
                    //draw the background
                    Backgrounds[backgroundIndex].Draw(_spriteBatch, Color.White);


                    //player, boss and ui
                    player.Draw(_spriteBatch);

                    if (manager.CurrentLevel == 0)
                    {
                        currentColor = Color.Red;
                    }
                    else if (manager.CurrentLevel == 1)
                    {
                        currentColor = Color.LimeGreen;
                    }
                    else if (manager.CurrentLevel == 2)
                    {
                        currentColor = Color.White;
                    }
                    else
                    {
                        currentColor = Color.Orange;
                    }

                    foreach (Projectile attack in manager.Projectiles)
                    {
                        attack.Draw(_spriteBatch, currentColor);
                    }
                  
                    boss.Draw(_spriteBatch);
                    healthBar.Draw(_spriteBatch);
                    userInterface.Draw(_spriteBatch);


                    break;

                //----------------------------------------Power Up
                case GameState.getPowerUp:

                    _spriteBatch.Draw(VictoryScreen, new Vector2(0), Color.White);
                    //Backgrounds[backgroundIndex].Draw(_spriteBatch, Color.White);
                    buttonManager.DamageUp.Draw(_spriteBatch);
                    buttonManager.HealthUp.Draw(_spriteBatch);
                    buttonManager.SpeedUP.Draw(_spriteBatch);
                    _spriteBatch.Draw(powerUpText, new Rectangle(-50,100, powerUpText.Width / 2, powerUpText.Height / 2), Color.White);

                    break;
                //------------------------------------------------------EndMenu
                case GameState.gameEnd:
                    _spriteBatch.Draw(GameOver, new Vector2(0), Color.White);
                    buttonManager.Menu.Draw(_spriteBatch);
                    buttonManager.Replay.Draw(_spriteBatch);
                    buttonManager.Quit.Draw(_spriteBatch);
                    break;
                
                //------------------------------------------------------Win Menu
                case GameState.gameWin:
                    _spriteBatch.Draw(VictoryScreen, new Vector2(0), Color.White);
                    buttonManager.Menu.Draw(_spriteBatch);
                    buttonManager.Next.Draw(_spriteBatch);
                    buttonManager.Quit.Draw(_spriteBatch);
                    break;
                
                case GameState.gameWinFinal:
                    _spriteBatch.Draw(VictoryScreen, new Vector2(0), Color.White);
                    buttonManager.Menu.Draw(_spriteBatch);
                    buttonManager.Quit.Draw(_spriteBatch);
                    buttonManager.Replay.Draw(_spriteBatch);
                    userInterface.DrawScore(_spriteBatch, timeScore);
                    break;

            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        //resets the manager, boss and player, to a new specified index
        public void resetGame()
        {   
            manager.Reset();
            boss = manager.BossList[manager.CurrentLevel];
            healthBar.Boss = manager.BossList[manager.CurrentLevel];
        }
        //resets to base level
        public void TotalReset()
        {
            manager.CurrentLevel = 0;
            resetGame();
            backgroundIndex = 0;
            userInterface.TimerReset();
            player.InitialHealth = 1;
            player.SetHealth = player.InitialHealth;
            foreach(Boss x in manager.BossList)
            {
                x.ResetAttackPattern();
            }
            Projectile temp = dataManager.ProjectileData[0];
            temp.Damage = 5;
            temp.Velocity = new Vector2(temp.Velocity.X, temp.DefaultProjectileSpeed.Y);
            dataManager.ProjectileData[0] = temp;
        }
    }
}
