using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Net;
using System.IO;
using FluorineFx.Net;
using FluorineFx.Messaging.Api.Service;

namespace ION
{
    class StateJoin : State
    {
        public enum SELECTION
        {
            JOIN = 1,
            BACK,
            REFRESH
        }
        public SELECTION selection = SELECTION.BACK;
       // private Host[] hosts;
        private int selectedHost=0;
        private static StateJoin instance;

        private Rectangle backButton;
        private Rectangle joinButton;
        private Rectangle refreshButton;

        private Rectangle hostsTable;
        private Rectangle TableColumnRoomname;
        private Rectangle TableColumnPlayers;
        private Rectangle TableColumnLevel;

        private Rectangle background_overlay;
        private Rectangle Logo;
        private Rectangle background_starfield;


        private bool mousePressed = false;
        public bool upPressed = false;
        public bool downPressed = false;
        public bool enterPressed = false;
        public bool wait = false;



        private Color fadeColor = Color.Orange;

        private Rectangle selected;

        private List<Rectangle> rows;
        
        //temp
        String[] tempHosts;




        public StateJoin()
        {
            instance = this;

            joinButton = new Rectangle(125, 125, Images.buttonJoin.Width, Images.buttonJoin.Height);
            backButton = new Rectangle(125, 200, Images.buttonBack.Width, Images.buttonBack.Height);
            //
            background_overlay = new Rectangle(ION.width - Images.background_overlay.Width, 0, Images.background_overlay.Width, Images.background_overlay.Height);
            Logo = new Rectangle(ION.width / 100 * 10, ION.height - ION.height / 100 * 7 - Images.Logo.Height, Images.Logo.Width, Images.Logo.Height);
            background_starfield = new Rectangle(0, 0, Images.background_starfield.Width, Images.background_starfield.Height);
            //
            hostsTable = new Rectangle(425, 125, Images.TableColumnRoomname.Width + Images.TableColumnPlayers.Width + Images.TableColumnLevel.Width, Images.TableColumnRoomname.Height);
            TableColumnRoomname = new Rectangle(hostsTable.X, hostsTable.Y, Images.TableColumnRoomname.Width, Images.TableColumnRoomname.Height);
            TableColumnPlayers = new Rectangle(hostsTable.X+Images.TableColumnRoomname.Width, hostsTable.Y, Images.TableColumnPlayers.Width, Images.TableColumnPlayers.Height);
            TableColumnLevel = new Rectangle(hostsTable.X + Images.TableColumnRoomname.Width+Images.TableColumnPlayers.Width, hostsTable.Y, Images.TableColumnLevel.Width, Images.TableColumnLevel.Height);
            //
            refreshButton = new Rectangle(hostsTable.Right - Images.buttonRefresh.Width, hostsTable.Bottom + 25, Images.buttonRefresh.Width, Images.buttonRefresh.Height);
            //
            rows = new List<Rectangle>();

            int nrows=6;
            int headerheight = 61;
            int rowheight = ((hostsTable.Height - headerheight) / nrows);
            for (int i = 0; i < nrows; i++)
            {
                rows.Add(new Rectangle(hostsTable.X, hostsTable.Y + (i * rowheight) + headerheight, hostsTable.Width, rowheight));
            }

            selected = rows[0];
           // for (int i = 0; i < 4; i++)
           // {

             //   ION.get().serverConnection.JoinRoom("room " + i);

            //}
            ION.get().serverConnection.getHosts();

        }

        public static StateJoin get()
        {
            return instance;
        }
        


