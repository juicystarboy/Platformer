using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
        List<Rectangle> charframes;
        Character character;
        SpriteFont font;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 200;
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
            spriteBatch = new SpriteBatch(GraphicsDevice);
            charspritesheet = Content.Load<Texture2D>("stick figure sprite sheet 50%");
            charspritesheetbackward = Content.Load<Texture2D>("stick figure sprite sheet 50% backward");
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
            font = Content.Load<SpriteFont>("Font");
            character = new Character(charspritesheet, charspritesheetbackward, new Vector2(0, GraphicsDevice.Viewport.Height - charframes[0].Height), Color.White, charframes, new Vector4(0, 0, 0, 0), 0);
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
            character.Update(gameTime);
            // TODO: Add your update logic here

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
            character.draw(spriteBatch, character.speedX);
            spriteBatch.DrawString(font, character.speedX.ToString(), new Vector2(0, 0), Color.White);
            // TODO: Add your drawing code here
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
