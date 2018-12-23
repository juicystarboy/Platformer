using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Platformer
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D charspritesheet;
        Texture2D charspritesheetbackward;
        Texture2D charspritesheetcrouch;
        Texture2D charspritesheetbackwardcrouch;
        Vector4 charhitboxoffset;
        List<Rectangle> charframes;
        Character character;
        SpriteFont font;
        SpriteFont big;
        SpriteFont title;
        Texture2D platformpiece;

        string controls = " move - A+D/L+R Arrows \n jump - space/W/Up \n restart - R \n crouch - L Shift/S/Down \n toggle crouch - Caps \n whymode on - O \n whymode off - P \n whymode randomize - N";
        string titletext = "Platformer XD";

        Level currentLevel;
        List<Level> levels;
        List<Rectangle> levelboxes;
        bool canreset = true;
        float LavaY = 0f;
        float Lavaspeed = 0.5f;
        bool levelstart = false;
        bool win = false;
        bool lose = false;
        int score = 0;

        bool gotbox = false;

        int r = 0;
        int g = 50;
        int b = 100;
        int dr = 1;
        int dg = 1;
        int db = 1;
        int dc = 1;

        MouseState lastMs;
        MouseState ms;

        bool debug = false;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 1080;
            graphics.PreferredBackBufferWidth = 1920;
            Window.Position = new Point(0, 0);
            //graphics.IsFullScreen = true;
            Window.IsBorderless = true;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            currentLevel = new Level();
            levels = new List<Level>();
            levelboxes = new List<Rectangle>();
            spriteBatch = new SpriteBatch(GraphicsDevice);
            charspritesheet = Content.Load<Texture2D>("stick figure sprite sheet 50% crouching");
            charspritesheetbackward = Content.Load<Texture2D>("stick figure sprite sheet 50% backward crouching");
            charspritesheetcrouch = Content.Load<Texture2D>("stick figure sprite sheet 50% super crouching");
            charspritesheetbackwardcrouch = Content.Load<Texture2D>("stick figure sprite sheet 50% backward super crouching");
            platformpiece = Content.Load<Texture2D>("platform piece");
            charhitboxoffset = new Vector4(30, 0, 30, -2);


            {
                charframes = new List<Rectangle>();
                //for(int i = 0; i < 70; i++)
                {
                    for (int r = 0; r < 8; r++)
                    {
                        for (int c = 0; c < 9; c++)
                        {
                            charframes.Add(new Rectangle(c * charspritesheet.Width / 9, r * charspritesheet.Height / 8, charspritesheet.Width / 9, charspritesheet.Height / 8));
                        }
                    }
                }
            }
            character = new Character(charspritesheet, charspritesheetbackward, charspritesheetcrouch, charspritesheetbackwardcrouch, new Vector2(100, GraphicsDevice.Viewport.Height - charframes[0].Height), Color.White, charframes, charhitboxoffset, 0); //new Vector4(30, 0, 30, 0)

            currentLevel.LoadLevel(platformpiece, score);
            levels.Add(currentLevel);


            #region old stuff
            /*
            for (int i = 0; i <= amountofplatforms; i++)
            {
                bool valid = false;

                while (!valid)
                {
                    valid = true;
                    randwidth = rand.Next(1, 10);
                    randx = rand.Next(0, 30);
                    randy = 17; //rand.Next(1, 10) * 1;
                    endx = randx + randwidth;
                    platformonlower = false;
                    foreach (Platform p in platform)
                    {
                        /*if (randx <= p.endx + 1 && randx >= p.x - 1 && randy == p.y)
                        {
                            randwidth = rand.Next(1, 10);
                            randx = rand.Next(1, 30);
                            endx = randx + randwidth;
                            valid = false;
                        }
                        else if (endx > p.x - 1 && endx < p.endx + 1 && randy == p.y)
                        {
                            randwidth = rand.Next(1, 10);
                            randx = rand.Next(1, 30);
                            endx = randx + randwidth;
                            valid = false;
                        }/
                        if (p.y == 17)
                        {
                            platformonlower = true;
                        }
                        if (new Rectangle((randx - 1) * 50, randy * 50, (randwidth + 2) * 50, 1).Intersects(p.hitbox))
                        {
                            valid = false;
                            randwidth = rand.Next(1, 10);
                            randx = rand.Next(1, 30);
                            randy = rand.Next(3, 17);
                            endx = randx + randwidth;
                        }
                        else
                        {
                            valid = true;
                        }
                    }
                    if (!platformonlower)
                    {
                        randy = 17;
                    }
                }
                platform.Add(new Platform(randwidth, 1, randx, randy, endx, platformpiece));
            }
*/
            #endregion




            font = Content.Load<SpriteFont>("font");
            big = Content.Load<SpriteFont>("big");
            title = Content.Load<SpriteFont>("title");
            // TODO: use this.Content to load your game content here
        }



        /// <summary>                     
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            lastMs = ms;
            ms = Mouse.GetState();

            //character.speedX = 0;

            if (character.hitbox.Intersects(currentLevel.box)) { gotbox = true; }

            if (!win && !lose && levelstart)
            {
                character.Update(gameTime, currentLevel.platforms);
                if (!debug)
                {
                    Lavaspeed = (float)(2.0f / (1 + 3f * (Math.Exp(-0.1f * score)))); //(2.0f - 2.5f / (1.5f * (score + 1.0f)));
                    LavaY += Lavaspeed;
                }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.N) && character.whymode || (Keyboard.GetState().IsKeyDown(Keys.R) && canreset))
            {
                LavaY = 0;
                win = false;
                lose = false;
                gotbox = false;
                currentLevel = new Level();
                currentLevel.LoadLevel(platformpiece, score);
                levels.Add(currentLevel);
                font = Content.Load<SpriteFont>("Font");
                if (!character.whymode)
                {
                    character = new Character(charspritesheet, charspritesheetbackward, charspritesheetcrouch, charspritesheetbackwardcrouch, new Vector2(100, GraphicsDevice.Viewport.Height - charframes[0].Height), Color.White, charframes, charhitboxoffset, 0); //new Vector4(30, 0, 30, 0)
                }
                if (Keyboard.GetState().IsKeyDown(Keys.R))
                {
                    canreset = false;
                }
            }
            // TODO: Add your update logic here
            if (!Keyboard.GetState().IsKeyDown(Keys.R))
            {
                canreset = true;
            }
            base.Update(gameTime);
        }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>

        void colorcycle()
        {
            if (r >= 100)
            {
                dr = -Math.Abs(dr);
            }
            if (r <= 0)
            {
                dr = Math.Abs(dr);
            }
            if (g >= 100)
            {
                dg = -Math.Abs(dg);
            }
            if (g <= 0)
            {
                dg = Math.Abs(dg);
            }
            if (b >= 100)
            {
                db = -Math.Abs(db);
            }
            if (b <= 0)
            {
                db = Math.Abs(db);
            }
            r += dr;
            g += dg;
            b += db;
        }

        int p = 0;
        int q = 0;

        void startscreen()
        {
            colorcycle();
            GraphicsDevice.Clear(new Color(r, g, b));
            IsMouseVisible = true;
            Rectangle startbutton = new Rectangle(750, 650, 420, 150);
            spriteBatch.Draw(platformpiece, startbutton, new Color(255 - r, 255 - g, 255 - b));
            spriteBatch.DrawString(big, "start", new Vector2(startbutton.X + startbutton.Width / 2 - big.MeasureString("start").X / 2, startbutton.Y + startbutton.Height / 2 - big.MeasureString("start").Y / 2), new Color(r, g, b));
            spriteBatch.DrawString(title, titletext, new Vector2(960 - title.MeasureString(titletext).X / 2, 250), new Color(255 - r, 255 - g, 255 - b));
            spriteBatch.DrawString(big, controls, new Vector2(20, 540 - big.MeasureString(controls).Y / 2), new Color(255 - r, 255 - g, 255 - b));
            if (startbutton.Contains(ms.Position) && ms.LeftButton == ButtonState.Pressed)
            {
                levelstart = true;
            }
        }

        void loadoldlevel(int levelnum)
        {
            LavaY = 0;
            win = false;
            lose = false;
            gotbox = false;
            currentLevel = levels[levelnum];
            //levels.Add(currentLevel);
            font = Content.Load<SpriteFont>("Font");
            if (!character.whymode)
            {
                character = new Character(charspritesheet, charspritesheetbackward, charspritesheetcrouch, charspritesheetbackwardcrouch, new Vector2(100, GraphicsDevice.Viewport.Height - charframes[0].Height), Color.White, charframes, charhitboxoffset, 0); //new Vector4(30, 0, 30, 0)
            }
        }

        void levelscreen()
        {
            IsMouseVisible = true;
            spriteBatch.Draw(platformpiece, new Rectangle(0, 180, 1920, 900), Color.Gray);
            spriteBatch.DrawString(big, score.ToString(), new Vector2(15, 0), Color.White);
            if (levels.Count > 54)
            {
                Rectangle next = new Rectangle(1650, 1000, (int)big.MeasureString("next page").X, (int)big.MeasureString("next page").Y);
                Rectangle previous = new Rectangle(50, 1000, (int)big.MeasureString("previous page").X, (int)big.MeasureString("previous page").Y);
                if (p < levels.Count / 54)
                {
                    spriteBatch.DrawString(big, "next page", new Vector2(1650, 1000), Color.Black);
                }
                else
                {
                    spriteBatch.DrawString(big, "next page", new Vector2(1650, 1000), Color.Gray);
                }
                if (p > 0)
                {
                    spriteBatch.DrawString(big, "previous page", new Vector2(50, 1000), Color.Black);
                }
                else
                {
                    spriteBatch.DrawString(big, "previous page", new Vector2(50, 1000), Color.Gray);
                }


                spriteBatch.DrawString(big, $"{(p + 1).ToString()}/{(levels.Count / 54 + 1).ToString()}", new Vector2(920 - (int)(big.MeasureString($"{(p + 1).ToString()}/{(levels.Count / 54 + 1).ToString()}").X / 2.0), 1000), Color.Black);

                if (next.Contains(ms.Position) && ms.LeftButton == ButtonState.Pressed && lastMs.LeftButton != ButtonState.Pressed)
                {
                    if (p < levels.Count / 54)
                    {
                        q = p + 1;
                        levelboxes.Clear();
                    }
                    p = q;
                }
                if (previous.Contains(ms.Position) && ms.LeftButton == ButtonState.Pressed && lastMs.LeftButton != ButtonState.Pressed)
                {
                    if (p > 0)
                    {
                        q = p - 1;
                        levelboxes.Clear();
                    }
                    p = q;
                }
            }

            int k = 0;
            int l = 0;
            spriteBatch.DrawString(font, controls, new Vector2(GraphicsDevice.Viewport.Width - font.MeasureString(controls).X - 20, 20), Color.White);
            levelboxes.Clear();
            for (int j = 0 + 54 * p; j < levels.Count && j < 54 + 54 * p; j++)
            {
                levelboxes.Add(new Rectangle(19 + 211 * k, 200 + 127 * l, 192, 108));
                if (levels[j].win)
                {
                    spriteBatch.Draw(platformpiece, levelboxes[levelboxes.Count - 1], Color.Green);
                }
                else if (levels[j].lose)
                {
                    spriteBatch.Draw(platformpiece, levelboxes[levelboxes.Count - 1], Color.Firebrick);
                }
                else
                {
                    spriteBatch.Draw(platformpiece, levelboxes[levelboxes.Count - 1], Color.Yellow);
                }

                levels[j].Createlist(19 + 211 * k, 200 + 127 * l, 5);
                for (int w = 0; w < levels[j].level.Count; w++)
                {
                    spriteBatch.Draw(platformpiece, new Rectangle(levels[j].level[w].X, levels[j].level[w].Y, levels[j].level[w].Width, levels[j].level[w].Height), Color.Black);
                }
                k++;
                if (k >= 9)
                {
                    k = 0;
                    l++;
                }
            }

            for (int r = 0; r < levelboxes.Count; r++)
            {
                if (levelboxes[r].Contains(ms.Position) && ms.LeftButton == ButtonState.Pressed && lastMs.LeftButton != ButtonState.Pressed)
                {
                    loadoldlevel(r + 54 * p);
                    break;
                }
            }
        }

        void won()
        {
            GraphicsDevice.Clear(Color.Green);
            spriteBatch.DrawString(big, "You win! Press R to play again", new Vector2((GraphicsDevice.Viewport.Width - big.MeasureString("You win! Press R to play again").X) / 2, 50), Color.White);
            if (!win)
            {
                score++;
            }
            win = true;
            currentLevel.win = true;

            levelscreen();
        }

        void lost()
        {
            GraphicsDevice.Clear(Color.Firebrick);
            spriteBatch.DrawString(big, "Ouchie ouch you got got by the lava. Press R to try again", new Vector2((GraphicsDevice.Viewport.Width - big.MeasureString("Ouchie ouch you got got by the lava. Press R to try again").X) / 2, 50), Color.White);
            if (!lose && score > 0)
            {
                score--;
            }
            lose = true;
            currentLevel.lose = true;

            levelscreen();
        }


        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            if (!levelstart)
            {
                startscreen();
            }
            else if ((((character.hitbox.Y + character.hitbox.Height - 15 <= currentLevel.highestplatformY * 50) && character.onAPlatform) || win) && !debug && gotbox)
            {
                won();
            }
            else if (((character.hitbox.Y + character.hitbox.Height >= GraphicsDevice.Viewport.Height - LavaY + 110) || lose) && !debug)
            {
                lost();
            }
            else
            {
                colorcycle();
                GraphicsDevice.Clear(new Color(r, g, b));
                spriteBatch.DrawString(big, score.ToString(), new Vector2(15, 0), Color.White);
                IsMouseVisible = false;
            }


            int c = 0;
            if (!win && !lose && levelstart)
            {
                foreach (Platform p in currentLevel.platforms)
                {
                    character.draw(spriteBatch, character.speedX, character.crouching);
                    p.draw(spriteBatch);
                    spriteBatch.Draw(platformpiece, p.top, null, Color.OrangeRed);
                    spriteBatch.Draw(platformpiece, p.bottom, null, Color.OrangeRed);
                    if (debug) { spriteBatch.Draw(platformpiece, p.hitbox, null, Color.Red * 0.40f); }
                    //spriteBatch.Draw(platformpiece, p.left, null, Color.OrangeRed);
                    //spriteBatch.Draw(platformpiece, p.right, null, Color.OrangeRed);
                    if (debug) spriteBatch.DrawString(font, $"{c++}: {p.x},{p.y} {p.width}x{p.height}", new Vector2(p.x * 50, p.y * 50), Color.White);

                }
                spriteBatch.Draw(platformpiece, new Rectangle(0, GraphicsDevice.Viewport.Height + character.groundY - 3, GraphicsDevice.Viewport.Width, 3), null, Color.Black);
                spriteBatch.Draw(platformpiece, new Rectangle(0, GraphicsDevice.Viewport.Height + 100 - (int)LavaY, GraphicsDevice.Viewport.Width, (int)LavaY), null, Color.OrangeRed);
                if (!gotbox)
                {
                    spriteBatch.Draw(platformpiece, currentLevel.box, null, new Color(255 - r, 255 - g, 255 - b));
                }
            }


            if (debug)
            {
                foreach (Rectangle r in currentLevel.phits)
                {
                    spriteBatch.Draw(platformpiece, r, Color.Red * 0.40f);
                }
                if (debug) spriteBatch.DrawString(font, $"right wall: {character.hitrightwall}, left wall: {character.hitleftwall}, left of platform: {character.hitleftplatform}, right of platform: {character.hitrightplatform}, top: {character.onPlatform}", new Vector2(100, 100), Color.White);
                spriteBatch.DrawString(font, Lavaspeed.ToString(), new Vector2(200, 200), Color.Black);
                spriteBatch.Draw(platformpiece, character.hitbox, null, Color.OrangeRed);
            }

            // TODO: Add your drawing code here
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
