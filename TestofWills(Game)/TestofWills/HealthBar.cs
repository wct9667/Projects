using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace TestofWills
{
    /// <summary>
    /// This healthbar will be used for the boss
    /// </summary>
    class HealthBar
    {
        private Texture2D texture;
        private Rectangle rectangleH; // health
        private Rectangle rectangleL; //red behind the health
        private Boss boss;


        //Properties, need to be able to reset the current boss
        public Boss Boss
        {
            get { return boss; }
            set { boss = value; }
        }


        //constructor
        public HealthBar(Texture2D texture, Rectangle rectangle, Boss boss)
        {
            this.texture = texture;
            rectangleL = rectangle;
            rectangleH = rectangleL;
            

            this.boss = boss;
        }

        //updates the heatlh bar based on the health of the boss, its width is relegated by the boss's health
        public void Update()
        {
            rectangleH.Width = boss.Health *1/6;
            rectangleL.Width = boss.InitialHealth * 1/6;
            rectangleH.X = boss.Position.X + boss.Position.Width/2 - rectangleH.Width/2;
            rectangleL.X = boss.Position.X + boss.Position.Width/2 - rectangleL.Width/2;
        }

        //draw the healthbar
        public void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, rectangleL, Color.Red * 0f);
            if (Game1.gameState == GameState.gamePlay)
            {
                sb.Draw(texture, rectangleH, Color.Green * .6f);
            }
            else
            {
                sb.Draw(texture, rectangleH, Color.Red * .6f);

            }

            
        }
    
    
    
    
    }
}
