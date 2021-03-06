﻿using Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using Objects.Magic;
using System.Reflection;
using Objects.Zone;
using Objects.Zone.Interface;
using Objects.Room;
using Objects.Damage;
using Objects.Global.DefaultValues;
using Objects.Personality.Personalities;
using static Objects.Guild.Guild;
using Objects.Global.Stats;
using Objects.Item.Items;
using Objects.Global;
using Objects.Material.Materials;
using Objects.Mob;
using static Objects.Global.Direction.Directions;
using Objects.Mob.Interface;
using Objects.Room.Interface;
using Objects.Personality.Personalities.Interface;
using Objects.Item.Items.Interface;

namespace GenerateZones.Zones
{
    public class GrandViewLibrary : IZoneCode
    {
        Zone zone = new Zone();
        int roomId = 1;
        int itemId = 1;
        int npcId = 1;
        public IZone Generate()
        {
            zone.Id = 2;
            zone.InGameDaysTillReset = 1;
            zone.Name = nameof(GrandViewLibrary);

            int methodCount = this.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).Count();
            for (int i = 1; i <= methodCount; i++)
            {
                string methodName = "GenerateRoom" + i;
                MethodInfo method = this.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);
                if (method != null)
                {
                    Room room = (Room)method.Invoke(this, null);
                    room.Zone = zone.Id;
                    ZoneHelper.AddRoom(zone, room);
                }
            }

            ConnectRooms();

