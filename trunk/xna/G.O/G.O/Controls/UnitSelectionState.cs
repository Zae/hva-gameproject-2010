using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using ION.UI;
using ION.MultiPlayer;

namespace ION.Controls
{
    class UnitSelectionState : ControlState
    {

        private bool shiftPressed = false;
        private bool rightMouseDown = false;
        private bool leftMouseDown = false;

        public override void draw()
        {
        }

        public override void handleInput(MouseState mouseState, KeyboardState keyState)
        {
            GUIManager.mousePointerState = Images.MOUSE_MOVE;

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                //This is a hack to not update the GUI directly, but we wait until in NeutralState to make sure we don't have a new selection
                //StateTest.get().gui.applyState(GUIManager.NONE_SELECTED);
                if (!leftMouseDown)
                {
                    leftMouseDown = true;
                }
            }
            else
            {
                //This means we have released the left mouse button
                if (leftMouseDown)
                {
                    StateTest.get().controls = new NeutralState();
                    return;
                }
                leftMouseDown = false;
            }

            if (keyState.IsKeyDown(Keys.LeftShift))
            {
                if (mouseState.RightButton == ButtonState.Pressed)
                {
                    if (!rightMouseDown)
                    {
                        shiftMouseRightPressed(mouseState.X, mouseState.Y, StateTest.get().translationX, StateTest.get().translationY);
                        rightMouseDown = true;
                    }
                }
                else
                {
                    rightMouseDown = false;
                }
            }
            else
            {
                if (mouseState.RightButton == ButtonState.Pressed)
                {
                    if (!rightMouseDown)
                    {
                        mouseRightPressed(mouseState.X, mouseState.Y, StateTest.get().translationX, StateTest.get().translationY);
                        rightMouseDown = true;
                    }
                }
                else
                {
                    rightMouseDown = false;
                }
            }

            base.handleInput(mouseState, keyState);
        }

        public void mouseRightPressed(float x, float y, float translationX, float translationY)
        {
            List<Unit> playerUnits = Grid.get().getPlayerUnits(Grid.playerNumber);
            
            Grid.get().selectTile(x, y, translationX, translationY);
            if (Grid.get().selectedTile != null)
            {
                for (int i = 0; i < playerUnits.Count(); i++)
                {
                    if (playerUnits[i] != null && playerUnits[i].selected)
                    {
                        CommandDispatcher.issueCommand(new NewMoveCommand(CommandDispatcher.getSupposedGameTick()
                                                                            ,CommandDispatcher.getSerial()
                                                                            ,playerUnits[i].owner
                                                                            ,playerUnits[i].id,Grid.get().selectedTile.indexX
                                                                            ,Grid.get().selectedTile.indexY));
                    }
                }
            }
        }

        public void shiftMouseRightPressed(float x, float y, float translationX, float translationY)
        {
            List<Unit> playerUnits = Grid.get().getPlayerUnits(Grid.playerNumber);
            
            Grid.get().selectTile(x, y, translationX, translationY);
            if (Grid.get().selectedTile != null)
            {
                for (int i = 0; i < playerUnits.Count(); i++)
                {
                    if (playerUnits[i] != null && playerUnits[i].selected)
                    {
                            CommandDispatcher.issueCommand(new AddMoveCommand(CommandDispatcher.getSupposedGameTick()
	                                                                            , CommandDispatcher.getSerial()
	                                                                            , playerUnits[i].owner
	                                                                            , playerUnits[i].id, Grid.get().selectedTile.indexX
	                                                                            , Grid.get().selectedTile.indexY));
                    }
                }
            }
        }
    }
}
