using System;
using System.Runtime.Serialization;
using AutoReservation.Dal.Entities;

namespace AutoReservation.Common.DataTransferObjects
{
    public class ReservationDto
    {
        public int ReservationNr { get; set; }
        DateTime Bis { get; set; }
        DateTime Von { get; set; }
        public byte[] RowVersion { get; set; }

        public Kunde Kunde { get; set; }
        public Auto Auto { get; set; }

        //public override string ToString()
        //    => $"{ReservationsNr}; {Von}; {Bis}; {Auto}; {Kunde}";
    }
}
