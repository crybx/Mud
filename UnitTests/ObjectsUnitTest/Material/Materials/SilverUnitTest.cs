﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Global;
using Objects.Global.Random.Interface;
using Objects.Material.Materials;

namespace ObjectsUnitTest.Material.Materials
{
    [TestClass]
    public class SilverUnitTest
    {
        private Silver material;
        [TestInitialize]
        public void Setup()
        {
            Mock<IRandom> random = new Mock<IRandom>();
            random.Setup(e => e.Next(6, 9)).Returns(1);
            random.Setup(e => e.Next(9, 12)).Returns(2);
            random.Setup(e => e.Next(12, 15)).Returns(3);
            GlobalReference.GlobalValues.Random = random.Object;
            material = new Silver();
        }

        [TestMethod]
        public void Silver_Constructor()
        {
            Assert.AreEqual(0.2M, material.Bludgeon);
            Assert.AreEqual(0.2M, material.Pierce);
            Assert.AreEqual(0.2M, material.Slash);

            Assert.AreEqual(0.2M, material.Force);
            Assert.AreEqual(0.2M, material.Necrotic);
            Assert.AreEqual(0.2M, material.Psychic);
            Assert.AreEqual(0.2M, material.Radiant);
            Assert.AreEqual(0.2M, material.Thunder);

            Assert.AreEqual(0.3M, material.Acid);
            Assert.AreEqual(0.1M, material.Cold);
            Assert.AreEqual(0.1M, material.Fire);
            Assert.AreEqual(0.1M, material.Lightning);
            Assert.AreEqual(0.3M, material.Poison);
        }
    }
}
