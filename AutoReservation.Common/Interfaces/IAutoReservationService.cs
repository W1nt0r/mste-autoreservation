using AutoReservation.Common.DataTransferObjects;
using System.ServiceModel;

namespace AutoReservation.Common.Interfaces
{
    [ServiceContract(
        SessionMode = SessionMode.Required,
        CallbackContract = typeof(IAutoReservationService))]
    public interface IAutoReservationService
    {
        [OperationContract(IsOneWay = true)]
        void GetAllAutos();

        [OperationContract(IsOneWay = true)]
        void GetAuto(int id);

        [OperationContract(IsOneWay = true)]
        void AddAuto(AutoDto auto);

        [OperationContract(IsOneWay = true)]
        void UpdateAuto(AutoDto auto);

        [OperationContract(IsOneWay = true)]
        void RemoveAuto(AutoDto auto);

        [OperationContract(IsOneWay = true)]
        void GetAllReservationen();

        [OperationContract(IsOneWay = true)]
        void GetReservation(int id);

        [OperationContract(IsOneWay = true)]
        void AddReservation(ReservationDto reservation);

        [OperationContract(IsOneWay = true)]
        void UpdateReservation(ReservationDto reservation);

        [OperationContract(IsOneWay = true)]
        void RemoveReservation(ReservationDto reservation);

        [OperationContract(IsOneWay = true)]
        void GetAllKunden();

        [OperationContract(IsOneWay = true)]
        void GetKunde(int id);

        [OperationContract(IsOneWay = true)]
        void AddKunde(KundeDto kunde);

        [OperationContract(IsOneWay = true)]
        void UpdateKunde(KundeDto kunde);

        [OperationContract(IsOneWay = true)]
        void RemoveKunde(KundeDto kunde);

        [OperationContract(IsOneWay = true)]
        void IsAutoAvailable(ReservationDto kunde);
    }
}
