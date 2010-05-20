using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ION
{
    class Images
    {

        public static Texture2D teamLogoImage;

        public static Texture2D mountainImage;
        public static Texture2D baseImage;
        public static Texture2D baseHitmapImage;
        public static Texture2D borderImage;
        public static Texture2D resourceImage;
        public static Texture2D white1px;
        public static Texture2D greenPixel;

        public static Texture2D tileHitmapImage;

        public static Texture2D unitWayPoint;

        public static Texture2D blueBaseImage;
        public static Texture2D redBaseImage;

        //environment
        public static Texture2D groundTexture;
        public static Texture2D gameBackground;

        public static Texture2D[,] unit;
        public static Texture2D[,] unit_selected;

        //guiItems
        public static Texture2D commandsBar;
        public static Texture2D moveButtonNormal;
        public static Texture2D attackButtonNormal;
        public static Texture2D stopButtonNormal;
        public static Texture2D defensiveButtonNormal;
        public static Texture2D newUnitButtonNormal;
        public static Texture2D statusBar;
        public static Texture2D selectionBar;
        public static Texture2D statusBarTemp;
        public static Texture2D emptyButton;
        public static Texture2D buttonOverlay;

        public static Texture2D textCommands;
        public static Texture2D textSelection;
        public static Texture2D textVictory;

        //menuItems
        public static Texture2D ION_LOGO;
        public static Texture2D buttonNewGame;
        public static Texture2D buttonNewGameF;
        public static Texture2D buttonQuit;
        public static Texture2D buttonQuitF;
        public static Texture2D buttonMP;
        public static Texture2D buttonMPF;
        public static Texture2D background;
        public static Texture2D background_overlay;
        public static Texture2D background_starfield;

        public static Texture2D buttonOptions;
        public static Texture2D buttonOptionsF;

        public static Texture2D buttonJoin;
        public static Texture2D buttonJoinF;
        public static Texture2D buttonHost;
        public static Texture2D buttonHostF;
        public static Texture2D buttonBack;
        public static Texture2D buttonBackF;

        public static Texture2D buttonRefresh;
        public static Texture2D buttonRefreshF;

        public static Texture2D TableColumnRoomname;
        public static Texture2D TableColumnPlayers;
        public static Texture2D TableColumnLevel;

        public static Texture2D helpFile;



        public static Texture2D[] chargeCountImages = new Texture2D[10];

        public static Texture2D getChargeCountImage(float charge) 
        {
            if (charge < 1.0f)
            {
                return chargeCountImages[(int)(charge*10)];
            }
            else
            {
                return chargeCountImages[chargeCountImages.Length-1];
            } 
        }

        public static Texture2D getUnitImage(int owner, int facing, bool selected)
        {
            if (selected)
            {
                return unit_selected[owner - 1, facing];
            }
            else
            {
                return unit[owner - 1, facing];
            }
        }
    }
}
