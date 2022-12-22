using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
//Need a asset for the internal hitbox
namespace TestofWills
{
    public enum PlayerState
    { 
    movingLeft,
    movingRight,
    straight,
    }

    /// <summary>
    /// Fields: internal rectangle for hitbox, prevState to make sure the shooting isn't spammed
    /// 2 constructors, one for file, one for not
    /// Methods: update, will check prev kbstate for shooting. checkposition, will make sure player is not beyond bounds
    /// Shoot() will create a projectile based on loaded data
    /// </summary>
    class Player : GameObject
    {
        //fields
        Rectangle internalHitbox;
        bool hasShot;
        private Texture2D BlueTiltLeft;
        private Texture2D BlueTiltRight;
        private Texture2D BlueStraight;
        private ContentManager Content;

        //testing 
        private Texture2D testSquare;
        private PlayerState playerState;
        private double velocityFixed;

        //properties
        public bool HasShot
        {
            get { return hasShot; }
        }

        //get for the rectangle internal
        public Rectangle InternalHitBox
        {
            get { return internalHitbox; }
        }



        //constructor leverages the parent
        public Player(ContentManager Content, Rectangle position, int height, int width, string filename) : base(position, height, width, filename)
        {
            this.Content = Content;
            LoadData();
            //center the player, then make the internal hitbox
            Center();
            internalHitbox = new Rectangle(this.position.X + this.position.Width * 3 / 8,
            this.position.Y + this.position.Height * 3 / 8,
            this.position.Width / 4,
            this.position.Height / 4);

            velocityFixed = .7071 * velocity.X;
            hasShot = false;
        }

        //constructor with no filename
        public Player(ContentManager Content, Rectangle position, int health, Vector2 velocity, int height, int width, string filename) : base(position, health, velocity, width, height)
        {
            this.Content = Content;
            LoadData();
            //center the player, then make the internal hitbox
            Center();
            internalHitbox = new Rectangle(this.position.X + this.position.Width * 3 / 8,
                  this.position.Y + this.position.Height * 3 / 8,
                  this.position.Width / 4,
                  this.position.Height / 4);

            velocityFixed = .7071 * velocity.X;

            hasShot = false;
        }

        /// <summary>
        /// Update Method, will not let the player get too high, too low, or too far to the left or right
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            KeyboardState kbState = Keyboard.GetState();
            PlayerIndex playerIndex = new PlayerIndex();
            GamePadState gPState = GamePad.GetState(playerIndex);
            
