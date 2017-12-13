using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Collections.ObjectModel;
using AutoReservation.Common.DataTransferObjects;
using AutoReservation.Common.DataTransferObjects.Faults;

namespace AutoReservation.Common.Interfaces
{
    public interface IAutoReservationResultCallback
    {
        [OperationContract(IsOneWay = true)]
        void SendAuto(AutoDto auto);

        [OperationContract(IsOneWay = true)]
        void SendAllAutos(List<AutoDto> autos);

        [OperationContract(IsOneWay = true)]
        void SendKunde(KundeDto kunde);

        [OperationContract(IsOneWay = true)]
        void SendAllKunden(List<KundeDto> kunden);

        [OperationContract(IsOneWay = true)]
        void SendReservation(ReservationDto reservation);

        [OperationContract(IsOneWay = true)]
        void SendAllReservationen(List<ReservationDto> reservationen);

        [OperationContract(IsOneWay = true)]
        void SendAutoAvailability(bool available);

        [OperationContract(IsOneWay = true)]
        void SendFault(CommunicationFault fault);
    }
}
