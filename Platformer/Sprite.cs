using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platformer
{
    public class Sprite
    {
        public Texture2D texture;
        public Vector2 position;
        public Color color;
        /*
        public float lerpAmount = .01f;
        Color invertedColor;
        Color originalColor;*/

        public Sprite(Texture2D texture, Vector2 position, Color color)
        {
            this.texture = texture;
            this.position = position;
            this.color = color;
            //originalColor = color;
            //invertedColor = new Color(255 - color.R, 255 - color.G, 255 - color.B);
        }

        public virtual Rectangle hitbox
        {
            get { return new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height); }
        }

        /*public void InvertColor()
        {
            if (lerpAmount <= 1)
            {
                color = Color.Lerp(originalColor, invertedColor, lerpAmount);
                lerpAmount += .01f;
            }
        }*/

        public virtual void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, color);
        }
    }
}
