using AutoReservation.Dal.Entities;
using AutoReservation.TestEnvironment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using AutoReservation.BusinessLayer;
using AutoReservation.BusinessLayer.Exceptions;

namespace AutoReservation.BusinessLayer.Testing
{
    [TestClass]
    public class AutoInsertTests
    {
        private AutoManager target;
        private AutoManager Target => target ?? (target = new AutoManager());


        [TestInitialize]
        public void InitializeTestData()
        {
            TestEnvironmentHelper.InitializeTestData();
        }

        [TestMethod]
        public void InsertAutoTest()
        {
            AutoManager autoManager = new AutoManager();
            Auto auto = new MittelklasseAuto { Marke = "Opel Corsa", Tagestarif = 100 };
            Auto insertedAuto = autoManager.Insert(auto);
            Assert.AreEqual(4, insertedAuto.Id);
            Auto getAuto = autoManager.Auto(4);
            Assert.AreEqual("Opel Corsa", getAuto.Marke);
        }
    }
}
