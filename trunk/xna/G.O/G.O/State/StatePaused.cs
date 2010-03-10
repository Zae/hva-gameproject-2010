using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GO
{
    class StatePaused : State
    {

        public StatePaused()
        {
            
        }

        public override void draw()
        {
            GO.spriteBatch.Begin();
           //Draw a box with yes/no option 
            GO.spriteBatch.DrawString(Fonts.font, "Return to menu? (Y/N)", new Vector2((GO.width / 2) - 100, (GO.height / 2)), Color.Red);


            GO.spriteBatch.End();
        }

        public override void update(int ellapsed)
        {
            KeyboardState keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.N))
            {
                GO.get().setState(StateTest.get());
            }
            else if (keyState.IsKeyDown(Keys.Y))
            {
                GO.get().setState(new StateTitle());
            }

        }

        public override void focusGained()
        {
        
        }

        public override void focusLost()
        {
           
        }
    }
}