        public override void draw()
        {
            ION.get().GraphicsDevice.Clear(Color.Black);
            ION.spriteBatch.Begin();

            //logo
            Double w = Math.Ceiling((Double)ION.width / (Double)Images.background_overlay.Width);
            Double h = Math.Ceiling((Double)ION.height / (Double)Images.background_overlay.Height);
            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    ION.spriteBatch.Draw(Images.background_starfield, new Rectangle(background_starfield.X + (i * background_starfield.Width), background_starfield.Y + (j * background_starfield.Height), background_starfield.Width, background_starfield.Height), Color.White);
                }
            }
            ION.spriteBatch.Draw(Images.background_overlay, background_overlay, Color.White);
            ION.spriteBatch.Draw(Images.Logo, Logo, Color.White);
            //
            ION.spriteBatch.Draw(Images.TableColumnRoomname, TableColumnRoomname, Color.White);
            ION.spriteBatch.Draw(Images.TableColumnPlayers, TableColumnPlayers, Color.White);
            ION.spriteBatch.Draw(Images.TableColumnLevel, TableColumnLevel, Color.White);
            //
            ION.spriteBatch.End();
            ION.primitiveBatch.Begin(PrimitiveType.LineList);
            for (int j = 1; j < rows.Count; j++)
            {
                ION.primitiveBatch.AddVertex(new Vector2(rows[j].Left, rows[j].Top), Color.White);
                ION.primitiveBatch.AddVertex(new Vector2(rows[j].Right, rows[j].Top), Color.White);
            }
            for (int k = 1; k < 3; k++)
            {
                ION.primitiveBatch.AddVertex(new Vector2(hostsTable.Left+k*(hostsTable.Width/3), hostsTable.Top), Color.White);
                ION.primitiveBatch.AddVertex(new Vector2(hostsTable.Left+k*(hostsTable.Width/3), hostsTable.Bottom), Color.White);
            }
            ION.primitiveBatch.End();
            ION.spriteBatch.Begin();
            //
            ION.spriteBatch.Draw(Images.white1px, selected, fadeColor);

            if (selection == SELECTION.BACK)
            {
                //Draw highlighted
                ION.spriteBatch.Draw(Images.buttonBackF, backButton, Color.White);
            }
            else
            {
                //Draw normally
                ION.spriteBatch.Draw(Images.buttonBack, backButton, Color.White);
            }
            if (selection == SELECTION.JOIN)
            {
                //Draw highlighted
                ION.spriteBatch.Draw(Images.buttonJoinF, joinButton, Color.White);
            }
            else
            {
                //Draw normally
                ION.spriteBatch.Draw(Images.buttonJoin, joinButton, Color.White);
            }


            if (selection == SELECTION.REFRESH)
            {
                //Draw highlighted
                ION.spriteBatch.Draw(Images.buttonRefreshF, refreshButton, Color.White);
            }
            else
            {
                //Draw normally
                ION.spriteBatch.Draw(Images.buttonRefresh, refreshButton, Color.White);
            }

