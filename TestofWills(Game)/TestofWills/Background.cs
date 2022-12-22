using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace TestofWills
{
    class Background
    {
        private Texture2D background; //The image to use
        private Vector2 offset; //Offsets image
        public Vector2 speed; //Speed of movement
        public float zoom; //zoom of image
        private Viewport Viewport;
        private ContentManager contentManager;
        
        private Rectangle Rectangle
        {
            get { return new Rectangle((int)(offset.X), (int)(offset.Y), (int)(Viewport.Width / zoom), (int)(Viewport.Height / zoom)); }
        }


        public Background(Vector2 speed, float zoom, ContentManager contentManager, string filename)
        {
            offset = Vector2.Zero;
            this.speed = speed;
            this.zoom = zoom;
            this.contentManager = contentManager;
            LoadContent(filename);
        }
        //this loads the content for the backgrounds

        private void LoadContent(string filename)
        {
            background = contentManager.Load<Texture2D>(filename);
        }

        public void Update(GameTime gametime, Vector2 direction, Viewport viewport)
        {
            float elapsed = (float)gametime.ElapsedGameTime.TotalSeconds;

            //Store the viewport
            Viewport = viewport;

            //distance background moves
            Vector2 distance = direction * speed * elapsed;

            //offsets the image
            offset += distance;
        }
        public void Draw(SpriteBatch sb, Color color)
        {
            sb.Draw(
                background,
                new Vector2(Viewport.X, Viewport.Y),
                Rectangle,
                color,
                0,
                Vector2.Zero,
                zoom,
                SpriteEffects.None,
                1);
        }
    }

}
