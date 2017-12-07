using AutoReservation.Common.DataTransferObjects;
using System.Collections.ObjectModel;
using System.ServiceModel;

namespace AutoReservation.Common.Interfaces
{
    public interface IAutoReservationService
    {
        [OperationContract(IsOneWay = true)]
        void SendAuto(AutoDto auto);

        [OperationContract(IsOneWay = true)]
        void SendAllAutos(Collection<AutoDto> autos);

        [OperationContract(IsOneWay = true)]
        void SendKunde(KundeDto kunde);

        [OperationContract(IsOneWay = true)]
        void SendAllKunden(Collection<KundeDto> kunden);

        [OperationContract(IsOneWay = true)]
        void SendReservation(ReservationDto reservation);

        [OperationContract(IsOneWay = true)]
        void SendAllReservationen(Collection<ReservationDto> reservationen);
    }
}
