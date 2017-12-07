using System;
using AutoReservation.Dal;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using AutoReservation.Dal.Entities;
using AutoReservation.BusinessLayer.Exceptions;

namespace AutoReservation.BusinessLayer
{
    public class ReservationManager
        : ManagerBase
    {
        public List<Reservation> List
        {
            get
            {
                using (AutoReservationContext context = new AutoReservationContext())
                {
                    return context.Reservationen.Include(r => r.Kunde).Include(r => r.Auto).ToList();
                }
            }
        }

        public Reservation Reservation(int reservationsNr)
        {
            using (AutoReservationContext context = new AutoReservationContext())
            {
                Reservation reservation = context.Reservationen.Include(r => r.Kunde).Include(r => r.Auto).SingleOrDefault(r => r.ReservationsNr == reservationsNr);
                if (reservation != default(Reservation))
                {
                    return reservation;
                }
                throw new EntityNotFoundException($"Could not find reservation with id {reservationsNr}");
            }
        }

        public Reservation Insert(Reservation reservation)
        {
            using (AutoReservationContext context = new AutoReservationContext())
            {
                CheckDateRangeConstraints(reservation);
                CheckAutoAvailability(context, reservation);
                context.Reservationen.Add(reservation);
                context.SaveChanges();
                if (context.Entry(reservation).State != EntityState.Unchanged)
                {
                    throw new DatabaseChangeException("Could not insert reservation");
                }
                return reservation;
            }
        }

        public Reservation Update(Reservation reservation)
        {
            using (AutoReservationContext context = new AutoReservationContext())
            {
                Reservation reservationFromList = context.Reservationen.SingleOrDefault(r => r.ReservationsNr == reservation.ReservationsNr);
                if (reservationFromList != default(Reservation))
                {
                    CheckDateRangeConstraints(reservation);
                    CheckAutoAvailability(context, reservation);
                    context.Entry(reservationFromList).OriginalValues["RowVersion"] = reservation.RowVersion;
                    reservationFromList.CopyFrom(reservation);
                    try
                    {
                        context.SaveChanges();
                        if (context.Entry(reservationFromList).State == EntityState.Unchanged)
                        {
                            return context.Reservationen.Include(r => r.Kunde).Include(r => r.Auto).SingleOrDefault(r => r.ReservationsNr == reservation.ReservationsNr);
                        }
                        throw new DatabaseChangeException("Could not update reservation");
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        throw CreateOptimisticConcurrencyException(context, reservation);
                    }
                }
                throw new EntityNotFoundException($"Could not find reservation with id {reservation.ReservationsNr}");
            }
        }

        public Reservation Delete(Reservation reservation)
        {
            using (AutoReservationContext context = new AutoReservationContext())
            {
                Reservation reservationFromList = context.Reservationen.Include(r => r.Kunde).Include(r => r.Auto).SingleOrDefault(r => r.ReservationsNr == reservation.ReservationsNr);
                if (reservationFromList != default(Reservation))
                {
                    context.Entry(reservationFromList).OriginalValues["RowVersion"] = reservation.RowVersion;
                    context.Reservationen.Remove(reservationFromList);
                    try
                    {
                        context.SaveChanges();
                        if (context.Entry(reservationFromList).State == EntityState.Detached)
                        {
                            return reservationFromList;
                        }
                        throw new DatabaseChangeException("Could not delete reservation");
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        throw CreateOptimisticConcurrencyException(context, reservation);
                    }
                }
                throw new EntityNotFoundException($"Could not find reservation with id {reservation.ReservationsNr}");
            }
        }

        private bool IsAutoAvailable(Reservation reservation)
        {
            using (AutoReservationContext context = new AutoReservationContext())
            {
                try
                {
                    CheckAutoAvailability(context, reservation);
                }
                catch (AutoUnavaliableException)
                {
                    return false;
                }
                return true;
            }
        }

        private void CheckDateRangeConstraints(Reservation reservation)
        {
            if (reservation.Von > reservation.Bis)
            {
                throw new InvalidDateRangeException("Bis must be after Von");
            }
            DateTime minBis = reservation.Von.AddHours(24);
            if (minBis > reservation.Bis)
            {
                throw new InvalidDateRangeException("Reservation must be at 24 hours long");
            }
        }

        private void CheckAutoAvailability(AutoReservationContext context, Reservation reservation)
        {
            if (context.Reservationen.Any(r => r.ReservationsNr != reservation.ReservationsNr && r.AutoId == reservation.AutoId && r.Von < reservation.Bis && r.Bis > reservation.Von))
            {
                throw new AutoUnavaliableException("auto already reserved in this range");
            }
        }
    }
}