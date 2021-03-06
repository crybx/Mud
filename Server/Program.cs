﻿using Mud;
using Objects.Global;
using ServerTelnetCommunication;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelnetCommunication;
using TelnetCommunication.Interface;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Pause Profiling");
            //Console.ReadLine();

            MudInstance instance = new MudInstance();
            LoadServerSettings();

            instance.StartMud();

            //Console.WriteLine("Restart Profiling");
            //Console.ReadLine();

            ConnectionHandler connectionHandler = new ConnectionHandler(new JsonMudMessage());
        }

        private static void LoadServerSettings()
        {
            //ConfigSettings c = new ConfigSettings();
            //using (TextWriter tw = new StreamWriter(@"C:\c#\Mud\Core\Server\AppConfig.json"))
            //{
            //    tw.Write(GlobalReference.GlobalValues.Serialization.Serialize(c));
            //}
            ConfigSettings config = GlobalReference.GlobalValues.Serialization.Deserialize<ConfigSettings>(File.ReadAllText("AppConfig.json"));

            GlobalReference.GlobalValues.Settings.AsciiArt = config.AsciiArt;
            GlobalReference.GlobalValues.Settings.PlayerCharacterDirectory = config.PlayerCharacterDirectory;
            GlobalReference.GlobalValues.Settings.ZoneDirectory = config.ZoneDirectory;
            GlobalReference.GlobalValues.Settings.AssetsDirectory = config.AssetsDirectory;
            GlobalReference.GlobalValues.Settings.Port = config.Port;
            GlobalReference.GlobalValues.Settings.SendMapPosition = config.SendMapPosition;

            GlobalReference.GlobalValues.Logger.Settings.LogDirectory = config.LogDirectory;
        }
    }
}
