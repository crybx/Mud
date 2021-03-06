﻿using System;
using System.Collections.Generic;
using System.Text;
using Objects.Zone.Interface;
using Objects.Room.Interface;
using Objects.Mob.Interface;
using Objects.Mob;
using Objects.Zone;
using Objects.Item.Items.Interface;
using Objects.Personality.Personalities;
using Objects.Item.Items;
using Objects.Damage.Interface;
using Objects.Global;
using static Objects.Damage.Damage;
using Objects.Room;
using System.Reflection;
using System.Linq;
using static Objects.Global.Direction.Directions;
using Objects.Magic.Interface;
using Objects.Magic.Enchantment;
using Objects.Effect.Interface;
using Objects.Effect;
using Objects.Die;
using Objects.Language;
using static Shared.TagWrapper.TagWrapper;

namespace GenerateZones.Zones.DeepWoodForest
{
    public class AbandonedDwarvenMine : IZoneCode
    {
        Zone zone = new Zone();
        int roomId = 1;
        int itemId = 1;
        int npcId = 1;

        public IZone Generate()
        {
            zone.Id = 13;
            zone.InGameDaysTillReset = 5;
            zone.Name = nameof(AbandonedDwarvenMine);

            //int methodCount = this.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).Count();
            for (int i = 1; i <= 113; i++)
            {
                IRoom room = null;
                if (i >= 1 && i <= 6)
                {
                    room = OreCartStorage();
                }
                else if (i >= 12 && i <= 46)
                {
                    room = GoldMine();
                }
                else if (i >= 52 && i <= 57)
                {
                    room = GoldMineFloorRoom1();
                }
                else if (i >= 58 && i <= 63)
                {
                    room = GoldMineFloorRoom2();
                }
                else if (i >= 64 && i <= 72)
                {
                    room = GoldMineFloorRoom3();
                }
                else if (i >= 81 && i <= 86)
                {
                    room = GoldMineFloorRoom5();
                }
                else if (i >= 87 && i <= 88)
                {
                    room = GoldMineFloorRoom6();
                }
                else if (i >= 89 && i <= 92)
                {
                    room = GoldMineFloorRoom7();
                }
                else if (i >= 93 && i <= 105)
                {
                    room = GoldMineFloorConnectingTunnel();
                }

                else
                {
                    string methodName = "GenerateRoom" + i;
                    MethodInfo method = this.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);
                    if (method != null)
                    {
                        room = (IRoom)method.Invoke(this, null);
                    }
                }
                if (room != null)
                {
                    room.Zone = zone.Id;
                    ZoneHelper.AddRoom(zone, room);
                }
            }

            AddMobs();

            ConnectRooms();

            return zone;
        }

        private void AddMobs()
        {
            for (int i = 0; i < 40; i++)
            {
                INonPlayerCharacter npc = Shadow();
                if (i % 2 == 0)
                {
                    npc.Personalities.Add(new Wanderer());
                }

                int roomId = GlobalReference.GlobalValues.Random.Next(zone.Rooms.Count);
                IRoom room = zone.Rooms[roomId];
                room.AddMobileObjectToRoom(npc);
            }
        }