            //use arrow keys or wasd
            //check diagonal, and normalize so speed is not too fast
            if ((kbState.IsKeyDown(Keys.A) || kbState.IsKeyDown(Keys.Left)) && ((kbState.IsKeyDown(Keys.W) || kbState.IsKeyDown(Keys.Up))))
            {
                if (kbState.IsKeyDown(Keys.LeftShift))
                {
                    playerState = PlayerState.movingLeft;
                    position.X -= (int)velocityFixed/2;
                    internalHitbox.X -= (int)velocityFixed/2;
                    position.Y -= (int)velocityFixed/2;
                    internalHitbox.Y -= (int)velocityFixed/2;
                }
                else
                {
                    playerState = PlayerState.movingLeft;
                    position.X -= (int)velocityFixed;
                    internalHitbox.X -= (int)velocityFixed;
                    position.Y -= (int)velocityFixed;
                    internalHitbox.Y -= (int)velocityFixed;
                }
               
            }
            else if ((kbState.IsKeyDown(Keys.A) || kbState.IsKeyDown(Keys.Left)) && (kbState.IsKeyDown(Keys.S) || kbState.IsKeyDown(Keys.Down)))
            {
                if (kbState.IsKeyDown(Keys.LeftShift))
                {
                    playerState = PlayerState.movingLeft;
                    position.X -= (int)velocityFixed/2;
                    internalHitbox.X -= (int)velocityFixed/2;
                    position.Y += (int)velocityFixed/2;
                    internalHitbox.Y += (int)velocityFixed/2;
                }
                else
                {
                    playerState = PlayerState.movingLeft;
                    position.X -= (int)velocityFixed;
                    internalHitbox.X -= (int)velocityFixed;
                    position.Y += (int)velocityFixed;
                    internalHitbox.Y += (int)velocityFixed;
                }
            }
            else if((kbState.IsKeyDown(Keys.D) || kbState.IsKeyDown(Keys.Right)) && (kbState.IsKeyDown(Keys.W) || kbState.IsKeyDown(Keys.Up)))
            {
                if (kbState.IsKeyDown(Keys.LeftShift))
                {
                    playerState = PlayerState.movingRight;
                    position.X += (int)velocityFixed/2;
                    internalHitbox.X += (int)velocityFixed/2;
                    position.Y -= (int)velocityFixed/2;
                    internalHitbox.Y -= (int)velocityFixed/2;
                }
                else
                {
                    playerState = PlayerState.movingRight;
                    position.X += (int)velocityFixed;
                    internalHitbox.X += (int)velocityFixed;
                    position.Y -= (int)velocityFixed;
                    internalHitbox.Y -= (int)velocityFixed;
                }
            }
            else if ((kbState.IsKeyDown(Keys.D) || kbState.IsKeyDown(Keys.Right)) && (kbState.IsKeyDown(Keys.S) || kbState.IsKeyDown(Keys.Down)))
            {
                playerState = PlayerState.movingRight;
                if (kbState.IsKeyDown(Keys.LeftShift))
                {
                    position.X += (int)velocityFixed/2;
                    internalHitbox.X += (int)velocityFixed/2;
                    position.Y += (int)velocityFixed/2;
                    internalHitbox.Y += (int)velocityFixed/2;
                }
                else
                {
                    position.X += (int)velocityFixed;
                    internalHitbox.X += (int)velocityFixed;
                    position.Y += (int)velocityFixed;
                    internalHitbox.Y += (int)velocityFixed;
                }
            }
            else if (kbState.IsKeyDown(Keys.W) || kbState.IsKeyDown(Keys.Up))
            {
                playerState = PlayerState.straight;
                if (kbState.IsKeyDown(Keys.LeftShift))
                {
                    position.Y -= (int)velocity.Y / 2;
                    internalHitbox.Y -= (int)velocity.Y / 2;
                }
                else
                {
                    position.Y -= (int)velocity.Y;
                    internalHitbox.Y -= (int)velocity.Y;
                }
            }
            else if (kbState.IsKeyDown(Keys.S) || kbState.IsKeyDown(Keys.Down) || gPState.ThumbSticks.Right.Y < 0 || gPState.ThumbSticks.Left.Y < 0)
            {
                playerState = PlayerState.straight;
                if (kbState.IsKeyDown(Keys.LeftShift))
                {
                    position.Y += (int)velocity.Y / 2;
                    internalHitbox.Y += (int)velocity.Y / 2;
                }
                else
                {
                    position.Y += (int)velocity.Y;
                    internalHitbox.Y += (int)velocity.Y;
                }
            }

            else if ((kbState.IsKeyDown(Keys.A) && kbState.IsKeyDown(Keys.D)) || (kbState.IsKeyDown(Keys.Left) && kbState.IsKeyDown(Keys.Right)) || (kbState.IsKeyDown(Keys.A) && kbState.IsKeyDown(Keys.Right)) || (kbState.IsKeyDown(Keys.D) && kbState.IsKeyDown(Keys.Left)))
            {
                playerState = PlayerState.straight;
            }
            else if (kbState.IsKeyUp(Keys.A) && kbState.IsKeyUp(Keys.D) && kbState.IsKeyUp(Keys.Left) && kbState.IsKeyUp(Keys.Right))
            {
                playerState = PlayerState.straight;
            }

            else if (kbState.IsKeyDown(Keys.A) || kbState.IsKeyDown(Keys.Left) || gPState.ThumbSticks.Right.X < 0 || gPState.ThumbSticks.Left.X < 0)
            {
                playerState = PlayerState.movingLeft;
                if (kbState.IsKeyDown(Keys.LeftShift))
                {
                    position.X -= (int)velocity.X / 2;
                    internalHitbox.X -= (int)velocity.X / 2;
                }
                else
                {
                    position.X -= (int)velocity.X;
                    internalHitbox.X -= (int)velocity.X;
                }
            }
            else if (kbState.IsKeyDown(Keys.D) || kbState.IsKeyDown(Keys.Right) || gPState.ThumbSticks.Right.X > 0 || gPState.ThumbSticks.Left.X > 0)
            {
                playerState = PlayerState.movingRight;
                if (kbState.IsKeyDown(Keys.LeftShift))
                {
                    position.X += (int)velocity.X / 2;
                    internalHitbox.X += (int)velocity.X / 2;
                }
                else
                {
                    position.X += (int)velocity.X;
                    internalHitbox.X += (int)velocity.X;
                }
            }
        
