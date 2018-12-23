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
        public Texture2D forward;
        public Texture2D backward;
        public Texture2D forwardcrouching;
        public Texture2D backwardcrouching;
        public Vector2 position;
        public Vector4 hitboxoffset;
        public Color color;
        /*
        public float lerpAmount = .01f;
        Color invertedColor;
        Color originalColor;*/

        public Sprite(Texture2D forward, Texture2D backward, Texture2D forwardcrouching, Texture2D backwardcrouching, Vector2 position, Vector4 hitboxoffset, Color color)
        {
            this.forward = forward;
            this.backward = backward;
            this.forwardcrouching = forwardcrouching;
            this.backwardcrouching = backwardcrouching;
            this.position = position;
            this.color = color;
            this.hitboxoffset = hitboxoffset;
            //originalColor = color;
            //invertedColor = new Color(255 - color.R, 255 - color.G, 255 - color.B);
        }

        public virtual Rectangle hitbox
        {
            get { return new Rectangle((int)position.X, (int)position.Y, forward.Width, forward.Height); }
        }

        /*public void InvertColor()
        {
            if (lerpAmount <= 1)
            {
                color = Color.Lerp(originalColor, invertedColor, lerpAmount);
                lerpAmount += .01f;
            }
        }*/

        public virtual void draw(SpriteBatch spriteBatch, int speedX, bool crouching)
        {
            if (speedX >= 0)
            {
                spriteBatch.Draw(forward, position, color);
            }
            else if (speedX < 0)
            {
                spriteBatch.Draw(backward, position, color);
            }
        }
    }
}
