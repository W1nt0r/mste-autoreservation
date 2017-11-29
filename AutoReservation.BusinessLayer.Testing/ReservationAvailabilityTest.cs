using AutoReservation.Dal.Entities;
using AutoReservation.TestEnvironment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using AutoReservation.BusinessLayer.Exceptions;

namespace AutoReservation.BusinessLayer.Testing
{
    [TestClass]
    public class ReservationAvailabilityTest
    {
        private ReservationManager target;
        private ReservationManager Target => target ?? (target = new ReservationManager());


        [TestInitialize]
        public void InitializeTestData()
        {
            TestEnvironmentHelper.InitializeTestData();
        }

        [TestMethod]
        public void AvailabilityCheckCarAvailableTest()
        {
            ReservationManager reservationManager = new ReservationManager();
            Reservation reservation = new Reservation { AutoId = 2, KundeId = 3, Von = new DateTime(2017, 11, 28), Bis = new DateTime(2018, 01, 01) };
            Reservation insertedReservation = reservationManager.Insert(reservation);
            Assert.AreEqual(3, insertedReservation.KundeId);
        }

        [TestMethod]
        public void AvailabilityCheckStartAtEndOfOther()
        {
            ReservationManager reservationManager = new ReservationManager();
            Reservation reservation = new Reservation { AutoId = 1, KundeId = 2, Von = new DateTime(2020, 01, 20), Bis = new DateTime(2025, 08, 31) };
            Reservation insertedReservation = reservationManager.Insert(reservation);
            Assert.AreEqual(2, insertedReservation.KundeId);
        }

        [TestMethod]
        public void AvailabilityCheckEndAtStartOfOther()
        {
            ReservationManager reservationManager = new ReservationManager();
            Reservation reservation = new Reservation { AutoId = 1, KundeId = 2, Von = new DateTime(2017, 11, 28), Bis = new DateTime(2020, 01, 10) };
            Reservation insertedReservation = reservationManager.Insert(reservation);
            Assert.AreEqual(2, insertedReservation.KundeId);
        }
        
        [TestMethod]
        [ExpectedException(typeof(AutoUnavaliableException))]
        public void AvailabilityCheckUpdateCarUnavailable()
        {
            ReservationManager reservationManager = new ReservationManager();
            Reservation reservation = reservationManager.Reservation(2);
            reservation.AutoId = 3;
            reservationManager.Update(reservation);
        }

        [TestMethod]
        [ExpectedException(typeof(AutoUnavaliableException))]
        public void AvailabilityCheckRangeCompletelyWrapped()
        {
            ReservationManager reservationManager = new ReservationManager();
            Reservation reservation = new Reservation { AutoId = 3, KundeId = 1, Von = new DateTime(2020, 01, 13), Bis = new DateTime(2020, 01, 15) };
            reservationManager.Insert(reservation);
        }

        [TestMethod]
        [ExpectedException(typeof(AutoUnavaliableException))]
        public void AvailabilityCheckOverlappingOneSecond()
        {
            ReservationManager reservationManager = new ReservationManager();
            Reservation reservation = reservationManager.Reservation(1);
            reservation.AutoId = 2;
            reservation.Von = reservation.Von.AddDays(10);
            reservation.Von = reservation.Von.AddSeconds(-1);
            reservation.Bis = reservation.Bis.AddDays(10);
            reservationManager.Update(reservation);
        }
    }
}
