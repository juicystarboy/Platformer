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
        List<Rectangle> charframes;
        Character character;
        SpriteFont font;
        List<Platform> platform;
        Texture2D platformpiece;
        Random rand;
        int randwidth;
        int randx;
        int randy;
        int endx;
        int amountofplatforms;
        bool platformonlower;
        bool canreset = true;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 1080;
            graphics.PreferredBackBufferWidth = 1920;
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
            platformonlower = false;
            spriteBatch = new SpriteBatch(GraphicsDevice);
            rand = new Random();
            charspritesheet = Content.Load<Texture2D>("stick figure sprite sheet 50%");
            charspritesheetbackward = Content.Load<Texture2D>("stick figure sprite sheet 50% backward");
            charspritesheetcrouch = Content.Load<Texture2D>("stick figure sprite sheet 50% crouching");
            charspritesheetbackwardcrouch = Content.Load<Texture2D>("stick figure sprite sheet 50% backward crouching");
            platformpiece = Content.Load<Texture2D>("platform piece");
            {
                charframes = new List<Rectangle>();
                //for(int i = 0; i < 70; i++)
                {
                    for(int r = 0; r < 8; r++)
                    {
                        for(int c = 0; c < 9; c++)
                        {
                            charframes.Add(new Rectangle(c * charspritesheet.Width / 9, r * charspritesheet.Height / 8, charspritesheet.Width / 9, charspritesheet.Height / 8));
                        }
                    }
                }
            }
            platform = new List<Platform>();
            amountofplatforms = rand.Next(10, 20);
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
                        }*/
                        if(p.y == 17)
                        {
                            platformonlower = true;
                        }
                        if (new Rectangle(randx * 50, randy * 50, randwidth * 50,  1).Intersects(p.hitbox))
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
            font = Content.Load<SpriteFont>("Font");
            character = new Character(charspritesheet, charspritesheetbackward, charspritesheetcrouch, charspritesheetbackwardcrouch, new Vector2(100, GraphicsDevice.Viewport.Height - charframes[0].Height), Color.White, charframes, new Vector4(30, 5, 30, 0), 0); //new Vector4(30, 0, 30, 0)
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
            //character.speedX = 0;
            character.Update(gameTime, platform);
            if (Keyboard.GetState().IsKeyDown(Keys.N) && character.whymode || (Keyboard.GetState().IsKeyDown(Keys.R) && canreset))
            {
                platform.Clear();
                amountofplatforms = rand.Next(10, 20);
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
                            if (p.y == 17)
                            {
                                platformonlower = true;
                            }
                            if (new Rectangle(randx * 50, randy * 50, randwidth * 50, 1).Intersects(p.hitbox))
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
                if(Keyboard.GetState().IsKeyDown(Keys.R))
                {
                    canreset = false;
                }
            }
            // TODO: Add your update logic here
            if (!Keyboard.GetState().IsKeyDown(Keys.N))
            {
                canreset = true;
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            character.draw(spriteBatch, character.speedX, character.crouching);
            spriteBatch.DrawString(font, character.speedX.ToString(), new Vector2(0, 0), Color.White);
            spriteBatch.DrawString(font, (amountofplatforms+1).ToString(), new Vector2(0, 12), Color.White);
            int c = 0;
            foreach (Platform p in platform)
            {
                p.draw(spriteBatch);
                //spriteBatch.DrawString(font, $"{c++}: {p.x * 50},{p.y * 50} {p.width * 50}x{p.height * 50}", new Vector2(p.x * 50, p.y * 50), Color.White);
            }
            spriteBatch.Draw(platformpiece, new Rectangle(0, GraphicsDevice.Viewport.Height + character.groundY - 3, GraphicsDevice.Viewport.Width, 3), null, Color.Red);

            // TODO: Add your drawing code here
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
