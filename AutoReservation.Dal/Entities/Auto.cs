using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace AutoReservation.Dal.Entities
{
    public abstract class Auto
    {
        public int Id { get; set; }
        [Required, MaxLength(20)]
        public string Marke { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        [Required]
        public int Tagestarif { get; set; }

        public ICollection<Reservation> Reservationen { get; set; }
    }

    public class StandardAuto : Auto { }

    public class LuxusklasseAuto : Auto
    {
        public int Basistarif { get; set; }
    }

    public class MittelklasseAuto : Auto { }
}
