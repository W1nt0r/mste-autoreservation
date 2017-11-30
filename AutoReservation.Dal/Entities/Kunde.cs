using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace AutoReservation.Dal.Entities
{
    [Table("Kunde")]
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

        [InverseProperty("Kunde")]
        public virtual ICollection<Reservation> Reservationen { get; set; }

        public void CopyFrom(Kunde kunde)
        {
            Geburtsdatum = kunde.Geburtsdatum;
            Nachname = kunde.Nachname;
            Vorname = kunde.Vorname;
        }
    }
}
