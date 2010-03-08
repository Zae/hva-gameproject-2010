using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace G.O
{
    class StatePaused : State
    {

        public StatePaused()
        {
            
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
           //Draw a box with yes/no option 
            spriteBatch.DrawString(Fonts.font, "Return to menu? (Y/N)", new Vector2((GO.width / 2) - 100, (GO.height / 2)), Color.Red);


            spriteBatch.End();
        }

        public override void update()
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
