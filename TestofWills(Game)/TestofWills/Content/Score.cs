using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TestofWills.Content
{
    /// <summary>
    /// This class manages the game timer and draws the timer to the screen
    /// </summary>
    /// 

    // TODO: How the fuck does a timer work
    class Score
    {
        // FIELDS

        private SpriteFont spriteFont;
        private int timer;
        private Vector2 scorePosition;

        // PROPERTIES

        public int Timer
        {
            get { return timer; }

            set { timer = value; }
        }

        // METHODS

        public void Update()
        {
            // Add code to update player score
        }

        public void Draw(SpriteBatch sb)
        {
            sb.DrawString(spriteFont,
                "Score: " + timer,
                scorePosition,
                Color.White);
        }

        // CONSTRUCTORS

        public Score(SpriteFont spriteFont, Vector2 scorePosition)
        {
            this.spriteFont = spriteFont;
            this.scorePosition = scorePosition;
        }

    }
}
