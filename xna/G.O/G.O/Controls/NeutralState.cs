using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace ION.Controls
{
    class NeutralState : ControlState
    {

        //put variables for specific buttons

        public override void draw()
        {
            //draw the right type of cursor

            //maybe keep track of what the mouse cursor is hoovering over?
            
        }

        public override void handleMouse(MouseState mouseState)
        {
            //nothing is selected

            //release left click tries to select something
            

            //holding and dragging left mouse makes a selection box

            base.handleMouse(mouseState);
        }

        public override void handleKeyboard(KeyboardState keyboardState)
        {
            //left click canceles the selection
            //but make sure that the left click is propegated to the NeutralState!

            //right click directs the units to the specified tile
            //if the tile is occupied by an enemy, this changes into a attack command on that enemy


        }
    }
}
