using AutoReservation.Dal.Entities;
using AutoReservation.TestEnvironment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using AutoReservation.BusinessLayer;
using AutoReservation.BusinessLayer.Exceptions;

namespace AutoReservation.BusinessLayer.Testing
{
    [TestClass]
    public class KundeDeleteTest
    {
        private KundeManager target;
        private KundeManager Target => target ?? (target = new KundeManager());


        [TestInitialize]
        public void InitializeTestData()
        {
            TestEnvironmentHelper.InitializeTestData();
        }

        [TestMethod]
        [ExpectedException(typeof(EntityNotFoundException))]
        public void DeleteKundeTest()
        {
            KundeManager kundeManager = new KundeManager();
            Kunde kunde = kundeManager.Kunde(3);
            kundeManager.Delete(kunde);
            kundeManager.Kunde(3);
        }

        [TestMethod]
        [ExpectedException(typeof(OptimisticConcurrencyException<Kunde>))]
        public void DeleteKundeOptimisticConcurrencyTest()
        {
            KundeManager kundeManager = new KundeManager();
            Kunde kunde = new Kunde { Id = 1, Nachname = "Später", Vorname = "Peter", Geburtsdatum = new DateTime(1976, 03, 27) };
            kundeManager.Delete(kunde);
        }
    }
}
