using AutoReservation.BusinessLayer.Exceptions;
using AutoReservation.Dal.Entities;
using AutoReservation.TestEnvironment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AutoReservation.BusinessLayer.Testing
{
    [TestClass]
    public class ReservationDateRangeTest
    {
        private ReservationManager target;
        private ReservationManager Target => target ?? (target = new ReservationManager());


        [TestInitialize]
        public void InitializeTestData()
        {
            TestEnvironmentHelper.InitializeTestData();
        }

        [TestMethod]
        public void ScenarioExactly24HoursTest()
        {
            ReservationManager reservationManager = new ReservationManager();
            Reservation reservation = reservationManager.Reservation(1);
            DateTime newDateTime = reservation.Bis.AddDays(-1);
            reservation.Von = newDateTime;
            reservationManager.Update(reservation);
            Assert.AreEqual(newDateTime, reservationManager.Reservation(1).Von);
        }

        [TestMethod]
        public void Scenario24HoursAnd1SecondTest()
        {
            ReservationManager reservationManager = new ReservationManager();
            Reservation reservation = reservationManager.Reservation(1);
            DateTime newDateTime = reservation.Bis.AddDays(-1).AddSeconds(-1);
            reservation.Von = newDateTime;
            reservationManager.Update(reservation);
            Assert.AreEqual(newDateTime, reservationManager.Reservation(1).Von);
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidDateRangeException))]
        public void ScenarioVonEqualsBisTest()
        {
            ReservationManager reservationManager = new ReservationManager();
            Reservation reservation = reservationManager.Reservation(1);
            reservation.Von = reservation.Bis;
            reservationManager.Update(reservation);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDateRangeException))]
        public void ScenarioVonIsAfterBisTest()
        {
            ReservationManager reservationManager = new ReservationManager();
            Reservation reservation = reservationManager.Reservation(1);
            reservation.Von = reservation.Bis.AddSeconds(1);
            reservationManager.Update(reservation);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDateRangeException))]
        public void ScenarioLessThan24HoursTest()
        {
            ReservationManager reservationManager = new ReservationManager();
            Reservation reservation = reservationManager.Reservation(1);
            reservation.Von = reservation.Bis.AddHours(-24).AddSeconds(1);
            reservationManager.Update(reservation);
        }
    }
}
