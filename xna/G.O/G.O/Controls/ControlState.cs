using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace ION.Controls
{
    class ControlState
    {

        public virtual void handleMouse(MouseState mouseState)
        {

            //Handle mouse input
            if (mouseState.ScrollWheelValue > StateTest.get().scrollValue)
            {
                Tile.zoomIn();
                Unit.zoomIn();
                //Debug.WriteLine("zoomIn");

            }
            else if (mouseState.ScrollWheelValue < StateTest.get().scrollValue)
            {
                Tile.zoomOut();
                Unit.zoomOut();
                // Debug.WriteLine("zoomOut");
            }

            StateTest.get().scrollValue = mouseState.ScrollWheelValue;
        }

        public virtual void handleKeyboard(KeyboardState keyboardState)
        {
        }
        public virtual void draw()
        {
        }

    }
}
