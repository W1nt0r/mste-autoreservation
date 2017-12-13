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
            Assert.Inconclusive("Test not implemented.");
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
            Assert.Inconclusive("Test not implemented.");
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
        [ExpectedException(typeof(EntityNotFoundException))]
        public void GetAutoByIdWithIllegalIdTest()
        {
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
            CallbackSpy.AutoSpy.Clear();

            Target.GetAllAutos();
            CallbackSpy.WaitForAnswer();
            var autos = CallbackSpy.AutoSpy;

            Assert.IsTrue(autos.Contains(insertedAuto));
            Assert.AreEqual(4, insertedAuto.Id);            
            Assert.AreEqual("Opel Corsa", insertedAuto.Marke);
            CallbackSpy.AutoSpy.Clear();
        }

        [TestMethod]
        public void InsertKundeTest()
        {
            Assert.Inconclusive("Test not implemented.");
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
            AutoDto insertedAuto = CallbackSpy.AutoSpy.First();
            CallbackSpy.AutoSpy.Clear();

            Target.RemoveAuto(insertedAuto);
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
            Assert.Inconclusive("Test not implemented.");
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
            Assert.Inconclusive("Test not implemented.");
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
            Assert.Inconclusive("Test not implemented.");
        }

        [TestMethod]
        public void CheckAvailabilityIsFalseTest()
        {
            Assert.Inconclusive("Test not implemented.");
        }

        #endregion
    }
}
