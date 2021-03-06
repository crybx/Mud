﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Global;
using Objects.Global.CanMobDoSomething.Interface;
using Objects.Language.Interface;
using Objects.Mob.Interface;
using Objects.Room.Interface;
using Objects.Zone.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace ObjectsUnitTest.Global.Notify
{
    [TestClass]
    public class NotifyUnitTest
    {
        Objects.Global.Notify.Notify notify;
        Mock<INonPlayerCharacter> npc;
        Mock<IPlayerCharacter> pc;
        Mock<IRoom> room;
        Mock<ICanMobDoSomething> canMobDoSometing;
        Mock<IZone> zone;
        Mock<ITranslationMessage> translationMessage;

        [TestInitialize]
        public void Setup()
        {
            notify = new Objects.Global.Notify.Notify();
            npc = new Mock<INonPlayerCharacter>();
            pc = new Mock<IPlayerCharacter>();
            room = new Mock<IRoom>();
            canMobDoSometing = new Mock<ICanMobDoSomething>();
            zone = new Mock<IZone>();
            translationMessage = new Mock<ITranslationMessage>();
            Dictionary<int, IRoom> dictionary = new Dictionary<int, IRoom>();

            room.Setup(e => e.NonPlayerCharacters).Returns(new List<INonPlayerCharacter>() { npc.Object });
            room.Setup(e => e.PlayerCharacters).Returns(new List<IPlayerCharacter>() { pc.Object });
            zone.Setup(e => e.Rooms).Returns(dictionary);
            dictionary.Add(1, room.Object);
            translationMessage.Setup(e => e.GetTranslatedMessage(npc.Object)).Returns("{performer} {target} message");
            translationMessage.Setup(e => e.GetTranslatedMessage(pc.Object)).Returns("{performer} {target} message");
            npc.Setup(e => e.SentenceDescription).Returns("npcSentance");
            pc.Setup(e => e.SentenceDescription).Returns("pcSentance");

            GlobalReference.GlobalValues.CanMobDoSomething = canMobDoSometing.Object;
        }

        [TestMethod]
        public void Notify_Zone_NoParams()
        {
            notify.Zone(npc.Object, pc.Object, zone.Object, translationMessage.Object);

            npc.Verify(e => e.EnqueueMessage("npcSentance pcSentance message"), Times.Once);
            pc.Verify(e => e.EnqueueMessage("npcSentance pcSentance message"), Times.Once);
        }

        [TestMethod]
        public void Notify_Zone_ExcludedNpc()
        {
            notify.Zone(npc.Object, pc.Object, zone.Object, translationMessage.Object, new List<IMobileObject>() { npc.Object });

            npc.Verify(e => e.EnqueueMessage("npcSentance pcSentance message"), Times.Never);
            pc.Verify(e => e.EnqueueMessage("npcSentance pcSentance message"), Times.Once);
        }

        [TestMethod]
        public void Notify_Zone_ExcludedPc()
        {
            notify.Zone(npc.Object, pc.Object, zone.Object, translationMessage.Object, new List<IMobileObject>() { pc.Object });

            npc.Verify(e => e.EnqueueMessage("npcSentance pcSentance message"), Times.Once);
            pc.Verify(e => e.EnqueueMessage("npcSentance pcSentance message"), Times.Never);
        }

        [TestMethod]
        public void Notify_Zone_ExcludedBoth()
        {
            notify.Zone(npc.Object, pc.Object, zone.Object, translationMessage.Object, new List<IMobileObject>() { npc.Object, pc.Object });

            npc.Verify(e => e.EnqueueMessage("npcSentance pcSentance message"), Times.Never);
            pc.Verify(e => e.EnqueueMessage("npcSentance pcSentance message"), Times.Never);
        }

        [TestMethod]
        public void Notify_Zone_NpcCanNotSee()
        {
            canMobDoSometing.Setup(e => e.SeeObject(pc.Object, pc.Object)).Returns(true);
            canMobDoSometing.Setup(e => e.SeeObject(pc.Object, npc.Object)).Returns(true);

            notify.Zone(npc.Object, pc.Object, zone.Object, translationMessage.Object, null, true, false);

            npc.Verify(e => e.EnqueueMessage("unknown unknown message"), Times.Once);
            pc.Verify(e => e.EnqueueMessage("npcSentance pcSentance message"), Times.Once);
        }

        [TestMethod]
        public void Notify_Zone_PcCanNotSee()
        {
            canMobDoSometing.Setup(e => e.SeeObject(npc.Object, pc.Object)).Returns(true);
            canMobDoSometing.Setup(e => e.SeeObject(npc.Object, npc.Object)).Returns(true);

            notify.Zone(npc.Object, pc.Object, zone.Object, translationMessage.Object, null, true, false);

            npc.Verify(e => e.EnqueueMessage("npcSentance pcSentance message"), Times.Once);
            pc.Verify(e => e.EnqueueMessage("unknown unknown message"), Times.Once);
        }

        [TestMethod]
        public void Notify_Zone_NpcCanNotHear()
        {
            canMobDoSometing.Setup(e => e.Hear(pc.Object, pc.Object)).Returns(true);
            canMobDoSometing.Setup(e => e.Hear(pc.Object, npc.Object)).Returns(true);

            notify.Zone(npc.Object, pc.Object, zone.Object, translationMessage.Object, null, false, true);

            npc.Verify(e => e.EnqueueMessage("unknown unknown message"), Times.Once);
            pc.Verify(e => e.EnqueueMessage("npcSentance pcSentance message"), Times.Once);
        }

        [TestMethod]
        public void Notify_Zone_PcCanNotHear()
        {
            canMobDoSometing.Setup(e => e.Hear(npc.Object, pc.Object)).Returns(true);
            canMobDoSometing.Setup(e => e.Hear(npc.Object, npc.Object)).Returns(true);

            notify.Zone(npc.Object, pc.Object, zone.Object, translationMessage.Object, null, false, true);

            npc.Verify(e => e.EnqueueMessage("npcSentance pcSentance message"), Times.Once);
            pc.Verify(e => e.EnqueueMessage("unknown unknown message"), Times.Once);
        }


    }
}
