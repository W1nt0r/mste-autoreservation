using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoReservation.Dal.Entities
{
    public class Reservation
    { 
        DateTime Bis { get; set; }
        DateTime Von { get; set; }
        public int ReservationNr { get; set; }
        public byte[] RowVersion { get; set; }

        public int AutoId { get; set; }
        [ForeignKey("AutoId")]
        public Auto Auto { get; set; }

        public int KundeId { get; set; }
        [ForeignKey("KundeId")]
        public Kunde Kunde { get; set; }

    }
}
