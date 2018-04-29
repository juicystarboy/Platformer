using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer
{
    public class AnimatedSprite : Sprite
    {
        public List<Rectangle> frames;
        Vector4 hitboxoffset;
        public int currentframe;
        public int framedelay = 0;
        public int framedelayamount = 5;
        public AnimatedSprite(Texture2D texture, Vector2 position, Color color, List<Rectangle> frames, Vector4 hitboxoffset, int framedelayamount) : base(texture, position, color)
        {
            this.frames = frames;
            this.hitboxoffset = hitboxoffset;
            this.framedelayamount = framedelayamount;
        }

        public override Rectangle hitbox
        {
            get { return new Rectangle((int)position.X + (int)hitboxoffset.X, (int)position.Y + (int)hitboxoffset.Y, frames[currentframe].Width - (int)hitboxoffset.X - (int)hitboxoffset.Z, frames[currentframe].Height - (int)hitboxoffset.Y - (int)hitboxoffset.W); }
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, frames[currentframe], color);
        }
    }
}
