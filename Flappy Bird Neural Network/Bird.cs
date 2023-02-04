using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flappy_Bird_Neural_Network
{
    public class Bird : SpriteBase
    {
        private int currentIndex = 0;
        Rectangle?[] sourceRectangles;

        //Note that there is no horizontal velocity because the bird will not move left or right
        float verticalVelocity;

        TimeSpan birdFlapTime;

        public Bird(Texture2D texture, Vector2 position, Rectangle?[] sourceRectangle, float rotation, Vector2 scale, TimeSpan birdFlapTime)
            : base(texture, position, Color.White, rotation, Vector2.Zero, scale, SpriteEffects.None, 0)
        {
            this.sourceRectangles = sourceRectangle;
            verticalVelocity = 0;
            this.birdFlapTime = birdFlapTime;
        }

        protected override Rectangle? sourceRectangle { get => sourceRectangles[currentIndex]; }

        //Jump is a bool because it allows us to start the game
        public bool Jump()
        {
            verticalVelocity = 6;
            return true;
        }

        public void Animate(SpriteBatch spriteBatch, bool gameStarted, GameTime gameTime)
        {
            birdFlapTime += gameTime.ElapsedGameTime;

            if (birdFlapTime.TotalMilliseconds > 100)
            {

                if (currentIndex == sourceRectangles.Length - 1)
                {
                    currentIndex = 0;
                }
                else
                {
                    currentIndex++;
                }

                birdFlapTime = TimeSpan.Zero;
            }

            if (gameStarted)
            {
                //Gravity
                if (verticalVelocity > -10)
                {
                    verticalVelocity -= 0.25f;
                }

                //Update the position    
                position.Y -= verticalVelocity * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 16.7f;
            }

            Draw(spriteBatch);
        }
    }
}
