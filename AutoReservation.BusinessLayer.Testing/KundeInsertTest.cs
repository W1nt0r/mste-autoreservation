using AutoReservation.Dal.Entities;
using AutoReservation.TestEnvironment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using AutoReservation.BusinessLayer;
using AutoReservation.BusinessLayer.Exceptions;

namespace AutoReservation.BusinessLayer.Testing
{
    [TestClass]
    public class KundeInsertTest
    {
        private KundeManager target;
        private KundeManager Target => target ?? (target = new KundeManager());


        [TestInitialize]
        public void InitializeTestData()
        {
            TestEnvironmentHelper.InitializeTestData();
        }

        [TestMethod]
        public void InsertKundeTest()
        {
            KundeManager kundeManager = new KundeManager();
            Kunde kunde = new Kunde { Nachname = "Später", Vorname = "Peter", Geburtsdatum = new DateTime(1976, 03, 27) };
            Kunde insertedKunde = kundeManager.Insert(kunde);
            Assert.AreEqual(5, insertedKunde.Id);
            Kunde getKunde = kundeManager.Kunde(5);
            Assert.AreEqual("Später", getKunde.Nachname);
        }
    }
}
