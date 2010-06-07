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

        private Rectangle r = new Rectangle();

        public override void draw()
        {
            if (boxSelection)
            {
                calcValidSelectionBox(mousePos, oldMousePos);

                ION.spriteBatch.Begin();
                ION.spriteBatch.Draw(Images.greenPixel,r, new Color(Color.GreenYellow, 127));
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
                    float diff = oldMousePos.X - mousePos.X;
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
                        selectOnMap(r);            
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
            else
            {
                base.showContext(mouseState.X, mouseState.Y);
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
            List<IDepthEnabled> result = new List<IDepthEnabled>();
            int count = depthItems.Count;
            for (int i = depthItems.Count - 1; i >= 0; i--)
            {
                if (depthItems[i].hitTest(r) && depthItems[i].getOwner() == Grid.playerNumber && depthItems[i] is Unit)
                {
                    result.Add(depthItems[i]);
                }
            }

            foreach(IDepthEnabled ide in result) 
            {
                ((Unit)ide).selected = true;
                selectedUnits = true;
            }
            //else if (result is BaseTile)
            //{
            //    // TODO //((BaseTile)result).sekected = true;
            //    selectedBase = true;
            //}
        }

        public void calcValidSelectionBox(Vector2 mousePos, Vector2 oldMousePos)
        {
            //r.X = (int)oldMousePos.X;
            //r.Y = (int)oldMousePos.Y;
            //r.Width = (int)(mousePos.X - oldMousePos.X);
            //r.Height = (int)(mousePos.Y - oldMousePos.Y);

            if (mousePos.X < oldMousePos.X)
            {
                r.X = (int)mousePos.X;
                r.Width = (int)(oldMousePos.X - mousePos.X);

          
            }
            else
            {
                r.X = (int)oldMousePos.X;
                r.Width = (int)(mousePos.X - oldMousePos.X);

            }
            if (mousePos.Y < oldMousePos.Y)
            {
                r.Y = (int)mousePos.Y;
                r.Height = (int)(oldMousePos.Y - mousePos.Y);
            }
            else
            {
                r.Y = (int)oldMousePos.Y;
                r.Height = (int)(mousePos.Y - oldMousePos.Y);
            }
        }

    }


}
