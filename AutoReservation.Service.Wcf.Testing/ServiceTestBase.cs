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
using AutoReservation.BusinessLayer.Exceptions;

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

        [TestCleanup]
        public void CleanUp()
        {
            CallbackSpy.AutoSpy.Clear();
            CallbackSpy.KundeSpy.Clear();
            CallbackSpy.ReservationSpy.Clear();
            CallbackSpy.IsAvailable = null;
            CallbackSpy.ExceptionSpy = null;
        }

        #region Read all entities

        [TestMethod]
        public void GetAutosTest()
        {
            Target.GetAllAutos();
            CallbackSpy.WaitForAnswer();
            var autos = CallbackSpy.AutoSpy;

            Assert.AreEqual(autos.Count, 3);
            CallbackSpy.AutoSpy.Clear();
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
            Target.GetAllReservationen();
            CallbackSpy.WaitForAnswer();
            var reservationen = CallbackSpy.ReservationSpy;

            Assert.AreEqual(reservationen.Count, 3);
            CallbackSpy.ReservationSpy.Clear();
        }

        #endregion

        #region Get by existing ID

        [TestMethod]
        public void GetAutoByIdTest()
        {
            Target.GetAuto(1);
            CallbackSpy.WaitForAnswer();
            AutoDto auto = CallbackSpy.AutoSpy.First();

            Assert.AreEqual(auto.Id, 1);
            Assert.AreEqual(auto.Marke, "Fiat Punto");

            CallbackSpy.AutoSpy.Clear();
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
            Target.GetReservation(1);
            CallbackSpy.WaitForAnswer();
            ReservationDto reservation = CallbackSpy.ReservationSpy.First();

            Assert.AreEqual(reservation.ReservationsNr, 1);
            CallbackSpy.AutoSpy.Clear();
        }
        #endregion

        #region Get by not existing ID

        [TestMethod]
        public void GetAutoByIdWithIllegalIdTest()
        {
            Target.GetAuto(10);
            CallbackSpy.WaitForAnswer();
            string ex = CallbackSpy.ExceptionSpy;
            CallbackSpy.ExceptionSpy = null;
            Assert.AreEqual($"Could not find auto with id 10", ex);
        }

        [TestMethod]
        public void GetKundeByIdWithIllegalIdTest()
        {
            Target.GetKunde(10);
            CallbackSpy.WaitForAnswer();
            string ex = CallbackSpy.ExceptionSpy;
            CallbackSpy.ExceptionSpy = null;
            Assert.AreEqual($"Could not find kunde with id 10", ex);
        }

        [TestMethod]
        public void GetReservationByNrWithIllegalIdTest()
        {
            Target.GetReservation(10);
            CallbackSpy.WaitForAnswer();
            string ex = CallbackSpy.ExceptionSpy;
            CallbackSpy.ExceptionSpy = null;
            Assert.AreEqual($"Could not find reservation with id 10", ex);
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
            Target.GetAuto(1);
            CallbackSpy.WaitForAnswer();
            AutoDto auto = CallbackSpy.AutoSpy.First();
            CallbackSpy.AutoSpy.Clear();

            Target.GetKunde(1);
            CallbackSpy.WaitForAnswer();
            KundeDto kunde = CallbackSpy.KundeSpy.First();
            CallbackSpy.KundeSpy.Clear();

            ReservationDto reservation = new ReservationDto { Auto = auto, Bis = DateTime.Now.AddDays(1), Von = DateTime.Now.AddDays(-1), Kunde = kunde, ReservationsNr = 9999};
            Target.AddReservation(reservation);
            CallbackSpy.WaitForAnswer();
            ReservationDto insertedReservation = CallbackSpy.ReservationSpy.First();

            Assert.AreEqual(insertedReservation.ReservationsNr, 4);
            Assert.AreEqual(insertedReservation.Von, reservation.Von);
            Assert.AreEqual(insertedReservation.Bis, reservation.Bis);
        }

        #endregion

        #region Delete  

        [TestMethod]
        public void DeleteAutoTest()
        {
            Target.GetAuto(1);
            CallbackSpy.WaitForAnswer();
            AutoDto auto = CallbackSpy.AutoSpy.First();
            CallbackSpy.AutoSpy.Clear();

            Target.RemoveAuto(auto);
            CallbackSpy.WaitForAnswer();
            AutoDto deletedAuto = CallbackSpy.AutoSpy.First();
            CallbackSpy.AutoSpy.Clear();

            Target.GetAllAutos();
            CallbackSpy.WaitForAnswer();
            var autos = CallbackSpy.AutoSpy;
            Assert.IsFalse(autos.Contains(deletedAuto));
            CallbackSpy.AutoSpy.Clear();
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
            Target.GetReservation(1);
            CallbackSpy.WaitForAnswer();
            ReservationDto res = CallbackSpy.ReservationSpy.First();
            CallbackSpy.ReservationSpy.Clear();

            Target.RemoveReservation(res);
            CallbackSpy.WaitForAnswer();
            ReservationDto deletedRes = CallbackSpy.ReservationSpy.First();
            CallbackSpy.ReservationSpy.Clear();

            Target.GetAllReservationen();
            CallbackSpy.WaitForAnswer();
            var reservationen = CallbackSpy.ReservationSpy;
            Assert.IsFalse(reservationen.Contains(deletedRes));
            CallbackSpy.ReservationSpy.Clear();
        }

        #endregion

        #region Update

        [TestMethod]
        public void UpdateAutoTest()
        {
            Target.GetAuto(1);
            CallbackSpy.WaitForAnswer();
            AutoDto auto = CallbackSpy.AutoSpy.First();
            CallbackSpy.AutoSpy.Clear();

            auto.Marke = "Es geils auto";

            Target.GetAuto(1);
            CallbackSpy.WaitForAnswer();
            AutoDto updatedauto = CallbackSpy.AutoSpy.First();
            CallbackSpy.AutoSpy.Clear();

            Assert.AreEqual(auto.Id, updatedauto.Id);
            Assert.AreEqual(auto.Marke, "Es geils auto");
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
            DateTime now = DateTime.Now;

            Target.GetReservation(1);
            CallbackSpy.WaitForAnswer();
            ReservationDto reservation = CallbackSpy.ReservationSpy.First();
            CallbackSpy.ReservationSpy.Clear();

            reservation.Von = now;
            reservation.Bis = now.AddDays(2);

            Target.UpdateReservation(reservation);
            CallbackSpy.WaitForAnswer();
            ReservationDto updatedReservation = CallbackSpy.ReservationSpy.First();
            CallbackSpy.ReservationSpy.Clear();

            Assert.AreEqual(updatedReservation.ReservationsNr, reservation.ReservationsNr);
            Assert.AreEqual(updatedReservation.Von, now);
            Assert.AreEqual(updatedReservation.Bis, now.AddDays(2));
        }

        #endregion

        #region Update with optimistic concurrency violation

        [TestMethod]
        public void UpdateAutoWithOptimisticConcurrencyTest()
        {
            AutoDto auto = new AutoDto { Id = 3, Marke = "Tesla", Tagestarif = 320, Basistarif = 10 };
            Target.UpdateAuto(auto);
            CallbackSpy.WaitForAnswer();
            CallbackSpy.AutoSpy.Clear();
            string ex = CallbackSpy.ExceptionSpy;
            CallbackSpy.ExceptionSpy = null;
            Assert.AreEqual(ex, $"Update Auto: Concurrency-Fehler");
        }

        [TestMethod]
        public void UpdateKundeWithOptimisticConcurrencyTest()
        {
            KundeDto kunde = new KundeDto { Id = 4, Nachname = "Später", Vorname = "Peter", Geburtsdatum = new DateTime(1976, 03, 27) };
            Target.UpdateKunde(kunde);
            CallbackSpy.WaitForAnswer();
            string ex = CallbackSpy.ExceptionSpy;
            CallbackSpy.ExceptionSpy = null;
            Assert.AreEqual(ex, $"Update Kunde: Concurrency-Fehler");
        }

        [TestMethod]
        public void UpdateReservationWithOptimisticConcurrencyTest()
        {
            Target.GetReservation(1);
            CallbackSpy.WaitForAnswer();
            ReservationDto res = CallbackSpy.ReservationSpy.First();
            CallbackSpy.ReservationSpy.Clear();

            res.Bis = res.Bis.AddHours(1);

            Target.UpdateReservation(res);
            CallbackSpy.WaitForAnswer();
            ReservationDto resNew = CallbackSpy.ReservationSpy.First();
            CallbackSpy.ReservationSpy.Clear();

            resNew.Bis = resNew.Bis.AddHours(1);
            resNew.RowVersion = res.RowVersion;
            Target.UpdateReservation(resNew);
            CallbackSpy.WaitForAnswer();
            string ex = CallbackSpy.ExceptionSpy;
            CallbackSpy.ExceptionSpy = null;
            Assert.AreEqual(ex, "Update Reservation: Concurrency-Fehler");
        }

        #endregion

        #region Insert / update invalid time range

        [TestMethod]
        public void InsertReservationWithInvalidDateRangeTest()
        {
            Target.GetAuto(1);
            CallbackSpy.WaitForAnswer();
            AutoDto auto = CallbackSpy.AutoSpy.First();
            CallbackSpy.AutoSpy.Clear();

            Target.GetKunde(1);
            CallbackSpy.WaitForAnswer();
            KundeDto kunde = CallbackSpy.KundeSpy.First();
            CallbackSpy.KundeSpy.Clear();

            ReservationDto reservation = new ReservationDto { Auto = auto, Bis = DateTime.Now.AddSeconds(1), Von = DateTime.Now, Kunde = kunde, ReservationsNr = 9999 };
            Target.AddReservation(reservation);
            CallbackSpy.WaitForAnswer();
            string ex = CallbackSpy.ExceptionSpy;

            Assert.AreEqual("Reservation must be at 24 hours long", ex);

            CallbackSpy.ExceptionSpy = null;
        }

        [TestMethod]
        public void InsertReservationWithAutoNotAvailableTest()
        {
            Target.GetReservation(2);
            CallbackSpy.WaitForAnswer();
            ReservationDto reservationFound = CallbackSpy.ReservationSpy.First();

            CallbackSpy.ReservationSpy.Clear();

            ReservationDto reservation = new ReservationDto { Auto = reservationFound.Auto, Kunde = reservationFound.Kunde, Von = reservationFound.Von, Bis = reservationFound.Bis };

            Target.AddReservation(reservation);
            CallbackSpy.WaitForAnswer();
            string ex = CallbackSpy.ExceptionSpy;

            Assert.AreEqual("auto already reserved in this range", ex);

            CallbackSpy.ExceptionSpy = null;
        }

        [TestMethod]
        public void UpdateReservationWithInvalidDateRangeTest()
        {
            Target.GetReservation(1);
            CallbackSpy.WaitForAnswer();
            ReservationDto reservation = CallbackSpy.ReservationSpy.First();
            CallbackSpy.ReservationSpy.Clear();

            reservation.Von = reservation.Bis;
            Target.UpdateReservation(reservation);
            CallbackSpy.WaitForAnswer();
            string ex = CallbackSpy.ExceptionSpy;

            Assert.AreEqual("Reservation must be at 24 hours long", ex);

            CallbackSpy.ExceptionSpy = null;
        }

        [TestMethod]
        public void UpdateReservationWithAutoNotAvailableTest()
        {
            Target.GetReservation(2);
            CallbackSpy.WaitForAnswer();
            ReservationDto reservation = CallbackSpy.ReservationSpy.First();
            CallbackSpy.ReservationSpy.Clear();

            Target.GetAuto(3);
            CallbackSpy.WaitForAnswer();
            AutoDto auto = CallbackSpy.AutoSpy.First();
            CallbackSpy.AutoSpy.Clear();

            reservation.Auto = auto;

            Target.UpdateReservation(reservation);
            CallbackSpy.WaitForAnswer();
            string ex = CallbackSpy.ExceptionSpy;

            Assert.AreEqual("auto already reserved in this range", ex);

            CallbackSpy.ExceptionSpy = null;
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
            Target.GetAuto(1);
            CallbackSpy.WaitForAnswer();
            AutoDto auto = CallbackSpy.AutoSpy.First();

            Target.GetKunde(2);
            CallbackSpy.WaitForAnswer();
            KundeDto kunde = CallbackSpy.KundeSpy.First();

            CallbackSpy.AutoSpy.Clear();
            CallbackSpy.KundeSpy.Clear();

            Target.GetReservation(1);
            CallbackSpy.WaitForAnswer();
            ReservationDto reservation = CallbackSpy.ReservationSpy.First();
            CallbackSpy.ReservationSpy.Clear();

            ReservationDto newRes = new ReservationDto { Auto = auto, Kunde = kunde, Von = reservation.Von, Bis = reservation.Bis };

            Target.IsAutoAvailable(newRes);
            CallbackSpy.WaitForAnswer();
            bool? isAvailable = CallbackSpy.IsAvailable;

            Assert.IsFalse(isAvailable ?? true);
            CallbackSpy.IsAvailable = null;
        }

        #endregion
    }
}
