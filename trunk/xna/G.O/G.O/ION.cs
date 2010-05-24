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

            //Setup the graphics configuration to match the client
            graphics = new GraphicsDeviceManager(this);
            graphics.PreparingDeviceSettings += new EventHandler<PreparingDeviceSettingsEventArgs>(preparingDeviceSettings);
            graphics.ToggleFullScreen();
            graphics.ToggleFullScreen(); //Toggle again to disable fullscreen

            //Now we can get this info without errors
            width = graphics.GraphicsDevice.DisplayMode.Width;
            halfWidth = width / 2;
            height = graphics.GraphicsDevice.DisplayMode.Height;
            halfHeight = height / 2;

            //Give the Content class a valid content root directory
            Content.RootDirectory = "Content";
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
           
            //Loading Fonts
            Fonts.font = Content.Load<SpriteFont>("fontItems/TitleFont");
            Fonts.small = Content.Load<SpriteFont>("fontItems/SmallFont");

            //Load Misc items
            Images.teamLogoImage = Content.Load<Texture2D>("miscItems/logo-game-ninjas");
            Images.white1px = Content.Load<Texture2D>("toolItems/white");
            Images.greenPixel = Content.Load<Texture2D>("menuItems/greenPixel");
            Images.helpFile = Content.Load<Texture2D>("miscItems/helpfile");            
            Images.gameBackground = Content.Load<Texture2D>("miscItems/blue_red");

            //GUI Items
            Images.commandsBar = Content.Load<Texture2D>("guiItems/commandsBar");
            Images.moveButtonNormal = Content.Load<Texture2D>("guiItems/moveButtonNormal");
            Images.attackButtonNormal = Content.Load<Texture2D>("guiItems/attackButtonNormal");
            Images.stopButtonNormal = Content.Load<Texture2D>("guiItems/stopButtonNormal");
            Images.defensiveButtonNormal = Content.Load<Texture2D>("guiItems/defensiveButtonNormal");
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


            ////Title Menu
            //Images.ION_LOGO = Content.Load<Texture2D>("menuItems/ION_LOGO");
            //Images.buttonNewGame = Content.Load<Texture2D>("menuItems/newGameButton");
            //Images.buttonNewGameF = Content.Load<Texture2D>("menuItems/newGameButtonF");
            //Images.buttonMP = Content.Load<Texture2D>("menuItems/mpButton");
            //Images.buttonMPF = Content.Load<Texture2D>("menuItems/mpButtonF");
            //Images.buttonQuit = Content.Load<Texture2D>("menuItems/quitButton");
            //Images.buttonQuitF = Content.Load<Texture2D>("menuItems/quitButtonF");

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

            ////MultiPlayer menu
            //Images.buttonJoin = Content.Load<Texture2D>("menuItems/JoinButton");
            //Images.buttonJoinF = Content.Load<Texture2D>("menuItems/JoinButtonF");
            //Images.buttonHost = Content.Load<Texture2D>("menuItems/HostButton");
            //Images.buttonHostF = Content.Load<Texture2D>("menuItems/HostButtonF");
            //Images.buttonBack = Content.Load<Texture2D>("menuItems/BackButton");
            //Images.buttonBackF = Content.Load<Texture2D>("menuItems/BackButtonF");

            //Load Tile images
            Images.borderImage = Content.Load<Texture2D>("tileItems/border_tile");
            Images.mountainImage = Content.Load<Texture2D>("tileItems/mountain_tile");           
            Images.resourceImage = Content.Load<Texture2D>("tileItems/resource_tile");
            
            //Load Base images           
            Images.baseImage = Content.Load<Texture2D>("tileItems/base_tile");
            Images.blueBaseImage = Content.Load<Texture2D>("playerItems/bluebase");
            Images.redBaseImage = Content.Load<Texture2D>("playerItems/redBase");

            //Load Tool images 
            Images.tileHitmapImage = Content.Load<Texture2D>("toolItems/tile_hitmap");

            ////Join menu
            //Images.buttonRefresh = Content.Load<Texture2D>("menuItems/RefreshButton");
            //Images.buttonRefreshF = Content.Load<Texture2D>("menuItems/RefreshButtonF");

            //Join menu
            Images.buttonRefresh = Content.Load<Texture2D>("menuItems/btn_refresh");
            Images.buttonRefreshF = Content.Load<Texture2D>("menuItems/btn_refresh_hover");
            Images.TableColumnRoomname = Content.Load<Texture2D>("menuItems/Roomname");
            Images.TableColumnPlayers = Content.Load<Texture2D>("menuItems/Players");
            Images.TableColumnLevel = Content.Load<Texture2D>("menuItems/Level");

            //Load Unit images
            Images.unitWayPoint = Content.Load<Texture2D>("unitItems/greenArrow");

            int players = 2;
            string[] directions = new string[] { "s", "se", "e", "ne", "n", "nw", "w", "sw" };

            Images.unit = new Texture2D[players, directions.Length];
            Images.unit_selected = new Texture2D[players, directions.Length];

            for (int i = 0; i < players; i++)
            {
                for (int j = 0; j < directions.Length; j++)
                {
                    Images.unit[i, j] = Images.chargeCountImages[i] = Content.Load<Texture2D>("unitItems/player" + (i + 1).ToString() + "unit_" + directions[j]);
                    Images.unit_selected[i, j] = Images.chargeCountImages[i] = Content.Load<Texture2D>("unitItems/player" + (i + 1).ToString() + "unit_selected_" + directions[j]);
                }
            }

            int frames = 2;
            Images.unit_shooting = new Texture2D[players, directions.Length, 2];
            Images.unit_selected_shooting = new Texture2D[players, directions.Length, 2];

            for (int frame = 0; frame < frames; frame++)
            {
                for (int i = 0; i < players; i++)
                {
                    for (int j = 0; j < directions.Length; j++)
                    {
                        Images.unit_shooting[i, j, frame] = Content.Load<Texture2D>("unitItems/shooting/player" + (i + 1).ToString() + "unit_firing_" + directions[j]+frame);
                        Images.unit_selected_shooting[i, j, frame] = Content.Load<Texture2D>("unitItems/shooting/player" + (i + 1).ToString() + "unit_selected_firing_" + directions[j]+frame);
                    }
                }
            }

            Images.bulletImpact = new Texture2D[2];

            for (int i = 0; i < 2; i++)
            {
                Images.bulletImpact[i] = Content.Load<Texture2D>("miscItems/bulletImpact/impact"+i);
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

            Sounds.fireSounds = new SoundEffect[2];
            //Load the various laser sounds
            for (int i = 0; i < 2; i++)
            {
                Sounds.fireSounds[i] = Content.Load<SoundEffect>("sfxItems/laser"+i);
            }

            //Set the inital state
            state = new StateTitle();
            //state = new StateIntro();
            //state = new StateTest();

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
