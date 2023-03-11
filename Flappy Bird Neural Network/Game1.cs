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
        private Sprite backgroundLeft;
        private Sprite backgroundRight;
        private MovingSprite scrollingBottomLeft;
        private MovingSprite scrollingBottomMiddle;
        private MovingSprite scrollingBottomRight;
        private Rectangle bottomOfScreen;
        private Bird yellowBird;
        private Bird blueBird;
        private Bird redBird;
        private Bird selectedBird;

        private bool gameStarted;
        private bool previousStateDown;
        private bool gameOver;
        private Vector2 scrollingSpeed = new Vector2(-3, 0);

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferWidth = 480;
            _graphics.PreferredBackBufferHeight = 640;
            _graphics.ApplyChanges();

            gameStarted = false;
            previousStateDown = false;
            gameOver = false;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //We load the spritesheet
            Texture2D spriteSheet = Content.Load<Texture2D>("Flappy Bird Sprite Sheet");

            Rectangle?[] yellowSourceRectangle = new Rectangle?[3];

            const float scale = 2 * 1.75f;
            const int birdWidth = (int)(17 * scale);
            const int birdHeight = (int)(12 * scale);
            //Yellow
            yellowSourceRectangle[0] = new Rectangle((int)(3 * scale), (int)(491 * scale), birdWidth, birdHeight);
            yellowSourceRectangle[1] = new Rectangle((int)(31 * scale), (int)(491 * scale), birdWidth, birdHeight);
            yellowSourceRectangle[2] = new Rectangle((int)(59 * scale), (int)(491 * scale), birdWidth, birdHeight);

            Rectangle?[] blueSourceRectangle = new Rectangle?[3];
            //Blue
            blueSourceRectangle[0] = new Rectangle((int)(87 * scale), (int)(491 * scale), birdWidth, birdHeight);
            blueSourceRectangle[1] = new Rectangle((int)(115 * scale), (int)(329 * scale), birdWidth, birdHeight);
            blueSourceRectangle[2] = new Rectangle((int)(115 * scale), (int)(355 * scale), birdWidth, birdHeight);

            Rectangle?[] redSourceRectangle = new Rectangle?[3];
            //Red
            redSourceRectangle[0] = new Rectangle((int)(115 * scale), (int)(381 * scale), birdWidth, birdHeight);
            redSourceRectangle[1] = new Rectangle((int)(115 * scale), (int)(407 * scale), birdWidth, birdHeight);
            redSourceRectangle[2] = new Rectangle((int)(115 * scale), (int)(433 * scale), birdWidth, birdHeight);

            yellowBird = new Bird(spriteSheet, new Vector2((GraphicsDevice.Viewport.Width - birdWidth) / 2, (GraphicsDevice.Viewport.Height - birdHeight) / 2), yellowSourceRectangle, 0, Vector2.One, new TimeSpan());
            selectedBird = yellowBird;

            backgroundLeft = new Sprite(spriteSheet, Vector2.Zero, new Rectangle(0, 0, 501, 896), 0f, new Vector2(0.75f, 0.75f));
            backgroundRight = new Sprite(spriteSheet, new Vector2(375.25f, 0), new Rectangle(0, 0, 501, 896), 0f, new Vector2(0.75f, 0.75f));

            int yPosition = 525;
            scrollingBottomLeft = new MovingSprite(spriteSheet, new Vector2(0, yPosition), new Rectangle(1022, 0, 588, 196), new Vector2(0.5f, 0.75f), scrollingSpeed);
            scrollingBottomMiddle = new MovingSprite(spriteSheet, new Vector2(294, yPosition), new Rectangle(1022, 0, 588, 196), new Vector2(0.5f, 0.75f), scrollingSpeed);
            scrollingBottomRight = new MovingSprite(spriteSheet, new Vector2(588, yPosition), new Rectangle(1022, 0, 588, 196), new Vector2(0.5f, 0.75f), scrollingSpeed);

            bottomOfScreen = new Rectangle(0, yPosition, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height - yPosition);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);

            if (selectedBird.Collide(bottomOfScreen))
            {
                gameOver = true;
            }

            if(gameOver)
            {
                gameStarted = false;
                GameOver();
            }
            else
            {
                GameRun();
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            //This is for the background
            backgroundLeft.Draw(_spriteBatch);
            backgroundRight.Draw(_spriteBatch);

            scrollingBottomLeft.Draw(_spriteBatch);
            scrollingBottomMiddle.Draw(_spriteBatch);
            scrollingBottomRight.Draw(_spriteBatch);

            selectedBird.Animate(_spriteBatch, gameStarted, gameOver, gameTime);

            _spriteBatch.End();
            base.Draw(gameTime);
        }

        public void GameOver()
        {

        }
        public void GameRun()
        {
            if (Keyboard.GetState().IsKeyUp(Keys.Space))
            {
                previousStateDown = false;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Space) && !previousStateDown)
            {
                previousStateDown = true;
                gameStarted = selectedBird.Jump();
            }

            if (scrollingBottomLeft.hitbox.Right < 0)
            {
                scrollingBottomLeft.SetPosition(new Vector2(scrollingBottomRight.hitbox.Right, 525));
            }
            if (scrollingBottomMiddle.hitbox.Right < 0)
            {
                scrollingBottomMiddle.SetPosition(new Vector2(scrollingBottomLeft.hitbox.Right, 525));
            }
            if (scrollingBottomRight.hitbox.Right < 0)
            {
                scrollingBottomRight.SetPosition(new Vector2(scrollingBottomMiddle.hitbox.Right, 525));
            }

            scrollingBottomLeft.Update();
            scrollingBottomMiddle.Update();
            scrollingBottomRight.Update();
        }
    }
}