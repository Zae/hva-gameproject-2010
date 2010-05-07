using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace ION.Controls
{
    public class ControlState
    {

        public virtual void handleInput(MouseState mouseState, KeyboardState keyboardState)
        {
            //Handle Zoom from every context
            if (mouseState.ScrollWheelValue > StateTest.get().scrollValue)
            {
                Tile t = Grid.get().getTile((float)ION.halfWidth, (float)ION.halfHeight, StateTest.translationX, StateTest.translationY);

                if (t != null)
                {
                    float oldX = (ION.halfWidth + (t.getVisualX() * Tile.baseHalfWidth));
                    float oldY = (t.getVisualY() * Tile.baseHalfHeight);

                    Tile.zoomIn();
                    Unit.zoomIn();
                    Grid.get().onResize();
          
                    float newX = (ION.halfWidth + (t.getVisualX() * Tile.baseHalfWidth));
                    float newY = (t.getVisualY() * Tile.baseHalfHeight);

                    StateTest.translationX -= (newX - oldX);
                    StateTest.translationY -= (newY - oldY);
                }
            }
            else if (mouseState.ScrollWheelValue < StateTest.get().scrollValue)
            {
                Tile t = Grid.get().getTile((float)ION.halfWidth, (float)ION.halfHeight, StateTest.translationX, StateTest.translationY);
                if (t != null)
                {
                    float oldX = (ION.halfWidth + (t.getVisualX() * Tile.baseHalfWidth));
                    float oldY = (t.getVisualY() * Tile.baseHalfHeight);

                    Tile.zoomOut();
                    Unit.zoomOut();
                    Grid.get().onResize();

                    float newX = (ION.halfWidth + (t.getVisualX() * Tile.baseHalfWidth));
                    float newY = (t.getVisualY() * Tile.baseHalfHeight);

                    StateTest.translationX -= (newX - oldX);
                    StateTest.translationY -= (newY - oldY);
                }
            }
            StateTest.get().scrollValue = mouseState.ScrollWheelValue;

            if (mouseState.MiddleButton == ButtonState.Pressed)
            {
                float newTranslationX = StateTest.translationX + mouseState.X - StateTest.previousMouseX;
                float newTranslationY = StateTest.translationY + mouseState.Y - StateTest.previousMouseY;

                Tile t = Grid.get().getTile((float)ION.halfWidth, (float)ION.halfHeight, newTranslationX, newTranslationY);
                if (t != null)
                {
                    StateTest.translationX += mouseState.X - StateTest.previousMouseX;
                    StateTest.translationY += mouseState.Y - StateTest.previousMouseY;
                }
            }

            StateTest.previousMouseX = mouseState.X;
            StateTest.previousMouseY = mouseState.Y;
        }

        public virtual void draw()
        {
        }

    }
}
