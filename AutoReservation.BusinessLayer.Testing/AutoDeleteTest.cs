using AutoReservation.Dal.Entities;
using AutoReservation.TestEnvironment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using AutoReservation.BusinessLayer;
using AutoReservation.BusinessLayer.Exceptions;


namespace AutoReservation.BusinessLayer.Testing
{
    [TestClass]
    public class AutoDeleteTests
    {
        private AutoManager target;
        private AutoManager Target => target ?? (target = new AutoManager());


        [TestInitialize]
        public void InitializeTestData()
        {
            TestEnvironmentHelper.InitializeTestData();
        }

        [TestMethod]
        [ExpectedException(typeof(EntityNotFoundException))]
        public void DeleteAutoTest()
        {
            AutoManager autoManager = new AutoManager();
            Auto auto = autoManager.Auto(1);
            autoManager.Delete(auto);
            autoManager.Auto(1);
        }

        [TestMethod]
        [ExpectedException(typeof(OptimisticConcurrencyException<Auto>))]
        public void DeleteAutoOptimisticConcurrencyTest()
        {
            AutoManager autoManager = new AutoManager();
            Auto auto = new LuxusklasseAuto { Id = 3, Marke = "Tesla", Tagestarif = 320, Basistarif = 10 };
            autoManager.Delete(auto);
        }
    }
}
