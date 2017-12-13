using System.Runtime.Serialization;

namespace AutoReservation.Common.DataTransferObjects
{
    [DataContract]
    public enum AutoKlasse
    {
        [EnumMember] Luxusklasse,
        [EnumMember] Mittelklasse,
        [EnumMember] Standard
    }
}
