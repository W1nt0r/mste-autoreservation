using System;
using System.Runtime.Serialization;

namespace AutoReservation.Common.DataTransferObjects
{
    public class KundeDto
    {
        public int Id { get; set; }
        public DateTime Geburtsdatum { get; set; }
        public String Nachname { get; set; }
        public String Vorname { get; set; }
        public byte[] RowVersion { get; set; }

        //public override string ToString()
        //    => $"{Id}; {Nachname}; {Vorname}; {Geburtsdatum}; {RowVersion}";
    }
}
