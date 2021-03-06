﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects.Command.PC;
using Moq;
using Shared.TagWrapper.Interface;
using Objects.Global;
using Objects.Command.Interface;
using static Shared.TagWrapper.TagWrapper;
using System.Collections.Generic;
using System.Linq;
using Objects.Mob.Interface;
using Objects.Item.Interface;
using Objects.Item;
using Objects.Room.Interface;
using Objects.Global.FindObjects.Interface;
using Objects.Trap.Interface;
using Objects.Global.Random.Interface;
using Objects.Damage.Interface;
using Objects.Die.Interface;
using Objects.Global.Stats;

namespace ObjectsUnitTest.Command.PC
{
    [TestClass]
    public class DisarmUnitTest
    {
        IMobileObjectCommand command;
        Mock<ITagWrapper> tagWrapper;
        Mock<IMobileObject> performer;
        Mock<IRoom> room;
        Mock<ITrap> trap;
        Mock<IRandom> random;

        [TestInitialize]
        public void Setup()
        {
            tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag("Disarm [Trap]", TagType.Info)).Returns("message");
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;
            command = new Disarm();

            performer = new Mock<IMobileObject>();
            room = new Mock<IRoom>();
            trap = new Mock<ITrap>();
            random = new Mock<IRandom>();

            performer.Setup(e => e.Room).Returns(room.Object);
            room.Setup(e => e.Traps).Returns(new List<ITrap>() { trap.Object });
            trap.Setup(e => e.DisarmWord).Returns(new List<string>() { "arrow" });
            random.Setup(e => e.Next(0)).Returns(5);

            GlobalReference.GlobalValues.Random = random.Object;
        }

        [TestMethod]
        public void Disarm_Instructions()
        {
            IResult result = command.Instructions;

            Assert.IsTrue(result.ResultSuccess);
            Assert.AreEqual("message", result.ResultMessage);
        }

