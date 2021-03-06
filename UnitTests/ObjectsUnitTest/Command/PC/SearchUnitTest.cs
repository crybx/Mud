﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Command.Interface;
using Moq;
using Shared.TagWrapper.Interface;
using Objects.Mob.Interface;
using Objects.Global;
using static Shared.TagWrapper.TagWrapper;
using System.Collections.Generic;
using Objects.Command.PC;
using System.Linq;
using Objects.Item.Interface;
using Objects.Item.Items.Interface;
using Objects.Global.FindObjects.Interface;
using Objects.Trap.Interface;
using Objects.Room.Interface;

namespace ObjectsUnitTest.Command.PC
{
    [TestClass]
    public class SearchUnitTest
    {
        IMobileObjectCommand command;
        Mock<ITagWrapper> tagWrapper;
        Mock<IMobileObject> mob;
        Mock<ICommand> mockCommand;
        Mock<IRoom> room;
        List<ITrap> traps;

        [TestInitialize]
        public void Setup()
        {
            tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag("Search", TagType.Info)).Returns("message");
            tagWrapper.Setup(e => e.WrapInTag("A trap was found in the pit." + Environment.NewLine + "A trap was found in the arrow.", TagType.Info)).Returns("message2");
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            mob = new Mock<IMobileObject>();
            mockCommand = new Mock<ICommand>();
            command = new Search();
            room = new Mock<IRoom>();
            traps = new List<ITrap>();

            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>());
            mob.Setup(e => e.Room).Returns(room.Object);
            room.Setup(e => e.Traps).Returns(traps);
        }

        [TestMethod]
        public void Search_Instructions()
        {
            IResult result = command.Instructions;

            Assert.IsTrue(result.ResultSuccess);
            Assert.AreEqual("message", result.ResultMessage);
        }

        [TestMethod]
        public void Search_CommandTrigger()
        {
            IEnumerable<string> result = command.CommandTrigger;
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Contains("Search"));
        }

        [TestMethod]
        public void Search_PerformCommand_NothingFound()
        {
            Mock<IParameter> parameter = new Mock<IParameter>();

            tagWrapper.Setup(e => e.WrapInTag("Nothing found was found.", TagType.Info)).Returns("message");
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter.Object });

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsTrue(result.ResultSuccess);
            Assert.AreEqual("message", result.ResultMessage);
        }

        [TestMethod]
        public void Search_PerformCommand_FoundTwoTraps()
        {
            Mock<IParameter> parameter = new Mock<IParameter>();
            Mock<ITrap> trap1 = new Mock<ITrap>();
            Mock<ITrap> trap2 = new Mock<ITrap>();

            tagWrapper.Setup(e => e.WrapInTag("Nothing found was found.", TagType.Info)).Returns("message");
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter.Object });
            trap1.Setup(e => e.DisarmWord).Returns(new List<string>() { "pit", "pit2" });
            trap2.Setup(e => e.DisarmWord).Returns(new List<string>() { "arrow", "arrow2" });

            traps.Add(trap1.Object);
            traps.Add(trap2.Object);

            IResult result = command.PerformCommand(mob.Object, mockCommand.Object);
            Assert.IsTrue(result.ResultSuccess);
            Assert.AreEqual("message2", result.ResultMessage);
        }
    }
}
