using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;

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
        Texture2D keyboardbutton;
        Vector4 charhitboxoffset;
        List<Rectangle> charframes;
        Character character;
        SpriteFont font;
        SpriteFont big;
        SpriteFont title;
        Texture2D platformpiece;
        Keys[] keys;
        List<Rectangle> keyrects;

        bool instartscreen;
        bool inlevelscreen;
        bool inoptionscreen;
        bool inlevel;
        bool insurescreen;

        bool wasinlevelscreen;
        bool wasinstartscreen;

        string titletext = "Platformer XD";

        Level currentLevel;
        List<Level> levels;
        List<Rectangle> levelboxes;
        bool canreset = true;
        float LavaY = 0f;
        float Lavaspeed = 0.5f;
        const int LavaStartY = 150;
        bool win = false;
        bool lose = false;

        SaveData save;

        bool gotbox = false;

        int r = 0;
        int g = 50;
        int b = 100;
        int dr = 1;
        int dg = 1;
        int db = 1;

        MouseState lastMs;
        MouseState ms;
        KeyboardState lastks;
        KeyboardState ks;

        bool debug = false;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 1080;
            graphics.PreferredBackBufferWidth = 1920;
            Window.Position = new Point(0, 0);
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

        string saveString;
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            if (File.Exists("saveFile.txt"))
            {
                saveString = File.ReadAllText("saveFile.txt");

                save = JsonConvert.DeserializeObject<SaveData>(saveString);
                if (save == null)
                {
                    save = new SaveData();
                }
            }
            else
            {
                File.Create("saveFile.txt");
            }

            
            currentLevel = new Level();
            levels = new List<Level>();
            levelboxes = new List<Rectangle>();
            keys = new Keys[] { Keys.A, Keys.D, Keys.Space, Keys.LeftShift, Keys.CapsLock, Keys.R, Keys.O, Keys.P, Keys.N };
            keyrects = new List<Rectangle>();
            spriteBatch = new SpriteBatch(GraphicsDevice);
            charspritesheet = Content.Load<Texture2D>("stick figure sprite sheet 50% crouching");
            charspritesheetbackward = Content.Load<Texture2D>("stick figure sprite sheet 50% backward crouching");
            charspritesheetcrouch = Content.Load<Texture2D>("stick figure sprite sheet 50% super crouching");
            charspritesheetbackwardcrouch = Content.Load<Texture2D>("stick figure sprite sheet 50% backward super crouching");
            platformpiece = Content.Load<Texture2D>("platform piece");
            keyboardbutton = Content.Load<Texture2D>("keyboard");
            charhitboxoffset = new Vector4(30, 0, 30, 0);


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


            font = Content.Load<SpriteFont>("font");
            big = Content.Load<SpriteFont>("big");
            title = Content.Load<SpriteFont>("title");

            instartscreen = true;
            inlevelscreen = false;
            inoptionscreen = false;
            inlevel = false;
            insurescreen = false;

            loadsave();
        }


        /// <summary>                     
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            //loop through Levels and save everything to save.Levels
            savegame();
        }
        
        protected override void Update(GameTime gameTime)
        {

            lastks = ks;
            ks = Keyboard.GetState();

            lastMs = ms;
            ms = Mouse.GetState();

            if (character.hitbox.Intersects(currentLevel.box)) { gotbox = true; }

            if (!win && !lose && inlevel)
            {
                character.Update(gameTime, currentLevel.platforms, keys);
                if (!debug)
                {
                    Lavaspeed = (float)(2.0f / (1 + 3f * (Math.Exp(-0.1f * save.Score))));
                    LavaY += Lavaspeed;
                }
            }

            if ((Keyboard.GetState().IsKeyDown(keys[8]) && character.whymode || (Keyboard.GetState().IsKeyDown(keys[5]) && canreset)) && !inoptionscreen)
            {
                reset();
                if (Keyboard.GetState().IsKeyDown(keys[5]) && !inoptionscreen)
                {
                    canreset = false;
                }
            }

            if (!Keyboard.GetState().IsKeyDown(keys[5]) && !inoptionscreen)
            {
                canreset = true;
            }

            base.Update(gameTime);
        }

        void savegame()
        {
            save.Levels.Clear();
            for (int i = 0; i < levels.Count; i++)
            {
                save.Levels.Add(new LevelData(levels[i].Seed, levels[i].win, levels[i].lose));
            }

            string saveString = JsonConvert.SerializeObject(save);
            File.WriteAllText("saveFile.txt", saveString);
        }

        void reset()
        {
            inlevelscreen = false;
            instartscreen = false;
            inoptionscreen = false;
            insurescreen = false;
            inlevel = true;

            LavaY = 0;
            win = false;
            lose = false;
            gotbox = false;
            currentLevel = new Level();
            currentLevel.LoadLevel(platformpiece, save.Score);
            levels.Add(currentLevel);
            font = Content.Load<SpriteFont>("Font");
            if (!character.whymode)
            {
                character = new Character(charspritesheet, charspritesheetbackward, charspritesheetcrouch, charspritesheetbackwardcrouch, new Vector2(100, GraphicsDevice.Viewport.Height - charframes[0].Height), Color.White, charframes, charhitboxoffset, 0); //new Vector4(30, 0, 30, 0)
            }
        }

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

        void loadsave()
        {
            levels.Clear();
            for (int l = 0; l < save.Levels.Count; l++)
            {
                Level temp = new Level();
                temp.LoadLevel(platformpiece, save.Score, save.Levels[l].Seed);
                temp.win = save.Levels[l].Win;
                temp.lose = save.Levels[l].Lose;
                levels.Add(temp);
            }
            currentLevel.LoadLevel(platformpiece, save.Score);
            levels.Add(currentLevel);
        }

        int p = 0;
        int q = 0;

        void startscreen()
        {
            wasinstartscreen = true;
            wasinlevelscreen = false;
            instartscreen = true;
            colorcycle();
            GraphicsDevice.Clear(new Color(r, g, b));
            IsMouseVisible = true;
            Rectangle loadbutton = new Rectangle(750, 500, 420, 150);
            Rectangle newbutton = new Rectangle(750, 670, 420, 150);
            Rectangle optionsbutton = new Rectangle(1785, 960, 100, 100);
            Rectangle exitbutton = new Rectangle(35, 960, 100, 100);
            spriteBatch.Draw(platformpiece, loadbutton, new Color(255 - r, 255 - g, 255 - b));
            spriteBatch.Draw(platformpiece, newbutton, new Color(255 - r, 255 - g, 255 - b));
            spriteBatch.Draw(keyboardbutton, optionsbutton, new Color(255 - r, 255 - g, 255 - b));
            spriteBatch.Draw(platformpiece, exitbutton, new Color(255 - r, 255 - g, 255 - b));
            spriteBatch.DrawString(big, "load", new Vector2(loadbutton.X + loadbutton.Width / 2 - big.MeasureString("load").X / 2, loadbutton.Y + loadbutton.Height / 2 - big.MeasureString("load").Y / 2), new Color(r, g, b));
            spriteBatch.DrawString(big, "new", new Vector2(newbutton.X + newbutton.Width / 2 - big.MeasureString("new").X / 2, newbutton.Y + newbutton.Height / 2 - big.MeasureString("new").Y / 2), new Color(r, g, b));
            spriteBatch.DrawString(big, "X", new Vector2(exitbutton.X + exitbutton.Width / 2 - big.MeasureString("X").X / 2, exitbutton.Y + exitbutton.Height / 2 - big.MeasureString("X").Y / 2), new Color(r, g, b));
            spriteBatch.DrawString(title, titletext, new Vector2(960 - title.MeasureString(titletext).X / 2, 250), new Color(255 - r, 255 - g, 255 - b));
            if (loadbutton.Contains(ms.Position) && ms.LeftButton == ButtonState.Pressed && ms.LeftButton != lastMs.LeftButton)
            {
                win = false;
                lose = false;
                instartscreen = false;
                inlevelscreen = false;
                inoptionscreen = false;
                insurescreen = false;
                inlevel = true;
                LavaY = 0;
                gotbox = false;
                character = new Character(charspritesheet, charspritesheetbackward, charspritesheetcrouch, charspritesheetbackwardcrouch, new Vector2(100, GraphicsDevice.Viewport.Height - charframes[0].Height), Color.White, charframes, charhitboxoffset, 0); //new Vector4(30, 0, 30, 0
                loadsave();
            }
            if (newbutton.Contains(ms.Position) && ms.LeftButton == ButtonState.Pressed && ms.LeftButton != lastMs.LeftButton)
            {
                instartscreen = false;
                inlevelscreen = false;
                inoptionscreen = false;
                insurescreen = true;
                inlevel = false;
            }
            if (optionsbutton.Contains(ms.Position) && ms.LeftButton == ButtonState.Pressed && ms.LeftButton != lastMs.LeftButton)
            {
                instartscreen = false;
                inlevelscreen = false;
                insurescreen = false;
                inlevel = false;
                inoptionscreen = true;
            }
            if (exitbutton.Contains(ms.Position) && ms.LeftButton == ButtonState.Pressed && ms.LeftButton != lastMs.LeftButton)
            {
                Exit();
            }
        }

        void surescreen()
        {
            GraphicsDevice.Clear(new Color(r, g, b));
            Rectangle yes = new Rectangle(660, 600, 200, 100);
            spriteBatch.Draw(platformpiece, yes, new Color(255 - r, 255 - g, 255 - b));
            spriteBatch.DrawString(big, "yes", new Vector2(yes.X + yes.Width / 2 - big.MeasureString("yes").X / 2, yes.Y + yes.Height / 2 - big.MeasureString("yes").Y / 2), new Color(r, g, b));
            Rectangle no = new Rectangle(1060, 600, 200, 100);
            spriteBatch.Draw(platformpiece, no, new Color(255 - r, 255 - g, 255 - b));
            spriteBatch.DrawString(big, "no", new Vector2(no.X + no.Width / 2 - big.MeasureString("no").X / 2, no.Y + no.Height / 2 - big.MeasureString("no").Y / 2), new Color(r, g, b));
            spriteBatch.DrawString(title, "Overwrite save data?", new Vector2(960 - title.MeasureString("Overwrite save data?").X / 2, 250), new Color(255 - r, 255 - g, 255 - b));

            if (yes.Contains(ms.Position) && ms.LeftButton == ButtonState.Pressed && ms.LeftButton != lastMs.LeftButton)
            {
                save.Score = 0;
                save.Levels.Clear();
                levels.Clear();
                instartscreen = false;
                inlevelscreen = false;
                inoptionscreen = false;
                insurescreen = false;
                inlevel = true;
                reset();
            }
            else if (no.Contains(ms.Position) && ms.LeftButton == ButtonState.Pressed && ms.LeftButton != lastMs.LeftButton)
            {
                instartscreen = true;
                inlevelscreen = false;
                inoptionscreen = false;
                insurescreen = false;
                inlevel = false;
            }
        }

        string controlsempty =
