﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ION
{
    class Images
    {
        public static Texture2D[] mousePointers;
        public const int MOUSE_POINTER = 0;
        public const int MOUSE_MOVE = 1;
        public const int MOUSE_ATTACK = 2;
        public const int MOUSE_TRANSLATE = 3;

        public static Texture2D teamLogoImage;

        public static Texture2D mountainImage;
        public static Texture2D mountainFloorImage;
        public static Texture2D iceImage;
        public static Texture2D glassImage;
        public static Texture2D crystalImage;
        public static Texture2D iceFloorImage;
        public static Texture2D glassFloorImage;
        public static Texture2D crystalFloorImage;
        public static Texture2D[] canyonFloorImage;
        public static Texture2D baseImage;
        public static Texture2D borderImage;
        public static Texture2D resourceImage;
        public static Texture2D white1px;
        public static Texture2D greenPixel;
        public static Texture2D selectionBoxBack;
        public static Texture2D selectionBoxFront;
        public static Texture2D selectionBoxBack2;
        public static Texture2D selectionBoxFront2;

        public static Texture2D bluebase2;
        public static Texture2D bluebase4;
        public static Texture2D bluebase5;
        public static Texture2D bluebase6;
        public static Texture2D bluebase8;

        public static Texture2D redbase2;
        public static Texture2D redbase4;
        public static Texture2D redbase5;
        public static Texture2D redbase6;
        public static Texture2D redbase8;

        public static Texture2D tileHitmapImage;

        public static Texture2D wonGameNotice;
        public static Texture2D lostGameNotice;

        public static Texture2D unitWayPoint;

        public static Texture2D blueBaseImage;
        public static Texture2D redBaseImage;

        //environment
        public static Texture2D groundTexture;
        public static Texture2D gameBackground;

        public static Texture2D[,] unit;
        public static Texture2D[,,] turret;

        public static Texture2D[] unitHealth;


        public static Texture2D[,] unit_shooting_overlay;
        public static Texture2D[,] tower_shooting_overlay;

        public static Texture2D[] explosion_overlay;

        public static Texture2D[] bulletImpact;

        //guiItems
        public static Texture2D commandsBar;
        public static Texture2D moveButtonNormal;
        public static Texture2D attackButtonNormal;
        public static Texture2D stopButtonNormal;
        public static Texture2D defensiveButtonNormal;
        public static Texture2D towerButtonNormal;
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
        public static Texture2D Logo;
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

        public static Texture2D inputField;
        public static Texture2D roomCaption;
        public static Texture2D waitScreen;

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

        public static Texture2D getUnitImage(int owner, int facing)
        {
            return unit[owner - 1, facing];
        }

        public static Texture2D getTurretImage(int owner, int facing, int specifyModelType)
        {
            return turret[owner - 1, facing, specifyModelType];
        }
    }
}
