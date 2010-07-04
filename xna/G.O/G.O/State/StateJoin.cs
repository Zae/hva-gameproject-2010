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
using WindowSystem;

namespace ION
{
    class StateJoin : State
    {
        private int selectedHost=0;
        private static StateJoin instance;

        private List<UIComponent> ComponentList;

        private TextButton backButton;
        private TextButton joinButton;
        private TextButton refreshButton;

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

            ComponentList = new List<UIComponent>();

            joinButton = new TextButton(ION.instance, ION.instance.gui);
            joinButton.X = 125; joinButton.Y = 125;
            joinButton.Text = "Join";
            joinButton.Click += new ClickHandler(joinButton_Click);
            backButton = new TextButton(ION.instance, ION.instance.gui);
            backButton.X = 125; backButton.Y = 200;
            backButton.Text = "Back";
            backButton.Click += new ClickHandler(backButton_Click);
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
            rows = new List<Rectangle>();
            //
            refreshButton = new TextButton(ION.instance, ION.instance.gui);
            refreshButton.X = hostsTable.Right-refreshButton.Width; refreshButton.Y = hostsTable.Bottom + 25;
            refreshButton.Text = "Refresh";
            refreshButton.Click += new ClickHandler(refreshButton_Click);
            //
            ComponentList.Add(joinButton);
            ComponentList.Add(backButton);
            ComponentList.Add(refreshButton);
            //
            int nrows=6;
            int headerheight = 61;
            int rowheight = ((hostsTable.Height - headerheight) / nrows);
            for (int i = 0; i < nrows; i++)
            {
                rows.Add(new Rectangle(hostsTable.X, hostsTable.Y + (i * rowheight) + headerheight, hostsTable.Width, rowheight));
            }

            selected = rows[0];
            ION.get().serverConnection.getHosts();
        }

        void refreshButton_Click(UIComponent sender)
        {
            ION.get().serverConnection.getHosts();
        }

        void backButton_Click(UIComponent sender)
        {
            ION.get().setState(new StateMP());
        }

        void joinButton_Click(UIComponent sender)
        {
            if (selectedHost < tempHosts.Length)
                ION.get().serverConnection.JoinRoom(tempHosts[selectedHost]);
            else Console.WriteLine("no host found");
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

            if (tempHosts != null)
            {

                fillTable();
            }
            ION.spriteBatch.End();
        }

        public override void update(int ellapsed)
        {
            //mouse handling
            Microsoft.Xna.Framework.Input.MouseState mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                mousePressed = true;
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

           
            tempHosts = hostList;
            

        }

        public override void focusGained()
        {
            foreach (UIComponent uicomponent in ComponentList)
            {
                ION.instance.gui.Add(uicomponent);
            }
            //
            MediaPlayer.Play(Sounds.titleSong);
            MediaPlayer.IsRepeating = true;
        }

        public override void focusLost()
        {
            foreach (UIComponent uicomponent in ComponentList)
            {
                ION.instance.gui.Remove(uicomponent);
            }
            //
            MediaPlayer.Stop();
        }

    }
}
