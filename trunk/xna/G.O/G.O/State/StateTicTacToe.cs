using System;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FluorineFx.Net;
using System.IO;

namespace ION
{
    class StateTicTacToe : State
    {
        private CheckedState[,] grid;
        private Rectangle gridLocation;

        private RemoteSharedObject GridRSO;
        private const String GRIDRSONAME = "GridRsoName";

        public StateTicTacToe()
        {
            grid = new CheckedState[3, 3];

            //Stupid C# can't automatically call constructor for all
            //items in the array. :-(
            for (uint i=0; i < 3; i++)
            {
                for (uint j=0; j < 3; j++)
                {
                    grid[i, j] = new CheckedState();
                }
            }

            gridLocation = new Rectangle(100, 100, 300, 300);

            if (ION.instance.serverConnection != null && ION.instance.serverConnection.GameConnection != null)
            {
                GridRSO = RemoteSharedObject.GetRemote(GRIDRSONAME, ION.instance.serverConnection.GameConnection.Uri.ToString(), false);
                GridRSO.NetStatus += new NetStatusHandler(GridRSO_NetStatus);
                GridRSO.OnConnect += new ConnectHandler(GridRSO_OnConnect);
                GridRSO.OnDisconnect += new DisconnectHandler(GridRSO_OnDisconnect);
                GridRSO.Sync += new SyncHandler(GridRSO_Sync);
                GridRSO.Connect(ION.instance.serverConnection.GameConnection);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        void GridRSO_Sync(object sender, SyncEventArgs e)
        {
            Object[] remotegrid = (Object[])GridRSO.GetAttribute("grid");
            
            if (remotegrid != null)
            {
                grid = Serializer.DeserializeCheckedState(remotegrid);
            }
        }

        void GridRSO_OnDisconnect(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        void GridRSO_OnConnect(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        void GridRSO_NetStatus(object sender, NetStatusEventArgs e)
        {
            throw new NotImplementedException();
        }

        public override void draw()
        {
            ION.get().GraphicsDevice.Clear(Color.Black);

            ION.primitiveBatch.Begin(Microsoft.Xna.Framework.Graphics.PrimitiveType.LineList);

            ION.primitiveBatch.AddVertex(new Vector2(gridLocation.Left, gridLocation.Top), Color.White);
            ION.primitiveBatch.AddVertex(new Vector2(gridLocation.Right, gridLocation.Top), Color.White);

            ION.primitiveBatch.AddVertex(new Vector2(gridLocation.Right, gridLocation.Top), Color.White);
            ION.primitiveBatch.AddVertex(new Vector2(gridLocation.Right, gridLocation.Bottom), Color.White);

            ION.primitiveBatch.AddVertex(new Vector2(gridLocation.Right, gridLocation.Bottom), Color.White);
            ION.primitiveBatch.AddVertex(new Vector2(gridLocation.Left, gridLocation.Bottom), Color.White);

            ION.primitiveBatch.AddVertex(new Vector2(gridLocation.Left, gridLocation.Bottom), Color.White);
            ION.primitiveBatch.AddVertex(new Vector2(gridLocation.Left, gridLocation.Top), Color.White);

            ION.primitiveBatch.AddVertex(new Vector2(gridLocation.Left + gridLocation.Width / 3, gridLocation.Top), Color.White);
            ION.primitiveBatch.AddVertex(new Vector2(gridLocation.Left + gridLocation.Width / 3, gridLocation.Bottom), Color.White);

            ION.primitiveBatch.AddVertex(new Vector2(gridLocation.Left + gridLocation.Width / 3*2, gridLocation.Top), Color.White);
            ION.primitiveBatch.AddVertex(new Vector2(gridLocation.Left + gridLocation.Width / 3*2, gridLocation.Bottom), Color.White);

            ION.primitiveBatch.AddVertex(new Vector2(gridLocation.Left, gridLocation.Top + gridLocation.Height / 3), Color.White);
            ION.primitiveBatch.AddVertex(new Vector2(gridLocation.Right, gridLocation.Top + gridLocation.Height / 3), Color.White);

            ION.primitiveBatch.AddVertex(new Vector2(gridLocation.Left, gridLocation.Top + gridLocation.Height / 3*2), Color.White);
            ION.primitiveBatch.AddVertex(new Vector2(gridLocation.Right, gridLocation.Top + gridLocation.Height / 3*2), Color.White);

            ION.primitiveBatch.End();

            ION.spriteBatch.Begin();

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    switch (grid[i, j].CurrentState)
                    {
                        case CheckedState.States.CROSS:
                            ION.spriteBatch.Draw(Images.blueUnitImage, new Rectangle(gridLocation.X*j+gridLocation.Width/3, gridLocation.Y*i+gridLocation.Height/3, gridLocation.Width / 3, gridLocation.Height / 3), Color.White);
                            break;
                        case CheckedState.States.CIRCLE:
                            ION.spriteBatch.Draw(Images.redUnitImage, new Rectangle(gridLocation.X * j + gridLocation.Width / 3, gridLocation.Y * i + gridLocation.Height / 3, gridLocation.Width / 3, gridLocation.Height / 3), Color.White);
                            break;
                    }
                }
            }

            ION.spriteBatch.End();
        }

        public override void update(int elapsed)
        {
            MouseState mouseState = Mouse.GetState();


            if(Keyboard.GetState().IsKeyDown(Keys.Escape)) ION.get().setState(new StateTitle());
            int i, j = 0;

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (mouseState.X > gridLocation.Left && mouseState.X < gridLocation.Right && mouseState.Y > gridLocation.Top && mouseState.Y < gridLocation.Bottom)
                {
                    //we clicked inside the tictactoe grid, calculate which tile
                    if (mouseState.X < gridLocation.Left + gridLocation.Width / 3)
                    {
                        //proably the first row
                        j = 0;
                    }
                    else if (mouseState.X < gridLocation.Left + (gridLocation.Width / 3) * 2)
                    {
                        //proably the second row
                        j = 1;
                    }
                    else
                    {
                        //proably the third 
                        j = 2;
                    }

                    if (mouseState.Y < gridLocation.Top + gridLocation.Height / 3)
                    {
                        i = 0;
                    }
                    else if (mouseState.Y < gridLocation.Top + gridLocation.Height / 3 * 2)
                    {
                        i = 1;
                    }
                    else
                    {
                        i = 2;
                    }
                    CheckedState tile = grid[i, j];
                    if (!tile.Checked)
                    {
                        if (ION.instance.serverConnection.isHost)
                        {
                            tile.CurrentState = CheckedState.States.CROSS;
                        }
                        else
                        {
                            tile.CurrentState = CheckedState.States.CIRCLE;
                        }
                    }
                    GridRSO.SetAttribute("grid", Serializer.Serialize(grid));
                }
            }
        }

        public override void focusLost()
        {
            ION.get().IsMouseVisible = false;
        }

        public override void focusGained()
        {
            ION.get().IsMouseVisible = true;
        }
    }
}
