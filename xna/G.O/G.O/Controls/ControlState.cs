﻿using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using ION.UI;
using Microsoft.Xna.Framework;

namespace ION.Controls
{
    public class ControlState
    {

        public virtual void handleInput(MouseState mouseState, KeyboardState keyboardState)
        {
            //Handle Zoom from every context
            if (mouseState.ScrollWheelValue > StateTest.get().scrollValue)
            {
                Tile t = Grid.get().getTile((float)ION.halfWidth, (float)ION.halfHeight, StateTest.get().translationX, StateTest.get().translationY);

                if (t != null)
                {
                    float oldX = (ION.halfWidth + (t.getVisualX() * Tile.baseHalfWidth));
                    float oldY = (t.getVisualY() * Tile.baseHalfHeight);

                    Tile.zoomIn();
                    Unit.zoomIn();
                    Grid.get().onResize();
          
                    float newX = (ION.halfWidth + (t.getVisualX() * Tile.baseHalfWidth));
                    float newY = (t.getVisualY() * Tile.baseHalfHeight);

                    StateTest.get().translationX -= (newX - oldX);
                    StateTest.get().translationY -= (newY - oldY);
                }
            }
            else if (mouseState.ScrollWheelValue < StateTest.get().scrollValue)
            {
                Tile t = Grid.get().getTile((float)ION.halfWidth, (float)ION.halfHeight, StateTest.get().translationX, StateTest.get().translationY);
                if (t != null)
                {
                    float oldX = (ION.halfWidth + (t.getVisualX() * Tile.baseHalfWidth));
                    float oldY = (t.getVisualY() * Tile.baseHalfHeight);

                    Tile.zoomOut();
                    Unit.zoomOut();
                    Grid.get().onResize();

                    float newX = (ION.halfWidth + (t.getVisualX() * Tile.baseHalfWidth));
                    float newY = (t.getVisualY() * Tile.baseHalfHeight);

                    StateTest.get().translationX -= (newX - oldX);
                    StateTest.get().translationY -= (newY - oldY);
                }
            }
            StateTest.get().scrollValue = mouseState.ScrollWheelValue;

            if (mouseState.MiddleButton == ButtonState.Pressed)
            {
                GUIManager.mousePointerState = Images.MOUSE_TRANSLATE;
                
                float newTranslationX = StateTest.get().translationX + mouseState.X - StateTest.previousMouseX;
                float newTranslationY = StateTest.get().translationY + mouseState.Y - StateTest.previousMouseY;

                Tile t = Grid.get().getTile((float)ION.halfWidth, (float)ION.halfHeight, newTranslationX, newTranslationY);
                if (t != null)
                {
                    StateTest.get().translationX += mouseState.X - StateTest.previousMouseX;
                    StateTest.get().translationY += mouseState.Y - StateTest.previousMouseY;
                }
            }

            StateTest.previousMouseX = mouseState.X;
            StateTest.previousMouseY = mouseState.Y;
        }

        public virtual void draw()
        {
        }

        public virtual void showContext(int x,int y)
        {
            //lights up what is under the mouse
            List<IDepthEnabled> depthItems = Grid.depthItems;
           
            int count = depthItems.Count;
            for (int i = depthItems.Count - 1; i >= 0; i--)
            {
                if (depthItems[i].hitTest(x,y))
                {
                    depthItems[i].displayDetails();
                    break;
                }
            }

            //if (result is Unit)
            //{
            //    ((Unit)result).selected = true;
            //    selectedUnits = true;
            //}
            //else if (result is BaseTile)
            //{
            //    // TODO //((BaseTile)result).sekected = true;
            //    selectedBase = true;
            //}
        }



    }
}
