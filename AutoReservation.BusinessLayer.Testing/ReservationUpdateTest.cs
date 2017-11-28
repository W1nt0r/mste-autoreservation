using AutoReservation.Dal.Entities;
using AutoReservation.TestEnvironment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AutoReservation.BusinessLayer.Testing
{
    [TestClass]
    public class ReservationUpdateTest
    {
        private ReservationManager target;
        private ReservationManager Target => target ?? (target = new ReservationManager());


        [TestInitialize]
        public void InitializeTestData()
        {
            TestEnvironmentHelper.InitializeTestData();
        }

        [TestMethod]
        public void UpdateReservationTest()
        {
            ReservationManager reservationManager = new ReservationManager();
            Reservation reservation = reservationManager.Reservation(1);
            reservation.KundeId = 2;
            Reservation updatedReservation = reservationManager.Update(reservation);
            Assert.AreEqual("Beil", updatedReservation.Kunde.Nachname);
        }
    }
}
