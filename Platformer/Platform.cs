using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platformer
{
    public class Platform
    {
        public int width;
        public int height;
        public int x;
        public int y;
        public int endx;
        public bool onplatform = false;
        public Rectangle top;
        public Rectangle bottom;
        public Rectangle left;
        public Rectangle right;
        public Rectangle hitbox;
        
        Texture2D texture;
        public Platform(int width, int height, int x, int y, int endx, Texture2D texture)
        {
            this.width = width;
            this.height = height;
            this.x = x;
            this.y = y;
            this.texture = texture;
            top = new Rectangle(x * 50, y * 50, width * 50, 2);
            bottom = new Rectangle(x * 50, y * 50 + height * 50, width * 50, 2);
            left = new Rectangle(x * 50 - 30, y * 50 + 4, 2, height * 50 - 4);
            right = new Rectangle(x * 50 + width * 50 + 30, y * 50 + 4, 2, height * 50 - 4);
            hitbox = new Rectangle((x-2) * 50, y * 50, (width+4) * 50, height * 50);
        }
        public void draw(SpriteBatch spritebatch)
        {
            for (int w = 0; w < width; w++)
            {
                for (int h = 0; h < height; h++)
                {
                    spritebatch.Draw(texture, new Vector2(50 * x + 50 * w, 50 * y + 50 * h), Color.Black);
                }
            }
        }
    }
}
