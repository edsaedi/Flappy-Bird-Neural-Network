using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flappy_Bird_Neural_Network
{
    public class Button : Sprite
    {
        private bool mousePressed;
        public Button(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Vector2 scale)
           : base(texture, position, sourceRectangle, 0, scale)
        {
            mousePressed = false;
        }

        public bool Clicked(MouseState mouseState)
        {
            if (!mousePressed && mouseState.LeftButton == ButtonState.Pressed)
            {
                if (hitbox.Contains(mouseState.Position))
                {
                    return true;
                }
            }

            mousePressed = mouseState.LeftButton != ButtonState.Released;
            return false;
        }
    }
}
