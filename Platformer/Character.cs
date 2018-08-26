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
        public bool grounded = true;
        public float speedY = 0.0f;
        public int y = 0;
        int initialY = 0;
        Vector4 hitboxoffset;
        public int speedX = 0;
        int maxspeed = 10;
        public bool hitleft = false;
        public bool hitright = false;
        public bool onPlatform = false;
        bool hitleftplatform = false;
        bool hitrightplatform = false;
        bool walljumped = false;
        bool justhit = false;
        KeyboardState lastks;
        KeyboardState ks;
        int prevspeedX;
        int leftbounds = 10;
        int rightbounds = 1820;
        public int groundY = 0;
        float jumpspeed = 15f;
        public bool whymode;
        public bool crouching;
        public bool cantcrouch;
        Rectangle standinghitbox;
        public bool onAPlatform = false;
        public bool wasOnPlatform = false;
        public bool alwayscrouch;

        public Character(Texture2D forward, Texture2D backward, Texture2D forwardcrouching, Texture2D backwardcrouching, Vector2 position, Color color, List<Rectangle> frames, Vector4 hitboxoffset, int framedelayamount) : base(forward, backward, forwardcrouching, backwardcrouching, position, color, frames, hitboxoffset, framedelayamount)
        {
            this.hitboxoffset = hitboxoffset;
            initialY = (int)position.Y;

        }

        public override Rectangle hitbox
        {
            get { return new Rectangle((int)position.X + (int)hitboxoffset.X, (int)position.Y + (int)hitboxoffset.Y, frames[currentframe].Width - (int)hitboxoffset.X - (int)hitboxoffset.Z, frames[currentframe].Height - (int)hitboxoffset.Y - (int)hitboxoffset.W); }
        }


        public void Update(GameTime gameTime, List<Platform> platform)
        {
            lastks = ks;
            ks = Keyboard.GetState();


            hitrightplatform = false;
            hitleftplatform = false;
            groundY = 0;
            onAPlatform = false;
            foreach (Platform p in platform)
            {

                if (hitbox.Intersects(p.left))
                {
                    hitleftplatform = true;
                    hitleft = true;
                    prevspeedX = speedX;
                    if (speedX >= 0)
                    {
                        speedX = 0;
                    }
                }
                else if (hitbox.Intersects(p.right))
                {
                    hitrightplatform = true;
                    hitright = true;
                    prevspeedX = speedX;
                    if (speedX <= 0)
                    {
                        speedX = 0;
                    }
                }
                else if (hitbox.Intersects(p.top))
                {
                    groundY = p.top.Y - 1080;
                    y = groundY;
                    grounded = true;
                    walljumped = false;
                    p.onplatform = true;
                    onAPlatform = true;
                }
                else if (hitbox.Intersects(p.bottom) || hitbox.Y < 0)
                {
                    if (speedY <= 0)
                    {
                        speedY = 0;
                    }
                }
                else if (ks.IsKeyDown(Keys.A) && position.X > leftbounds)
                {
                    if (p.onplatform)
                    {
                        p.onplatform = false;
                        grounded = false;
                        speedX = prevspeedX;
                        groundY = 0;
                    }

                }
                else if (ks.IsKeyDown(Keys.D) && position.X < rightbounds)
                {
                    if (p.onplatform)
                    {
                        p.onplatform = false;
                        grounded = false;
                        speedX = prevspeedX;
                        groundY = 0;
                    }
                }
                else if (whymode)
                {
                    p.onplatform = false;
                    grounded = false;
                    speedX = prevspeedX;
                    groundY = 0;
                }
                /*
                if (!hitbox.Intersects(p.top) && y!=0)
                {
                    grounded = false;
                }
                */
            }
            if (!onAPlatform && wasOnPlatform)
            {
                grounded = false;
            }
            wasOnPlatform = onAPlatform;
            cantcrouch = false;
            standinghitbox = new Rectangle(hitbox.X, hitbox.Y - (int)hitboxoffset.Y, hitbox.Width, hitbox.Height + (int)hitboxoffset.Y);
            foreach (Platform b in platform)
            {
                if (standinghitbox.Intersects(b.bottom))
                {
                    cantcrouch = true;
                }
            }
            if (ks.IsKeyDown(Keys.LeftShift) || ks.CapsLock || alwayscrouch)
            {
                crouching = true;
                hitboxoffset.Y = 55;
            }
            else if (!cantcrouch)
            {
                crouching = false;
                hitboxoffset.Y = 5;
            }
            elapsedGameTime += gameTime.ElapsedGameTime;
            if (position.X + speedX >= leftbounds - 10 && position.X + speedX <= rightbounds + 10)
            {
                if (ks.IsKeyDown(Keys.Space) && !lastks.IsKeyDown(Keys.Space))
                {
                    groundY = 0;
                    if (grounded)
                    {
                        speedY = -jumpspeed;
                        grounded = false;
                    }
                    if (hitleft && !walljumped && !onAPlatform)
                    {
                        speedY = -jumpspeed;
                        speedX = -Math.Abs(prevspeedX);
                        walljumped = true;
                    }
                    if (hitright && !walljumped && !onAPlatform)
                    {
                        speedY = -jumpspeed;
                        speedX = Math.Abs(prevspeedX);
                        walljumped = true;
                    }
                }

                if (ks.IsKeyDown(Keys.D) && position.X < rightbounds && !hitleftplatform)
                {
                    if (speedX < maxspeed)
                    {
                        speedX++;
                    }
                    else
                    {
                        //speedX = maxspeed;
                    }
                }
                else if (ks.IsKeyDown(Keys.A) && position.X > leftbounds && !hitrightplatform)
                {
                    if (speedX > -maxspeed)
                    {
                        speedX--;
                    }
                    else
                    {
                        //speedX = -maxspeed;
                    }
                }
                else if (position.X < leftbounds && !grounded)
                {
                    speedX = 0;
                    hitright = true;
                }
                else if (position.X > rightbounds && !grounded)
                {
                    speedX = 0;
                    hitleft = true;
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
                hitright = false;
            }
            if (position.X < rightbounds)
            {
                justhit = false;
                hitleft = false;
            }

            if (!grounded)
            {
                speedY += 0.4f;
                currentframe = 18;
            }


            if (y >= groundY && !grounded && speedY > 0)
            {
                grounded = true;
                y = groundY;
                speedY = 0;
                walljumped = false;
            }

            y += (int)speedY;


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

            if (Keyboard.GetState().IsKeyDown(Keys.O))
            {
                whymode = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.P))
            {
                whymode = false;
            }
            position.Y = initialY + y;
            position.X += speedX;
        }
    }
}
