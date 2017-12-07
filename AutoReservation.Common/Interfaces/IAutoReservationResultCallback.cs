using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Collections.ObjectModel;
using AutoReservation.Common.DataTransferObjects;

namespace AutoReservation.Common.Interfaces
{
    public interface IAutoReservationResultCallback
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
