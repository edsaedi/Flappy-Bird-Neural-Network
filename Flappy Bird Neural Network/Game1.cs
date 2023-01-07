using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Flappy_Bird_Neural_Network
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //We load the spritesheet
            Texture2D spriteSheet = Content.Load<Texture2D>("Flappy Bird Sprite Sheet");

            Rectangle?[] yellowSourceRectangle = new Rectangle?[3];
            //Yellow
            yellowSourceRectangle[0] = new Rectangle(3, 491, 17, 12);
            yellowSourceRectangle[1] = new Rectangle(31, 491, 17, 12);
            yellowSourceRectangle[2] = new Rectangle(59, 491, 17, 12);

            Rectangle?[] blueSourceRectangle = new Rectangle?[3];
            //Blue
            blueSourceRectangle[0] = new Rectangle(87, 491, 17, 12);
            blueSourceRectangle[1] = new Rectangle(115, 329, 17, 12);
            blueSourceRectangle[2] = new Rectangle(115, 355, 17, 12);

            Rectangle?[] redSourceRectangle = new Rectangle?[3];
            //Red
            redSourceRectangle[0] = new Rectangle(115, 381, 17, 12);
            redSourceRectangle[1] = new Rectangle(115, 407, 17, 12);
            redSourceRectangle[2] = new Rectangle(115, 433, 17, 12);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}