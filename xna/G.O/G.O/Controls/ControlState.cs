using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ION.Controls
{
    abstract class ControlState
    {

        public abstract void handleMouse(int ellapsed);
        public abstract void handleKeyboard(int ellapsed);
        public abstract void draw(); 

    }
}