        private void ConnectRooms()
        {
            zone.RecursivelySetZone();

            #region Ore Cart Storage
            ZoneHelper.ConnectRoom(zone.Rooms[1], Direction.East, zone.Rooms[2]);
            ZoneHelper.ConnectRoom(zone.Rooms[1], Direction.South, zone.Rooms[3]);
            ZoneHelper.ConnectRoom(zone.Rooms[2], Direction.South, zone.Rooms[4]);
            ZoneHelper.ConnectRoom(zone.Rooms[3], Direction.East, zone.Rooms[4]);
            ZoneHelper.ConnectRoom(zone.Rooms[3], Direction.South, zone.Rooms[5]);
            ZoneHelper.ConnectRoom(zone.Rooms[4], Direction.South, zone.Rooms[6]);
            ZoneHelper.ConnectRoom(zone.Rooms[5], Direction.East, zone.Rooms[6]);
            #endregion Ore Cart Storage

            #region Path Ore Cart Storage -- Gold Mine
            ZoneHelper.ConnectRoom(zone.Rooms[1], Direction.West, zone.Rooms[7]);
            ZoneHelper.ConnectRoom(zone.Rooms[7], Direction.West, zone.Rooms[8]);
            ZoneHelper.ConnectRoom(zone.Rooms[8], Direction.West, zone.Rooms[9]);
            ZoneHelper.ConnectRoom(zone.Rooms[9], Direction.West, zone.Rooms[10]);
            ZoneHelper.ConnectRoom(zone.Rooms[10], Direction.West, zone.Rooms[11]);

            ZoneHelper.ConnectRoom(zone.Rooms[26], Direction.East, zone.Rooms[106]);
            ZoneHelper.ConnectRoom(zone.Rooms[106], Direction.East, zone.Rooms[107]);
            ZoneHelper.ConnectRoom(zone.Rooms[107], Direction.East, zone.Rooms[108]);
            ZoneHelper.ConnectRoom(zone.Rooms[108], Direction.North, zone.Rooms[109]);
            ZoneHelper.ConnectRoom(zone.Rooms[109], Direction.North, zone.Rooms[110]);
            ZoneHelper.ConnectRoom(zone.Rooms[110], Direction.North, zone.Rooms[111]);
            ZoneHelper.ConnectRoom(zone.Rooms[111], Direction.North, zone.Rooms[112]);
            ZoneHelper.ConnectRoom(zone.Rooms[112], Direction.North, zone.Rooms[8]);
            #endregion  Path Ore Cart Storage -- Gold Mine
            ZoneHelper.ConnectZone(zone.Rooms[108], Direction.East, 15, 1);


            #region Gold Mine
            #region Gold Pit
            for (int i = 11; i < 16; i++)
            {
                ZoneHelper.ConnectRoom(zone.Rooms[i], Direction.West, zone.Rooms[i + 1]);
            }

            for (int i = 16; i < 21; i++)
            {
                ZoneHelper.ConnectRoom(zone.Rooms[i], Direction.South, zone.Rooms[i + 1]);
            }

            for (int i = 21; i < 26; i++)
            {
                ZoneHelper.ConnectRoom(zone.Rooms[i], Direction.East, zone.Rooms[i + 1]);
            }

            for (int i = 26; i < 30; i++)
            {
                ZoneHelper.ConnectRoom(zone.Rooms[i], Direction.North, zone.Rooms[i + 1]);
            }

            ZoneHelper.ConnectRoom(zone.Rooms[30], Direction.Down, zone.Rooms[31]);

            for (int i = 31; i < 35; i++)
            {
                ZoneHelper.ConnectRoom(zone.Rooms[i], Direction.West, zone.Rooms[i + 1]);
            }

            for (int i = 35; i < 38; i++)
            {
                ZoneHelper.ConnectRoom(zone.Rooms[i], Direction.South, zone.Rooms[i + 1]);
            }

            for (int i = 38; i < 41; i++)
            {
                ZoneHelper.ConnectRoom(zone.Rooms[i], Direction.East, zone.Rooms[i + 1]);
            }

            for (int i = 41; i < 43; i++)
            {
                ZoneHelper.ConnectRoom(zone.Rooms[i], Direction.North, zone.Rooms[i + 1]);
            }

            ZoneHelper.ConnectRoom(zone.Rooms[43], Direction.Down, zone.Rooms[44]);

            for (int i = 44; i < 45; i++)
            {
                ZoneHelper.ConnectRoom(zone.Rooms[i], Direction.West, zone.Rooms[i + 1]);
            }

            for (int i = 45; i < 46; i++)
            {
                ZoneHelper.ConnectRoom(zone.Rooms[i], Direction.South, zone.Rooms[i + 1]);
            }

            for (int i = 46; i < 47; i++)
            {
                ZoneHelper.ConnectRoom(zone.Rooms[i], Direction.East, zone.Rooms[i + 1]);
            }
            #endregion Gold Pit

            #region Mine Shaft
            for (int i = 47; i <= 51; i++)
            {
                ZoneHelper.ConnectRoom(zone.Rooms[i], Direction.Down, zone.Rooms[i + 1]);
            }

            #endregion Mine Shaft

            #region Gold Mine Floor
            //Room 1
            ZoneHelper.ConnectRoom(zone.Rooms[52], Direction.East, zone.Rooms[53]);
            ZoneHelper.ConnectRoom(zone.Rooms[52], Direction.South, zone.Rooms[54]);
            ZoneHelper.ConnectRoom(zone.Rooms[52], Direction.North, zone.Rooms[56]);
            ZoneHelper.ConnectRoom(zone.Rooms[53], Direction.South, zone.Rooms[55]);
            ZoneHelper.ConnectRoom(zone.Rooms[53], Direction.North, zone.Rooms[57]);
            ZoneHelper.ConnectRoom(zone.Rooms[54], Direction.East, zone.Rooms[55]);
            ZoneHelper.ConnectRoom(zone.Rooms[56], Direction.East, zone.Rooms[57]);

            //Room 2
            ZoneHelper.ConnectRoom(zone.Rooms[58], Direction.East, zone.Rooms[59]);
            ZoneHelper.ConnectRoom(zone.Rooms[59], Direction.East, zone.Rooms[60]);
            ZoneHelper.ConnectRoom(zone.Rooms[61], Direction.East, zone.Rooms[62]);
            ZoneHelper.ConnectRoom(zone.Rooms[62], Direction.East, zone.Rooms[63]);
            ZoneHelper.ConnectRoom(zone.Rooms[58], Direction.South, zone.Rooms[61]);
            ZoneHelper.ConnectRoom(zone.Rooms[59], Direction.South, zone.Rooms[62]);
            ZoneHelper.ConnectRoom(zone.Rooms[60], Direction.South, zone.Rooms[63]);

            //Room 3
            ZoneHelper.ConnectRoom(zone.Rooms[64], Direction.East, zone.Rooms[65]);
            ZoneHelper.ConnectRoom(zone.Rooms[65], Direction.East, zone.Rooms[66]);
            ZoneHelper.ConnectRoom(zone.Rooms[67], Direction.East, zone.Rooms[68]);
            ZoneHelper.ConnectRoom(zone.Rooms[68], Direction.East, zone.Rooms[69]);
            ZoneHelper.ConnectRoom(zone.Rooms[70], Direction.East, zone.Rooms[71]);
            ZoneHelper.ConnectRoom(zone.Rooms[71], Direction.East, zone.Rooms[72]);

            ZoneHelper.ConnectRoom(zone.Rooms[70], Direction.North, zone.Rooms[67]);
            ZoneHelper.ConnectRoom(zone.Rooms[71], Direction.North, zone.Rooms[68]);
            ZoneHelper.ConnectRoom(zone.Rooms[72], Direction.North, zone.Rooms[69]);
            ZoneHelper.ConnectRoom(zone.Rooms[68], Direction.North, zone.Rooms[65]);

            //Room4
            ZoneHelper.ConnectRoom(zone.Rooms[73], Direction.East, zone.Rooms[74]);
            ZoneHelper.ConnectRoom(zone.Rooms[74], Direction.North, zone.Rooms[75]);
            ZoneHelper.ConnectRoom(zone.Rooms[75], Direction.North, zone.Rooms[76]);
            ZoneHelper.ConnectRoom(zone.Rooms[76], Direction.West, zone.Rooms[77]);
            ZoneHelper.ConnectRoom(zone.Rooms[77], Direction.West, zone.Rooms[78]);
            ZoneHelper.ConnectRoom(zone.Rooms[78], Direction.South, zone.Rooms[79]);
            ZoneHelper.ConnectRoom(zone.Rooms[79], Direction.South, zone.Rooms[80]);
            ZoneHelper.ConnectRoom(zone.Rooms[80], Direction.East, zone.Rooms[73]);

            //Room5
            ZoneHelper.ConnectRoom(zone.Rooms[86], Direction.West, zone.Rooms[85]);
            ZoneHelper.ConnectRoom(zone.Rooms[86], Direction.South, zone.Rooms[84]);
            ZoneHelper.ConnectRoom(zone.Rooms[85], Direction.South, zone.Rooms[83]);
            ZoneHelper.ConnectRoom(zone.Rooms[84], Direction.West, zone.Rooms[83]);
            ZoneHelper.ConnectRoom(zone.Rooms[84], Direction.South, zone.Rooms[82]);
            ZoneHelper.ConnectRoom(zone.Rooms[83], Direction.South, zone.Rooms[81]);
            ZoneHelper.ConnectRoom(zone.Rooms[82], Direction.West, zone.Rooms[81]);

            //Room6
            ZoneHelper.ConnectRoom(zone.Rooms[87], Direction.West, zone.Rooms[88]);

            //Room7
            ZoneHelper.ConnectRoom(zone.Rooms[89], Direction.East, zone.Rooms[90]);
            ZoneHelper.ConnectRoom(zone.Rooms[89], Direction.South, zone.Rooms[91]);
            ZoneHelper.ConnectRoom(zone.Rooms[91], Direction.East, zone.Rooms[92]);
            ZoneHelper.ConnectRoom(zone.Rooms[90], Direction.South, zone.Rooms[92]);

            //Room 1 to Room 2
            ZoneHelper.ConnectRoom(zone.Rooms[55], Direction.South, zone.Rooms[94]);
            ZoneHelper.ConnectRoom(zone.Rooms[94], Direction.South, zone.Rooms[95]);
            ZoneHelper.ConnectRoom(zone.Rooms[95], Direction.South, zone.Rooms[58]);

            //Room 2 to Room 3
            ZoneHelper.ConnectRoom(zone.Rooms[60], Direction.North, zone.Rooms[96]);
            ZoneHelper.ConnectRoom(zone.Rooms[96], Direction.East, zone.Rooms[97]);
            ZoneHelper.ConnectRoom(zone.Rooms[97], Direction.East, zone.Rooms[70]);

            //Room 3 to Room 4
            ZoneHelper.ConnectRoom(zone.Rooms[66], Direction.East, zone.Rooms[98]);
            ZoneHelper.ConnectRoom(zone.Rooms[98], Direction.North, zone.Rooms[99]);
            ZoneHelper.ConnectRoom(zone.Rooms[99], Direction.North, zone.Rooms[74]);

            //Room 1 to Room 5
            ZoneHelper.ConnectRoom(zone.Rooms[56], Direction.West, zone.Rooms[100]);
            ZoneHelper.ConnectRoom(zone.Rooms[100], Direction.West, zone.Rooms[101]);
            ZoneHelper.ConnectRoom(zone.Rooms[101], Direction.South, zone.Rooms[86]);

            //Room 5 to Room 6
            ZoneHelper.ConnectRoom(zone.Rooms[81], Direction.South, zone.Rooms[102]);
            ZoneHelper.ConnectRoom(zone.Rooms[102], Direction.West, zone.Rooms[103]);
            ZoneHelper.ConnectRoom(zone.Rooms[103], Direction.South, zone.Rooms[87]);

            //Room 5 to Room 7
            ZoneHelper.ConnectRoom(zone.Rooms[82], Direction.East, zone.Rooms[104]);
            ZoneHelper.ConnectRoom(zone.Rooms[104], Direction.South, zone.Rooms[105]);
            ZoneHelper.ConnectRoom(zone.Rooms[105], Direction.South, zone.Rooms[89]);

            #endregion Gold Mine Floor
            #endregion Gold Mine
        }

