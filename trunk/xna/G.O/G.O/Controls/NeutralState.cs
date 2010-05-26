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

        public override void draw()
        {
            if (leftMouseDown)
            {
                ION.spriteBatch.Begin();
                //draw a rectangle from oldMousePos to mousePos
                ION.spriteBatch.Draw(Images.greenPixel, new Rectangle((int)oldMousePos.X, (int)oldMousePos.Y, (int)(mousePos.X - oldMousePos.X), (int)(mousePos.Y - oldMousePos.Y)), new Color(Color.GreenYellow, 127));// normal
                ION.spriteBatch.Draw(Images.greenPixel, new Rectangle((int)mousePos.X, (int)oldMousePos.Y, (int)(oldMousePos.X - mousePos.X), (int)(mousePos.Y - oldMousePos.Y)), new Color(Color.GreenYellow, 127));// inverted
                ION.spriteBatch.End();
            }
        }

        public override void handleInput(MouseState mouseState, KeyboardState keyboardState)
        {
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                //grid.selectOnMap(mouseState.X, mouseState.Y, translationX, translationY);
                //allUnits[0].SetTarget(new Vector2(mouseState.X, mouseState.Y));

                if (!leftMouseDown)
                {
                    oldMousePos.X = (mouseState.X);
                    oldMousePos.Y = (mouseState.Y);
                }
                leftMouseDown = true;
                mousePos.X = (mouseState.X);
                mousePos.Y = (mouseState.Y);

                selectOnMap(mouseState.X, mouseState.Y, StateTest.get().translationX, StateTest.get().translationY);   
            }
            else if (mouseState.LeftButton == ButtonState.Released)
            {
                leftMouseDown = false;
            }

            if (selectedUnits && !leftMouseDown)
            {
                StateTest.get().gui.applyState(GUIManager.UNITS_SELECTED);
                StateTest.get().controls = new UnitSelectionState();
            }
            else if (selectedBase && !leftMouseDown)
            {
                StateTest.get().gui.applyState(GUIManager.BASE_SELECTED);
                StateTest.get().controls = new BaseSelectionState();
            }
            //This is a hack to not let the GUI update until it is sure it has not selected anything, also see UnitSelectionState
            else if (!leftMouseDown && GUIManager.state != GUIManager.NONE_SELECTED)
            {
                StateTest.get().gui.applyState(GUIManager.NONE_SELECTED);
            }

            base.handleInput(mouseState, keyboardState);
        }


        public void selectOnMap(float x, float y, float translationX, float translationY)
        {
            List<Unit> playerArmy = Grid.get().getPlayerUnits(Grid.playerNumber);

     
            Grid.get().selectTile(x, y, translationX, translationY);
            if (Grid.get().selectedTile != null)
            {
                for (int i = 0; i < playerArmy.Count(); i++)
                {
                    //unselect this unit by default
                    //playerArmy[i].selected = false;

                    //check if the current tile matches the units tile, if so changed the units selected to true
                    if (Grid.get().GetTile(x, y, translationX, translationY) == playerArmy[i].GetTile())
                    {
                        playerArmy[i].selected = true;//select unit
                        selectedUnits = true;

                        SoundManager.selectUnitSound();
                    }
                    else
                    {
                        playerArmy[i].selected = false;
                    }

                    // if this unit is in between the 2 mouse positions
                    if (
                        ((playerArmy[i].GetVirtualPos().X > x && playerArmy[i].GetVirtualPos().X < oldMousePos.X)
                        || (playerArmy[i].GetVirtualPos().X < x && playerArmy[i].GetVirtualPos().X > oldMousePos.X))
                        && ((playerArmy[i].GetVirtualPos().Y > y && playerArmy[i].GetVirtualPos().Y < oldMousePos.Y)
                        || (playerArmy[i].GetVirtualPos().Y < y && playerArmy[i].GetVirtualPos().Y > oldMousePos.Y))
                        )
                    {
                        // set unit to selected
                        playerArmy[i].selected = true;
                        selectedUnits = true;

                        SoundManager.selectUnitSound();
                    }
                    else
                    {
                        playerArmy[i].selected = false;
                    }


                }
            }

            if (!selectedUnits)
            {
                if (Grid.get().selectedTile != null && Grid.get().selectedTile is BaseTile)
                {
                    if (((BaseTile)Grid.get().selectedTile).owner == Grid.playerNumber)
                    {
                        selectedBase = true;
                    }
                }
            }

        }

    }


}
