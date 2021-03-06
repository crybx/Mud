﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Global;
using Objects.Global.CanMobDoSomething.Interface;
using Objects.Global.Notify.Interface;
using Objects.Global.Settings.Interface;
using Objects.Language.Interface;
using Objects.Mob.Interface;
using Objects.Room.Interface;
using Shared.FileIO.Interface;
using Shared.TagWrapper.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace ObjectsUnitTest.Map
{
    [TestClass]
    public class MapUnitTest
    {
        Mock<IFileIO> fileIO;
        Mock<IMobileObject> mob;
        Mock<ISettings> settings;
        Mock<ICanMobDoSomething> canMobDoSomething;
        Mock<IRoom> room;
        Mock<ITagWrapper> tagwrapper;
        Mock<INotify> notify;
        Objects.Global.Map.Map map;

        [TestInitialize]
        public void Setup()
        {
            fileIO = new Mock<IFileIO>();
            mob = new Mock<IMobileObject>();
            settings = new Mock<ISettings>();
            canMobDoSomething = new Mock<ICanMobDoSomething>();
            room = new Mock<IRoom>();
            tagwrapper = new Mock<ITagWrapper>();
            notify = new Mock<INotify>();
            map = new Objects.Global.Map.Map();

            mob.Setup(e => e.Room).Returns(room.Object);
            room.Setup(e => e.Zone).Returns(1);
            room.Setup(e => e.Id).Returns(2);
            settings.Setup(e => e.AssetsDirectory).Returns("assetsDir");
            fileIO.Setup(e => e.Exists(@"assetsDir\Map\1.MapConversion")).Returns(true);
            fileIO.Setup(e => e.ReadLines(@"assetsDir\Map\1.MapConversion")).Returns(new List<string>() { "2|1|90|10" });
            tagwrapper.Setup(e => e.WrapInTag("1|1|90|10", Shared.TagWrapper.TagWrapper.TagType.Map)).Returns("mapInfo");

            GlobalReference.GlobalValues.FileIO = fileIO.Object;
            GlobalReference.GlobalValues.Settings = settings.Object;
            GlobalReference.GlobalValues.CanMobDoSomething = canMobDoSomething.Object;
            GlobalReference.GlobalValues.TagWrapper = tagwrapper.Object;
            GlobalReference.GlobalValues.Notify = notify.Object;
        }

        [TestMethod]
        public void Map_SendMapPosition_SettingsOnMobCanSee()
        {
            settings.Setup(e => e.SendMapPosition).Returns(true);
            canMobDoSomething.Setup(e => e.SeeDueToLight(mob.Object)).Returns(true);

            map.SendMapPosition(mob.Object);

            notify.Verify(e => e.Mob(mob.Object, It.IsAny<ITranslationMessage>()));
        }

        [TestMethod]
        public void Map_SendMapPosition_SettingsOffMobCanSee()
        {
            settings.Setup(e => e.SendMapPosition).Returns(false);
            canMobDoSomething.Setup(e => e.SeeDueToLight(mob.Object)).Returns(true);

            map.SendMapPosition(mob.Object);

            mob.Verify(e => e.EnqueueMessage("mapInfo"), Times.Never);
        }

        [TestMethod]
        public void Map_SendMapPosition_SettingsOnfMobCanNotSee()
        {
            settings.Setup(e => e.SendMapPosition).Returns(true);
            canMobDoSomething.Setup(e => e.SeeDueToLight(mob.Object)).Returns(false);

            map.SendMapPosition(mob.Object);

            mob.Verify(e => e.EnqueueMessage("mapInfo"), Times.Never);
        }
    }
}
