using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace ION.Controls
{
    public class ControlState
    {

        public virtual void handleInput(MouseState mouseState, KeyboardState keyboardState)
        {
            //Handle Zoom from every context
            if (mouseState.ScrollWheelValue > StateTest.get().scrollValue)
            {
                Tile.zoomIn();
                Unit.zoomIn();
            }
            else if (mouseState.ScrollWheelValue < StateTest.get().scrollValue)
            {
                Tile.zoomOut();
                Unit.zoomOut();
            }
            StateTest.get().scrollValue = mouseState.ScrollWheelValue;

            if (mouseState.MiddleButton == ButtonState.Pressed)
            {
                StateTest.translationX += mouseState.X - StateTest.previousMouseX;
                StateTest.translationY += mouseState.Y - StateTest.previousMouseY;
            }

            StateTest.previousMouseX = mouseState.X;
            StateTest.previousMouseY = mouseState.Y;
        }

        public virtual void draw()
        {
        }

    }
}
