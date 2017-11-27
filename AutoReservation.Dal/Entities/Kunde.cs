using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace AutoReservation.Dal.Entities
{
    public class Kunde
    {
        public int Id { get; set; }
        [Required]
        public DateTime Geburtsdatum { get; set; }
        [Required, MaxLength(20)]
        public String Nachname { get; set; }
        [Required, MaxLength(20)]
        public String Vorname { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }

        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}
