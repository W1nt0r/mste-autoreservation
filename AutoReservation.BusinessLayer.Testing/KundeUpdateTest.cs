using AutoReservation.Dal.Entities;
using AutoReservation.TestEnvironment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using AutoReservation.BusinessLayer;
using AutoReservation.BusinessLayer.Exceptions;

namespace AutoReservation.BusinessLayer.Testing
{
    [TestClass]
    public class KundeUpdateTest
    {
        private KundeManager target;
        private KundeManager Target => target ?? (target = new KundeManager());


        [TestInitialize]
        public void InitializeTestData()
        {
            TestEnvironmentHelper.InitializeTestData();
        }

        [TestMethod]
        public void UpdateKundeTest()
        {
            KundeManager kundeManager = new KundeManager();
            Kunde kunde = kundeManager.Kunde(2);
            kunde.Vorname = "Sergej";
            kunde.Nachname = "Fährlich";
            Kunde updatedKunde = kundeManager.Update(kunde);
            Assert.AreEqual("Sergej", updatedKunde.Vorname);
            Assert.AreEqual("Fährlich", updatedKunde.Nachname);
            updatedKunde.Vorname = "Herr";
            updatedKunde.Nachname = "Döpfel";
            Kunde updated2Kunde = kundeManager.Update(updatedKunde);
            Assert.AreEqual("Herr", updated2Kunde.Vorname);
            Assert.AreEqual("Döpfel", updated2Kunde.Nachname);
        }

        [TestMethod]
        [ExpectedException(typeof(OptimisticConcurrencyException<Kunde>))]
        public void UpdateKundeOptimisticConcurrencyTest()
        {
            KundeManager kundeManager = new KundeManager();
            Kunde kunde = new Kunde { Id = 4, Nachname = "Später", Vorname = "Peter", Geburtsdatum = new DateTime(1976, 03, 27) };
            kundeManager.Update(kunde);
        }
    }
}
