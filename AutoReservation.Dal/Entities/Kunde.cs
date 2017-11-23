using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace AutoReservation.Dal.Entities
{
    public class Kunde
    {
        public DateTime Geburtsdatum { get; set; }
        public int Id { get; set; }
        public String Nachname { get; set; }
        public byte[] RowVersion { get; set; }
        public String Vorname { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}