@"                         left - 
                        right - 
                        jump - 
                     crouch - 
           toggle crouch - 
                    restart - 
             whymode on - 
            whymode off - 
 whymode randomize - ";

        bool changingkey = false;
        bool keydup = false;
        int dupedkey = 10;
        int changekey = 0;

        void loadoldlevel(int levelnum)
        {
            inlevelscreen = false;
            inlevel = true;

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
            wasinlevelscreen = true;
            wasinstartscreen = false;
            inlevelscreen = true;
            if (!win && !lose)
            {
                GraphicsDevice.Clear(new Color(r, g, b));
            }
            Rectangle optionsbutton = new Rectangle(1785, 20, 100, 100);
            if (optionsbutton.Contains(ms.Position) && ms.LeftButton == ButtonState.Pressed && ms.LeftButton != lastMs.LeftButton || inoptionscreen)
            {
                inlevelscreen = false;
                inoptionscreen = true;
            }
            if (!inoptionscreen)
            {
                IsMouseVisible = true;
                spriteBatch.Draw(keyboardbutton, optionsbutton, new Color(255 - r, 255 - g, 255 - b));
                spriteBatch.Draw(platformpiece, new Rectangle(0, 180, 1920, 900), Color.Gray);
                spriteBatch.DrawString(big, save.Score.ToString(), new Vector2(15, 0), Color.White);
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
        }

        void won()
        {
            if (!inoptionscreen)
            {
                GraphicsDevice.Clear(Color.Green);
                spriteBatch.DrawString(big, $"You win! Press {keys[5]} to play again", new Vector2((GraphicsDevice.Viewport.Width - big.MeasureString($"You win! Press {keys[5]} to play again").X) / 2, 50), Color.White);
                if (!win)
                {
                    save.Score++;
                }
                win = true;
                lose = false;
                currentLevel.win = true;
                currentLevel.lose = false;
            }
            levelscreen();
        }

        void lost()
        {
            if (!inoptionscreen)
            {
                GraphicsDevice.Clear(Color.Firebrick);
                spriteBatch.DrawString(big, $"Ouchie ouch you got got by the lava. Press {keys[5]} to try again", new Vector2((GraphicsDevice.Viewport.Width - big.MeasureString($"Ouchie ouch you got got by the lava. Press {keys[5]} to try again").X) / 2, 50), Color.White);
                if (!lose && save.Score > 0)
                {
                    save.Score--;
                }
                lose = true;
                win = false;
                currentLevel.lose = true;
                currentLevel.win = false;
            }
            levelscreen();
        }

        void optionscreen()
        {
            inoptionscreen = true;
            colorcycle();
            GraphicsDevice.Clear(new Color(r, g, b));
            spriteBatch.DrawString(big, controlsempty, new Vector2(500, 250), new Color(255 - r, 255 - g, 255 - b));
            Rectangle back = new Rectangle(1785, 960, 110, 80);
            spriteBatch.Draw(platformpiece, back, new Color(255 - r, 255 - g, 255 - b));
            spriteBatch.DrawString(big, "back", back.Location.ToVector2(), new Color(r, g, b));
            keyrects.Clear();
            keydup = false;
            dupedkey = 10;
            for (int i = 0; i < 9; i++)
            {
                keyrects.Add(new Rectangle(500 + (int)big.MeasureString(controlsempty).X, 252 + ((int)big.MeasureString(controlsempty).Y / 9) * i, (int)big.MeasureString(keys[i].ToString()).X, (int)big.MeasureString(controlsempty).Y / 9 - 4));

                for (int j = 0; j < 9; j++)
                {
                    if (keys[i].ToString() == keys[j].ToString() && j != i)
                    {
                        keydup = true;
                        dupedkey = i;
                    }
                }
                if (dupedkey == i)
                {
                    spriteBatch.Draw(platformpiece, keyrects[i], Color.Red);
                }
                else
                {
                    spriteBatch.Draw(platformpiece, keyrects[i], new Color(255 - r, 255 - g, 255 - b));
                }
                spriteBatch.DrawString(big, keys[i].ToString(), new Vector2(500 + (int)big.MeasureString(controlsempty).X, 252 + ((int)big.MeasureString(controlsempty).Y / 9) * i), new Color(r, g, b));
                if (keyrects[i].Contains(ms.Position) && ms.LeftButton == ButtonState.Pressed)
                {
                    changekey = i;
                    changingkey = true;
                }
                if (changingkey)
                {
                    if (ks != lastks && ks.GetPressedKeys().Length > 0)
                    {
                        keys[changekey] = (ks.GetPressedKeys()[0]);
                        changingkey = false;
                    }
                }
            }
            if (back.Contains(ms.Position) && ms.LeftButton == ButtonState.Pressed && ms.LeftButton != lastMs.LeftButton && !keydup)
            {
                if (wasinstartscreen)
                {
                    instartscreen = true;
                    inoptionscreen = false;
                }
                else if (wasinlevelscreen)
                {
                    inlevelscreen = true;
                    inoptionscreen = false;
                }
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            if (Keyboard.GetState().IsKeyDown(Keys.Escape) && ks.IsKeyDown(Keys.Escape) != lastks.IsKeyDown(Keys.Escape))
            {
                if (instartscreen)
                {
                    Exit();
                }
                else if (insurescreen)
                {
                    inlevelscreen = false;
                    inoptionscreen = false;
                    inlevel = false;
                    instartscreen = true;
                    insurescreen = false;
                    startscreen();
                }
                else if (inlevelscreen)
                {
                    inlevelscreen = false;
                    inoptionscreen = false;
                    inlevel = false;
                    instartscreen = true;
                    insurescreen = false;
                    savegame();
                }
                else if (inlevel)
                {
                    instartscreen = false;
                    inoptionscreen = false;
                    inlevel = false;
                    inlevelscreen = true;
                    insurescreen = false;
                    win = false;
                    lose = false;
                    currentLevel.win = false;
                    currentLevel.lose = false;
                    savegame();
                }
            }


            if (!inlevel && !instartscreen && !inoptionscreen && !insurescreen && inlevelscreen)
            {
                levelscreen();
            }
            else if (!inlevel && !instartscreen && !inlevelscreen && !insurescreen && inoptionscreen)
            {
                optionscreen();
            }
            else if (!inlevel && !inlevelscreen && !inoptionscreen && !insurescreen && instartscreen)
            {
                startscreen();
            }
            else if(!inlevel && !inlevelscreen && !inoptionscreen && !instartscreen && insurescreen)
            {
                surescreen();
            }


            else if ((((character.hitbox.Y + character.hitbox.Height - 15 <= currentLevel.highestplatformY * 50) && character.onAPlatform) || win) && !debug && gotbox)
            {
                won();
            }
            else if (((character.hitbox.Y + character.hitbox.Height >= GraphicsDevice.Viewport.Height - LavaY + LavaStartY + 10) || lose) && !debug)
            {
                lost();
            }
            else
            {
                colorcycle();
                GraphicsDevice.Clear(new Color(r, g, b));
                spriteBatch.DrawString(big, save.Score.ToString(), new Vector2(15, 0), Color.White);
                IsMouseVisible = false;
            }


            int c = 0;
            if (!win && !lose && inlevel)
            {
                inlevel = true;
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
                spriteBatch.Draw(platformpiece, new Rectangle(0, GraphicsDevice.Viewport.Height + LavaStartY - (int)LavaY, GraphicsDevice.Viewport.Width, (int)LavaY), null, Color.OrangeRed);
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
                spriteBatch.DrawString(font, $"right wall: {character.hitrightwall}, left wall: {character.hitleftwall}, left of platform: {character.hitleftplatform}, right of platform: {character.hitrightplatform}, top: {character.onPlatform}", new Vector2(100, 100), Color.White);
                spriteBatch.DrawString(font, Lavaspeed.ToString(), new Vector2(200, 200), Color.Black);
                spriteBatch.Draw(platformpiece, character.hitbox, null, Color.OrangeRed);
                spriteBatch.Draw(platformpiece, new Rectangle(0, GraphicsDevice.Viewport.Height + character.groundY - 3, GraphicsDevice.Viewport.Width, 3), null, Color.Black);
            }

            // TODO: Add your drawing code here
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
