using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Platformer
{
    public class Character : AnimatedSprite
    {
        TimeSpan elapsedGameTime;
        bool grounded = true;
        public float speedY = 0.0f;
        public int y = 0;
        int initialY = 0;
        Vector4 hitboxoffset;
        public int speedX = 0;
        int maxspeed = 10;
        bool hitleft = false;
        bool hitright = false;
        bool walljumped = false;
        bool justhit = false;
        KeyboardState lastks;
        KeyboardState ks;
        int prevspeedX;
        int leftbounds = 10;
        int rightbounds = 1820;

        public Character(Texture2D forward, Texture2D backward, Vector2 position, Color color, List<Rectangle> frames, Vector4 hitboxoffset, int framedelayamount) : base(forward, backward, position, color, frames, hitboxoffset, framedelayamount)
        {
            this.hitboxoffset = hitboxoffset;
            initialY = (int)position.Y;

        }

        public override Rectangle hitbox
        {
            get { return new Rectangle((int)position.X + (int)hitboxoffset.X, (int)position.Y + (int)hitboxoffset.Y, frames[currentframe].Width - (int)hitboxoffset.X - (int)hitboxoffset.Z, frames[currentframe].Height - (int)hitboxoffset.Y - (int)hitboxoffset.W); }
        }


        public void Update(GameTime gameTime)
        {
            lastks = ks;
            ks = Keyboard.GetState();

            elapsedGameTime += gameTime.ElapsedGameTime;
            if (position.X + speedX >= leftbounds - 10 && position.X + speedX <= rightbounds + 10)
            {
                if (ks.IsKeyDown(Keys.D) && position.X < rightbounds)
                {
                    if (speedX < maxspeed)
                    {
                        speedX++;
                    }
                    else
                    {
                        speedX = maxspeed;
                    }
                }
                else if (ks.IsKeyDown(Keys.A) && position.X > leftbounds)
                {
                    if (speedX > -maxspeed)
                    {
                        speedX--;
                    }
                    else
                    {
                        speedX = -maxspeed;
                    }
                }
                else if (position.X < leftbounds && !grounded)
                {
                    speedX = 0;
                    hitleft = true;
                }
                else if (position.X > rightbounds && !grounded)
                {
                    speedX = 0;
                    hitright = true;
                }
                else if (speedX < 0 && grounded)
                {
                    speedX++;
                }
                else if (speedX > 0 && grounded)
                {
                    speedX--;
                }
                else if (grounded)
                {
                    speedX = 0;
                    prevspeedX = 0;
                }
            }
            else
            {
                if (!justhit && !grounded)
                {
                    prevspeedX = speedX;
                    justhit = true;
                }
                speedX = 0;
            }

            if (position.X > leftbounds)
            {
                justhit = false;
                hitleft = false;
            }
            if (position.X < rightbounds)
            {
                justhit = false;
                hitright = false;
            }

            if (ks.IsKeyDown(Keys.Space) && !lastks.IsKeyDown(Keys.Space))
            {
                if (grounded)
                {
                    speedY = -12f;
                    grounded = false;
                }
                if (hitleft && !walljumped)
                {
                    speedY = -12f;
                    speedX = -prevspeedX;
                    walljumped = true;
                }
                if (hitright && !walljumped)
                {
                    speedY = -12f;
                    speedX = -prevspeedX;
                    walljumped = true;
                }
            }

            if (!grounded)
            {
                speedY += 0.4f;
                currentframe = 18;
            }

            y += (int)speedY;

            if (y >= 0 && !grounded && speedY > 0)
            {
                grounded = true;
                y = 0;
                speedY = 0;
                walljumped = false;
            }


            if (speedX != 0 && elapsedGameTime >= TimeSpan.FromMilliseconds(50 / speedX) && grounded)
            {
                if (currentframe < 68)
                {
                    currentframe += 2;
                }
                else
                {
                    currentframe = 0;
                }

                elapsedGameTime = TimeSpan.Zero;
            }
            if (speedX == 0)
            {
                currentframe = 70;
            }

            position.Y = initialY + y;
            position.X += speedX;
        }
    }
}
