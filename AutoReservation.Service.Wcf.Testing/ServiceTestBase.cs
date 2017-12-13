using AutoReservation.Common.DataTransferObjects;
using AutoReservation.Common.Interfaces;
using AutoReservation.TestEnvironment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using AutoReservation.Common.DataTransferObjects.Faults;
using System.Threading;

namespace AutoReservation.Service.Wcf.Testing
{
    [TestClass]
    public abstract class ServiceTestBase
    {
        protected abstract IAutoReservationService Target { get; }
        protected abstract AutoReservationResultCallbackSpy CallbackSpy { get; }

        [TestInitialize]
        public void InitializeTestData()
        {
            TestEnvironmentHelper.InitializeTestData();
        }

        #region Read all entities

        [TestMethod]
        public void GetAutosTest()
        {
            Assert.Inconclusive("Test not implemented.");
        }

        [TestMethod]
        public void GetKundenTest()
        {
            Target.GetAllKunden();

            CallbackSpy.WaitForAnswer();
            List<KundeDto> allKunden = CallbackSpy.KundeSpy;

            Assert.AreEqual(4, allKunden.Count());

            CallbackSpy.KundeSpy.Clear();
        }

        [TestMethod]
        public void GetReservationenTest()
        {
            Assert.Inconclusive("Test not implemented.");
        }

        #endregion

        #region Get by existing ID

        [TestMethod]
        public void GetAutoByIdTest()
        {
            Assert.Inconclusive("Test not implemented.");
        }

        [TestMethod]
        public void GetKundeByIdTest()
        {
            int targetId = 1;

            Target.GetKunde(targetId);

            CallbackSpy.WaitForAnswer();
            KundeDto foundKunde = CallbackSpy.KundeSpy.First();

            Assert.AreEqual(1, foundKunde.Id);
            Assert.AreEqual("Nass", foundKunde.Nachname);

            CallbackSpy.KundeSpy.Clear();
        }

        [TestMethod]
        public void GetReservationByNrTest()
        {
            Assert.Inconclusive("Test not implemented.");
        }

        #endregion

        #region Get by not existing ID

        [TestMethod]
        public void GetAutoByIdWithIllegalIdTest()
        {
            Assert.Inconclusive("Test not implemented.");
        }

        [TestMethod]
        public void GetKundeByIdWithIllegalIdTest()
        {
            Assert.Inconclusive("Test not implemented.");
        }

        [TestMethod]
        public void GetReservationByNrWithIllegalIdTest()
        {
            Assert.Inconclusive("Test not implemented.");
        }

        #endregion

        #region Insert

        [TestMethod]
        public void InsertAutoTest()
        {
            AutoDto auto = new AutoDto { Marke = "Opel Corsa", Tagestarif = 100, AutoKlasse = AutoKlasse.Mittelklasse };
            Target.AddAuto(auto);

            CallbackSpy.WaitForAnswer();

            AutoDto insertedAuto = CallbackSpy.AutoSpy.First();

            Assert.AreEqual(4, insertedAuto.Id);
            Assert.AreEqual("Opel Corsa", insertedAuto.Marke);

            CallbackSpy.AutoSpy.Clear();
        }

        [TestMethod]
        public void InsertKundeTest()
        {
            KundeDto kunde = new KundeDto { Nachname = "Später", Vorname = "Peter", Geburtsdatum = new DateTime(1976, 03, 27) };
            Target.AddKunde(kunde);

            CallbackSpy.WaitForAnswer();

            KundeDto insertedKunde = CallbackSpy.KundeSpy.First();

            Assert.AreEqual(5, insertedKunde.Id);
            Assert.AreEqual("Später", insertedKunde.Nachname);

            CallbackSpy.KundeSpy.Clear();
        }

        [TestMethod]
        public void InsertReservationTest()
        {
            Assert.Inconclusive("Test not implemented.");
        }

        #endregion

        #region Delete  

        [TestMethod]
        public void DeleteAutoTest()
        {
            Assert.Inconclusive("Test not implemented.");
        }

