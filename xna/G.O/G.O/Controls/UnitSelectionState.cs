using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using ION.UI;

namespace ION.Controls
{
    class UnitSelectionState : ControlState
    {

        public override void draw()
        {
        }

        public override void handleInput(MouseState mouseState, KeyboardState keyState)
        {
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                //This is a hack to not update the GUI directly, but we wait until in NeutralState to make sure we don't have a new selection
                //StateTest.get().gui.applyState(GUIManager.NONE_SELECTED);
                StateTest.get().controls = new NeutralState();
                return;
            }

            if (keyState.IsKeyDown(Keys.LeftShift))// if actions are being queued up
            {
                if (mouseState.RightButton == ButtonState.Pressed)
                {//here
                    shiftMouseRightPressed(mouseState.X, mouseState.Y, StateTest.translationX, StateTest.translationY);
                }
            }
            else if (mouseState.RightButton == ButtonState.Pressed)
            {
                mouseRightPressed(mouseState.X, mouseState.Y, StateTest.translationX, StateTest.translationY);
            }

            base.handleInput(mouseState, keyState);
        }

        public void mouseRightPressed(float x, float y, float translationX, float translationY)
        {
            List<Unit> playerUnits = Grid.get().getPlayerUnits();
            
            Grid.get().selectTile(x, y, translationX, translationY);
            if (Grid.get().selectedTile != null)
            {
                for (int i = 0; i < playerUnits.Count(); i++)
                {
                    if (playerUnits[i] != null && playerUnits[i].selected)
                    {
                        playerUnits[i].EmptyWayPoints();
                        playerUnits[i].SetTarget(Grid.get().selectedTile.GetPos(translationX, translationY));
                    }
                }
            }
        }

        public void shiftMouseRightPressed(float x, float y, float translationX, float translationY)
        {
            List<Unit> playerUnits = Grid.get().getPlayerUnits();
            
            Grid.get().selectTile(x, y, translationX, translationY);
            if (Grid.get().selectedTile != null)
            {
                for (int i = 0; i < playerUnits.Count(); i++)
                {
                    if (playerUnits[i] != null && playerUnits[i].selected)
                    {
                        playerUnits[i].AddDestination(Grid.get().selectedTile);// here
                    }
                }
            }
        }
    }
}
