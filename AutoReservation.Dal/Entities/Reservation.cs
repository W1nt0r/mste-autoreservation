using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoReservation.Dal.Entities
{
    public class Reservation
    {
        [Key]
        public int ReservationsNr { get; set; }
        [Required]
        public DateTime Bis { get; set; }
        [Required]
        public DateTime Von { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }

        public int AutoId { get; set; }
        [ForeignKey("AutoId")]
        public virtual Auto Auto { get; set; }

        public int KundeId { get; set; }
        [ForeignKey("KundeId")]
        public virtual Kunde Kunde { get; set; }

        public void CopyFrom(Reservation reservation)
        {
            Bis = reservation.Bis;
            Von = reservation.Von;
            AutoId = reservation.AutoId;
            KundeId = reservation.KundeId;
        }
    }
}
