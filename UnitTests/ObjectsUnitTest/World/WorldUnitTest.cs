﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Global;
using Shared.FileIO.Interface;
using Moq;
using Objects.Global.Logging.Interface;
using Objects.Global.Engine.Engines.Interface;
using Objects.Global.Engine.Interface;
using Objects.Global.Random.Interface;
using Objects.Global.GameDateTime.Interface;
using Objects.Mob.Interface;
using Objects.Room.Interface;
using System.Collections.Generic;
using Objects.Room;
using Objects.Zone.Interface;
using System.Reflection;
using Objects.Global.Serialization;
using Objects.Item.Interface;
using System.Diagnostics;
using Objects.Global.Serialization.Interface;
using static Objects.Room.Room;
using Objects.Global.Commands.Interface;
using Objects.Command.Interface;
using Objects.Personality.Interface;
using Shared.TagWrapper.Interface;
using static Shared.TagWrapper.TagWrapper;
using static Objects.Mob.MobileObject;
using Objects.Command.World.Interface;
using Objects.Mob;
using static Objects.Global.Logging.LogSettings;
using System.IO;
using Objects.Global.MultiClassBonus.Interface;
using Objects.Global.Settings.Interface;
using Objects.Magic.Interface;
using Objects.Global.PerformanceCounters.Interface;
using Objects.Global.TickTimes.Interface;
using System.Threading;
using Objects.Global.Commands;
using Objects.Crafting.Interface;
using Objects.Interface;
using Objects.Global.Notify.Interface;
using Objects.Language.Interface;

namespace ObjectsUnitTest.World
{
    [TestClass]
    public class WorldUnitTest
    {
        Objects.World.World world;
        Mock<IEngine> engine;
        Mock<IEvent> evnt;
        Mock<ICombat> combat;
        Mock<IRandom> random;
        Mock<IGameDateTime> gameDateTime;
        Mock<INonPlayerCharacter> npc;
        Mock<IPlayerCharacter> pc;
        Mock<IRoom> room;
        Mock<IZone> zone;
        Mock<INotify> notify;
        Mock<ITagWrapper> tagWrapper;
        [TestInitialize]
        public void Setup()
        {
            world = new Objects.World.World();
            engine = new Mock<IEngine>();
            evnt = new Mock<IEvent>();
            combat = new Mock<ICombat>();
            random = new Mock<IRandom>();
            gameDateTime = new Mock<IGameDateTime>();
            npc = new Mock<INonPlayerCharacter>();
            pc = new Mock<IPlayerCharacter>();
            room = new Mock<IRoom>();
            zone = new Mock<IZone>();
            notify = new Mock<INotify>();
            tagWrapper = new Mock<ITagWrapper>();

            Mock<ILogger> logger = new Mock<ILogger>();
            Mock<ICounters> counters = new Mock<ICounters>();
            Mock<ITickTimes> tickTimes = new Mock<ITickTimes>();
            object zoneLockObject = new object();
            Dictionary<int, IRoom> dictionaryRoom = new Dictionary<int, IRoom>();

            world.Zones.Add(0, zone.Object);
            dictionaryRoom.Add(0, room.Object);
            engine.Setup(e => e.Combat).Returns(combat.Object);
            engine.Setup(e => e.Event).Returns(evnt.Object);
            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>());
            room.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter>());
            room.Setup(e => e.Enchantments).Returns(new List<IEnchantment>());
            room.Setup(e => e.Attributes).Returns(new List<RoomAttribute>());
            zone.Setup(e => e.LockObject).Returns(zoneLockObject);
            zone.Setup(e => e.Rooms).Returns(dictionaryRoom);
            npc.Setup(e => e.LastProccessedTick).Returns(1);
            pc.Setup(e => e.LastProccessedTick).Returns(1);
            pc.Setup(e => e.CraftsmanObjects).Returns(new List<Objects.Crafting.Interface.ICraftsmanObject>());
            tickTimes.Setup(e => e.MedianTime).Returns(1m);

