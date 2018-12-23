using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Platformer
{
    public class Level
    {
        public List<Platform> platforms;
        public Random rand;
        public int highestplatformY;
        public Rectangle box;
        Rectangle savedbox;
        public List<int> reachableplatforms;
        public List<Rectangle> phits;
        public int randreachableplatform = 0;
        public List<Rectangle> level;
        public bool win;
        public bool lose;
        public int Seed { get; private set; }
        
        int scale = 1;
        
        public void LoadLevel(Texture2D platformpiece,int score)
        {
            rand = new Random();

            Seed = rand.Next();

            rand = new Random(Seed);            

            reachableplatforms = new List<int>();
            phits = new List<Rectangle>();

            int c = 0;
            platforms = new List<Platform>();
            int amountofplatforms = rand.Next(10, 100);
            //ADD PLATFORM AT 17 HERE
            platforms.Add(new Platform(rand.Next(2, 10), 1, rand.Next(0, 30), 17, platformpiece));
            while (platforms.Count < amountofplatforms)
            {
                bool valid;
                Platform temp;
                c = 0;
                do
                {
                    //create random numbers
                    temp = new Platform(rand.Next(2, 10), 1, rand.Next(0, 30), rand.Next(3, 18), platformpiece);
                    valid = true;
                    foreach (Platform p in platforms)
                    {
                        if (p.hitbox.Intersects(temp.hitbox))
                        {
                            valid = false;
                        }
                    }
                    if (c >= 1000)
                    {
                        amountofplatforms = platforms.Count;
                        break;
                    }
                    c++;
                } while (!valid);

                if (valid)
                {
                    platforms.Add(temp);
                }
            }
            reachableplatforms.Clear();
            phits.Clear();
            highestplatformY = 17;
            for (int i = 0; i < platforms.Count; i++)
            {
                /*for (int j = 0; j < platform.Count; j++)
                {
                    if ((platform[j].x > platform[i].x && platform[j].x < platform[i].x + platform[i].width) || (platform[j].x + platform[j].width < platform[i].x + platform[i].width && platform[j].x + platform[j].width > platform[i].x) || (platform[j].x < platform[i].x && platform[j].x + platform[j].width > platform[i].x + platform[i].width) || (platform[i].x < platform[j].x && platform[i].x + platform[i].width > platform[j].x + platform[j].width))
                    {
                        if (platform[i].y>platform[j].y && platform[i].y - platform[j].y < 4)
                        {
                            reachable = false;
                        }
                    }
                }*/
                platforms[i].reachable = true;
                if (platforms[i].y <= highestplatformY)
                {
                    highestplatformY = platforms[i].y;
                }

                phits.Add(new Rectangle((platforms[i].x-1) * 50, (platforms[i].y - 3) * 50, (platforms[i].width+2) * 50, (platforms[i].height + 3) * 50));

                foreach (Platform p in platforms)
                {
                    if (platforms[i] != p && phits[i].Intersects(p.hitbox))
                    {
                        platforms[i].reachable = false;
                        break;
                    }
                }

                if (platforms[i].reachable)
                {
                    reachableplatforms.Add(i);
                }
            }
            if (reachableplatforms.Count > 0)
            {
                randreachableplatform = reachableplatforms[rand.Next(0, reachableplatforms.Count - 1)];
                savedbox = new Rectangle((platforms[randreachableplatform].x * 50) + (platforms[randreachableplatform].width * 50) / 2 - 20, (platforms[randreachableplatform].y * 50) - 45, 40, 40);
                box = new Rectangle((platforms[randreachableplatform].x * 50) + (platforms[randreachableplatform].width * 50) / 2 - 20, (platforms[randreachableplatform].y * 50) - 45, 40, 40);
            }
        }

        public void Createlist(int xoffset, int yoffset, int scale)
        {
            this.scale = scale;
            level = new List<Rectangle>();
            foreach (Platform p in platforms)
            {
                level.Add(new Rectangle(p.x * scale + xoffset, p.y * scale + yoffset, p.width * scale, p.height * scale));
            }
            level.Add(new Rectangle(savedbox.X / 50 * scale + xoffset, savedbox.Y / 50 * scale + yoffset, (int)(savedbox.Width / 50.0 * scale), (int)(savedbox.Height / 50.0 * scale)));
        }
    }
}
