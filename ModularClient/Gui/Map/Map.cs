﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TelnetCommunication;

namespace Client.Map
{
    public partial class Map : Form
    {
        private TelnetHandler _telnetHandler;
        public Map(TelnetHandler telnetHandler)
        {
            _telnetHandler = telnetHandler;
            InitializeComponent();
            pictureBox_Map.BackgroundImage = new Bitmap("map\\map.jpg");

            pictureBox_Map.BackColor = Color.Transparent;
        }

        public void Update(string rawMessage)
        {
            Message message = new Message(rawMessage);
            string fileName = Path.Combine("Map", $"{message.Zone}-{message.Z}.png");
            if (!File.Exists(fileName))
            {
                RequestMap(fileName);
                return;
            }
            else
            {
                Image oldImage = pictureBox_Map.Image;

                Image map = Image.FromFile(fileName);
                using (Graphics g = Graphics.FromImage(map))
                {
                    Brush brush = new SolidBrush(Color.Red);
                    g.FillRectangle(brush, int.Parse(message.X), ReverseY(int.Parse(message.Y), map) - 10, 10, 10);
                }
                pictureBox_Map.Image = map;

                oldImage?.Dispose();
            }
        }

        private float ReverseY(int y, Image map)
        {
            return map.Height - y;
        }

        private void RequestMap(string file)
        {
            _telnetHandler.OutQueue.Enqueue(string.Format("RequestAsset|Map|{0}", file));
        }

        private class Message
        {
            public string Zone { get; set; }
            public string Z { get; set; }
            public string X { get; set; }
            public string Y { get; set; }
            public Message(string message)
            {
                message = message.Replace("<Map>", "").Replace("</Map>", "");
                string[] array = message.Split('|');
                Zone = array[0];
                Z = array[1];
                X = array[2];
                Y = array[3];
            }
        }
    }
}
