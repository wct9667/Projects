using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TestofWills
{
    /// <summary>
    /// This class just handles all the buttons that are needed, purpose is mainly to clear up game1 a bit
    /// </summary>
    class ButtonManager
    {
        //textures for all of the buttons
        private Texture2D buttonPauseHover;
        private Texture2D buttonPause;
        private Texture2D buttonSettingHover;
        private Texture2D buttonSetting;
        private Texture2D buttonPlayHover;
        private Texture2D buttonPlay;
        private Texture2D buttonQuitHover;
        private Texture2D buttonQuit;
        private Texture2D buttonMenuHover;
        private Texture2D buttonMenu;
        private Texture2D buttonReplayHover;
        private Texture2D buttonReplay;
        private Texture2D buttonStartHover;
        private Texture2D buttonStart;
        private Texture2D buttonNext;
        private Texture2D buttonNextHover;
        private Texture2D buttonCredits;
        private Texture2D buttonCreditsHover;

        //Power up buttons
        private Texture2D buttonDamageUp;
        private Texture2D buttonHealthUp;
        private Texture2D buttonSpeedUp;
        private Texture2D buttonDamageUpHover;
        private Texture2D buttonHealthUpHover;
        private Texture2D buttonSpeedUpHover;


        //buttons, could use a list, but easier to simply create each one with a get as we can easily read what each button is
        private RolloverButton pause;
        private RolloverButton setting;
        private RolloverButton replay;
        private RolloverButton play;
        private RolloverButton quit;
        private RolloverButton menu;
        private RolloverButton start;
        private RolloverButton menuSet;
        private RolloverButton next;
        private RolloverButton credits;

        //Power up buttons
        private RolloverButton healthUp;
        private RolloverButton damageUp;
        private RolloverButton speedUp;


        //properties, get for each
        public RolloverButton Pause
        {
            get { return pause; }
        }
        public RolloverButton Setting
        {
            get { return setting; }
        }
        public RolloverButton Replay
        {
            get { return replay; }
        }
        public RolloverButton Play
        {
            get { return play; }
        }
        public RolloverButton Quit
        {
            get { return quit; }
        }
        public RolloverButton Menu
        {
            get { return menu; }
        }
        public RolloverButton Start
        {
            get { return start; }
        }
        public RolloverButton MenuSet
        {
            get { return menuSet; }
        }
        public RolloverButton Next
        {
            get { return next; }
        }
        public RolloverButton HealthUp
        {
            get { return healthUp; }
        }
        public RolloverButton DamageUp
        {
            get { return damageUp; }
        }
        public RolloverButton SpeedUP
        {
            get { return speedUp; }
        }
        public RolloverButton Credits
        {
            get { return credits; }
        }

        //Content
        private ContentManager Content;

        //screen width and height
        private int screenWidth;
        private int screenHeight;

        //constructor
        public ButtonManager(ContentManager Content, int screenWidth, int screenHeight)
        {
            this.screenHeight = screenHeight;
            this.screenWidth = screenWidth;
            this.Content = Content;
            
            //load all the button content
            LoadContent();
            
            //create all of the buttons based on the window size
            pause = new RolloverButton(buttonPause, buttonPauseHover, new Rectangle(screenWidth - buttonPause.Width,
                0,
                buttonPause.Width,
                buttonPause.Height));

            setting = new RolloverButton(buttonSetting, buttonSettingHover, new Rectangle(screenWidth / 2 - buttonSetting.Width / 2
                , screenHeight / 2 - buttonSetting.Height / 2,
                buttonSetting.Width,
                buttonSetting.Height));

            play = new RolloverButton(buttonPlay, buttonPlayHover, new Rectangle(screenWidth / 2 - (2 * buttonPlay.Width) / 2,
                screenHeight / 3 - (2 * buttonPlay.Height) / 2,
                2 * buttonPlay.Width,
                2 * buttonPlay.Height));

            quit = new RolloverButton(buttonQuit, buttonQuitHover, new Rectangle(screenWidth / 2 - buttonQuit.Width / 2,
                screenHeight * 3 / 4 - 2 * buttonQuit.Height,
                buttonQuit.Width, buttonQuit.Height));

            start = new RolloverButton(buttonStart, buttonStartHover, new Rectangle(screenWidth / 2 - buttonStart.Width / 2,
                screenHeight / 4 + buttonStart.Height,
                buttonStart.Width, buttonStart.Height));

            replay = new RolloverButton(buttonReplay, buttonReplayHover, new Rectangle(screenWidth / 2 - buttonReplay.Width / 2,
                screenHeight / 4 + buttonReplay.Height,
                buttonReplay.Width, buttonReplay.Height));

            menu = new RolloverButton(buttonMenu, buttonMenuHover, new Rectangle(screenWidth / 2 - buttonMenu.Width / 2
                , screenHeight / 2 - buttonMenu.Height / 2,
                buttonMenu.Width,
                buttonMenu.Height));
           
            menuSet = new RolloverButton(buttonMenu, buttonMenuHover, new Rectangle(0
                , 0,
                buttonMenu.Width,
                buttonMenu.Height));

            next = new RolloverButton(buttonNext, buttonNextHover, new Rectangle(screenWidth / 2 - buttonReplay.Width / 2,
                screenHeight / 4 + buttonReplay.Height,
                buttonReplay.Width, buttonReplay.Height));

            damageUp = new RolloverButton(buttonDamageUp, buttonDamageUpHover, new Rectangle(screenWidth / 2 - buttonDamageUp.Width / 2
                , screenHeight / 2 - buttonDamageUp.Height / 2,
                buttonDamageUp.Width,
                buttonDamageUp.Height));

            healthUp = new RolloverButton(buttonHealthUp, buttonHealthUpHover, new Rectangle(screenWidth / 3 - buttonHealthUp.Width / 2 - 25
                , screenHeight / 2 - buttonHealthUp.Height / 2 - 47,
                buttonHealthUp.Width * 10,
                buttonHealthUp.Height * 10));

            speedUp = new RolloverButton(buttonSpeedUp, buttonSpeedUpHover, new Rectangle(screenWidth * 2 / 3 - buttonSpeedUp.Width / 2 
                , screenHeight / 2 - buttonSpeedUp.Height / 2 - 15,
                buttonSpeedUp.Width,
                buttonSpeedUp.Height));

            credits = new RolloverButton(buttonCredits, buttonCreditsHover, new Rectangle(screenWidth / 2 - buttonCredits.Width / 2,
                screenHeight * 9/10 - 2 * buttonCredits.Height,
                buttonCredits.Width, buttonCredits.Height));
        }

        private  void LoadContent()
        {
            //load in the textures for the buttons
            buttonPauseHover = Content.Load<Texture2D>("ButtonMenuPauseHover");
            buttonPause = Content.Load<Texture2D>("ButtonMenuPauseNormal");
            buttonSettingHover = Content.Load<Texture2D>("ButtonOptionsHover");
            buttonSetting = Content.Load<Texture2D>("ButtonOptionsNormal");
            buttonPlayHover = Content.Load<Texture2D>("ButtonMenuPlayHover");
            buttonPlay = Content.Load<Texture2D>("ButtonMenuPlayNormal");
            buttonQuitHover = Content.Load<Texture2D>("ButtonExitHover");
            buttonQuit = Content.Load<Texture2D>("ButtonExitNormal");
            buttonMenuHover = Content.Load<Texture2D>("ButtonMenuHover");
            buttonMenu = Content.Load<Texture2D>("ButtonMenuNormal");
            buttonReplayHover = Content.Load<Texture2D>("ButtonReplayHover");
            buttonReplay = Content.Load<Texture2D>("ButtonReplayNormal");
            buttonStartHover = Content.Load<Texture2D>("ButtonStartHover");
            buttonStart = Content.Load<Texture2D>("ButtonStartNormal");
            buttonNext = Content.Load<Texture2D>("next");
            buttonNextHover = Content.Load<Texture2D>("nextHovered");
            buttonCredits = Content.Load<Texture2D>("ButtonCreditsNormal");
            buttonCreditsHover = Content.Load<Texture2D>("ButtonCreditsHover");

            //Load powerups buttons
            buttonDamageUp = Content.Load<Texture2D>("missile_Launcher_A");
            buttonDamageUpHover = Content.Load<Texture2D>("missile_dark");
            buttonHealthUp = Content.Load<Texture2D>("Medipack");
            buttonHealthUpHover = Content.Load<Texture2D>("health_dark");
            buttonSpeedUp = Content.Load<Texture2D>("unknown");
            buttonSpeedUpHover = Content.Load<Texture2D>("lightning_dark");


        }

    }
}
