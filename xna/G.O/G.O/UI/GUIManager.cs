using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace ION.UI
{
    public class GUIManager
    {

        private List<GUIComponent> components = new List<GUIComponent>();

        public const int NONE_SELECTED = 0;
        public const int BASE_SELECTED = 1;
        public const int UNITS_SELECTED = 2;
        public static int state = 0;

        public Rectangle mousePointer = new Rectangle();
        public static int mousePointerState = 0;
        public int mousePointerOffsetX = (int)Images.mousePointers[0].Width/2;
        public int mousePointerOffsetY = (int)Images.mousePointers[0].Height/2;

        public Point evalPoint = new Point(0, 0);

        private static GUIComposite commandsBar;

        public GUIManager()
        {
            GUIComponent commands = new GUIComponent(ION.width - Images.commandsBar.Width - 20 + (15), ION.height - Images.commandsBar.Height - 20 - (25), Images.textCommands);
            addComponent(commands);

            commandsBar = new GUIComposite(ION.width - Images.commandsBar.Width - 20,ION.height - Images.commandsBar.Height - 20, Images.commandsBar);
            addComponent(commandsBar);

            GUIComposite statusBar = new GUIComposite(ION.width - Images.statusBar.Width - 20,20, Images.statusBar);
            statusBar.add(new ResourceCounter(67, 32));
            statusBar.add(new InfluenceDisplayer(168, 32, Images.statusBarTemp));
            addComponent(statusBar);

            //GUIComponent victory = new GUIComponent(20 + 10, 20 + 5, Images.textVictory);
            //addComponent(victory);

            GUIComposite generalInfo = new GUIComposite(20, 20, Images.selectionBar);
            generalInfo.add(new GUIComponent(10,5, Images.textVictory));
            generalInfo.add(new LevelInfo(15, 30));
            //generalInfo.add(new StrategyInfo(15, 25));
            generalInfo.add(new Label(15, 40, "Hold H for Help"));
            addComponent(generalInfo);

            //ION.get().IsMouseVisible = false;
            mousePointer.Width = Images.mousePointers[0].Width;
            mousePointer.Height = Images.mousePointers[0].Height;


            applyState(NONE_SELECTED); 
        }

        public void addComponent(GUIComponent component)
        {
            components.Add(component);
        }

        public bool handleMouse(MouseState mouseState)
        {
            evalPoint.X = mouseState.X;
            evalPoint.Y = mouseState.Y;

            mousePointerState = Images.MOUSE_POINTER;

            mousePointer.X = evalPoint.X - mousePointerOffsetX;
            mousePointer.Y = evalPoint.Y - mousePointerOffsetY;


            bool leftPressed = false;
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                leftPressed = true;
            }

            foreach (GUIComponent component in components)
            {
                if (component.handleMouse(evalPoint, leftPressed))
                {
                    return true; 
                }
            }

            return false;
        }

        public void draw()
        {
            ION.spriteBatch.Begin();
            foreach (GUIComponent guic in components)
            {
                guic.draw();
            }

            drawMousePointer();

            ION.spriteBatch.End();
        }

        public void drawMousePointer()
        {
           
            ION.spriteBatch.Draw(Images.mousePointers[mousePointerState], mousePointer, Color.White);
        }

        public void applyState(int guiState) 
        {
            if(guiState == NONE_SELECTED) 
            {
                state = guiState;
                
                //update the selection screen
                //update the buttons
                commandsBar.clear();
                commandsBar.add(new GUIComponent(12,5,Images.emptyButton));
                commandsBar.add(new GUIComponent(18 + Images.emptyButton.Width, 5, Images.emptyButton));
                commandsBar.add(new GUIComponent(23 + (Images.emptyButton.Width * 2), 5, Images.emptyButton));

                commandsBar.add(new GUIComponent(12, 10+Images.emptyButton.Height, Images.emptyButton));
                commandsBar.add(new GUIComponent(18 + Images.emptyButton.Width, 10 + Images.emptyButton.Height, Images.emptyButton));
                commandsBar.add(new GUIComponent(23 + (Images.emptyButton.Width * 2), 10 + Images.emptyButton.Height, Images.emptyButton));
            }
            else if(guiState == BASE_SELECTED) 
            {
                state = guiState;
                
                //update the selection screen
                //update the buttons
                commandsBar.clear();
                commandsBar.add(new Button(12, 5, Images.newUnitButtonNormal, new NewUnitHandler()));
                commandsBar.add(new GUIComponent(18 + Images.emptyButton.Width, 5, Images.emptyButton));
                commandsBar.add(new GUIComponent(23 + (Images.emptyButton.Width * 2), 5, Images.emptyButton));

                commandsBar.add(new GUIComponent(12, 10 + Images.emptyButton.Height, Images.emptyButton));
                commandsBar.add(new GUIComponent(18 + Images.emptyButton.Width, 10 + Images.emptyButton.Height, Images.emptyButton));
                commandsBar.add(new GUIComponent(23 + (Images.emptyButton.Width * 2), 10 + Images.emptyButton.Height, Images.emptyButton));
            }
            else if(guiState == UNITS_SELECTED) 
            {
                state = guiState;

                commandsBar.clear();
                commandsBar.add(new Button(12, 5, Images.moveButtonNormal, new MoveHandler()));
                commandsBar.add(new Button(18 + Images.emptyButton.Width, 5, Images.stopButtonNormal, new StopHandler()));
                commandsBar.add(new Button(23 + (Images.emptyButton.Width * 2), 5, Images.towerButtonNormal, new TowerHandler()));

                commandsBar.add(new Button(12, 10 + Images.emptyButton.Height, Images.attackButtonNormal, new AttackHandler()));
                commandsBar.add(new Button(18 + Images.emptyButton.Width, 10 + Images.emptyButton.Height, Images.defensiveButtonNormal, new DefensiveHandler()));
                commandsBar.add(new GUIComponent(23 + (Images.emptyButton.Width * 2), 10 + Images.emptyButton.Height, Images.emptyButton));
            }
        }



    }
}
