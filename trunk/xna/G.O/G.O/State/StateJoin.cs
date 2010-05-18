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
            BACK = 1,
            JOIN,
            REFRESH

        }
        public SELECTION selection = SELECTION.BACK;
       // private Host[] hosts;
        private int selectedHost=0;
        private static StateJoin instance;

        private Rectangle backButton;
        private Rectangle hostsTable;
        private Rectangle joinButton;
        private Rectangle refreshButton;

        public Rectangle background_overlay;
        public Rectangle background_starfield;


        private bool mousePressed = false;
        public bool upPressed = false;
        public bool downPressed = false;
        public bool enterPressed = false;
        public bool wait = false;



        private Color fadeColor = new Color(0, 255, 255, 200);

        private Rectangle selected;

        private Rectangle row1;
        private Rectangle row2;
        private Rectangle row3;
        private Rectangle row4;
        private Rectangle row5;
        private Rectangle row6;
        private List<Rectangle> rows;
        
        //temp
        String[] tempHosts;




        public StateJoin()
        {
            instance = this;



            //tempHosts = new String[,] { { "server1", "room1", "grid1" }, { "server2", "room2", "grid2" }, { "server3", "room3", "grid3" } };
            backButton = new Rectangle((ION.width / 2) - 500, (ION.height / 2) +300, Images.buttonBack.Width, Images.buttonBack.Height);
            joinButton = new Rectangle((ION.width / 2) - 125, (ION.height / 2) + 300, Images.buttonJoin.Width, Images.buttonJoin.Height);
            refreshButton = new Rectangle((ION.width / 2) + 225, (ION.height / 2) + 300, Images.buttonRefresh.Width, Images.buttonRefresh.Height);
            hostsTable = new Rectangle((ION.width / 2) - 300, (ION.height / 2) - 70, Images.tableHosts.Width, Images.tableHosts.Height);
            //
            background_overlay = new Rectangle(ION.width - Images.background_overlay.Width, 0, Images.background_overlay.Width, Images.background_overlay.Height);
            background_starfield = new Rectangle(0, 0, Images.background_starfield.Width, Images.background_starfield.Height);
            //
            row1 = new Rectangle(hostsTable.X, hostsTable.Y + 35, 600, 50);
            row2 = new Rectangle(hostsTable.X, row1.Y + 50, 600, 50);
            row3 = new Rectangle(hostsTable.X, row1.Y + 100, 600, 50);
            row4 = new Rectangle(hostsTable.X, row1.Y + 150, 600, 50);
            row5 = new Rectangle(hostsTable.X, row1.Y + 200, 600, 50);
            row6 = new Rectangle(hostsTable.X, row1.Y + 250, 600, 50);

            rows = new List<Rectangle>();
           
            rows.Add(row1);
            rows.Add(row2);
            rows.Add(row3);
            rows.Add(row4);
            rows.Add(row5);
            rows.Add(row6);

            selected = row1;
           // for (int i = 0; i < 4; i++)
           // {

             //   ION.get().serverConnection.JoinRoom("room " + i);

            //}

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

            ION.spriteBatch.Draw(Images.tableHosts, hostsTable, Color.White);
            //ION.spriteBatch.DrawString(Fonts.font, title, new Vector2((ION.width / 2) - 15, (ION.height / 2) - 150), Color.White);
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
            if (selection < SELECTION.BACK)
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
                selection = SELECTION.BACK;
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
            MediaPlayer.Play(Music.titleSong);
            MediaPlayer.IsRepeating = true;
        }


        public override void focusLost()
        {
            ION.get().IsMouseVisible = false;
            MediaPlayer.Stop();
        }

    }
}