        #region Rooms
        #region Ore Cart Storage
        private IRoom OreCartStorage()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "Rows of mining cart tracks all converge and continue north and outside the room.";
            room.LongDescription = "Rows and rows of tracks can be seen indicating this is some type of mining cart storage area.";
            room.ShortDescription = "Ore Cart Storage.";

            return room;
        }
        #endregion Ore Cart Storage

        #region Path Ore Cart Storage -- Gold Mine
        private IRoom GenerateRoom7()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The ore track is beginning to rust from years of neglect.";
            room.LongDescription = "An ore track runs to the west off into the darkness and to the south.";
            room.ShortDescription = "Ore Track";

            return room;
        }
        private IRoom GenerateRoom8()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "Some type of fire happened here but it happened so long ago it would be hard to tell what it was.";
            room.LongDescription = "Some ashes lie on the mine floor here.";
            room.ShortDescription = "Ore Track";

            return room;
        }
        private IRoom GenerateRoom9()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The metal track has warped and bent in the intense heat of a fire.";
            room.LongDescription = "The walls of the mine have been covered in soot and the rail ties for the ore cars have been burned away.";
            room.ShortDescription = "Ore Track";

            return room;
        }
        private IRoom GenerateRoom10()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The sound of the water echo off the cave walls and reverberates down the tunnel.  The pool of water over flows a little with each drop making the floor wet before flowing into a crack in the wall to the north.";
            room.LongDescription = "A slow but steady drip falls into a shallow pool of water off to the side of the track.";
            room.ShortDescription = "Ore Track";

            return room;
        }
        private IRoom GenerateRoom11()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The walls still bares the scares of the pick axes used to carve out this tunnel.";
            room.LongDescription = "A small column of stone reaches the ceiling seeming to indicate the seem that the miners followed spit in two and then rejoined a dozen feet later.";
            room.ShortDescription = "Ore Track";

            return room;
        }
        private IRoom GenerateRoom106()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The mine does not appear to have collapsed but looks like it could.";
            room.LongDescription = "The ceiling has been reinforced several times here.  Possibly indicating a weak spot in the mine.";
            room.ShortDescription = "Ore Track";

            return room;
        }
        private IRoom GenerateRoom107()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The cave in appears like has been here a while but was never cleared.";
            room.LongDescription = "A tunnel to the south goes about five feet before a cave in seals the way.";
            room.ShortDescription = "Ore Track";

            return room;
        }
        private IRoom GenerateRoom108()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The mushrooms glow with a pale blue light that is to dim to be any more than a novelty.";
            room.LongDescription = "Small iridescent mushrooms glow faintly in the dark.";
            room.ShortDescription = "Ore Track";

            return room;
        }
        private IRoom GenerateRoom109()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The mushrooms glow with a pale blue light that is to dim to be any more than a novelty.";
            room.LongDescription = "Small iridescent mushrooms glow faintly in the dark.";
            room.ShortDescription = "Ore Track";

            return room;
        }
        private IRoom GenerateRoom110()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The steel track appears to have been cut out and dragged away.";
            room.LongDescription = "One side of the steel track is missing.";
            room.ShortDescription = "Ore Track";

            return room;
        }
        private IRoom GenerateRoom111()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The steel track appears to have been cut out and dragged away.";
            room.LongDescription = "The steel track has been removed from the area here.";
            room.ShortDescription = "Ore Track";

            return room;
        }
        private IRoom GenerateRoom112()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The barricade failed though and something got through.";
            room.LongDescription = "A make shift barricade was built here to hold back something.";
            room.ShortDescription = "Ore Track";

            return room;
        }
        private IRoom GenerateRoom113()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The paintings show a fiery monster coming up out of the ground and attacking miners.";
            room.LongDescription = "Several paintings are painted on the walls here.";
            room.ShortDescription = "Ore Track";

            return room;
        }
        #endregion Path Ore Cart Storage -- Gold Mine

        #region Gold Mine
        private IRoom GoldMine()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The fact that the earth does not collapse in filling the pit is a testament to the original dwarf miners ingenuity.";
            room.LongDescription = "You at the edge of a great big open pit mine.";
            room.ShortDescription = "Ore Track";

            return room;
        }

        #region Mine Shaft
        private IRoom GenerateRoom47()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The small tunnel leading down seems to squeeze in on you from all around.";
            room.LongDescription = "The roughly hewn mine shaft descends into the darkness below.";
            room.ShortDescription = "Dark Mine Shaft";

            return room;
        }

        private IRoom GenerateRoom48()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "Water can be dribbling down the shaft wall on the east.  Perhaps the miners hit a natural underground stream in their quest for gold.";
            room.LongDescription = "The roughly hewn mine shaft descends into the darkness below.";
            room.ShortDescription = "Dark Mine Shaft";

            return room;
        }

        private IRoom GenerateRoom49()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "You briefly hear a the sound of a far off cry of help as if someone is falling off a ladder and then silence.";
            room.LongDescription = "The roughly hewn mine shaft descends into the darkness below.";
            room.ShortDescription = "Dark Mine Shaft";

            return room;
        }

        private IRoom GenerateRoom50()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "Stopping momentarily on the ladder you feel a icy brush as if something feel past you.";
            room.LongDescription = "The roughly hewn mine shaft descends into the darkness below.";
            room.ShortDescription = "Dark Mine Shaft";

            return room;
        }

        private IRoom GenerateRoom51()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The walls still bear the marks of the pick axes that carved the shaft in search for more gold.";
            room.LongDescription = "The roughly hewn mine shaft descends into the darkness below.";
            room.ShortDescription = "Dark Mine Shaft";

            return room;
        }
        #endregion Mine Shaft

        #region Gold Mine Floor
        private IRoom GoldMineFloorRoom1()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "There cavern walls occasionally sparkle here hinting that there may still be gold in these cave walls.";
            room.LongDescription = "The room opens up into a large area hinting at a natural cavern of sorts.";
            room.ShortDescription = "Dark Mine Floor";

            return room;
        }

        private IRoom GoldMineFloorConnectingTunnel()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The tunnel walls are covered in soot as is something has been burned in it.";
            room.LongDescription = "The tunnel twists slightly slowly rising and falling as you continue to make your way through.";
            room.ShortDescription = "Dark Mine Floor";

            return room;
        }

        private IRoom GoldMineFloorRoom2()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "The room is about twenty five feet in height and forty feet in diameter.  The column is about two feet wide at the base and two inches in the middle.";
            room.LongDescription = "The room opens up again to a natural dome with a single pillar in the center where a stalactite and stalagmite have met.";
            room.ShortDescription = "Dark Mine Floor";

            return room;
        }

        private IRoom GoldMineFloorRoom3()
        {
            IRoom room = ZoneRoom(1);
            room.Attributes.Add(Room.RoomAttribute.Light);
            room.ExamineDescription = "The magma is slow and poses no immediate danger other than falling into the crevice.  The temperature of the room though has risen to a slightly warmish temperature.";
            room.LongDescription = "The cavern glows with a dull red as magma slowly flows from a hole in the wall to the east down into a deep crevice and to the west.";
            room.ShortDescription = "Dark Mine Floor";

            return room;
        }

        #region GoldMineFloorRoom4
        private IRoom GenerateRoom73()
        {
            IRoom room = ZoneRoom(1);
            room.Attributes.Add(Room.RoomAttribute.Light);
            room.ExamineDescription = "The stone throne to the north is huge standing sixteen feet to the seat and faces to the north.";
            room.LongDescription = "A large stone throne dominates the room.";
            room.ShortDescription = "Dark Mine Floor";

            return room;
        }

        private IRoom GenerateRoom74()
        {
            IRoom room = ZoneRoom(1);
            room.Attributes.Add(Room.RoomAttribute.Light);
            room.ExamineDescription = "The pedestal stands eight feet tall and has a large metal brazier with carvings of fire demons on it.";
            room.LongDescription = "A large pedestal supports a massive brazier giving light to the room.";
            room.ShortDescription = "Dark Mine Floor";

            return room;
        }

        private IRoom GenerateRoom75()
        {
            IRoom room = ZoneRoom(1);
            room.Attributes.Add(Room.RoomAttribute.Light);
            room.ExamineDescription = "The stone throne to the west is huge standing sixteen feet to the seat and faces to the north.";
            room.LongDescription = "A large stone throne dominates the room.";
            room.ShortDescription = "Dark Mine Floor";

            return room;
        }

        private IRoom GenerateRoom76()
        {
            IRoom room = ZoneRoom(1);
            room.Attributes.Add(Room.RoomAttribute.Light);
            room.ExamineDescription = "The pedestal stands eight feet tall and has a large metal brazier with carvings of fire demons on it.";
            room.LongDescription = "A large pedestal supports a massive brazier giving light to the room.";
            room.ShortDescription = "Dark Mine Floor";

            return room;
        }

        private IRoom GenerateRoom77()
        {
            IRoom room = ZoneRoom(1);
            room.Attributes.Add(Room.RoomAttribute.Light);
            room.ExamineDescription = "The stone throne to the south is huge standing sixteen feet to the seat and faces to the north.";
            room.LongDescription = "A large stone throne dominates the room.";
            room.ShortDescription = "Dark Mine Floor";

            room.AddMobileObjectToRoom(Balrog());

            return room;
        }

        private IRoom GenerateRoom78()
        {
            IRoom room = ZoneRoom(1);
            room.Attributes.Add(Room.RoomAttribute.Light);
            room.ExamineDescription = "The pedestal stands eight feet tall and has a large metal brazier with carvings of fire demons on it.";
            room.LongDescription = "A large pedestal supports a massive brazier giving light to the room.";
            room.ShortDescription = "Dark Mine Floor";

            return room;
        }

        private IRoom GenerateRoom79()
        {
            IRoom room = ZoneRoom(1);
            room.Attributes.Add(Room.RoomAttribute.Light);
            room.ExamineDescription = "The stone throne to the east is huge standing sixteen feet to the seat and faces to the north.";
            room.LongDescription = "A large stone throne dominates the room.";
            room.ShortDescription = "Dark Mine Floor";

            return room;
        }

        private IRoom GenerateRoom80()
        {
            IRoom room = ZoneRoom(1);
            room.Attributes.Add(Room.RoomAttribute.Light);
            room.ExamineDescription = "The pedestal stands eight feet tall and has a large metal brazier with carvings of fire demons on it.";
            room.LongDescription = "A large pedestal supports a massive brazier giving light to the room.";
            room.ShortDescription = "Dark Mine Floor";

            return room;
        }
        #endregion GoldMineFloorRoom4

        private IRoom GoldMineFloorRoom5()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "Dozen of reflections of yourself can be seen in the cavern walls.  Short and fat as well as tall and thin versions of yourself.";
            room.LongDescription = "The walls of the room are made of black obsidian glass creating a fun house effect with your reflection.";
            room.ShortDescription = "Dark Mine Floor";

            return room;
        }

        private IRoom GoldMineFloorRoom6()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "Hints of rich gold veins still sparkles through parts of the cavern walls.";
            room.LongDescription = "This small room seems to be mostly untouched by the dwarven miners.";
            room.ShortDescription = "Dark Mine Floor";

            return room;
        }

        private IRoom GoldMineFloorRoom7()
        {
            IRoom room = ZoneRoom(1);
            room.ExamineDescription = "It would seem to be that the dwarves stumbled upon a underground section of rock salt.";
            room.LongDescription = "Small salt crystals protrude from the cavern walls.";
            room.ShortDescription = "Dark Mine Floor";

            return room;
        }

        #endregion Gold Mine Floor
        #endregion Gold Mine

        private IRoom ZoneRoom(int movementCost)
        {
            IRoom room = new Room();
            room.Id = roomId++;
            room.MovementCost = movementCost;
            room.Attributes.Add(Room.RoomAttribute.Indoor);
            room.Attributes.Add(Room.RoomAttribute.NoLight);
            return room;
        }
        #endregion Rooms

        #region NPC
        private INonPlayerCharacter Balrog()
        {
            INonPlayerCharacter npc = new NonPlayerCharacter();
            npc.Id = npcId++;
            npc.Level = 87;
            npc.KeyWords.Add("Balrog");
            npc.Personalities.Add(new Aggressive());
            npc.Personalities.Add(new Wanderer());


            npc.ExamineDescription = "The demon is ablaze with fire will smoke hides its true form from view.";
            npc.LongDescription = "A large demon of fire and smoke standing twenty feet tall.";
            npc.ShortDescription = "A large flaming Balrog.";
            npc.SentenceDescription = "Balrog";

            npc.Enchantments.Add(FireAura());
            npc.AddEquipment(BalrogSword());

            npc.Race.Fire = decimal.MaxValue;
            npc.Race.Poison = decimal.MaxValue;
            npc.Race.Cold = 1.5M;
            npc.Race.Lightning = 1.5M;
            npc.Race.Bludgeon = 1.5M;
            npc.Race.Pierce = 1.5M;
            npc.Race.Slash = 1.5M;

            return npc;
        }

        private IEnchantment FireAura()
        {
            IEnchantment enchantment = new DamageDealtAfterDefenseEnchantment();
            IEffect effect = new Damage();
            IEffectParameter effectParameter = new EffectParameter();
            effectParameter.Message = new TranslationMessage("The fire from the Balrog burns you.");

            enchantment.ActivationPercent = 100;
            enchantment.Effect = effect;
            enchantment.Parameter = effectParameter;

            effectParameter.Damage = new Objects.Damage.Damage();
            effectParameter.Damage.Dice = GlobalReference.GlobalValues.DefaultValues.DiceForWeaponLevel(84);
            effectParameter.Damage.Type = DamageType.Fire;

            return enchantment;
        }

        private IWeapon BalrogSword()
        {
            IWeapon weapon = new Weapon();
            weapon.Id = itemId++;
            weapon.Level = 87;
            weapon.ItemPosition = Equipment.AvalableItemPosition.Wield;

            IDamage damage = new Objects.Damage.Damage();
            damage.Dice = GlobalReference.GlobalValues.DefaultValues.DiceForWeaponLevel(weapon.Level);
            damage.Type = DamageType.Fire;
            weapon.DamageList.Add(damage);

            damage = new Objects.Damage.Damage();
            damage.Dice = GlobalReference.GlobalValues.DefaultValues.DiceForWeaponLevel(weapon.Level);
            damage.Type = DamageType.Poison;
            weapon.DamageList.Add(damage);

            damage = new Objects.Damage.Damage();
            damage.Dice = GlobalReference.GlobalValues.DefaultValues.DiceForWeaponLevel(weapon.Level);
            damage.Type = DamageType.Slash;
            weapon.DamageList.Add(damage);

            weapon.RequiredHands = 2;
            weapon.KeyWords.Add("Balrog");
            weapon.KeyWords.Add("Sword");
            weapon.ExamineDescription = "As you get closer to the sword flames flair up engulfing the sword in fire and smoke choking the air and making it hard to determine its true size and shape.";
            weapon.LongDescription = "When the Balrog wielded the sword it had flames leaping from it, now it just smolders.";
            weapon.SentenceDescription = "sword";
            weapon.ShortDescription = "A large flaming sword.";

            return weapon;
        }

        private INonPlayerCharacter Shadow()
        {
            INonPlayerCharacter npc = new NonPlayerCharacter();
            npc.Id = npcId++;
            npc.Level = 14;
            npc.KeyWords.Add("Shadow");
            npc.Personalities.Add(new Wanderer());

            npc.ExamineDescription = "The dark figure is hard to see and blends into the shadows.";
            npc.LongDescription = "The figure seems to fade in and out of existence as it moves among the shadows.";
            npc.ShortDescription = "A shadowy figure.";
            npc.SentenceDescription = "shadow";

            npc.Race.Acid = 1.5M;
            npc.Race.Cold = 1.5M;
            npc.Race.Fire = 1.5M;
            npc.Race.Lightning = 1.5M;
            npc.Race.Thunder = 1.5M;
            npc.Race.Bludgeon = 1.5M;
            npc.Race.Pierce = 1.5M;
            npc.Race.Slash = 1.5M;

            npc.Race.Necrotic = Decimal.MaxValue;
            npc.Race.Poison = Decimal.MaxValue;

            npc.Race.Radiant = .5M;

            return npc;
        }
        #endregion NPC
    }
}
