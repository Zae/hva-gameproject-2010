﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using ION.UI;
using ION.MultiPlayer;
using ION.Tools;

namespace ION.Controls
{
    class UnitSelectionState : ControlState
    {

        private bool shiftPressed = false;
        private bool rightMouseDown = false;
        private bool leftMouseDown = false;

        private bool moveOrder = false;
        private bool attackOrder = false;

        public override void draw()
        {
        }

        public override void handleInput(MouseState mouseState, KeyboardState keyState)
        {
            //GUIManager.mousePointerState = Images.MOUSE_MOVE;

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                
                if (!leftMouseDown)
                {
                    StateTest.get().gui.applyState(GUIManager.NONE_SELECTED);
                    StateTest.get().controls = new NeutralState();
                    deselectUnits();
                    leftMouseDown = false;
                    return;
                }

            }
            //else
            //{
            //    //This means we have released the left mouse button
            //    if (leftMouseDown)
            //    {
                    
            //    }
            //    leftMouseDown = false;
            //}

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

            if (moveOrder)
            {
                SoundManager.selectUnitSound();
                StateTest.get().gui.applyState(GUIManager.UNITS_SELECTED);
                StateTest.get().controls = new UnitSelectionState();
            }
            else if (attackOrder)
            {
                //TODO //SoundManager.selectBaseSound();
                StateTest.get().gui.applyState(GUIManager.BASE_SELECTED);
                StateTest.get().controls = new BaseSelectionState();
            }
            else
            {
                showContext(mouseState.X, mouseState.Y);
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
                    if (playerUnits[i] != null && playerUnits[i].selected && playerUnits[i] is Robot)
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
                    if (playerUnits[i] != null && playerUnits[i].selected && playerUnits[i] is Robot)
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

        private void deselectUnits()
        {
            foreach (Unit u in Grid.get().getPlayerUnits(Grid.playerNumber))
            {
                u.selected = false;
            }
        }

        public override void showContext(int x, int y)
        {
            //lights up what is under the mouse
            List<IDepthEnabled> depthItems = Grid.depthItems;

            int count = depthItems.Count;
            for (int i = depthItems.Count - 1; i >= 0; i--)
            {
                if (depthItems[i].hitTest(x, y))
                {
                    if (depthItems[i].getOwner() != Grid.playerNumber)
                    {
                        GUIManager.mousePointerState = Images.MOUSE_ATTACK;
                        
                    }
                    else
                    {
                        GUIManager.mousePointerState = Images.MOUSE_POINTER;
                       
                    }

                    depthItems[i].displayDetails();
                    return;
                }
            }

            GUIManager.mousePointerState = Images.MOUSE_MOVE;

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
