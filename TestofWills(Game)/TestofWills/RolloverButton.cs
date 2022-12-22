using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TestofWills
{
    public class RolloverButton
    {
        // FIELDS

        private Texture2D hoverButton;
        private Texture2D idleButton;
        private Rectangle rectangle;
        private MouseState prevMouseState;

        // PROPERTIES

        public int Width
        {
            get { return rectangle.Width; }
        }

        public int Height
        {
            get { return rectangle.Height; }
        }

        public int X
        {
            get { return rectangle.X; }
        }

        public int Y
        {
            get { return rectangle.Y; }
        }
        public Rectangle Rectangle
        {
            get { return rectangle; }
        }

        // METHODS

        public void Draw(SpriteBatch sb)
        {
            // Checks if mouse is in the bounds of the button rectangle
            MouseState mouseState = Mouse.GetState();
            if ((mouseState.X < (rectangle.X + rectangle.Width)) &&
                (mouseState.Y < (rectangle.Y + rectangle.Height)) &&
                (mouseState.X > (rectangle.X)) &&
                (mouseState.Y > (rectangle.Y)))
            {
                // If in bounds, button updates to hover sprite
                sb.Draw(hoverButton, rectangle, Color.Purple);

            }
            else
            {
                // If out of bounds, button remains idle
                sb.Draw(idleButton, rectangle, Color.White);

            }

            //feedback, not working atm, not sure why, will try passing in the state of the button later
            if (isMouseClicked())
            {
                sb.Draw(idleButton, rectangle, Color.Black);
            }
        }

        public void DrawReversed(SpriteBatch sb)
        {
            MouseState mouseState = Mouse.GetState();
            if ((mouseState.X < (rectangle.X + rectangle.Width)) &&
                (mouseState.Y < (rectangle.Y + rectangle.Height)) &&
                (mouseState.X > (rectangle.X)) &&
                (mouseState.Y > (rectangle.Y)))
            {
                // If in bounds, button updates to hover sprite
                sb.Draw(hoverButton, rectangle, null, Color.Purple, 0.0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0.0f);
            }
            else if (isMouseClicked())
            {
                sb.Draw(idleButton, rectangle, null, Color.Black, 0.0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0.0f);
            } 
            else
            {
                // If out of bounds, button remains idle
                sb.Draw(idleButton, rectangle, null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0.0f);
            }
        }
     

        // Checks if LMB is being pressed and returns
        // true if so, and false otherwise.
        public bool isMouseClicked()
        {
            MouseState mouseState = Mouse.GetState();
            if ((mouseState.X < (rectangle.X + rectangle.Width)) &&
                (mouseState.Y < (rectangle.Y + rectangle.Height)) &&
                (mouseState.X > (rectangle.X)) &&
                (mouseState.Y > (rectangle.Y)) && mouseState.LeftButton == ButtonState.Pressed && (prevMouseState.LeftButton == ButtonState.Released))
            {
                prevMouseState = mouseState;
                return true;
            } 
                prevMouseState = mouseState;
                return false;
        }


        /// <summary>
        /// Not sure we need this, as the update logic seems to be held in the draw
        /// </summary>
        public void Update()
        {
            isMouseClicked();
        }

        // CONSTRUCTORS

        public RolloverButton(Texture2D idleButton,
            Texture2D hoverButton,
            Rectangle rectangle)
        {
            this.idleButton = idleButton;
            this.hoverButton = hoverButton;
            this.rectangle = rectangle;
        }

    }
}