using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using FluorineFx.Net;
using WindowSystem;
using InputEventSystem;
using ION.Tools;
using System.Globalization;


namespace ION
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class ION : Microsoft.Xna.Framework.Game
    {
        /// <summary>
        /// To enable a connection to a non-static ION instance from anywhere in the program, 
        /// a Singleton mechanism is employed. 
        /// </summary>
        public static ION instance;

        /// <summary>
        /// Links to important static tools to do with the graphics rendering. 
        /// Many other objects outside the ION class depend on these to render themself.
        /// </summary>
        public static SpriteBatch spriteBatch;
        public static PrimitiveBatch primitiveBatch;

        /// <summary>
        /// These variables are initialized during construction to reflect the client's hardware and screen configuration.
        /// </summary>
        public static GraphicsDeviceManager graphics;
        public static int width;
        public static int halfWidth;
        public static int height;
        public static int halfHeight;

        public InputEvents input;
        public GUIManager gui;
        private Skin skin;

        /// <summary>
        /// Helper variable for the user input handled by this class.
        /// </summary>
        private bool restartPressed = false;

        /// <summary>
        /// This is the state the game currently is in and gets called every cycle.
        /// This conforms to the State pattern type.
        /// </summary>
        private State state;

        /// <summary>
        /// TODO @ezra @mustapha
        /// </summary>
        public ServerConnection serverConnection;

        public ION()
        {
            //Set the singleton instance for static reference
            instance = this;

            CultureInfo newCulture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            newCulture.NumberFormat.NumberDecimalSeparator = ",";
            newCulture.NumberFormat.NumberGroupSeparator = ".";
            System.Threading.Thread.CurrentThread.CurrentCulture = newCulture;

            //Setup the graphics configuration to match the client
            graphics = new GraphicsDeviceManager(this);
            graphics.PreparingDeviceSettings += new EventHandler<PreparingDeviceSettingsEventArgs>(preparingDeviceSettings);
            graphics.ToggleFullScreen();
            //graphics.ToggleFullScreen(); //Toggle again to disable fullscreen

            //Now we can get this info without errors this.owner, this.id

            
            width = graphics.GraphicsDevice.DisplayMode.Width;
            halfWidth = width / 2;
            height = graphics.GraphicsDevice.DisplayMode.Height;
            halfHeight = height / 2;

            //Give the Content class a valid content root directory
            Content.RootDirectory = "Content";

            //graphics.GraphicsDevice.SamplerStates[0].MinFilter = TextureFilter.Linear;
            //graphics.ApplyChanges();

            this.input = new InputEvents(this);
            Components.Add(this.input);
            this.gui = new GUIManager(this);
            Components.Add(this.gui);
            // GUI requires variable timing to function correctly
        }

        /// <summary>
        /// Returns the registered instance of the ION class
        /// </summary>
        public static ION get()
        {
            return instance;   
        }

        /// <summary>
        /// Sets a new State and notifies the old and new State of this change.
        /// </summary>
        public void setState(State newState)
        {
            state.focusLost();
            state = newState;
            state.focusGained();
        }

        /// <summary>
        /// Sets a new State and notifies the old and new State of this change.
        /// </summary>
        private void preparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            DisplayMode displayMode = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;
            e.GraphicsDeviceInformation.PresentationParameters.BackBufferFormat = displayMode.Format;
            e.GraphicsDeviceInformation.PresentationParameters.BackBufferWidth = displayMode.Width;
            e.GraphicsDeviceInformation.PresentationParameters.BackBufferHeight = displayMode.Height;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            this.gui.Initialize();
            //We currently don't specify anything at this stage in the program.
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to drawDebug textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            primitiveBatch = new PrimitiveBatch(GraphicsDevice);

            //Loading skin for GUI
            skin = Content.Load<Skin>("IONSkin");
            this.gui.ApplySkin(skin, true, true);
           
            //Loading Fonts
            Fonts.font = Content.Load<SpriteFont>("fontItems/TitleFont");
            Fonts.small = Content.Load<SpriteFont>("fontItems/SmallFont");

            //Load Misc items
            Images.teamLogoImage = Content.Load<Texture2D>("miscItems/logo-game-ninjas");
            Images.white1px = Content.Load<Texture2D>("toolItems/white");
            Images.greenPixel = Content.Load<Texture2D>("menuItems/greenPixel");
            Images.helpFile = Content.Load<Texture2D>("miscItems/helpfile");            
            Images.gameBackground = Content.Load<Texture2D>("miscItems/blue_red");

            Images.selectionBoxBack = Content.Load<Texture2D>("miscItems/boxback");
            Images.selectionBoxFront = Content.Load<Texture2D>("miscItems/boxfront");

            Images.wonGameNotice = Content.Load<Texture2D>("miscItems/you-win");
            Images.lostGameNotice = Content.Load<Texture2D>("miscItems/you-lose");



            //GUI Items
            Images.commandsBar = Content.Load<Texture2D>("guiItems/commandsBar");
            Images.moveButtonNormal = Content.Load<Texture2D>("guiItems/moveButtonNormal");
            Images.attackButtonNormal = Content.Load<Texture2D>("guiItems/attackButtonNormal");
            Images.stopButtonNormal = Content.Load<Texture2D>("guiItems/stopButtonNormal");
            Images.defensiveButtonNormal = Content.Load<Texture2D>("guiItems/defensiveButtonNormal");
            Images.towerButtonNormal = Content.Load<Texture2D>("guiItems/tower");
            Images.emptyButton = Content.Load<Texture2D>("guiItems/emptyButton");
            Images.buttonOverlay = Content.Load<Texture2D>("guiItems/buttonOverlay");
            Images.newUnitButtonNormal = Content.Load<Texture2D>("guiItems/newUnitButtonNormal");
            Images.statusBarTemp = Content.Load<Texture2D>("guiItems/statusBarTemp");
            Images.statusBar = Content.Load<Texture2D>("guiItems/statusBar");
            Images.selectionBar = Content.Load<Texture2D>("guiItems/selectionBar");

            Images.textCommands = Content.Load<Texture2D>("guiItems/txt_commands");
            Images.textSelection = Content.Load<Texture2D>("guiItems/txt_selection");
            Images.textVictory = Content.Load<Texture2D>("guiItems/txt_victory");
               
            //Title Menu
            Images.ION_LOGO = Content.Load<Texture2D>("menuItems/ION_LOGO");
            Images.buttonNewGame = Content.Load<Texture2D>("menuItems/btn_newgame");
            Images.buttonNewGameF = Content.Load<Texture2D>("menuItems/btn_newgame_hover");
            Images.buttonMP = Content.Load<Texture2D>("menuItems/btn_multiplayer");
            Images.buttonMPF = Content.Load<Texture2D>("menuItems/btn_multiplayer_hover");
            Images.buttonQuit = Content.Load<Texture2D>("menuItems/btn_quit");
            Images.buttonQuitF = Content.Load<Texture2D>("menuItems/btn_quit_hover");

            Images.buttonOptions = Content.Load<Texture2D>("menuItems/btn_options");
            Images.buttonOptionsF = Content.Load<Texture2D>("menuItems/btn_options_hover");

            Images.background_overlay = Content.Load<Texture2D>("menuItems/planet");
            Images.Logo = Content.Load<Texture2D>("menuItems/ION");
            Images.background_starfield = Content.Load<Texture2D>("menuItems/starfield");

           

            //MultiPlayer menu
            Images.buttonJoin = Content.Load<Texture2D>("menuItems/btn_join");
            Images.buttonJoinF = Content.Load<Texture2D>("menuItems/btn_join_hover");
            Images.buttonHost = Content.Load<Texture2D>("menuItems/btn_host");
            Images.buttonHostF = Content.Load<Texture2D>("menuItems/btn_host_hover");
            Images.buttonBack = Content.Load<Texture2D>("menuItems/btn_back");
            Images.buttonBackF = Content.Load<Texture2D>("menuItems/btn_back_hover");

            Images.inputField = Content.Load<Texture2D>("menuItems/input");
            Images.roomCaption = Content.Load<Texture2D>("menuItems/room");
            Images.waitScreen = Content.Load<Texture2D>("menuItems/wait");

            //Load Tile images
            Images.borderImage = Content.Load<Texture2D>("tileItems/border_tile");
            Images.mountainImage = Content.Load<Texture2D>("tileItems/mountain_tile");
            Images.mountainFloorImage = Content.Load<Texture2D>("tileItems/mountain_tile_floor");
            Images.iceImage = Content.Load<Texture2D>("tileItems/ice_tile");
            Images.glassImage = Content.Load<Texture2D>("tileItems/glass_tile");
            Images.crystalImage = Content.Load<Texture2D>("tileItems/crystal_tile");
            Images.iceFloorImage = Content.Load<Texture2D>("tileItems/ice_tile_floor");
            Images.glassFloorImage = Content.Load<Texture2D>("tileItems/glass_tile_floor");
            Images.crystalFloorImage = Content.Load<Texture2D>("tileItems/crystal_tile_floor");           
            Images.resourceImage = Content.Load<Texture2D>("tileItems/resource_tile");

            Images.canyonFloorImage = new Texture2D[13];
            Images.canyonFloorImage[0] = Content.Load<Texture2D>("tileItems/canyons/0_land_cave_leeg");
            Images.canyonFloorImage[1] = Content.Load<Texture2D>("tileItems/canyons/1_land_cave_noord");
            Images.canyonFloorImage[2] = Content.Load<Texture2D>("tileItems/canyons/2_land_cave_noordoost");
            Images.canyonFloorImage[3] = Content.Load<Texture2D>("tileItems/canyons/3_land_cave_noordoost_klein");
            Images.canyonFloorImage[4] = Content.Load<Texture2D>("tileItems/canyons/4_land_cave_noordwest");
            Images.canyonFloorImage[5] = Content.Load<Texture2D>("tileItems/canyons/5_land_cave_noordwest_klein");
            Images.canyonFloorImage[6] = Content.Load<Texture2D>("tileItems/canyons/6_land_cave_oost");
            Images.canyonFloorImage[7] = Content.Load<Texture2D>("tileItems/canyons/7_land_cave_west");
            Images.canyonFloorImage[8] = Content.Load<Texture2D>("tileItems/canyons/8_land_cave_zuid");
            Images.canyonFloorImage[9] = Content.Load<Texture2D>("tileItems/canyons/9_land_cave_zuidoost");
            Images.canyonFloorImage[10] = Content.Load<Texture2D>("tileItems/canyons/A_land_cave_zuidoost_klein");
            Images.canyonFloorImage[11] = Content.Load<Texture2D>("tileItems/canyons/B_land_cave_zuidwest");
            Images.canyonFloorImage[12] = Content.Load<Texture2D>("tileItems/canyons/C_land_cave_zuidwest_klein");  
            
            //Load Base images           
            Images.baseImage = Content.Load<Texture2D>("tileItems/base_tile");
            Images.blueBaseImage = Content.Load<Texture2D>("playerItems/bluebase");
            Images.redBaseImage = Content.Load<Texture2D>("playerItems/redBase");

            //Load Tool images 
            Images.tileHitmapImage = Content.Load<Texture2D>("toolItems/tile_hitmap");

            //Join menu
            Images.buttonRefresh = Content.Load<Texture2D>("menuItems/btn_refresh");
            Images.buttonRefreshF = Content.Load<Texture2D>("menuItems/btn_refresh_hover");
            Images.TableColumnRoomname = Content.Load<Texture2D>("menuItems/Roomname");
            Images.TableColumnPlayers = Content.Load<Texture2D>("menuItems/Players");
            Images.TableColumnLevel = Content.Load<Texture2D>("menuItems/Level");

            //Load Unit images
            Images.unitWayPoint = Content.Load<Texture2D>("unitItems/greenArrow");

            //Load Unit images
            Images.unitWayPoint = Content.Load<Texture2D>("unitItems/greenArrow");

            Images.mousePointers = new Texture2D[4];
            Images.mousePointers[0] = Content.Load<Texture2D>("guiItems/mousePointers/mousepointer");
            Images.mousePointers[1] = Content.Load<Texture2D>("guiItems/mousePointers/moveunitpointer");
            Images.mousePointers[2] = Content.Load<Texture2D>("guiItems/mousePointers/attackunitpointer");
            Images.mousePointers[3] = Content.Load<Texture2D>("guiItems/mousePointers/mousetranslation");

            Images.unitHealth = new Texture2D[3];
            Images.unitHealth[0] = Content.Load<Texture2D>("miscItems/statusBars/HealthMeter");
            Images.unitHealth[1] = Content.Load<Texture2D>("miscItems/statusBars/FullHealth");
            Images.unitHealth[2] = Content.Load<Texture2D>("miscItems/statusBars/NotFullHealth");

            int players = 2;
            string[] directions = new string[] { "s", "se", "e", "ne", "n", "nw", "w", "sw" };
            int modelType = 2;

            // Units
            Images.unit = new Texture2D[players, directions.Length];

            for (int i = 0; i < players; i++)
            {
                for (int j = 0; j < directions.Length; j++)
                {
                    Images.unit[i, j] = Content.Load<Texture2D>("unitItems/player" + (i + 1).ToString() + "unit_" + directions[j]);
                }
            }

            int frames = 2;
            Images.unit_shooting_overlay = new Texture2D[directions.Length,frames];
            for (int frame = 0; frame < frames; frame++)
            {
                    for (int j = 0; j < directions.Length; j++)
                    {
                        Images.unit_shooting_overlay[j, frame] = Content.Load<Texture2D>("unitItems/shooting/overlay_unit_firing_" + directions[j]+frame);
                        //Images.unit_shooting_overlay[j, frame] = Content.Load<Texture2D>("unitItems/shooting/overlay_unit_firing_ne" + frame);
                    }
            }

            //Load bullet impact images
            Images.bulletImpact = new Texture2D[2];
            for (int i = 0; i < 2; i++)
            {
                Images.bulletImpact[i] = Content.Load<Texture2D>("miscItems/bulletImpact/impact"+i);
            }

            //Load bullet impact images
            Images.explosion_overlay = new Texture2D[3];
            for (int i = 0; i < 3; i++)
            {
                Images.explosion_overlay[i] = Content.Load<Texture2D>("miscItems/explosion/explosion" + i);
            }


            // Towers
            Images.turret = new Texture2D[players, directions.Length, modelType];

            for (int i = 0; i < players; i++)
            {
                for (int j = 0; j < directions.Length; j++)
                {
                    for (int z = 0; z < modelType; z++)
                    {
                        Images.turret[i, j, z] = Content.Load<Texture2D>("turretItems/player" + (i + 1).ToString() + "turret_" + directions[j] + "_" + (z + 1).ToString() );
                    }
                }
            }

            frames = 2;
            Images.tower_shooting_overlay = new Texture2D[directions.Length, frames];
            for (int frame = 0; frame < frames; frame++)
            {
                for (int j = 0; j < directions.Length; j++)
                {
                    Images.tower_shooting_overlay[j, frame] = Content.Load<Texture2D>("turretItems/firing_animation/overlay_turret_firing_" + directions[j] + frame);
                }
            }

            ////Load the numerical pictures that can signal the value of a tile
            //for (int i = 0; i < Images.chargeCountImages.Length; i++)
            //{
            //    Images.chargeCountImages[i] = Content.Load<Texture2D>("chargeCountItems/" + (i + 1).ToString());
            //}
           
            Sounds.titleSong = Content.Load<Song>("musicItems/TitleSong");
            Sounds.gameSong1 = Content.Load<Song>("musicItems/GameSong1");
            Sounds.gameSong2 = Content.Load<Song>("musicItems/GameSong2");

            Sounds.logoSound = Content.Load<SoundEffect>("sfxItems/LogoSound");
            Sounds.actionSound1 = Content.Load<SoundEffect>("sfxItems/ActionSound1");

           // Sounds.selectUnit = Content.Load<SoundEffect>("sfxItems/selectunit");

            Sounds.selectUnitSounds = new SoundEffect[5];
            Sounds.selectUnitSounds[0] = Content.Load<SoundEffect>("voiceItems/Ezra_GoAhead_1");
            Sounds.selectUnitSounds[1] = Content.Load<SoundEffect>("voiceItems/Ezra_Sir_1");
            Sounds.selectUnitSounds[2] = Content.Load<SoundEffect>("voiceItems/Ezra_Standy_1");
            Sounds.selectUnitSounds[3] = Content.Load<SoundEffect>("voiceItems/Ezra_GoAhead_2");
            Sounds.selectUnitSounds[4] = Content.Load<SoundEffect>("voiceItems/Ezra_WhatDoYouWant_1");

            Sounds.attackOrderSounds = new SoundEffect[5];
            Sounds.attackOrderSounds[0] = Content.Load<SoundEffect>("voiceItems/Ezra_ConsidderItDone_1");
            Sounds.attackOrderSounds[1] = Content.Load<SoundEffect>("voiceItems/Ezra_Copy_1");
            Sounds.attackOrderSounds[2] = Content.Load<SoundEffect>("voiceItems/Ezra_Affirmative_1");
            Sounds.attackOrderSounds[3] = Content.Load<SoundEffect>("voiceItems/Ezra_Copy_2");
            Sounds.attackOrderSounds[4] = Content.Load<SoundEffect>("voiceItems/Ezra_Roger_1");


            Sounds.orderUnit = Content.Load<SoundEffect>("sfxItems/orderunit");

            Sounds.turretSound = Content.Load<SoundEffect>("sfxItems/turret0");


            Sounds.fireSounds = new SoundEffect[2];
            //Load the various laser sounds
            for (int i = 0; i < 2; i++)
            {
                Sounds.fireSounds[i] = Content.Load<SoundEffect>("sfxItems/gun"+i);
            }
            Sounds.explosionSounds = new SoundEffect[3];
            //Load the various laser sounds
            for (int i = 0; i < 3; i++)
            {
                Sounds.explosionSounds[i] = Content.Load<SoundEffect>("sfxItems/explosion" + i);
            }

            Sounds.alphabet = new SoundEffect[3, 26];
            int k = 1;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 26; j++)
                {
                    Sounds.alphabet[i, j] = Content.Load<SoundEffect>("voiceItems/alphabet/Ezra_" + CoordinateTool.alphabet[j] + "_" + k);
                }
                k++;
            }
            k = 1;
            Sounds.numbers = new SoundEffect[3, 10];
            for (int i = 0; i < 3; i++)
            {
                
                for (int j = 0; j < 10; j++)
                {
                    Sounds.numbers[i, j] = Content.Load<SoundEffect>("voiceItems/numbers/Ezra_" + j + "_" + k);
                }
                k++;
            }

            Sounds.baseExplosionSound = Content.Load<SoundEffect>("sfxItems/baseExplosion");


            Sounds.winSound = Content.Load<SoundEffect>("sfxItems/win");
            Sounds.loseSound = Content.Load<SoundEffect>("sfxItems/lose");


            //Set the inital state
            //state = new StateTitle();
            state = new StateIntro();

            //state = new StateTest();

            ION.get().IsMouseVisible = false;

            //Manualy initialize the first state
            state.focusGained();
        }

        //Here we load those resources that depend on the level's theme
        public void loadThemedResources(string name)
        {           
            Images.groundTexture = Content.Load<Texture2D>("tileItems/"+name);  
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            //// Allows the game to exit (#michiel XBox360 specific I think)
            //if (gamepad.getstate(playerindex.one).buttons.back == buttonstate.pressed)
            //    this.exit();

            KeyboardState keyState = Keyboard.GetState();

            //Lets you quit from anywhere in the game
            if (keyState.IsKeyDown(Keys.F1))
            {
                this.Exit();
            }

            //Lets you reset the game from anywhere
            if (keyState.IsKeyDown(Keys.R)&& keyState.IsKeyDown(Keys.LeftShift))
            {
                if (!restartPressed)
                {
                    setState(new StateIntro());
                }
                
                restartPressed = true;
            }
            if (keyState.IsKeyUp(Keys.R))
            {
                restartPressed = false;
            }

            state.update(gameTime.ElapsedGameTime.Milliseconds);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //This is propagated to the State class
            state.draw();

            base.Draw(gameTime);
        }

        public static bool onScreen(int x, int y)
        {
            if (x > 0 && x <= ION.width && y > 0 && y <= ION.height)
            {
                return true;
            }
            return false;
        }
    }
}
