using System.Runtime.Serialization;

namespace AutoReservation.Common.DataTransferObjects
{
    [DataContract]
    public enum AutoKlasse
    {
        [DataMember] Luxusklasse,
        [DataMember] Mittelklasse,
        [DataMember] Standard
    }
}