           // if (kbState.IsKeyDown(Keys.Space) || gPState.Triggers.Left > 0 || gPState.Triggers.Right > 0)
            //{
                //due to scope, the collision manager will create the projectile if this value is true: event here?
                hasShot = true;
           // }
            //else
            // {
            //     hasShot = false;
             //}
           
            CheckPosition();
        }

        /// <summary>
        /// This method checks the position of the player and resets their position to not go over the bounds
        /// It also makes sure the internal hitbox stays in the middle of the player
        /// </summary>
        public override void CheckPosition()
        {
            //if statements
            if (position.X <= 0)
            {
                position.X = 0;
                internalHitbox.X = position.X + this.position.Width *3/8;
            }
            else if (position.X + position.Width >= screenWidth)
            {
                position.X = screenWidth - position.Width;
                internalHitbox.X = position.X + this.position.Width *3/8;
            }
            if (position.Y <= screenHeight / 3)
            {
                position.Y = screenHeight / 3;
                internalHitbox.Y = position.Y + this.position.Height * 3/8;
            }
            else if (position.Y + position.Height >= screenHeight)
            {
                position.Y = screenHeight - position.Height;
                internalHitbox.Y = position.Y + this.position.Height *3/8;
            }
        }

        //draws the player and its internal hitbox
        public override void Draw(SpriteBatch sb)
        {
            /*
            base.Draw(sb);
            sb.Draw(asset,
              internalHitbox,
              Color.Yellow);
            */
            switch (playerState)
            {
                case PlayerState.movingLeft:
              sb.Draw(BlueTiltLeft,
              position, 
               Color.White);

              sb.Draw(BlueTiltLeft,
              internalHitbox,
              Color.YellowGreen);

                   /* if (UserInterface.GodMode)
                    {
                        sb.Draw(testSquare,
                       internalHitbox,
                       Color.YellowGreen * .5f);
                    }*/
                    break;

                case PlayerState.movingRight:
                    sb.Draw(BlueTiltRight,
                    position,
                    Color.White);

                    sb.Draw(BlueTiltRight,
                    internalHitbox,
                    Color.YellowGreen * .5f);

                   /* if (UserInterface.GodMode)
                    {
                        sb.Draw(testSquare,
                       internalHitbox,
                       Color.YellowGreen * .5f);
                    }*/
                   
                    break;

         

                case PlayerState.straight:
                    sb.Draw(BlueStraight,
                        position,
                        Color.White);

                    sb.Draw(BlueStraight,
                    internalHitbox,
                    Color.YellowGreen);

                  /*  if (UserInterface.GodMode)
                    {
                        sb.Draw(testSquare,
                       internalHitbox,
                       Color.YellowGreen * .5f);
                    }*/
                    break;


            }
        }

        //updates the internal hitbox
        public void ResetInternal()
        {
            internalHitbox = new Rectangle(this.position.X + this.position.Width * 3 / 8,
                  this.position.Y + this.position.Height * 3 / 8,
                  this.position.Width / 4,
                  this.position.Height / 4);
        }

        //load data
        private void LoadData()
        {
            asset = Content.Load<Texture2D>("SpaceshipTest");
            BlueTiltLeft = Content.Load<Texture2D>("ship left");
            BlueStraight = Content.Load<Texture2D>("ship straight");
            BlueTiltRight = Content.Load<Texture2D>("ship right");
            testSquare = Content.Load<Texture2D>("Grey rectangle");
        }

        //this method moves the player, meant to be used externally by bosses to move the player(black hole)
        public void MovePlayer(int x, int y)
        {
            position.X += x;
            internalHitbox.X += x;
            position.Y += y;
            internalHitbox.Y += y;
            CheckPosition();
        }

            
    }
}
