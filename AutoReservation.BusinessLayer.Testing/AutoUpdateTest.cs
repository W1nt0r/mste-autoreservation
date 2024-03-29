﻿using AutoReservation.Dal.Entities;
using AutoReservation.TestEnvironment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using AutoReservation.BusinessLayer;
using AutoReservation.BusinessLayer.Exceptions;


namespace AutoReservation.BusinessLayer.Testing
{
    [TestClass]
    public class AutoUpdateTests
    {
        private AutoManager target;
        private AutoManager Target => target ?? (target = new AutoManager());


        [TestInitialize]
        public void InitializeTestData()
        {
            TestEnvironmentHelper.InitializeTestData();
        }

        [TestMethod]
        public void UpdateAutoTest()
        {
            AutoManager autoManager = new AutoManager();
            Auto auto = autoManager.Auto(1);
            auto.Marke = "Fiat Multipla";
            auto.Tagestarif = 100;
            Auto updatedAuto = autoManager.Update(auto);
            Assert.AreEqual("Fiat Multipla", updatedAuto.Marke);
            Assert.AreEqual(100, updatedAuto.Tagestarif);
        }

        [TestMethod]
        [ExpectedException(typeof(OptimisticConcurrencyException<Auto>))]
        public void UpdateAutoOptimisticConcurrencyTest()
        {
            AutoManager autoManager = new AutoManager();
            Auto auto = new LuxusklasseAuto { Id = 3, Marke = "Tesla", Tagestarif = 320, Basistarif = 10 };
            autoManager.Update(auto);
        }
    }
}