            if (tempHosts != null)
            {

                fillTable();
            }
            ION.spriteBatch.End();
        }


        public override void update(int ellapsed)
        {
            

            //mouse handling
            MouseState mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                mousePressed = true;
            }
           

            if (mouseIn(mouseState.X, mouseState.Y, refreshButton))
            {
                selection = SELECTION.REFRESH;
                if (mouseState.LeftButton == ButtonState.Released && mousePressed == true)
                {
                    Console.WriteLine("refresh pressed");
                    makeSelection();
                    mousePressed = false;
                }

            }
            if (mouseIn(mouseState.X, mouseState.Y, joinButton))
            {
                selection = SELECTION.JOIN;
                if (mouseState.LeftButton == ButtonState.Released && mousePressed == true)
                {
                    makeSelection();
                    mousePressed = false;
                }

            }
            if (mouseIn(mouseState.X, mouseState.Y, backButton))
            {
                selection = SELECTION.BACK;
                if (mouseState.LeftButton == ButtonState.Released && mousePressed == true)
                {
                    makeSelection();
                    mousePressed = false;
                }

            }

            //row selection
            ION.spriteBatch.Begin();
            foreach (Rectangle row in rows)
            {
                if (mouseIn(mouseState.X, mouseState.Y, row)&& mouseState.LeftButton == ButtonState.Pressed==true)
                {
                    selectedHost = rows.IndexOf(row);
                    selected = row;
                    ION.spriteBatch.Draw(Images.white1px, selected, fadeColor);
                   
                }
            }
            
                
          

            ION.spriteBatch.End();



            //Keyboard handling
            KeyboardState keyState = Keyboard.GetState();


            if (keyState.IsKeyDown(Keys.Up) && !upPressed)
            {
                selectionUp();
                upPressed = true;
            }
            else if (keyState.IsKeyUp(Keys.Up) && upPressed)
            {
                upPressed = false;
            }


            if (keyState.IsKeyDown(Keys.Down) && !downPressed)
            {
                selectionDown();
                downPressed = true;
            }
            else if (keyState.IsKeyUp(Keys.Down) && downPressed)
            {
                downPressed = false;
            }

            if (keyState.IsKeyDown(Keys.Enter))
            {
                enterPressed = true;
            }

            if (keyState.IsKeyUp(Keys.Enter) && enterPressed == true)
            {
                enterPressed = false;
                makeSelection();
            }

            if (mouseState.LeftButton == ButtonState.Released && mousePressed == true)
            {
                mousePressed = false;
            }


        }


       

        private void makeSelection()
        {
            if (selection == SELECTION.REFRESH)
            {
                Console.WriteLine("servercon.gethosts()");
                ION.get().serverConnection.getHosts();
            }
            else if (selection == SELECTION.BACK)
            {
                StateMP st = new StateMP();

                ION.get().setState(st);
            }

            else if (selection == SELECTION.JOIN)
            {
                if (selectedHost < tempHosts.Length)
                    ION.get().serverConnection.JoinRoom(tempHosts[selectedHost]);
                else Console.WriteLine("no host found");
            }
        }

        private void selectionUp()
        {

            selection--;
            if (selection < SELECTION.JOIN)
            {
                Console.WriteLine("if sel: " + (int)selection);
                selection = (SELECTION)Enum.GetNames(typeof(SELECTION)).Length;
            }
            Console.WriteLine("sel: " + (int)selection);
        }

        private void selectionDown()
        {
            selection++;
            if ((int)selection > Enum.GetNames(typeof(SELECTION)).Length)
            {
                Console.WriteLine("if sel: " + (int)selection);
                selection = SELECTION.JOIN;
            }
            Console.WriteLine("sel: " + (int)selection);
        }


        //fills the table with server information
        private bool fillTable()
        {
          

            int row =0;
            //int collumn = 0;
           
            foreach(String s in tempHosts){
                ION.spriteBatch.DrawString(Fonts.font, s, new Vector2((rows[row].X + 15 ), (rows[row].Y) + 15), Color.White);
                row++;
                //ION.spriteBatch.DrawString(Fonts.font, "Return to menu? (Y/N)", new Vector2((ION.width / 2) - 100, (ION.height / 2)), Color.Red);

                //ION.spriteBatch.DrawString(Fonts.font, s, new Vector2((rows[row].X + 15 + collumn * 210), (rows[row].Y) + 15), Color.Green);
                //collumn++;
               // if (collumn > 2)
               // {
                   // collumn = 0;
                   // row++;
               // }
                


           }

            return false;
        }
        
        public void showHosts(String[] hostList)
        {

           // Console.WriteLine("lijstje uit serverConnection, eerste server: " + hostList[0].hostname);
            tempHosts = hostList;
            /*
            int i = 0;
            tempHosts = new String[hostList.Length];
            foreach (string s in hostList)
            {

                tempHosts[0, i] = s;
                i++;
            }
             */


        }

        public override void focusGained()
        {
            ION.get().IsMouseVisible = true;
            MediaPlayer.Play(Sounds.titleSong);
            MediaPlayer.IsRepeating = true;
        }


        public override void focusLost()
        {
            ION.get().IsMouseVisible = false;
            MediaPlayer.Stop();
        }

    }
}