            return zone;
        }

        #region Rooms
        #region Library Basement

        private IRoom GenerateRoom1()
        {
            IRoom room = GenerateRoom();
            room.MovementCost = 1;
            room.Attributes.Add(Room.RoomAttribute.Indoor);
            room.Attributes.Add(Room.RoomAttribute.Light);
            room.Attributes.Add(Room.RoomAttribute.NoNPC);

            room.ExamineDescription = "The floor is a beautiful mosaic of the surrounding areas.  The mountains to the north and west and the forest to the east are both represented. For some reason the map maker left out the south.  Still the map must be old because fort Woodbrook is shown miles from the forest and it has long since been overgrown and lies deep in heart of the forest.";
            room.LongDescription = "The entrance to the library is a sandstone entry way.  The ceiling is domed and has \"Cave ab homine unius libri.\" written on it.  The floor is a mosaic of the surrounding lands.";
            room.ShortDescription = "Entrance to the great library";
            return room;
        }

        private IRoom GenerateRoom()
        {
            IRoom room = new Room();
            room.Id = roomId++;
            return room;
        }

        private IRoom GenerateRoom2()
        {
            return LibraryBasement();
        }

        private IRoom GenerateRoom3()
        {
            return LibraryBasement();
        }

        private IRoom GenerateRoom4()
        {
            return LibraryBasement();
        }

        private IRoom GenerateRoom5()
        {
            return LibraryBasement();
        }

        private IRoom GenerateRoom6()
        {
            return LibraryBasement();
        }

        private IRoom GenerateRoom7()
        {
            IRoom room = GenerateRoom();
            room.MovementCost = 1;
            room.Attributes.Add(Room.RoomAttribute.Indoor);
            room.Attributes.Add(Room.RoomAttribute.Light);

            room.ExamineDescription = "This corner of the library is used by the mages as their guild hall.  Scrolls and books are scattered about with stacks ranging from a few feet to as hight as the ceiling.";
            room.LongDescription = "This corner of the basement is designated as the mages guild.  Dimly lit candles burn at desks with scrolls inviting practitioners of magic to learn something new.";
            room.ShortDescription = "Library Basement";

            INonPlayerCharacter guildMaster = MageGuildMaster();

            room.AddMobileObjectToRoom(guildMaster);

            return room;
        }

        private INonPlayerCharacter MageGuildMaster()
        {
            INonPlayerCharacter npc = new NonPlayerCharacter();
            npc.Id = npcId++;
            npc.Level = 20;
            npc.ExamineDescription = "The Guildmaster is dressed in a tattered gray cloak that looks to at one point been white.  He has a beard that is almost as long as he is tall and is long since lost any sign of color.";
            npc.LongDescription = "He stares into space as if contemplating things you couldn't even imagine.  Occasionally he says something as if he is talking to someone yet you can not see who.  Has he gone mad or talking to something beyond this realm?";
            npc.ShortDescription = "The mage Guildmaster.";
            npc.SentenceDescription = "Guildmaster";
            npc.KeyWords.Add("GuildMaster");
            npc.KeyWords.Add("Mage");
            npc.Personalities.Add(new GuildMaster(Guilds.Mage));
            return npc;
        }

        private IRoom GenerateRoom8()
        {
            return LibraryBasement();
        }

        private IRoom GenerateRoom9()
        {
            IRoom room = LibraryBasement();

            INonPlayerCharacter apprentice = Male_Apprentice();
            room.AddMobileObjectToRoom(apprentice);
            apprentice.Room = room;

            return room;
        }

        private IRoom GenerateRoom10()
        {
            return LibraryBasement();
        }

        private IRoom GenerateRoom11()
        {
            return LibraryBasement();
        }

        private IRoom GenerateRoom12()
        {
            return LibraryBasement();
        }

        private IRoom GenerateRoom13()
        {
            return LibraryBasement();
        }

        private IRoom GenerateRoom14()
        {
            return LibraryBasement();
        }

        private IRoom GenerateRoom15()
        {
            return LibraryBasement();
        }

        private IRoom GenerateRoom16()
        {
            return LibraryBasement();
        }

        private IRoom GenerateRoom17()
        {
            IRoom room = LibraryBasement();

            INonPlayerCharacter apprentice = Female_Apprentice();
            room.AddMobileObjectToRoom(apprentice);
            apprentice.Room = room;
            apprentice.AddEquipment(WizardStaff());

            return room;
        }

        private IEquipment WizardStaff()
        {
            IWeapon staff = new Weapon();
            staff.Id = itemId++;

            staff.Level = 1;
            staff.ExamineDescription = "Examining the staff reveals the slight shimmer is a thin layer of frost.  The head of the staff is emitting extreme cold that could useful in battle or drinks at parties.";
            staff.LongDescription = "The gnarled staff is twisted age seems to have a slight shimmer at the head of the staff.";
            staff.ShortDescription = "A wizards staff hewn from an oak tree.";
            staff.SentenceDescription = "wizard staff";
            staff.KeyWords.Add("staff");
            staff.KeyWords.Add("ice");
            staff.AttackerStat = Stats.Stat.Dexterity;
            staff.DeffenderStat = Stats.Stat.Dexterity;

            Damage damage = new Damage();
            damage.Dice = GlobalReference.GlobalValues.DefaultValues.DiceForWeaponLevel(staff.Level);
            damage.Type = Damage.DamageType.Bludgeon;
            staff.DamageList.Add(damage);

            damage = new Damage();
            damage.Dice = GlobalReference.GlobalValues.DefaultValues.DiceForWeaponLevel(staff.Level);
            damage.Type = Damage.DamageType.Cold;
            damage.BonusDamageStat = Stats.Stat.Intelligence;
            staff.DamageList.Add(damage);

            return staff;
        }

        private IRoom GenerateRoom18()
        {
            return LibraryBasement();
        }

        private IRoom GenerateRoom19()
        {
            return LibraryBasement();
        }

        private IRoom GenerateRoom20()
        {
            IRoom room = LibraryBasement();

            INonPlayerCharacter apprentice = Female_Apprentice();
            room.AddMobileObjectToRoom(apprentice);
            apprentice.Room = room;

            return room;
        }

        private IRoom GenerateRoom21()
        {
            return LibraryBasement();
        }

        private IRoom GenerateRoom22()
        {
            return LibraryBasement();
        }

        private IRoom GenerateRoom23()
        {
            return LibraryBasement();
        }

        private IRoom GenerateRoom24()
        {
            return LibraryBasement();
        }

        private IRoom GenerateRoom25()
        {
            return LibraryBasement();
        }

        private IRoom GenerateRoom26()
        {
            return LibraryBasement();
        }

        private IRoom GenerateRoom27()
        {
            return LibraryBasement();
        }

        private IRoom GenerateRoom28()
        {
            return LibraryBasement();
        }

        private IRoom GenerateRoom29()
        {
            IRoom room = LibraryBasement();

            INonPlayerCharacter apprentice = Male_Apprentice();
            room.AddMobileObjectToRoom(apprentice);
            apprentice.Room = room;

            return room;
        }

        private IRoom GenerateRoom30()
        {
            return LibraryBasement();
        }

        private IRoom GenerateRoom31()
        {
            return LibraryBasement();
        }

        private IRoom GenerateRoom32()
        {
            return LibraryBasement();
        }

        private IRoom GenerateRoom33()
        {
            return LibraryBasement();
        }

        private IRoom GenerateRoom34()
        {
            IRoom room = LibraryBasement();

            INonPlayerCharacter apprentice = Female_Apprentice();
            room.AddMobileObjectToRoom(apprentice);
            apprentice.Room = room;

            return room;
        }

        private IRoom GenerateRoom35()
        {
            return LibraryBasement();
        }

        private IRoom GenerateRoom36()
        {
            return LibraryBasement();
        }

        private IRoom GenerateRoom37()
        {
            IRoom room = LibraryBasement();

            INonPlayerCharacter apprentice = Male_Apprentice();
            room.AddMobileObjectToRoom(apprentice);
            apprentice.Room = room;

            apprentice.AddEquipment(Ring());

            return room;
        }

        private IRoom LibraryBasement()
        {
            IRoom room = GenerateRoom();
            room.MovementCost = 1;
            room.Attributes.Add(Room.RoomAttribute.Indoor);
            room.Attributes.Add(Room.RoomAttribute.Light);

            room.ExamineDescription = "The stairs are eerily quiet, to quiet to be exact.  Perhaps since it is a library there is some magic that helps maintain the quietness.";
            room.LongDescription = "Worn stone steps connect the basement to the entrance of the library.";
            room.ShortDescription = "Basement Stairs";
            return room;
        }

        private Equipment Ring()
        {
            Armor ring = new Armor();
            ring.Material = new Gold();

            ring.Id = itemId++;
            ring.Level = 1;
            ring.ExamineDescription = "You throughly examine the ring but can find nothing of interest.  It appears to be nothing more than a gold ring.";
            ring.LongDescription = "A small round gold ring which otherwise is quite ordinary.";
            ring.ShortDescription = "A small gold ring.";
            ring.SentenceDescription = "gold ring";
            ring.KeyWords.Add("gold");
            ring.KeyWords.Add("ring");
            ring.Dice = GlobalReference.GlobalValues.DefaultValues.DiceForArmorLevel(ring.Level);

            return ring;
        }

        private INonPlayerCharacter Female_Apprentice()
        {
            INonPlayerCharacter npc = new NonPlayerCharacter();
            npc.Personalities.Add(new Wanderer());

            npc.Id = npcId++;
            npc.Level = 2;
            npc.ExamineDescription = "She glances at you staring at her but quickly returns her task.";
            npc.LongDescription = "She wears a {adjective} {color} robe with {embroiderment} embroiderment.  A white sash is draped over her shoulders indicating her status of a {year} level apprentice.";
            npc.ShortDescription = "An female apprentice is wandering around looking for books.";
            npc.SentenceDescription = "Female apprentice";
            npc.KeyWords.Add("female");
            npc.KeyWords.Add("apprentice");

            List<string> adjective = new List<string>() { "dark", "light" };
            List<string> colors = new List<string>() { "red", "blue", "green", "purple", "yellow" };
            List<string> embroiderment = new List<string>() { "gold", "silver" };
            List<string> year = new List<string>() { "first", "second", "third" };

            npc.FlavorOptions.Add("{adjective}", adjective);
            npc.FlavorOptions.Add("{color}", colors);
            npc.FlavorOptions.Add("{embroiderment}", embroiderment);
            npc.FlavorOptions.Add("{year}", year);
            return npc;
        }

        private INonPlayerCharacter Male_Apprentice()
        {
            INonPlayerCharacter npc = new NonPlayerCharacter();
            npc.Personalities.Add(new Wanderer());

            npc.Id = npcId++;
            npc.Level = 2;
            npc.ExamineDescription = "The apprentice is wandering around the basement aimlessly.";
            npc.LongDescription = "He wears a {adjective} {color} robe with {embroiderment} embroiderment.  A white sash is draped over his shoulders indicating his status of a {year} level apprentice.";
            npc.ShortDescription = "An male apprentice is wandering around looking for books.";
            npc.SentenceDescription = "Male apprentice";
            npc.KeyWords.Add("male");
            npc.KeyWords.Add("apprentice");

            List<string> adjective = new List<string>() { "dark", "light" };
            List<string> colors = new List<string>() { "red", "blue", "green", "purple", "yellow" };
            List<string> embroiderment = new List<string>() { "gold", "silver" };
            List<string> year = new List<string>() { "first", "second", "third" };

            npc.FlavorOptions.Add("{adjective}", adjective);
            npc.FlavorOptions.Add("{color}", colors);
            npc.FlavorOptions.Add("{embroiderment}", embroiderment);
            npc.FlavorOptions.Add("{year}", year);
            return npc;
        }
        #endregion Library Basement

        #region Library Upstairs
        private IRoom GenerateRoom38()
        {
            IRoom room = LibraryShelves();
            room.ExamineDescription = "Looking through all the old books on the shelves you come across a copy of \"Land of Nothing.\"";
            room.LongDescription = "Light streams through the stained glass to the west depicting a beautiful green field with mountains in the background.  Several small flowers are depicted breaking up the large expanse of green grass.";

            return room;
        }

        private IRoom GenerateRoom39()
        {
            return LibraryTables();
        }

        private IRoom GenerateRoom40()
        {
            IRoom room = LibraryShelves();
            room.ExamineDescription = "Looking through all the old books on the shelves you come across a copy of \"Black Shaman.\"";
            room.LongDescription = "Light streams through the stained glass to the east.  The scene is from the perspective of a person looking out a balcony overlooking a town below.";

            return room;
        }

        private IRoom GenerateRoom41()
        {
            IRoom room = LibraryShelves();
            room.ExamineDescription = "Looking through all the old books on the shelves you come across a copy of \"The Neverending Song.\"";
            room.LongDescription = "Light streams through the stained glass to the west depicting a well. The bucket sits on the edge of the well and a path leads towards a village but no one is around.";

            return room;
        }

        private IRoom GenerateRoom42()
        {
            IRoom room = LibraryTables();
            INonPlayerCharacter npc = LibaryPatron();
            room.AddMobileObjectToRoom(npc);
            npc.Room = room;

            return room;
        }

        private IRoom GenerateRoom43()
        {
            IRoom room = LibraryShelves();
            room.ExamineDescription = "Looking through all the old books on the shelves you come across a copy of \"Star Saga.\"";
            room.LongDescription = "Light streams through the stained glass to the east depicting a majestic dragon flying low over an ocean while a lighting storms strikes in the distance.";

            return room;
        }

        private IRoom GenerateRoom44()
        {
            IRoom room = LibraryShelves();
            room.ExamineDescription = "Looking through all the old books on the shelves you come across a copy of \"The Storm Game.\"";
            room.LongDescription = "Light streams through the stained glass to the west.  The stained glass is of a mountain vista overlooking a lake.  Hues of greens meld into too hues of blue.  Snow covered mountain tops give stark contrast to image below.";

            return room;
        }

        private IRoom GenerateRoom45()
        {
            IRoom room = LibraryTables();
            INonPlayerCharacter npc = LibaryPatron();
            room.AddMobileObjectToRoom(npc);
            npc.Room = room;

            return room;
        }

        private IRoom GenerateRoom46()
        {
            IRoom room = LibraryShelves();
            room.ExamineDescription = "Looking through all the old books on the shelves you come across a copy of \"The Cry of the Rose.\"";
            room.LongDescription = "Light streams through the stained glass to the east.  The stained glass windows is of a two armies battling each other.  Swords and spears are drawn as each side charges the other.";

            return room;
        }

        private IRoom GenerateRoom47()
        {
            IRoom room = LibraryShelves();
            room.ExamineDescription = "Looking through all the old books on the shelves you come across a copy of \"The Chair's Trader.\"";
            room.LongDescription = "Light streams through the stained glass to the west depicting a foggy sunset.  The setting sun causes the light to be hues of orange and red while the fog causes the landscape colors to be hidden.  Combined the image caused the artist to go for a monochromatic scene broken only by silhouette of evergreens.";

            return room;
        }

        private IRoom GenerateRoom48()
        {
            return LibraryTables();
        }

        private IRoom GenerateRoom49()
        {
            IRoom room = LibraryShelves();
            room.ExamineDescription = "Looking through all the old books on the shelves you come across a copy of \"The Magic Argument.\"";
            room.LongDescription = "Light streams through the stained glass to the east depicting a humming bird eating from a red flower.  Drops of dew hang from the petals indicating it is still morning.";

            return room;
        }

        private IRoom GenerateRoom50()
        {
            IRoom room = LibraryShelves();
            room.ExamineDescription = "Looking through all the old books on the shelves you come across a copy of \"The Son of Winter.\"";
            room.LongDescription = "Light streams through the stained glass to the west which is of a cloud covered moon over looking an ocean port.  A sailing ship can be seen leaving port.";

            return room;
        }

        private IRoom GenerateRoom51()
        {
            IRoom room = LibraryTables();
            INonPlayerCharacter npc = LibaryPatron();
            room.AddMobileObjectToRoom(npc);
            npc.Room = room;

            return room;
        }

        private IRoom GenerateRoom52()
        {
            IRoom room = LibraryShelves();
            room.ExamineDescription = "Looking through all the old books on the shelves you come across a copy of \"The Atlas's Code.\"";
            room.LongDescription = "Light streams through the stained glass to the east which is of a mermaid sitting on a rock while a tall sailing ship is anchored in a harbor behind her.";

            return room;
        }

        private IRoom LibraryShelves()
        {
            IRoom room = GenerateRoom();
            room.MovementCost = 1;
            room.Attributes.Add(Room.RoomAttribute.Indoor);
            room.Attributes.Add(Room.RoomAttribute.Light);

            room.ShortDescription = "Library shelves";
            return room;
        }

        private IRoom LibraryTables()
        {
            IRoom room = GenerateRoom();
            room.MovementCost = 1;
            room.Attributes.Add(Room.RoomAttribute.Indoor);
            room.Attributes.Add(Room.RoomAttribute.Light);

            room.ExamineDescription = "The tables are strong and well built with many chairs on either side.  The tables sits on a pure white stone floor causing you to double check you didn't track anything into the library.";
            room.LongDescription = "Two large tables stretch from the west to east shelves filling up most of the room except for a center isle.";
            room.ShortDescription = "Library tables";
            return room;
        }

        private IRoom GenerateRoom53()
        {
            IRoom room = LibraryStairs();
            INonPlayerCharacter npc = LibaryPatron();
            room.AddMobileObjectToRoom(npc);
            npc.Room = room;

            return room;
        }

        private IRoom GenerateRoom54()
        {
            IRoom room = LibraryBalcony();
            INonPlayerCharacter npc = LibaryPatron();
            room.AddMobileObjectToRoom(npc);
            npc.Room = room;

            return room;
        }

        private IRoom GenerateRoom55()
        {
            return LibraryStairs();
        }

        private IRoom GenerateRoom56()
        {
            IRoom room = LibraryBalcony();
            INonPlayerCharacter npc = LibaryPatron();
            room.AddMobileObjectToRoom(npc);
            npc.Room = room;

            return room;
        }

        private IRoom GenerateRoom57()
        {
            return LibraryBalcony();
        }

        private IRoom GenerateRoom58()
        {
            return LibraryBalcony();
        }

        private IRoom GenerateRoom59()
        {
            IRoom room = LibraryBalcony();
            INonPlayerCharacter npc = LibaryPatron();
            room.AddMobileObjectToRoom(npc);
            npc.Room = room;

            return room;
        }

        private IRoom GenerateRoom60()
        {
            return LibraryBalcony();
        }

        private IRoom GenerateRoom61()
        {
            return LibraryBalcony();
        }

        private IRoom GenerateRoom62()
        {
            return LibraryStairs();
        }

        private IRoom GenerateRoom63()
        {
            return LibraryBalcony();
        }

        private IRoom GenerateRoom64()
        {
            return LibraryStairs();
        }

        private IRoom LibraryStairs()
        {
            IRoom room = GenerateRoom();
            room.MovementCost = 1;
            room.Attributes.Add(Room.RoomAttribute.Indoor);
            room.Attributes.Add(Room.RoomAttribute.Light);

            room.ExamineDescription = "The spiral stairs are well worn with age and use.";
            room.LongDescription = "While there are no books in this part of the library it has seen its fair use of traffic as well.";
            room.ShortDescription = "Spiral Staircase";
            return room;
        }

        private IRoom LibraryBalcony()
        {
            IRoom room = GenerateRoom();
            room.MovementCost = 1;
            room.Attributes.Add(Room.RoomAttribute.Indoor);
            room.Attributes.Add(Room.RoomAttribute.Light);

            room.ExamineDescription = "You look around the room but are drawn back to the balcony and its simple beauty.";
            room.LongDescription = "There are books up here but the true prize is the balcony over looking the library below and the arched dome above.";
            room.ShortDescription = "Library Balcony";
            return room;
        }

        private INonPlayerCharacter LibaryPatron()
        {
            INonPlayerCharacter npc = new NonPlayerCharacter();
            npc.Personalities.Add(new Wanderer());

            npc.Id = npcId++;
            npc.Level = 2;
            npc.ExamineDescription = "The library patron is carrying a stack of {BookCount} books.";
            npc.LongDescription = "The library patron glances at you and smiles then returns to looking for the next book on their list.";
            npc.ShortDescription = "The library patron is wandering around the library looking for another book to read.";
            npc.SentenceDescription = "A library patron";
            npc.KeyWords.Add("library");
            npc.KeyWords.Add("patron");


            List<string> bookCount = new List<string>() { "two", "three", "four" };
            npc.FlavorOptions.Add("{BookCount}", bookCount);

            return npc;
        }
        #endregion Library Upstairs
        #endregion End Rooms

        private void ConnectRooms()
        {
            zone.RecursivelySetZone();

            ZoneHelper.ConnectZone(zone.Rooms[1], Direction.South, 6, 25);
            ZoneHelper.ConnectRoom(zone.Rooms[1], Direction.Down, zone.Rooms[3]);
            ZoneHelper.ConnectRoom(zone.Rooms[1], Direction.North, zone.Rooms[39]);

            #region Basement
            ZoneHelper.ConnectRoom(zone.Rooms[2], Direction.East, zone.Rooms[3]);
            ZoneHelper.ConnectRoom(zone.Rooms[2], Direction.South, zone.Rooms[8]);
            ZoneHelper.ConnectRoom(zone.Rooms[3], Direction.East, zone.Rooms[4]);
            ZoneHelper.ConnectRoom(zone.Rooms[3], Direction.South, zone.Rooms[9]);
            ZoneHelper.ConnectRoom(zone.Rooms[4], Direction.East, zone.Rooms[5]);
            ZoneHelper.ConnectRoom(zone.Rooms[4], Direction.South, zone.Rooms[10]);
            ZoneHelper.ConnectRoom(zone.Rooms[5], Direction.East, zone.Rooms[6]);
            ZoneHelper.ConnectRoom(zone.Rooms[5], Direction.South, zone.Rooms[11]);
            ZoneHelper.ConnectRoom(zone.Rooms[6], Direction.East, zone.Rooms[7]);
            ZoneHelper.ConnectRoom(zone.Rooms[6], Direction.South, zone.Rooms[12]);
            ZoneHelper.ConnectRoom(zone.Rooms[7], Direction.South, zone.Rooms[13]);

            ZoneHelper.ConnectRoom(zone.Rooms[8], Direction.East, zone.Rooms[9]);
            ZoneHelper.ConnectRoom(zone.Rooms[8], Direction.South, zone.Rooms[14]);
            ZoneHelper.ConnectRoom(zone.Rooms[9], Direction.East, zone.Rooms[10]);
            ZoneHelper.ConnectRoom(zone.Rooms[9], Direction.South, zone.Rooms[15]);
            ZoneHelper.ConnectRoom(zone.Rooms[10], Direction.East, zone.Rooms[11]);
            ZoneHelper.ConnectRoom(zone.Rooms[10], Direction.South, zone.Rooms[16]);
            ZoneHelper.ConnectRoom(zone.Rooms[11], Direction.East, zone.Rooms[12]);
            ZoneHelper.ConnectRoom(zone.Rooms[11], Direction.South, zone.Rooms[17]);
            ZoneHelper.ConnectRoom(zone.Rooms[12], Direction.East, zone.Rooms[13]);
            ZoneHelper.ConnectRoom(zone.Rooms[12], Direction.South, zone.Rooms[18]);
            ZoneHelper.ConnectRoom(zone.Rooms[13], Direction.South, zone.Rooms[19]);

            ZoneHelper.ConnectRoom(zone.Rooms[14], Direction.East, zone.Rooms[15]);
            ZoneHelper.ConnectRoom(zone.Rooms[14], Direction.South, zone.Rooms[20]);
            ZoneHelper.ConnectRoom(zone.Rooms[15], Direction.East, zone.Rooms[16]);
            ZoneHelper.ConnectRoom(zone.Rooms[15], Direction.South, zone.Rooms[21]);
            ZoneHelper.ConnectRoom(zone.Rooms[16], Direction.East, zone.Rooms[17]);
            ZoneHelper.ConnectRoom(zone.Rooms[16], Direction.South, zone.Rooms[22]);
            ZoneHelper.ConnectRoom(zone.Rooms[17], Direction.East, zone.Rooms[18]);
            ZoneHelper.ConnectRoom(zone.Rooms[17], Direction.South, zone.Rooms[23]);
            ZoneHelper.ConnectRoom(zone.Rooms[18], Direction.East, zone.Rooms[19]);
            ZoneHelper.ConnectRoom(zone.Rooms[18], Direction.South, zone.Rooms[24]);
            ZoneHelper.ConnectRoom(zone.Rooms[19], Direction.South, zone.Rooms[25]);

            ZoneHelper.ConnectRoom(zone.Rooms[20], Direction.East, zone.Rooms[21]);
            ZoneHelper.ConnectRoom(zone.Rooms[20], Direction.South, zone.Rooms[26]);
            ZoneHelper.ConnectRoom(zone.Rooms[21], Direction.East, zone.Rooms[22]);
            ZoneHelper.ConnectRoom(zone.Rooms[21], Direction.South, zone.Rooms[27]);
            ZoneHelper.ConnectRoom(zone.Rooms[22], Direction.East, zone.Rooms[23]);
            ZoneHelper.ConnectRoom(zone.Rooms[22], Direction.South, zone.Rooms[28]);
            ZoneHelper.ConnectRoom(zone.Rooms[23], Direction.East, zone.Rooms[24]);
            ZoneHelper.ConnectRoom(zone.Rooms[23], Direction.South, zone.Rooms[29]);
            ZoneHelper.ConnectRoom(zone.Rooms[24], Direction.East, zone.Rooms[25]);
            ZoneHelper.ConnectRoom(zone.Rooms[24], Direction.South, zone.Rooms[30]);
            ZoneHelper.ConnectRoom(zone.Rooms[25], Direction.South, zone.Rooms[31]);

            ZoneHelper.ConnectRoom(zone.Rooms[26], Direction.East, zone.Rooms[27]);
            ZoneHelper.ConnectRoom(zone.Rooms[26], Direction.South, zone.Rooms[32]);
            ZoneHelper.ConnectRoom(zone.Rooms[27], Direction.East, zone.Rooms[28]);
            ZoneHelper.ConnectRoom(zone.Rooms[27], Direction.South, zone.Rooms[33]);
            ZoneHelper.ConnectRoom(zone.Rooms[28], Direction.East, zone.Rooms[29]);
            ZoneHelper.ConnectRoom(zone.Rooms[28], Direction.South, zone.Rooms[34]);
            ZoneHelper.ConnectRoom(zone.Rooms[29], Direction.East, zone.Rooms[30]);
            ZoneHelper.ConnectRoom(zone.Rooms[29], Direction.South, zone.Rooms[35]);
            ZoneHelper.ConnectRoom(zone.Rooms[30], Direction.East, zone.Rooms[31]);
            ZoneHelper.ConnectRoom(zone.Rooms[30], Direction.South, zone.Rooms[36]);
            ZoneHelper.ConnectRoom(zone.Rooms[31], Direction.South, zone.Rooms[37]);

            ZoneHelper.ConnectRoom(zone.Rooms[32], Direction.East, zone.Rooms[33]);
            ZoneHelper.ConnectRoom(zone.Rooms[33], Direction.East, zone.Rooms[34]);
            ZoneHelper.ConnectRoom(zone.Rooms[34], Direction.East, zone.Rooms[35]);
            ZoneHelper.ConnectRoom(zone.Rooms[35], Direction.East, zone.Rooms[36]);
            ZoneHelper.ConnectRoom(zone.Rooms[36], Direction.East, zone.Rooms[37]);

            #endregion Basement

            #region Library Upstairs

            ZoneHelper.ConnectRoom(zone.Rooms[38], Direction.North, zone.Rooms[41]);
            ZoneHelper.ConnectRoom(zone.Rooms[38], Direction.East, zone.Rooms[39]);
            ZoneHelper.ConnectRoom(zone.Rooms[39], Direction.North, zone.Rooms[42]);
            ZoneHelper.ConnectRoom(zone.Rooms[39], Direction.East, zone.Rooms[40]);
            ZoneHelper.ConnectRoom(zone.Rooms[40], Direction.North, zone.Rooms[43]);

            ZoneHelper.ConnectRoom(zone.Rooms[41], Direction.North, zone.Rooms[44]);
            ZoneHelper.ConnectRoom(zone.Rooms[41], Direction.East, zone.Rooms[42]);
            ZoneHelper.ConnectRoom(zone.Rooms[42], Direction.North, zone.Rooms[45]);
            ZoneHelper.ConnectRoom(zone.Rooms[42], Direction.East, zone.Rooms[43]);
            ZoneHelper.ConnectRoom(zone.Rooms[43], Direction.North, zone.Rooms[46]);

            ZoneHelper.ConnectRoom(zone.Rooms[44], Direction.North, zone.Rooms[47]);
            ZoneHelper.ConnectRoom(zone.Rooms[44], Direction.East, zone.Rooms[45]);
            ZoneHelper.ConnectRoom(zone.Rooms[45], Direction.North, zone.Rooms[48]);
            ZoneHelper.ConnectRoom(zone.Rooms[45], Direction.East, zone.Rooms[46]);
            ZoneHelper.ConnectRoom(zone.Rooms[46], Direction.North, zone.Rooms[49]);

            ZoneHelper.ConnectRoom(zone.Rooms[47], Direction.North, zone.Rooms[50]);
            ZoneHelper.ConnectRoom(zone.Rooms[47], Direction.East, zone.Rooms[48]);
            ZoneHelper.ConnectRoom(zone.Rooms[48], Direction.North, zone.Rooms[51]);
            ZoneHelper.ConnectRoom(zone.Rooms[48], Direction.East, zone.Rooms[49]);
            ZoneHelper.ConnectRoom(zone.Rooms[49], Direction.North, zone.Rooms[52]);

            ZoneHelper.ConnectRoom(zone.Rooms[50], Direction.East, zone.Rooms[51]);
            ZoneHelper.ConnectRoom(zone.Rooms[51], Direction.East, zone.Rooms[52]);

            ZoneHelper.ConnectRoom(zone.Rooms[53], Direction.Down, zone.Rooms[38]);
            ZoneHelper.ConnectRoom(zone.Rooms[55], Direction.Down, zone.Rooms[40]);
            ZoneHelper.ConnectRoom(zone.Rooms[62], Direction.Down, zone.Rooms[50]);
            ZoneHelper.ConnectRoom(zone.Rooms[64], Direction.Down, zone.Rooms[52]);

            ZoneHelper.ConnectRoom(zone.Rooms[53], Direction.East, zone.Rooms[54]);
            ZoneHelper.ConnectRoom(zone.Rooms[54], Direction.East, zone.Rooms[55]);
            ZoneHelper.ConnectRoom(zone.Rooms[62], Direction.East, zone.Rooms[63]);
            ZoneHelper.ConnectRoom(zone.Rooms[63], Direction.East, zone.Rooms[64]);

            ZoneHelper.ConnectRoom(zone.Rooms[53], Direction.North, zone.Rooms[56]);
            ZoneHelper.ConnectRoom(zone.Rooms[56], Direction.North, zone.Rooms[58]);
            ZoneHelper.ConnectRoom(zone.Rooms[58], Direction.North, zone.Rooms[60]);
            ZoneHelper.ConnectRoom(zone.Rooms[60], Direction.North, zone.Rooms[62]);

            ZoneHelper.ConnectRoom(zone.Rooms[55], Direction.North, zone.Rooms[57]);
            ZoneHelper.ConnectRoom(zone.Rooms[57], Direction.North, zone.Rooms[59]);
            ZoneHelper.ConnectRoom(zone.Rooms[59], Direction.North, zone.Rooms[61]);
            ZoneHelper.ConnectRoom(zone.Rooms[61], Direction.North, zone.Rooms[64]);

            #endregion Library Upstairs
        }

    }
}
