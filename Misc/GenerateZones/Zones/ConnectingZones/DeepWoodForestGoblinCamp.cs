﻿using Objects.Room;
using Objects.Room.Interface;
using Objects.Zone;
using Objects.Zone.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using static Objects.Global.Direction.Directions;

namespace GenerateZones.Zones.ConnectingZones
{
    public class DeepWoodForestGoblinCamp : IZoneCode
    {
        Zone zone = new Zone();
        private int roomId = 1;
        //private int npcId = 1;
        private int zoneId = 17;

        IZone IZoneCode.Generate()
        {
            zone.InGameDaysTillReset = 5;
            zone.Id = zoneId;
            zone.Name = nameof(DeepWoodForestGoblinCamp);

            int methodCount = this.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).Count();
            for (int i = 1; i <= methodCount; i++)
            {
                string methodName = "GenerateRoom" + i;
                MethodInfo method = this.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);
                if (method != null)
                {
                    IRoom room = (IRoom)method.Invoke(this, null);
                    room.Zone = zone.Id;
                    ZoneHelper.AddRoom(zone, room);
                }
            }

            //AddMobs();

            ConnectRooms();

            return zone;
        }

        private void ConnectRooms()
        {
            zone.RecursivelySetZone();

            ZoneHelper.ConnectRoom(zone.Rooms[1], Direction.North, zone.Rooms[2]);
            ZoneHelper.ConnectRoom(zone.Rooms[2], Direction.North, zone.Rooms[3]);
            ZoneHelper.ConnectRoom(zone.Rooms[3], Direction.North, zone.Rooms[4]);
            ZoneHelper.ConnectRoom(zone.Rooms[4], Direction.West, zone.Rooms[5]);
            ZoneHelper.ConnectRoom(zone.Rooms[5], Direction.West, zone.Rooms[6]);
            ZoneHelper.ConnectRoom(zone.Rooms[6], Direction.South, zone.Rooms[7]);
        }

        #region Rooms
        private IRoom ZoneRoom(int movementCost)
        {
            IRoom room = new Room();
            room.Id = roomId++;
            room.MovementCost = movementCost;
            room.Attributes.Add(Room.RoomAttribute.Outdoor);
            return room;
        }

        private IRoom GenerateRoom1()
        {
            IRoom room = ZoneRoom(100);
            room.ExamineDescription = "The ravine is narrow enough to block out a lot of light making it seem darker than it really is.";
            room.LongDescription = "Steep rock walls tower above you on both sides.";
            room.ShortDescription = "In a ravine";

            return room;
        }

        private IRoom GenerateRoom2()
        {
            IRoom room = ZoneRoom(100);
            room.ExamineDescription = "The pass narrows and makes a series of zig zags making it impossible to see forward or back.";
            room.LongDescription = "The walls are jagged enough to climb twenty or thirty feet but then the next sixty or seventy is smooth making it impossible to climb.";
            room.ShortDescription = "In a ravine";

            room.Attributes.Add(Room.RoomAttribute.NoNPC);
            return room;
        }

        private IRoom GenerateRoom3()
        {
            IRoom room = ZoneRoom(100);
            room.ExamineDescription = "The path through the ravine seems surprisingly barren of plant life.";
            room.LongDescription = "The ravine narrows enough that you have to turn sideways to continue on.";
            room.ShortDescription = "In a ravine";

            return room;
        }

        private IRoom GenerateRoom4()
        {
            IRoom room = ZoneRoom(100);
            room.ExamineDescription = "The path to the East rises slightly before turning out of sight.  The path to the West descends slightly.  The path to the South also rises slightly.";
            room.LongDescription = "Here the path splits forming a T.";
            room.ShortDescription = "In a ravine";

            return room;
        }

        private IRoom GenerateRoom5()
        {
            IRoom room = ZoneRoom(100);
            room.ExamineDescription = "The markings are several round Os made in some kind of red paint.";
            room.LongDescription = "There are several markings on the ravine wall.";
            room.ShortDescription = "In a ravine";

            return room;
        }

        private IRoom GenerateRoom6()
        {
            IRoom room = ZoneRoom(100);
            room.ExamineDescription = "The cave seems to emanate a foul earthy smell.";
            room.LongDescription = "Several bones lie scattered on the ground around the entrance to a cave.";
            room.ShortDescription = "In a ravine";

            return room;
        }

        private IRoom GenerateRoom7()
        {
            IRoom room = ZoneRoom(100);
            room.ExamineDescription = "There is a half eaten goat in one corner along with several other unidentifiable animals.";
            room.LongDescription = "The cave has a large pile of straw in one corner.";
            room.ShortDescription = "In a ravine";

            return room;
        }
        #endregion Rooms
    }
}