            GlobalReference.GlobalValues.Engine = engine.Object;
            GlobalReference.GlobalValues.Random = random.Object;
            GlobalReference.GlobalValues.GameDateTime = gameDateTime.Object;
            GlobalReference.GlobalValues.Logger = logger.Object;
            GlobalReference.GlobalValues.Counters = counters.Object;
            GlobalReference.GlobalValues.TickTimes = tickTimes.Object;
            GlobalReference.GlobalValues.Notify = notify.Object;
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;
        }

        #region PerformTick
        #region PerformCombatTick
        [TestMethod]
        public void World_PerformTick_PerformCombatTick()
        {
            world.PerformTick();

            combat.Verify(e => e.ProcessCombatRound(), Times.Once);
        }
        #endregion PerformCombatTick

        #region PutPlayersIntoWorld
        [TestMethod]
        public void World_PerformTick_PutPlayersIntoWorld_HasRoom()
        {
            List<IPlayerCharacter> lPc = new List<IPlayerCharacter>();

            room.Setup(e => e.PlayerCharacters).Returns(lPc);
            pc.Setup(e => e.Room).Returns(room.Object);

            world.AddPlayerQueue.Enqueue(pc.Object);

            world.PerformTick();

            room.Verify(e => e.AddMobileObjectToRoom(pc.Object));
            Assert.IsTrue(world.CurrentPlayers.Contains(pc.Object));
            pc.Verify(e => e.EnqueueCommand("Look"), Times.Once);
        }

        [TestMethod]
        public void World_PerformTick_PutPlayersIntoWorld_HasRoomId()
        {
            List<IPlayerCharacter> lPc = new List<IPlayerCharacter>();
            Dictionary<int, IRoom> rooms = new Dictionary<int, IRoom>();

            world.Zones.Add(1, zone.Object);
            zone.Setup(e => e.Rooms).Returns(rooms);
            rooms.Add(1, room.Object);
            room.Setup(e => e.PlayerCharacters).Returns(lPc);
            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>());
            room.Setup(e => e.Enchantments).Returns(new List<IEnchantment>());
            pc.Setup(e => e.RoomId).Returns(new RoomId(1, 1));
            pc.SetupSequence(e => e.Room)
                .Returns(null)
                .Returns(room.Object);


            world.AddPlayerQueue.Enqueue(pc.Object);

            world.PerformTick();

            Assert.IsTrue(world.CurrentPlayers.Contains(pc.Object));
            room.Verify(e => e.AddMobileObjectToRoom(pc.Object));
            pc.VerifySet(e => e.Room = room.Object, Times.Once);
            pc.Verify(e => e.EnqueueCommand("Look"), Times.Once);
        }

        [TestMethod]
        public void World_PerformTick_PutPlayersIntoWorld_HasRoomIdButInvalid()
        {
            List<IPlayerCharacter> lPc = new List<IPlayerCharacter>();
            Dictionary<int, IRoom> rooms = new Dictionary<int, IRoom>();

            world.Zones.Add(1, zone.Object);
            zone.Setup(e => e.Rooms).Returns(rooms);
            rooms.Add(1, room.Object);
            room.Setup(e => e.PlayerCharacters).Returns(lPc);
            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>());
            room.Setup(e => e.Enchantments).Returns(new List<IEnchantment>());
            pc.Setup(e => e.RoomId).Returns(new RoomId(1, 2));
            pc.SetupSequence(e => e.Room)
                .Returns(null)
                .Returns(room.Object);


            world.AddPlayerQueue.Enqueue(pc.Object);

            world.PerformTick();

            room.Verify(e => e.AddMobileObjectToRoom(pc.Object));
            Assert.IsTrue(world.CurrentPlayers.Contains(pc.Object));
            pc.VerifySet(e => e.Room = room.Object, Times.Once);
            pc.Verify(e => e.EnqueueCommand("Look"), Times.Once);
        }

        [TestMethod]
        public void World_PerformTick_PutPlayersIntoWorld_NoRoomInfo()
        {
            Dictionary<int, IRoom> rooms = new Dictionary<int, IRoom>();

            world.Zones.Add(1, zone.Object);
            zone.Setup(e => e.Rooms).Returns(rooms);
            rooms.Add(1, room.Object);
            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>());
            room.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter>());
            pc.Setup(e => e.Room).Returns<IRoom>(null);

            world.AddPlayerQueue.Enqueue(pc.Object);

            world.PerformTick();

            Assert.IsTrue(world.CurrentPlayers.Contains(pc.Object));
            room.Verify(e => e.Enter(pc.Object), Times.Once);
            pc.VerifySet(e => e.Room = room.Object, Times.Once);
        }
        #endregion PutPlayersIntoWorld

        #region UpdateWeather
        [TestMethod]
        public void World_PerformTick_UpdateWeather()
        {
            world.PerformTick();

            Assert.AreEqual(0, world.PrecipitationGoal);
            Assert.AreEqual(0, world.WindSpeedGoal);
            Assert.AreEqual(49, world.Precipitation);
            Assert.AreEqual(49, world.WindSpeed);
        }

        [TestMethod]
        public void World_PerformTick_UpdateWeather_MoveUpScale()
        {
            PropertyInfo notifyPrecipitation = world.GetType().GetProperty("NotifyPrecipitation", BindingFlags.NonPublic | BindingFlags.Instance);
            PropertyInfo notifiyWindSpeed = world.GetType().GetProperty("NotifyWindSpeed", BindingFlags.NonPublic | BindingFlags.Instance);
            Mock<ICounters> counter = new Mock<ICounters>();
            Mock<ITickTimes> tickTimes = new Mock<ITickTimes>();

            world.Precipitation = 10;
            world.WindSpeed = 10;

            GlobalReference.GlobalValues.Counters = counter.Object;
            GlobalReference.GlobalValues.TickTimes = tickTimes.Object;

            world.PerformTick();

            Assert.AreEqual(11, world.Precipitation);
            Assert.AreEqual(11, world.WindSpeed);
            Assert.IsFalse((bool)notifyPrecipitation.GetValue(world));
            Assert.IsFalse((bool)notifiyWindSpeed.GetValue(world));
        }
        #endregion UpdateWeather

        #region ReloadZones
        [TestMethod]
        public void World_PerformTick_ReloadZones()
        {
            world.Zones.Clear();  //clears out the zone added at initialization

            PropertyInfo info = world.GetType().GetProperty("_zoneIdToFileMap", BindingFlags.Instance | BindingFlags.NonPublic);
            Mock<IFileIO> fileIo = new Mock<IFileIO>();
            Mock<ISerialization> xmlSerialization = new Mock<ISerialization>();
            Objects.Zone.Zone deserializeZone = new Objects.Zone.Zone();

            ((Dictionary<int, string>)info.GetValue(world)).Add(0, "blah");
            gameDateTime.Setup(e => e.InGameDateTime).Returns(new DateTime(1, 2, 3));
            world.Zones.Add(0, zone.Object);
            fileIo.Setup(e => e.ReadAllText("blah")).Returns("seraializedZone");
            xmlSerialization.Setup(e => e.Deserialize<Objects.Zone.Zone>("seraializedZone")).Returns(deserializeZone);

            GlobalReference.GlobalValues.FileIO = fileIo.Object;
            GlobalReference.GlobalValues.Serialization = xmlSerialization.Object;

            world.PerformTick();

            Assert.AreEqual(1, world.Zones.Count);
            Assert.AreSame(deserializeZone, world.Zones[0]);
        }
        #endregion ReloadZones

        #region UpdatePerformanceCounters
        [TestMethod]
        public void WorldPerformTick_UpdatePerformanceCounters()
        {
            Mock<ICounters> counter = new Mock<ICounters>();
            Mock<ITickTimes> tickTimes = new Mock<ITickTimes>();

            GlobalReference.GlobalValues.Counters = counter.Object;
            GlobalReference.GlobalValues.TickTimes = tickTimes.Object;

            world.PerformTick();

            counter.VerifySet(e => e.ConnnectedPlayers = 0);
            counter.VerifySet(e => e.CPU = 0);
            counter.VerifySet(e => e.MaxTickTimeInMs = 0);
            counter.VerifySet(e => e.Memory = It.IsAny<int>());
        }
        #endregion UpdatePerformanceCounters

        #region ProcessRoom
        [TestMethod]
        public void World_PerformTick_ProcessRoom_VerifyWeatherMessage()
        {
            Mock<IParser> parser = new Mock<IParser>();
            Mock<ICommand> command = new Mock<ICommand>();
            Mock<ICommandList> commandList = new Mock<ICommandList>();
            Mock<IMobileObjectCommand> mobCommand = new Mock<IMobileObjectCommand>();
            Mock<IResult> result = new Mock<IResult>();

            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>() { npc.Object });
            room.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter>() { pc.Object });
            room.Setup(e => e.Attributes).Returns(new List<RoomAttribute>() { RoomAttribute.Weather });
            room.Setup(e => e.Enchantments).Returns(new List<IEnchantment>());
            room.Setup(e => e.PrecipitationNotification).Returns("rain");
            room.Setup(e => e.WindSpeedNotification).Returns("wind");
            world.Precipitation = 10;
            world.WindSpeed = 10;
            npc.SetupSequence(e => e.DequeueCommunication())
                .Returns("say hi")
                .Returns(null);
            npc.Setup(e => e.Personalities).Returns(new List<IPersonality>());
            parser.Setup(e => e.Parse("say hi")).Returns(command.Object);
            command.Setup(e => e.CommandName).Returns("say");
            commandList.Setup(e => e.GetCommand(npc.Object, "say")).Returns(mobCommand.Object);
            result.Setup(e => e.ResultMessage).Returns("result");
            mobCommand.Setup(e => e.PerformCommand(npc.Object, command.Object)).Returns(result.Object);

            GlobalReference.GlobalValues.Parser = parser.Object;
            GlobalReference.GlobalValues.CommandList = commandList.Object;

            world.PerformTick();

            evnt.Verify(e => e.HeartbeatBigTick(room.Object), Times.Once);
            mobCommand.Verify(e => e.PerformCommand(npc.Object, command.Object));
            //npc.Verify(e => e.EnqueueMessage("result"), Times.Once);
            //npc.Verify(e => e.EnqueueMessage("rain"), Times.Once);
            //npc.Verify(e => e.EnqueueMessage("wind"), Times.Once);
            notify.Verify(e => e.Mob(npc.Object, It.IsAny<ITranslationMessage>()), Times.Exactly(3));
        }

        [TestMethod]
        public void World_PerformTick_ProcessRoom_CommunicationCommandNotReconnized()
        {
            Mock<IParser> parser = new Mock<IParser>();
            Mock<ICommand> command = new Mock<ICommand>();
            Mock<ICommandList> commandList = new Mock<ICommandList>();
            Mock<ITagWrapper> tagWrapper = new Mock<ITagWrapper>();

            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>() { npc.Object });
            room.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter>() { pc.Object });
            room.Setup(e => e.Enchantments).Returns(new List<IEnchantment>());
            npc.SetupSequence(e => e.DequeueCommunication())
                .Returns("say hi")
                .Returns(null);
            npc.Setup(e => e.Personalities).Returns(new List<IPersonality>());
            parser.Setup(e => e.Parse("say hi")).Returns(command.Object);
            command.Setup(e => e.CommandName).Returns("say");
            commandList.Setup(e => e.GetCommand(npc.Object, "say")).Returns<IMobileObjectCommand>(null);
            tagWrapper.Setup(e => e.WrapInTag("Unknown command.", TagType.Info)).Returns("result");

            GlobalReference.GlobalValues.Parser = parser.Object;
            GlobalReference.GlobalValues.CommandList = commandList.Object;
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            world.PerformTick();

            evnt.Verify(e => e.HeartbeatBigTick(room.Object), Times.Once);
            notify.Verify(e => e.Mob(npc.Object, It.IsAny<ITranslationMessage>()));
        }

        [TestMethod]
        public void World_PerformTick_ProcessRoom_ProcessEnchantments()
        {
            Mock<IEnchantment> enchantment = new Mock<IEnchantment>();

            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>() { npc.Object });
            room.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter>() { pc.Object });
            room.Setup(e => e.Enchantments).Returns(new List<IEnchantment>());
            room.Setup(e => e.Enchantments).Returns(new List<IEnchantment>() { enchantment.Object });
            npc.Setup(e => e.Personalities).Returns(new List<IPersonality>());

            world.PerformTick();

            //the enchantment does not count per parameter but instead counts the number of times it was called
            //so we have to combine the number of times the pc and npc were called.
            enchantment.Verify(e => e.HeartbeatBigTick(npc.Object), Times.Exactly(2));
        }

        [TestMethod]
        public void World_PerformTick_ProcessRoom_ProcessMobPersonality()
        {
            Dictionary<int, IRoom> rooms = new Dictionary<int, IRoom>();
            Mock<IPersonality> personality = new Mock<IPersonality>();

            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>() { npc.Object });
            room.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter>() { pc.Object });
            room.Setup(e => e.Enchantments).Returns(new List<IEnchantment>());
            world.Zones.Add(1, zone.Object);
            zone.Setup(e => e.Rooms).Returns(rooms);
            rooms.Add(1, room.Object);
            npc.Setup(e => e.Personalities).Returns(new List<IPersonality>() { personality.Object });
            personality.Setup(e => e.Process(npc.Object, null)).Returns("test");

            world.PerformTick();

            npc.Verify(e => e.EnqueueCommand("test"));
        }

        [TestMethod]
        public void World_PerformTick_ProcessRoom_ProcessMobCommand()
        {
            Mock<IParser> parser = new Mock<IParser>();
            Mock<ICommand> command = new Mock<ICommand>();
            Mock<ICommandList> commandList = new Mock<ICommandList>();
            Mock<IMobileObjectCommand> mobCommand = new Mock<IMobileObjectCommand>();
            Mock<IResult> result = new Mock<IResult>();

            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>() { npc.Object });
            room.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter>() { pc.Object });
            room.Setup(e => e.Enchantments).Returns(new List<IEnchantment>());
            npc.Setup(e => e.Personalities).Returns(new List<IPersonality>());
            npc.Setup(e => e.LastProccessedTick).Returns(1);
            npc.SetupSequence(e => e.DequeueCommand())
                .Returns("command")
                .Returns(null)
                .Returns("3");
            parser.Setup(e => e.Parse("command")).Returns(command.Object);
            command.Setup(e => e.CommandName).Returns("command");
            commandList.Setup(e => e.GetCommand(npc.Object, "command")).Returns(mobCommand.Object);
            mobCommand.Setup(e => e.PerformCommand(npc.Object, command.Object)).Returns(result.Object);
            result.Setup(e => e.ResultMessage).Returns("result");

            GlobalReference.GlobalValues.Parser = parser.Object;
            GlobalReference.GlobalValues.CommandList = commandList.Object;

            world.PerformTick();

            notify.Verify(e => e.Mob(npc.Object, It.IsAny<ITranslationMessage>()));
            npc.VerifySet(e => e.LastProccessedTick = 0, Times.Once);
        }

        [TestMethod]
        public void World_PerformTick_ProcessRoom_PerformHeartBeatBigTick()
        {
            Mock<ICounters> counter = new Mock<ICounters>();
            Mock<ITickTimes> tickTimes = new Mock<ITickTimes>();

            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>() { npc.Object });
            room.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter>() { pc.Object });
            room.Setup(e => e.Enchantments).Returns(new List<IEnchantment>());
            npc.Setup(e => e.Personalities).Returns(new List<IPersonality>());
            engine.Setup(e => e.Event).Returns(evnt.Object);

            GlobalReference.GlobalValues.Counters = counter.Object;
            GlobalReference.GlobalValues.TickTimes = tickTimes.Object;

            world.PerformTick();

            evnt.Verify(e => e.HeartbeatBigTick(npc.Object), Times.Once);
        }

        [TestMethod]
        public void World_PerformTick_ProcessRoom_MobRegenerateStand()
        {
            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>() { npc.Object });
            room.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter>() { pc.Object });
            room.Setup(e => e.Enchantments).Returns(new List<IEnchantment>());
            npc.Setup(e => e.Personalities).Returns(new List<IPersonality>());
            npc.Setup(e => e.MaxHealth).Returns(100);
            npc.Setup(e => e.MaxMana).Returns(1000);
            npc.Setup(e => e.MaxStamina).Returns(10000);
            npc.Setup(e => e.Position).Returns(CharacterPosition.Stand);
            engine.Setup(e => e.Event).Returns(evnt.Object);

            world.PerformTick();

            npc.VerifySet(e => e.Health = 1, Times.Once);
            npc.VerifySet(e => e.Mana = 10, Times.Once);
            npc.VerifySet(e => e.Stamina = 100, Times.Once);
        }

        [TestMethod]
        public void World_PerformTick_ProcessRoom_MobRegenerateMounted()
        {
            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>() { npc.Object });
            room.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter>() { pc.Object });
            room.Setup(e => e.Enchantments).Returns(new List<IEnchantment>());
            npc.Setup(e => e.Personalities).Returns(new List<IPersonality>());
            npc.Setup(e => e.MaxHealth).Returns(100);
            npc.Setup(e => e.MaxMana).Returns(1000);
            npc.Setup(e => e.MaxStamina).Returns(10000);
            npc.Setup(e => e.Position).Returns(CharacterPosition.Mounted);
            engine.Setup(e => e.Event).Returns(evnt.Object);

            world.PerformTick();

            npc.VerifySet(e => e.Health = 1, Times.Once);
            npc.VerifySet(e => e.Mana = 10, Times.Once);
            npc.VerifySet(e => e.Stamina = 100, Times.Once);
        }

        [TestMethod]
        public void World_PerformTick_ProcessRoom_MobRegenerateSit()
        {
            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>() { npc.Object });
            room.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter>() { pc.Object });
            room.Setup(e => e.Enchantments).Returns(new List<IEnchantment>());
            npc.Setup(e => e.Personalities).Returns(new List<IPersonality>());
            npc.Setup(e => e.MaxHealth).Returns(100);
            npc.Setup(e => e.MaxMana).Returns(1000);
            npc.Setup(e => e.MaxStamina).Returns(10000);
            npc.Setup(e => e.Position).Returns(CharacterPosition.Sit);
            engine.Setup(e => e.Event).Returns(evnt.Object);

            world.PerformTick();

            npc.VerifySet(e => e.Health = 2, Times.Once);
            npc.VerifySet(e => e.Mana = 20, Times.Once);
            npc.VerifySet(e => e.Stamina = 200, Times.Once);
        }

        [TestMethod]
        public void World_PerformTick_ProcessRoom_MobRegenerateRelax()
        {
            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>() { npc.Object });
            room.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter>() { pc.Object });
            room.Setup(e => e.Enchantments).Returns(new List<IEnchantment>());
            npc.Setup(e => e.Personalities).Returns(new List<IPersonality>());
            npc.Setup(e => e.MaxHealth).Returns(100);
            npc.Setup(e => e.MaxMana).Returns(1000);
            npc.Setup(e => e.MaxStamina).Returns(10000);
            npc.Setup(e => e.Position).Returns(CharacterPosition.Relax);
            engine.Setup(e => e.Event).Returns(evnt.Object);

            world.PerformTick();

            npc.VerifySet(e => e.Health = 3, Times.Once);
            npc.VerifySet(e => e.Mana = 30, Times.Once);
            npc.VerifySet(e => e.Stamina = 303, Times.Once);
        }

        [TestMethod]
        public void World_PerformTick_ProcessRoom_MobRegenerateSleep()
        {
            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>() { npc.Object });
            room.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter>() { pc.Object });
            room.Setup(e => e.Enchantments).Returns(new List<IEnchantment>());
            npc.Setup(e => e.Personalities).Returns(new List<IPersonality>());
            npc.Setup(e => e.MaxHealth).Returns(100);
            npc.Setup(e => e.MaxMana).Returns(1000);
            npc.Setup(e => e.MaxStamina).Returns(10000);
            npc.Setup(e => e.Position).Returns(CharacterPosition.Sleep);
            engine.Setup(e => e.Event).Returns(evnt.Object);

            world.PerformTick();

            npc.VerifySet(e => e.Health = 4, Times.Once);
            npc.VerifySet(e => e.Mana = 40, Times.Once);
            npc.VerifySet(e => e.Stamina = 400, Times.Once);
        }

        [TestMethod]
        public void World_PerformTick_ProcessRoom_ProcessPlayerNotifications()
        {
            List<ICraftsmanObject> craftsmanObjects = new List<ICraftsmanObject>();
            Mock<ICraftsmanObject> craftsmanObject = new Mock<ICraftsmanObject>();
            Mock<IBaseObjectId> objectId = new Mock<IBaseObjectId>();
            Mock<ITagWrapper> tagwrapper = new Mock<ITagWrapper>();

            craftsmanObjects.Add(craftsmanObject.Object);
            craftsmanObject.Setup(e => e.Completion).Returns(new DateTime(1, 1, 1));
            craftsmanObject.Setup(e => e.NextNotifcation).Returns(new DateTime(1, 1, 1));
            craftsmanObject.Setup(e => e.CraftmanDescripition).Returns("craftmanDescription");
            craftsmanObject.Setup(e => e.CraftmanDescripition).Returns("craftmanDescription");
            craftsmanObject.Setup(e => e.CraftsmanId).Returns(objectId.Object);
            objectId.Setup(e => e.Zone).Returns(1);
            room.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter>() { pc.Object });
            pc.Setup(e => e.CraftsmanObjects).Returns(craftsmanObjects);
            tagwrapper.Setup(e => e.WrapInTag("\"craftmanDescription\" has completed your item in zone 1.", TagType.Info)).Returns("message");

            GlobalReference.GlobalValues.TagWrapper = tagwrapper.Object;

            world.PerformTick();

            notify.Verify(e => e.Mob(pc.Object, It.IsAny<ITranslationMessage>()));
            craftsmanObject.VerifySet(e => e.NextNotifcation = It.IsAny<DateTime>());
        }
        #endregion ProcessRoom

        #region CatchPlayersOutSideOfTheWorldDueToReloadedZones
        [TestMethod]
        public void World_PerformTick_CatchPlayersOutSideOfTheWorldDueToReloadedZones()
        {
            PropertyInfo info = world.GetType().GetProperty("characters", BindingFlags.NonPublic | BindingFlags.Instance);
            Dictionary<int, IRoom> rooms = new Dictionary<int, IRoom>();
            Mock<IRoom> room2 = new Mock<IRoom>();

            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>());
            room.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter>());
            world.Zones.Add(1, zone.Object);
            zone.Setup(e => e.Rooms).Returns(rooms);
            rooms.Add(1, room.Object);
            pc.Setup(e => e.Room).Returns(room2.Object);
            pc.Setup(e => e.LastProccessedTick).Returns(1);
            ((List<IPlayerCharacter>)info.GetValue(world)).Add(pc.Object);
            room2.Setup(e => e.Zone).Returns(1);
            room2.Setup(e => e.Id).Returns(1);
            room2.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>());
            room2.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter>());

            world.PerformTick();

            evnt.Verify(e => e.HeartbeatBigTick(room2.Object), Times.Once);
        }
        #endregion CatchPlayersOutSideOfTheWorldDueToReloadedZones

        #region DoWorldCommand
        [TestMethod]
        public void World_PerformTick_DoWorldCommand()
        {
            Mock<IGameStats> gameStats = new Mock<IGameStats>();

            world.WorldCommands.Enqueue("GameStats");
            world.GameStatsInterface = gameStats.Object;
            gameStats.SetupSequence(e => e.GenerateGameStats())
                .Returns("result")
                .Returns("result2");

            world.PerformTick();

            string result = null;
            world.WorldResults.TryGetValue("GameStats", out result);
            Assert.AreEqual("result", result);

            world.WorldCommands.Enqueue("GameStats");

            world.PerformTick();
            world.WorldResults.TryGetValue("GameStats", out result);
            Assert.AreEqual("result2", result);
        }
        #endregion DoWorldCommand


        #endregion PerformTick

        [TestMethod]
        public void World_SaveCharcter()
        {
            Mock<ISettings> settings = new Mock<ISettings>();
            Mock<IFileIO> fileIO = new Mock<IFileIO>();
            Mock<ISerialization> serializer = new Mock<ISerialization>();

            settings.Setup(e => e.PlayerCharacterDirectory).Returns("c:\\");
            pc.Setup(e => e.Name).Returns("test");
            pc.Setup(e => e.Room).Returns(room.Object);
            serializer.Setup(e => e.Serialize(pc.Object)).Returns("serialized");

            GlobalReference.GlobalValues.Settings = settings.Object;
            GlobalReference.GlobalValues.FileIO = fileIO.Object;
            GlobalReference.GlobalValues.Serialization = serializer.Object;

            world.SaveCharcter(pc.Object);

            pc.VerifySet(e => e.Room = null, Times.Once);
            fileIO.Verify(e => e.WriteFile(@"c:\test.char", "serialized"), Times.Once);
        }

        [TestMethod]
        public void World_LoadCharacter_AllReadyInGame()
        {
            PropertyInfo info = world.GetType().GetProperty("characters", BindingFlags.NonPublic | BindingFlags.Instance);
            Mock<ISettings> settings = new Mock<ISettings>();

            pc.Setup(e => e.Name).Returns("name");
            ((List<IPlayerCharacter>)info.GetValue(world)).Add(pc.Object);
            settings.Setup(e => e.PlayerCharacterDirectory).Returns("directory");

            GlobalReference.GlobalValues.Settings = settings.Object;

            IPlayerCharacter result = world.LoadCharacter("name");
            Assert.AreSame(pc.Object, result);
        }

        [TestMethod]
        public void World_LoadCharacter_LoadFromFile()
        {
            PropertyInfo info = world.GetType().GetProperty("characters", BindingFlags.NonPublic | BindingFlags.Instance);
            PlayerCharacter realPc = new PlayerCharacter();
            Mock<IFileIO> fileIO = new Mock<IFileIO>();
            Mock<ISerialization> seralizer = new Mock<ISerialization>();
            Mock<IPlayerCharacter> pc2 = new Mock<IPlayerCharacter>();
            Mock<ISettings> settings = new Mock<ISettings>();

            pc.Setup(e => e.Name).Returns("name");
            pc2.Setup(e => e.Name).Returns("bob");
            ((List<IPlayerCharacter>)info.GetValue(world)).Add(pc2.Object);
            fileIO.Setup(e => e.GetFilesFromDirectory("directory")).Returns(new string[] { "c:\\name.char" });
            fileIO.Setup(e => e.ReadAllText("c:\\name.char")).Returns("serializedPlayer");
            seralizer.Setup(e => e.Deserialize<PlayerCharacter>("serializedPlayer")).Returns(realPc);
            settings.Setup(e => e.PlayerCharacterDirectory).Returns("directory");

            GlobalReference.GlobalValues.FileIO = fileIO.Object;
            GlobalReference.GlobalValues.Serialization = seralizer.Object;
            GlobalReference.GlobalValues.Settings = settings.Object;

            IPlayerCharacter result = world.LoadCharacter("name");
            Assert.AreSame(realPc, result);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void World_LoadCharacter_LoadFromFileUnableToDeserialize()
        {
            PlayerCharacter realPc = new PlayerCharacter();
            Mock<IFileIO> fileIO = new Mock<IFileIO>();
            Mock<ISerialization> seralizer = new Mock<ISerialization>();
            Mock<ILogger> logger = new Mock<ILogger>();
            Mock<ISettings> settings = new Mock<ISettings>();

            pc.Setup(e => e.Name).Returns("name");
            fileIO.Setup(e => e.GetFilesFromDirectory("directory")).Returns(new string[] { "c:\\name.char" });
            fileIO.Setup(e => e.ReadAllText("c:\\name.char")).Returns("serializedPlayer");
            seralizer.Setup(e => e.Deserialize<PlayerCharacter>("serializedPlayer")).Returns<PlayerCharacter>(null);
            settings.Setup(e => e.PlayerCharacterDirectory).Returns("directory");

            GlobalReference.GlobalValues.FileIO = fileIO.Object;
            GlobalReference.GlobalValues.Serialization = seralizer.Object;
            GlobalReference.GlobalValues.Logger = logger.Object;
            GlobalReference.GlobalValues.Settings = settings.Object;

            try
            {
                world.LoadCharacter("name");
            }
            catch
            {
                logger.Verify(e => e.Log(LogLevel.ERROR, "Unable to deserialize string as PlayerCharacter.\r\nserializedPlayer"), Times.Once);
                throw;
            }
        }

        [TestMethod]
        public void World_LoadCharacter_PlayerNotFound()
        {
            Mock<IFileIO> fileIO = new Mock<IFileIO>();
            Mock<ISettings> settings = new Mock<ISettings>();

            fileIO.Setup(e => e.GetFilesFromDirectory("directory")).Returns(new string[] { });
            settings.Setup(e => e.PlayerCharacterDirectory).Returns("directory");

            GlobalReference.GlobalValues.FileIO = fileIO.Object;
            GlobalReference.GlobalValues.Settings = settings.Object;

            IPlayerCharacter result = world.LoadCharacter("name");
            Assert.IsNull(result);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void World_LoadWorld_NoFilesFound()
        {
            Mock<IFileIO> fileIO = new Mock<IFileIO>();
            Mock<ISettings> settings = new Mock<ISettings>();

            fileIO.Setup(e => e.GetFilesFromDirectory("zonelocation", "*.zone")).Returns(new string[] { });
            settings.Setup(e => e.ZoneDirectory).Returns("zonelocation");

            GlobalReference.GlobalValues.FileIO = fileIO.Object;
            GlobalReference.GlobalValues.Settings = settings.Object;

            world.LoadWorld();
        }

        [TestMethod]
        public void World_LoadWorld_ZoneLoads()
        {
            world.Zones.Clear(); //remove the item added from above
            Mock<IFileIO> fileIO = new Mock<IFileIO>();
            Mock<ILogger> logger = new Mock<ILogger>();
            Mock<ISerialization> xmlSerializer = new Mock<ISerialization>();
            Objects.Zone.Zone realZone = new Objects.Zone.Zone();
            Mock<IRoom> room = new Mock<IRoom>();
            PropertyInfo info = world.GetType().GetProperty("_zoneIdToFileMap", BindingFlags.Instance | BindingFlags.NonPublic);
            Mock<ISettings> settings = new Mock<ISettings>();

            fileIO.Setup(e => e.GetFilesFromDirectory("zonelocation", "*.zone")).Returns(new string[] { "c:\\zone.zone" });
            fileIO.Setup(e => e.ReadAllText("c:\\zone.zone")).Returns("serializedZone");
            xmlSerializer.Setup(e => e.Deserialize<Objects.Zone.Zone>("serializedZone")).Returns(realZone);
            realZone.Rooms.Add(1, room.Object);
            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>() { npc.Object });
            room.Setup(e => e.Items).Returns(new List<IItem>());
            settings.Setup(e => e.ZoneDirectory).Returns("zonelocation");

            GlobalReference.GlobalValues.FileIO = fileIO.Object;
            GlobalReference.GlobalValues.Logger = logger.Object;
            GlobalReference.GlobalValues.Serialization = xmlSerializer.Object;
            GlobalReference.GlobalValues.Settings = settings.Object;

            world.LoadWorld();

            npc.VerifySet(e => e.Room = room.Object, Times.Once);
            Assert.AreSame(realZone, world.Zones[0]);
            string storedFileName = ((Dictionary<int, string>)info.GetValue(world))[0];
            Assert.AreEqual("c:\\zone.zone", storedFileName);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void World_LoadWorld_ZoneUnableToDeserialize()
        {
            Mock<IFileIO> fileIO = new Mock<IFileIO>();
            Mock<ILogger> logger = new Mock<ILogger>();
            Mock<ISerialization> xmlSerializer = new Mock<ISerialization>();
            Mock<ISettings> settings = new Mock<ISettings>();

            fileIO.Setup(e => e.GetFilesFromDirectory("zonelocation", "*.zone")).Returns(new string[] { "c:\\zone.zone" });
            fileIO.Setup(e => e.ReadAllText("c:\\zone.zone")).Returns("serializedZone");
            xmlSerializer.Setup(e => e.Deserialize<Objects.Zone.Zone>("serializedZone")).Returns<Objects.Zone.Zone>(null);
            settings.Setup(e => e.ZoneDirectory).Returns("zonelocation");

            GlobalReference.GlobalValues.FileIO = fileIO.Object;
            GlobalReference.GlobalValues.Logger = logger.Object;
            GlobalReference.GlobalValues.Serialization = xmlSerializer.Object;
            GlobalReference.GlobalValues.Settings = settings.Object;

            try
            {
                world.LoadWorld();
            }
            catch
            {
                logger.Verify(e => e.Log(LogLevel.ERROR, "Unable to deserialize string as Zone.\r\nserializedZone"), Times.Once);
                throw;
            }
        }

        [TestMethod]
        public void World_SaveWorld()
        {
            world.Zones.Clear();  //clears out the zone added at initialization

            Mock<IFileIO> fileIO = new Mock<IFileIO>();
            Dictionary<int, IRoom> rooms = new Dictionary<int, IRoom>();
            List<IPlayerCharacter> pcList = new List<IPlayerCharacter>();
            Mock<ISerialization> xmlSerializer = new Mock<ISerialization>();
            Mock<ISettings> settings = new Mock<ISettings>();

            rooms.Add(0, room.Object);
            zone.Setup(e => e.Name).Returns("zone");
            zone.Setup(e => e.Rooms).Returns(rooms);
            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>() { npc.Object });
            room.Setup(e => e.PlayerCharacters).Returns(pcList);
            pcList.Add(pc.Object);
            xmlSerializer.Setup(e => e.Serialize(zone.Object)).Returns("serializedZone");
            world.Zones.Add(0, zone.Object);
            settings.Setup(e => e.ZoneDirectory).Returns("c:\\");

            GlobalReference.GlobalValues.FileIO = fileIO.Object;
            GlobalReference.GlobalValues.Serialization = xmlSerializer.Object;
            GlobalReference.GlobalValues.Settings = settings.Object;

            world.SaveWorld();

            npc.VerifySet(e => e.Room = null);
            room.Verify(e => e.RemoveMobileObjectFromRoom(pc.Object));
            fileIO.Verify(e => e.WriteFile("c:\\zone.zone", "serializedZone"), Times.Once);
        }

        [TestMethod]
        public void World_LogOutCharacter_CharacterFound()
        {
            PropertyInfo info = world.GetType().GetProperty("characters", BindingFlags.NonPublic | BindingFlags.Instance);
            List<IPlayerCharacter> listPC = new List<IPlayerCharacter>();
            Mock<ISettings> settings = new Mock<ISettings>();
            Mock<IFileIO> fileIO = new Mock<IFileIO>();
            Mock<ISerialization> serializer = new Mock<ISerialization>();

            pc.Setup(e => e.Name).Returns("name");
            pc.Setup(e => e.Room).Returns(room.Object);
            room.Setup(e => e.PlayerCharacters).Returns(listPC);
            listPC.Add(pc.Object);
            ((List<IPlayerCharacter>)info.GetValue(world)).Add(pc.Object);
            settings.Setup(e => e.PlayerCharacterDirectory).Returns("c:\\");
            serializer.Setup(e => e.Serialize(pc.Object)).Returns("serializedPC");

            GlobalReference.GlobalValues.Settings = settings.Object;
            GlobalReference.GlobalValues.FileIO = fileIO.Object;
            GlobalReference.GlobalValues.Serialization = serializer.Object;

            world.LogOutCharacter("name");

            Assert.AreEqual(0, ((List<IPlayerCharacter>)info.GetValue(world)).Count);
            room.Verify(e => e.RemoveMobileObjectFromRoom(pc.Object));
        }

        [TestMethod]
        public void World_CreateCharacter()
        {
            Mock<ISettings> settings = new Mock<ISettings>();
            Mock<IMultiClassBonus> multicClassBonus = new Mock<IMultiClassBonus>();

            settings.Setup(e => e.BaseStatValue).Returns(1);
            settings.Setup(e => e.AssignableStatPoints).Returns(2);

            GlobalReference.GlobalValues.Settings = settings.Object;
            GlobalReference.GlobalValues.MultiClassBonus = multicClassBonus.Object;

            IPlayerCharacter result = world.CreateCharacter("userName", "password");

            Assert.AreEqual("userName", result.Name);
            Assert.AreEqual("password", result.Password);
            Assert.AreEqual(1, result.StrengthStat);
            Assert.AreEqual(1, result.DexterityStat);
            Assert.AreEqual(1, result.ConstitutionStat);
            Assert.AreEqual(1, result.IntelligenceStat);
            Assert.AreEqual(1, result.WisdomStat);
            Assert.AreEqual(1, result.CharismaStat);
            Assert.AreEqual(1, result.Level);
            Assert.AreEqual(result.MaxHealth, result.Health);
            Assert.AreEqual(result.MaxStamina, result.Stamina);
            Assert.AreEqual(result.MaxMana, result.Mana);
            Assert.AreEqual("userName", result.SentenceDescription);
            Assert.AreEqual("userName", result.ShortDescription);
            Assert.AreEqual("userName", result.LongDescription);
            Assert.IsTrue(result.KeyWords.Contains("userName"));
            Assert.AreEqual(1, result.GuildPoints);
            Assert.AreEqual(1, world.AddPlayerQueue.Count);
        }
    }
}
