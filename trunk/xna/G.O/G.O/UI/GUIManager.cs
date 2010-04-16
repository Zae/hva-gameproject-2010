﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ION.UI
{
    class GUIManager
    {

        private List<GUIComponent> components = new List<GUIComponent>();

        public GUIManager()
        {
 
            //make some components here

            GUIComposite commandsBar = new GUIComposite(ION.width - Images.commandsBar.Width - 20,ION.height - Images.commandsBar.Height - 20, Images.commandsBar);
            commandsBar.add(new GUIComponent(25,25,Images.moveButtonNormal));
            commandsBar.add(new GUIComponent(50+Images.moveButtonNormal.Width, 25, Images.attackButtonNormal));
            addComponent(commandsBar);

            GUIComposite statusBar = new GUIComposite(ION.width - Images.statusBar.Width - 20,20, Images.statusBar);
            statusBar.add(new ResourceCounter(67, 32));
            addComponent(statusBar);
        }

        public void addComponent(GUIComponent component)
        {
            components.Add(component);
        }

        public bool resolveMouseMoved(int mouseX, int mouseY)
        {
            return false;
        }

        public bool resolveMouseDown(int mouseX, int mouseY)
        {
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

    }
}