        [TestMethod]
        public void DeleteKundeTest()
        {
            Target.GetKunde(2);

            CallbackSpy.WaitForAnswer();
            KundeDto kundeToRemove = CallbackSpy.KundeSpy.First();
            CallbackSpy.KundeSpy.Clear();

            Target.RemoveKunde(kundeToRemove);
            CallbackSpy.WaitForAnswer();
            KundeDto removedKunde = CallbackSpy.KundeSpy.First();

            Assert.AreEqual(kundeToRemove.Id, removedKunde.Id);
            Assert.AreEqual(kundeToRemove.Nachname, removedKunde.Nachname);
            Assert.AreEqual(kundeToRemove.Vorname, removedKunde.Vorname);
            Assert.AreEqual(kundeToRemove.Geburtsdatum, removedKunde.Geburtsdatum);

            CallbackSpy.KundeSpy.Clear();
        }

        [TestMethod]
        public void DeleteReservationTest()
        {
            Assert.Inconclusive("Test not implemented.");
        }

        #endregion

        #region Update

        [TestMethod]
        public void UpdateAutoTest()
        {
            Assert.Inconclusive("Test not implemented.");
        }

        [TestMethod]
        public void UpdateKundeTest()
        {
            Target.GetKunde(1);

            CallbackSpy.WaitForAnswer();
            KundeDto kundeToUpdate = CallbackSpy.KundeSpy.First();
            CallbackSpy.KundeSpy.Clear();

            kundeToUpdate.Nachname = "Bholika";
            Target.UpdateKunde(kundeToUpdate);
            CallbackSpy.WaitForAnswer();
            KundeDto updatedKunde = CallbackSpy.KundeSpy.First();

            Assert.AreEqual(kundeToUpdate.Id, updatedKunde.Id);
            Assert.AreEqual("Bholika", updatedKunde.Nachname);

            CallbackSpy.KundeSpy.Clear();
        }

        [TestMethod]
        public void UpdateReservationTest()
        {
            Assert.Inconclusive("Test not implemented.");
        }

        #endregion

        #region Update with optimistic concurrency violation

        [TestMethod]
        public void UpdateAutoWithOptimisticConcurrencyTest()
        {
            Assert.Inconclusive("Test not implemented.");
        }

        [TestMethod]
        public void UpdateKundeWithOptimisticConcurrencyTest()
        {
            Assert.Inconclusive("Test not implemented.");
        }

        [TestMethod]
        public void UpdateReservationWithOptimisticConcurrencyTest()
        {
            Assert.Inconclusive("Test not implemented.");
        }

        #endregion

        #region Insert / update invalid time range

        [TestMethod]
        public void InsertReservationWithInvalidDateRangeTest()
        {
            Assert.Inconclusive("Test not implemented.");
        }

        [TestMethod]
        public void InsertReservationWithAutoNotAvailableTest()
        {
            Assert.Inconclusive("Test not implemented.");
        }

        [TestMethod]
        public void UpdateReservationWithInvalidDateRangeTest()
        {
            Assert.Inconclusive("Test not implemented.");
        }

        [TestMethod]
        public void UpdateReservationWithAutoNotAvailableTest()
        {
            Assert.Inconclusive("Test not implemented.");
        }

        #endregion

        #region Check Availability

        [TestMethod]
        public void CheckAvailabilityIsTrueTest()
        {
            Target.GetAuto(2);
            CallbackSpy.WaitForAnswer();
            AutoDto auto = CallbackSpy.AutoSpy.First();

            Target.GetKunde(3);
            CallbackSpy.WaitForAnswer();
            KundeDto kunde = CallbackSpy.KundeSpy.First();

            CallbackSpy.AutoSpy.Clear();
            CallbackSpy.KundeSpy.Clear();

            ReservationDto reservation = new ReservationDto { Auto = auto, Kunde = kunde, Von = new DateTime(2017, 11, 28), Bis = new DateTime(2018, 01, 01) };

            Target.IsAutoAvailable(reservation);
            CallbackSpy.WaitForAnswer();
            bool? isAvailable = CallbackSpy.IsAvailable;

            Assert.IsTrue(isAvailable ?? false);

            CallbackSpy.IsAvailable = null;
        }

        [TestMethod]
        public void CheckAvailabilityIsFalseTest()
        {
            Assert.Inconclusive("Test not implemented.");
        }

        #endregion
    }
}
