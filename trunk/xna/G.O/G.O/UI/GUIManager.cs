using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ION.UI
{
    class GUIManager
    {

        private List<GUIComponent> components = new List<GUIComponent>();

        public const int NEUTRAL = 0;
        public const int BASE_SELECTED = 1;
        public const int UNITS_SELECTED = 2;

        private static GUIComposite commandsBar;

        public GUIManager()
        {
 
            commandsBar = new GUIComposite(ION.width - Images.commandsBar.Width - 20,ION.height - Images.commandsBar.Height - 20, Images.commandsBar);
            addComponent(commandsBar);

            GUIComposite statusBar = new GUIComposite(ION.width - Images.statusBar.Width - 20,20, Images.statusBar);
            statusBar.add(new ResourceCounter(67, 32));
            statusBar.add(new InfluenceDisplayer(168, 32, Images.statusBarTemp));
            addComponent(statusBar);

            GUIComposite generalInfo = new GUIComposite(20, 20, Images.selectionBar);
            generalInfo.add(new LevelInfo(15, 10));
            generalInfo.add(new StrategyInfo(15, 25));
            generalInfo.add(new GUILabel(15, 40, "Hold H for Help"));
            addComponent(generalInfo);

            applyState(NEUTRAL);
        }

        public void addComponent(GUIComponent component)
        {
            components.Add(component);
        }

        public bool handleMouse(int x, int y)
        {
            bool mouseHandled = false;

            foreach (GUIComponent component in components)
            {
                if(component.handleMouse(x,y))
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
            ION.spriteBatch.End();
        }

        public static void applyState(int guiState) 
        {
            if(guiState == NEUTRAL) 
            {
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
                //update the selection screen
                //update the buttons
                commandsBar.clear();
                commandsBar.add(new GUIComponent(12, 5, Images.emptyButton));
                commandsBar.add(new GUIComponent(18 + Images.emptyButton.Width, 5, Images.emptyButton));
                commandsBar.add(new GUIComponent(23 + (Images.emptyButton.Width * 2), 5, Images.emptyButton));

                commandsBar.add(new GUIComponent(12, 10 + Images.emptyButton.Height, Images.emptyButton));
                commandsBar.add(new GUIComponent(18 + Images.emptyButton.Width, 10 + Images.emptyButton.Height, Images.emptyButton));
                commandsBar.add(new GUIComponent(23 + (Images.emptyButton.Width * 2), 10 + Images.emptyButton.Height, Images.emptyButton));
            }
            else if(guiState == UNITS_SELECTED) 
            {
                commandsBar.clear();
                commandsBar.add(new Button(12, 5, Images.moveButtonNormal, new MoveHandler()));
                commandsBar.add(new Button(18 + Images.emptyButton.Width, 5, Images.stopButtonNormal, new StopHandler()));
                commandsBar.add(new GUIComponent(23 + (Images.emptyButton.Width * 2), 5, Images.emptyButton));

                commandsBar.add(new Button(12, 10 + Images.emptyButton.Height, Images.attackButtonNormal, new AttackHandler()));
                commandsBar.add(new Button(18 + Images.emptyButton.Width, 10 + Images.emptyButton.Height, Images.defensiveButtonNormal, new DefensiveHandler()));
                commandsBar.add(new GUIComponent(23 + (Images.emptyButton.Width * 2), 10 + Images.emptyButton.Height, Images.emptyButton));
            }
        }



    }
}
