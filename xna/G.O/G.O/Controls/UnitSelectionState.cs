using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace ION.Controls
{
    class UnitSelectionState : ControlState
    {

        public override void draw()
        {
            //draw bounding box/health bar around unit?
            //should that be done from here or from the selected unit?
        }

        public override void handleMouse(MouseState mouseState)
        {
           //left click canceles the selection
            //but make sure that the left click is propegated to the NeutralState!

            //right click directs the units to the specified tile
            //if the tile is occupied by an enemy, this changes into a attack command on that enemy


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
