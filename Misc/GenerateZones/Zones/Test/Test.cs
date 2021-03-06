﻿using Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objects.Magic;
using System.Reflection;
using Objects.Zone.Interface;
using Objects.Zone;
using Objects.Room;
using Objects.Room.Interface;
using Objects.Damage;
using Objects.Global.DefaultValues;
using Objects.Item.Items;
using Objects.Global;
using Objects.Global.Stats;
using Objects.Material.Materials;
using Objects.Personality.Personalities;
using Objects.Mob;
using static Objects.Global.Direction.Directions;
using Objects.Mob.Interface;
using Objects.Magic.Interface;
using Objects.Magic.Spell;
using Objects.Effect;
using Objects.Die;
using Objects.Magic.Spell.Generic;
using Objects.Language;
using static Shared.TagWrapper.TagWrapper;
using Objects.Language.Interface;
using Objects.Global.Language;
using Objects.Personality.Personalities.Interface;
using Objects.Guild.Guilds;
using Objects.Guild;
using static Objects.Personality.Personalities.Responder;
using static Objects.Personality.Personalities.Responder.Response;

namespace GenerateZones.Zones
{
    public class Test : IZoneCode
    {
        Zone zone = new Zone();
        int roomId = 1;
        //int itemId = 1;
        int npcId = 1;

        public IZone Generate()
        {
            zone.Id = -1;
            zone.InGameDaysTillReset = 1;
            zone.Name = nameof(Test);

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
        private IRoom GenerateRoom1()
        {
            IRoom room = GenerateRoom();
            room.Attributes.Add(Room.RoomAttribute.Outdoor);
            room.Attributes.Add(Room.RoomAttribute.Weather);

            room.ExamineDescription = "What can I say, its a room for testing.";
            room.LongDescription = "This room looks very much like a test.";
            room.ShortDescription = "Test Room";

            return room;
        }

        private IRoom GenerateRoom()
        {
            IRoom room = new Room();
            room.Id = roomId++;
            room.MovementCost = 1;

            room.AddMobileObjectToRoom(NPC());
            return room;
        }

        private INonPlayerCharacter NPC()
        {
            INonPlayerCharacter npc = new NonPlayerCharacter();
            npc.Id += npcId++;

            npc.Level = 100;
            npc.KeyWords.Add("npc");

            npc.ExamineDescription = "A test mob dressed in its finished lab coat.";
            npc.LongDescription = "A test mob performing tests.";
            npc.ShortDescription = "A test mob.";
            npc.SentenceDescription = "test";

            IMagicUser magicUser = new MagicUser();
            magicUser.AddSpells(npc, new Mage());
            //npc.Personalities.Add(magicUser);


            IResponder responder = new Responder();
            Response response = new Response();
            response.RequiredWords.Add(new OptionalWords() { TriggerWords = new List<string>() { "hi" } });
            response.Message = new TranslationMessage("hello there");
            responder.Responses.Add(response);
            npc.Personalities.Add(responder);

            return npc;
        }
        #endregion Rooms

        private void ConnectRooms()
        {
            zone.RecursivelySetZone();
        }
    }
}
