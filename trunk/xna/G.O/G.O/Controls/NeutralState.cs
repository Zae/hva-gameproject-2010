using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ION.UI;
using ION.Tools;

namespace ION.Controls
{
    class NeutralState : ControlState
    {

        //put variables for specific buttons
        private bool leftMouseDown = false;

        private Vector2 oldMousePos, mousePos;

        private bool selectedUnits = false;
        private bool selectedBase = false;

        private bool boxSelection = false;
        private float deadzone = 25.0f;

        private Rectangle r0 = new Rectangle();
        private Rectangle r1 = new Rectangle();

        public override void draw()
        {
            if (boxSelection)
            {
                r0.X = (int)oldMousePos.X;
                r0.Y = (int)oldMousePos.Y;
                r0.Width = (int)(mousePos.X - oldMousePos.X);
                r0.Height = (int)(mousePos.Y - oldMousePos.Y);
                r1.X = (int)mousePos.X;
                r1.Y = (int)oldMousePos.Y;
                r1.Width = (int)(oldMousePos.X - mousePos.X);
                r1.Height =  (int)(mousePos.Y - oldMousePos.Y);
                
                ION.spriteBatch.Begin();
                ION.spriteBatch.Draw(Images.greenPixel,r0, new Color(Color.GreenYellow, 127));// normal
                ION.spriteBatch.Draw(Images.greenPixel, r1, new Color(Color.GreenYellow, 127));// inverted
                ION.spriteBatch.End();
            }
        }

        public override void handleInput(MouseState mouseState, KeyboardState keyboardState)
        {
            //Set the right cursor image
            GUIManager.mousePointerState = Images.MOUSE_POINTER;
            
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                //This detects the first moment the left mouse button got pressed down
                if (!leftMouseDown)
                {
                    //Record the position where this happened
                    oldMousePos.X = (mouseState.X);
                    oldMousePos.Y = (mouseState.Y);
                    //Set the flag
                    leftMouseDown = true;
                }
                
                //We update these as long as the left mouse button is held down
                mousePos.X = (mouseState.X);
                mousePos.Y = (mouseState.Y);

                //if currently not box selecting 
                if (!boxSelection)
                {
                    float diff = oldMousePos.Length() - mousePos.Length();
                    if (diff > deadzone || diff < -deadzone)
                    {
                        boxSelection = true;
                    }
                }
            }
            else if (mouseState.LeftButton == ButtonState.Released)
            {
                if (leftMouseDown)
                {         
                    if (!boxSelection)
                    {
                        selectOnMap(mouseState.X, mouseState.Y);
                    }
                    else
                    {
                        selectOnMap(r0);
                        //selectOnMap(r1);                      
                    }
                    //Reset both flags
                    leftMouseDown = false;
                    boxSelection = false;
                }
            }

            if (selectedUnits)
            {
                SoundManager.selectUnitSound();
                StateTest.get().gui.applyState(GUIManager.UNITS_SELECTED);
                StateTest.get().controls = new UnitSelectionState();
            }
            else if (selectedBase)
            {
                //TODO //SoundManager.selectBaseSound();
                StateTest.get().gui.applyState(GUIManager.BASE_SELECTED);
                StateTest.get().controls = new BaseSelectionState();
            }

            base.handleInput(mouseState, keyboardState);
        }


        public void selectOnMap(int x, int y)
        {
            List<IDepthEnabled> depthItems = Grid.depthItems;
            IDepthEnabled result = null;
            int count = depthItems.Count;
            for (int i = depthItems.Count - 1; i >= 0; i--)
            {
                if (depthItems[i].hitTest(x,y) && depthItems[i].getOwner() == Grid.playerNumber)
                {
                    result = depthItems[i];
                    break;
                }
            }

            if (result is Unit)
            {
                ((Unit)result).selected = true;
                selectedUnits = true;
            }
            else if (result is BaseTile)
            {
                // TODO //((BaseTile)result).sekected = true;
                selectedBase = true;
            }

        }

        public void selectOnMap(Rectangle r)
        {
            List<IDepthEnabled> depthItems = Grid.depthItems;
            IDepthEnabled result = null;
            int count = depthItems.Count;
            for (int i = depthItems.Count - 1; i >= 0; i--)
            {
                if (depthItems[i].hitTest(r) && depthItems[i].getOwner() == Grid.playerNumber)
                {
                    result = depthItems[i];
                    break;
                }
            }

            if (result is Unit)
            {
                ((Unit)result).selected = true;
                selectedUnits = true;
            }
            else if (result is BaseTile)
            {
                // TODO //((BaseTile)result).sekected = true;
                selectedBase = true;
            }
        }

    }


}
