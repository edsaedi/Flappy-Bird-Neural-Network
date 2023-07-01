using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Threading;

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

        private (MovingSprite topPipe, MovingSprite bottomPipe)[] pipes;

        //Labels
        private Sprite gameOverLabel;
        private List<SpriteLabel> scoreLabel;
        private Rectangle[] scoreDigits;
        private SpriteLabel[] scoreSprites;

        private Texture2D spriteSheet;

        private bool gameStarted;
        private bool previousStateDown;
        private bool gameOver;
        private Vector2 scrollingSpeed = new Vector2(-3, 0);
        private int floorYValue = 525;
        private int bufferHeight = 110;

        private int score;
        private bool passedPipe1 = false;
        private bool passedPipe2 = false;

        private Button restartButton;


        //Testing spriteFont
        private SpriteFont spriteFont;


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
            spriteSheet = Content.Load<Texture2D>("Flappy Bird Sprite Sheet");

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

            scrollingBottomLeft = new MovingSprite(spriteSheet, new Vector2(0, floorYValue), new Rectangle(1022, 0, 588, 196), new Vector2(0.5f, 0.75f), scrollingSpeed);
            scrollingBottomMiddle = new MovingSprite(spriteSheet, new Vector2(294, floorYValue), new Rectangle(1022, 0, 588, 196), new Vector2(0.5f, 0.75f), scrollingSpeed);
            scrollingBottomRight = new MovingSprite(spriteSheet, new Vector2(588, floorYValue), new Rectangle(1022, 0, 588, 196), new Vector2(0.5f, 0.75f), scrollingSpeed);

            bottomOfScreen = new Rectangle(0, floorYValue, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height - floorYValue);

            pipes = new (MovingSprite, MovingSprite)[2];

            restartButton = new Button(spriteSheet, new Vector2((GraphicsDevice.Viewport.Width - 182) / 2, GraphicsDevice.Viewport.Height / 2 - 45), new Rectangle(1239, 413, 182, 101), Vector2.One);

            RestartGame();

            //Label creation
            gameOverLabel = new Sprite(spriteSheet, new Vector2((GraphicsDevice.Viewport.Width - 336) / 2, GraphicsDevice.Viewport.Height / 2 - 125), new Rectangle(1382, 206, 336, 74), 0, Vector2.One);

            spriteFont = Content.Load<SpriteFont>("Sprite Font");

            scoreDigits = new Rectangle[] {
                new Rectangle(1736, 210, 42, 63),
                new Rectangle(476, 1592, 28, 63),
                new Rectangle(1022, 560, 42, 63),
                new Rectangle(1071, 560, 42, 63),
                new Rectangle(1120, 560, 42, 63),
                new Rectangle(1169, 560, 42, 63),
                new Rectangle(1022, 644, 42, 63),
                new Rectangle(1071, 644, 42, 63),
                new Rectangle(1120, 644, 42, 63),
                new Rectangle(1169, 644, 42, 63)
            };

            scoreSprites = new SpriteLabel[scoreDigits.Length];

            scoreLabel = new List<SpriteLabel>();
        }


        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);

            //Game Run logic
            if (!gameOver)
            {
                GameRun(gameStarted);
                gameOver = birdCollided();
            }
            //Gameover Logic
            else
            {
                gameStarted = false;
                GameOver();
            }
        }

        public void GameRun(bool isGameStarted)
        {
            //Scrolling logic
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

            //Jumping Logic    
            if (Keyboard.GetState().IsKeyUp(Keys.Space))
            {
                previousStateDown = false;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Space) && !previousStateDown)
            {
                previousStateDown = true;
                gameStarted = selectedBird.Jump();
            }

            //Clicking Logic
            if (Mouse.GetState().LeftButton == ButtonState.Pressed && !previousStateDown)
            {
                gameStarted = selectedBird.Jump();
            }
            previousStateDown = Mouse.GetState().LeftButton != ButtonState.Released;

            if (isGameStarted)
            {
                //Pipe updating logic
                for (var i = 0; i < pipes.Length; i++)
                {
                    if (pipes[i].topPipe.hitbox.Right < 0)
                    {
                        pipes[i].topPipe.SetXPosition(GraphicsDevice.Viewport.Width);
                        pipes[i].bottomPipe.SetXPosition(GraphicsDevice.Viewport.Width);
                        if (i == 0)
                        {
                            passedPipe1 = false;
                        }
                        else
                        {
                            passedPipe2 = false;
                        }
                    }
                    pipes[i].topPipe.Update();
                    pipes[i].bottomPipe.Update();
                }
            }

            if (!passedPipe1 && (pipes[0].topPipe.hitbox.Left < selectedBird.hitbox.Left))
            {
                score++;
                passedPipe1 = true;
            }
            if (!passedPipe2 && (pipes[1].topPipe.hitbox.Left < selectedBird.hitbox.Left))
            {
                score++;
                passedPipe2 = true;
            }
        }

        //Checks to see if the bird dies
        public bool birdCollided()
        {
            //Ground collision
            if (selectedBird.Collide(bottomOfScreen))
            {
                return true;
            }

            //Pipe Collision
            for (var i = 0; i < pipes.Length; i++)
            {
                if (selectedBird.Collide(pipes[i].topPipe.hitbox))
                {
                    return true;
                }
                if (selectedBird.Collide(pipes[i].bottomPipe.hitbox))
                {
                    return true;
                }
                if ((selectedBird.hitbox.Top < 0) && (selectedBird.hitbox.Left > pipes[i].topPipe.hitbox.Left) && (selectedBird.hitbox.Left < pipes[i].topPipe.hitbox.Right))
                {
                    return true;
                }
            }

            return false;
        }

        public void GameOver()
        {
            if (restartButton.Clicked(Mouse.GetState()))
            {
                gameOver = false;
                RestartGame();
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

            for (var i = 0; i < pipes.Length; i++)
            {
                pipes[i].topPipe.Draw(_spriteBatch);
                pipes[i].bottomPipe.Draw(_spriteBatch);
            }

            if (gameOver)
            {
                GameOverDraw();
            }


            _spriteBatch.DrawString(spriteFont, score.ToString(), new Vector2(10, 10), Color.Black);
            DisplayScore();

            _spriteBatch.End();
            base.Draw(gameTime);
        }

        public void GameOverDraw()
        {
            gameOverLabel.Draw(_spriteBatch);
            restartButton.Draw(_spriteBatch);
        }

        //Upper pipe size can be from 1-9
        //returns the MovingSprite for topPipe and bottomPipe
        public (MovingSprite topPipe, MovingSprite bottomPipe) PairedPipeGenerator(int upperPipeSize, int xPosition)
        {
            float xScale = 0.9f;
            int pipeSpireSheetHeight = 560;

            int range = floorYValue - (selectedBird.hitbox.Height + bufferHeight);
            float discreteUnit = range / 10;
            float upperHeight = discreteUnit * upperPipeSize;
            float bottomHeight = range - upperHeight;

            float yUpperScale = upperHeight / pipeSpireSheetHeight;
            float yBottomScale = bottomHeight / pipeSpireSheetHeight;

            MovingSprite topPipe = new MovingSprite(spriteSheet, new Vector2(xPosition, 0), new Rectangle(196, 1130, 92, pipeSpireSheetHeight), new Vector2(xScale, yUpperScale), scrollingSpeed);
            MovingSprite bottomPipe = new MovingSprite(spriteSheet, new Vector2(xPosition, floorYValue - bottomHeight), new Rectangle(294, 1130, 92, pipeSpireSheetHeight), new Vector2(xScale, yBottomScale), scrollingSpeed);

            return (topPipe, bottomPipe);
        }

        public void RestartGame()
        {
            pipes[0] = PairedPipeGenerator(3, (GraphicsDevice.Viewport.Width / 2) * 3);
            pipes[1] = PairedPipeGenerator(6, (GraphicsDevice.Viewport.Width / 2) * 4 + 25);
            selectedBird.ResetPosition();
            score = 1000;
            passedPipe1 = false;
            passedPipe2 = false;
        }

        public void DisplayScore()
        {
            scoreLabel.Clear();
            string scoreString = score.ToString();
            foreach (char character in scoreString)
            {
                //Converts the character into an ascii value that is then shifted to give me the correct index
                int index = character - '0';
                scoreLabel.Add(new SpriteLabel(spriteSheet, Vector2.Zero, scoreDigits[index], Vector2.One));
            }

            int amountOfDigits = (int)Math.Log10(score) + 1;
            if (score == 0)
            {
                amountOfDigits = 1;
            }

            const int gap = 5;
            const int yPosition = 25;

            float sumX = 0;
            for (int i = 0; i < amountOfDigits; i++)
            {
                sumX += scoreLabel[i].hitbox.Width;
            }

            float boundingSize = sumX + (gap * (amountOfDigits - 1));
            float boundingLeft = (GraphicsDevice.Viewport.Width / 2) - (boundingSize / 2);

            float previousX = boundingLeft;
            for (int i = 0; i < amountOfDigits; i++)
            {
                scoreLabel[i].SetPosition(new Vector2(previousX, yPosition));
                previousX += (gap + scoreLabel[i].hitbox.Width);
            }

            /*if (amountOfDigits == 1)
            {
                scoreLabel[0].SetPosition(new Vector2((GraphicsDevice.Viewport.Width - scoreLabel[0].hitbox.Width) / 2, 25));
            }
            else if (amountOfDigits == 2)
            {
                scoreLabel[0].SetPosition(new Vector2((GraphicsDevice.Viewport.Width / 2) - scoreLabel[0].hitbox.Width - 2, 25));
                scoreLabel[1].SetPosition(new Vector2((GraphicsDevice.Viewport.Width / 2) + 2, 25));
            }*/

            foreach (var label in scoreLabel)
            {
                label.Draw(_spriteBatch);
            }
        }
    }
}