        [TestMethod]
        public void Disarm_CommandTrigger()
        {
            IEnumerable<string> result = command.CommandTrigger;
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Contains("Disarm"));
        }

        [TestMethod]
        public void Disarm_PerformCommand_NoParameter()
        {
            Mock<ITagWrapper> tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag("What would you like to disarm?", TagType.Info)).Returns("message");
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            Mock<ICommand> mockCommand = new Mock<ICommand>();
            Mock<IParameter> parameter = new Mock<IParameter>();
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>());

            IResult result = command.PerformCommand(performer.Object, mockCommand.Object);
            Assert.IsFalse(result.ResultSuccess);
            Assert.AreEqual("message", result.ResultMessage);
        }

        [TestMethod]
        public void Disarm_PerformCommand_NoTrap()
        {
            Mock<ITagWrapper> tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag("Unable to find a big trap.", TagType.Info)).Returns("message");
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            Mock<ICommand> mockCommand = new Mock<ICommand>();
            Mock<IParameter> parameter = new Mock<IParameter>();

            parameter.Setup(e => e.ParameterValue).Returns("big");
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter.Object });

            IResult result = command.PerformCommand(performer.Object, mockCommand.Object);
            Assert.IsTrue(result.ResultSuccess);
            Assert.AreEqual("message", result.ResultMessage);
        }

        [TestMethod]
        public void Disarm_PerformCommand_TrapSuccessfulDisarm()
        {
            Mock<ITagWrapper> tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag("You successfully disarm the arrow trap.", TagType.Info)).Returns("message");
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            Mock<ICommand> mockCommand = new Mock<ICommand>();
            Mock<IParameter> parameter = new Mock<IParameter>();

            parameter.Setup(e => e.ParameterValue).Returns("arrow");
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter.Object });

            IResult result = command.PerformCommand(performer.Object, mockCommand.Object);
            Assert.IsTrue(result.ResultSuccess);
            Assert.AreEqual("message", result.ResultMessage);
        }

        [TestMethod]
        public void Disarm_PerformCommand_TrapFailDisarm()
        {
            Mock<ITagWrapper> tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag("You were unable to disarm the arrow trap.", TagType.Info)).Returns("message");
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            Mock<ICommand> mockCommand = new Mock<ICommand>();
            Mock<IParameter> parameter = new Mock<IParameter>();

            parameter.Setup(e => e.ParameterValue).Returns("arrow");
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter.Object });
            trap.Setup(e => e.DisarmSuccessRoll).Returns(10);

            IResult result = command.PerformCommand(performer.Object, mockCommand.Object);
            Assert.IsTrue(result.ResultSuccess);
            Assert.AreEqual("message", result.ResultMessage);
        }

        [TestMethod]
        public void Disarm_PerformCommand_TrapFailDisarmCustomMessage()
        {
            Mock<ITagWrapper> tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag("failure message", TagType.Info)).Returns("message");
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            Mock<ICommand> mockCommand = new Mock<ICommand>();
            Mock<IParameter> parameter = new Mock<IParameter>();
            Mock<IDamage> damage = new Mock<IDamage>();
            Mock<IDice> dice = new Mock<IDice>();


            parameter.Setup(e => e.ParameterValue).Returns("arrow");
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter.Object });
            trap.Setup(e => e.DisarmSuccessRoll).Returns(10);
            trap.Setup(e => e.DisarmFailureDamage).Returns(damage.Object);
            trap.Setup(e => e.DisarmFailureMessage).Returns("failure message");
            damage.Setup(e => e.Dice).Returns(dice.Object);
            dice.Setup(e => e.Die).Returns(1);
            dice.Setup(e => e.Sides).Returns(1);

            IResult result = command.PerformCommand(performer.Object, mockCommand.Object);
            Assert.IsTrue(result.ResultSuccess);
            Assert.AreEqual("message", result.ResultMessage);
            performer.Verify(e => e.TakeDamage(0, damage.Object, null));
        }

        [TestMethod]
        public void Disarm_PerformCommand_TrapFailDisarmDefaultMessage()
        {
            Mock<ITagWrapper> tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag("You tried to disarm the trap but accidentally tripped it.", TagType.Info)).Returns("message");
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            Mock<ICommand> mockCommand = new Mock<ICommand>();
            Mock<IParameter> parameter = new Mock<IParameter>();
            Mock<IDamage> damage = new Mock<IDamage>();
            Mock<IDice> dice = new Mock<IDice>();

            parameter.Setup(e => e.ParameterValue).Returns("arrow");
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter.Object });
            trap.Setup(e => e.DisarmSuccessRoll).Returns(10);
            trap.Setup(e => e.DisarmFailureDamage).Returns(damage.Object);
            damage.Setup(e => e.Dice).Returns(dice.Object);
            dice.Setup(e => e.Die).Returns(1);
            dice.Setup(e => e.Sides).Returns(1);

            IResult result = command.PerformCommand(performer.Object, mockCommand.Object);
            Assert.IsTrue(result.ResultSuccess);
            Assert.AreEqual("message", result.ResultMessage);
            performer.Verify(e => e.TakeDamage(0, damage.Object, null));
        }


        [TestMethod]
        public void Disarm_PerformCommand_CodeCompletion()
        {
            Mock<ITagWrapper> tagWrapper = new Mock<ITagWrapper>();
            tagWrapper.Setup(e => e.WrapInTag("You successfully disarm the arrow trap.", TagType.Info)).Returns("message");
            GlobalReference.GlobalValues.TagWrapper = tagWrapper.Object;

            Mock<ICommand> mockCommand = new Mock<ICommand>();
            Mock<IParameter> parameter = new Mock<IParameter>();

            parameter.Setup(e => e.ParameterValue).Returns("arrow");
            mockCommand.Setup(e => e.Parameters).Returns(new List<IParameter>() { parameter.Object });
            trap.Setup(e => e.DisarmSuccessRoll).Returns(10);

            foreach (var item in Enum.GetValues(typeof(Stats.Stat)))
            {
                trap.Setup(e => e.DisarmStat).Returns((Stats.Stat)item);
                command.PerformCommand(performer.Object, mockCommand.Object);
            }
        }
    }
}
