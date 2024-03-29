using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace AutoReservation.Dal.Entities
{
    [Table("Auto")]
    public abstract class Auto
    {
        public int Id { get; set; }
        [Required, MaxLength(20)]
        public string Marke { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        [Required]
        public int Tagestarif { get; set; }

        [InverseProperty("Auto")]
        public virtual ICollection<Reservation> Reservationen { get; set; }

        public virtual void CopyFrom(Auto auto)
        {
            Marke = auto.Marke;
            Tagestarif = auto.Tagestarif;
        }
    }

    public class StandardAuto : Auto { }

    public class LuxusklasseAuto : Auto
    {
        public int Basistarif { get; set; }

        public override void CopyFrom(Auto auto)
        {
            base.CopyFrom(auto);
            Basistarif = ((LuxusklasseAuto)auto).Basistarif;
        }
    }

    public class MittelklasseAuto : Auto { }
}
