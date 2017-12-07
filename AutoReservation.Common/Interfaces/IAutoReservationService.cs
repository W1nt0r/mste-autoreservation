using AutoReservation.Common.DataTransferObjects;
using AutoReservation.Common.DataTransferObjects.Faults;
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
        [FaultContract(typeof(EntityNotFoundFault))]
        void GetAuto(int id);

        [OperationContract(IsOneWay = true)]
        [FaultContract(typeof(DatabaseChangeFault))]
        void AddAuto(AutoDto auto);

        [OperationContract(IsOneWay = true)]
        [FaultContract(typeof(EntityNotFoundFault))]
        [FaultContract(typeof(DatabaseChangeFault))]
        [FaultContract(typeof(OptimisticConcurrencyFault))]
        void UpdateAuto(AutoDto auto);

        [OperationContract(IsOneWay = true)]
        [FaultContract(typeof(EntityNotFoundFault))]
        [FaultContract(typeof(DatabaseChangeFault))]
        [FaultContract(typeof(OptimisticConcurrencyFault))]
        void RemoveAuto(AutoDto auto);

        [OperationContract(IsOneWay = true)]
        void GetAllReservationen();

        [OperationContract(IsOneWay = true)]
        [FaultContract(typeof(EntityNotFoundFault))]
        void GetReservation(int id);

        [OperationContract(IsOneWay = true)]
        [FaultContract(typeof(DatabaseChangeFault))]
        void AddReservation(ReservationDto reservation);

        [OperationContract(IsOneWay = true)]
        [FaultContract(typeof(EntityNotFoundFault))]
        [FaultContract(typeof(DatabaseChangeFault))]
        [FaultContract(typeof(OptimisticConcurrencyFault))]
        [FaultContract(typeof(InvalidDateRangeFault))]
        [FaultContract(typeof(AutoUnavailableFault))]
        void UpdateReservation(ReservationDto reservation);

        [OperationContract(IsOneWay = true)]
        [FaultContract(typeof(EntityNotFoundFault))]
        [FaultContract(typeof(DatabaseChangeFault))]
        [FaultContract(typeof(OptimisticConcurrencyFault))]
        void RemoveReservation(ReservationDto reservation);

        [OperationContract(IsOneWay = true)]
        void GetAllKunden();

        [OperationContract(IsOneWay = true)]
        [FaultContract(typeof(EntityNotFoundFault))]
        void GetKunde(int id);

        [OperationContract(IsOneWay = true)]
        [FaultContract(typeof(DatabaseChangeFault))]
        void AddKunde(KundeDto kunde);

        [OperationContract(IsOneWay = true)]
        [FaultContract(typeof(EntityNotFoundFault))]
        [FaultContract(typeof(DatabaseChangeFault))]
        [FaultContract(typeof(OptimisticConcurrencyFault))]
        void UpdateKunde(KundeDto kunde);

        [OperationContract(IsOneWay = true)]
        [FaultContract(typeof(EntityNotFoundFault))]
        [FaultContract(typeof(DatabaseChangeFault))]
        [FaultContract(typeof(OptimisticConcurrencyFault))]
        void RemoveKunde(KundeDto kunde);

        [OperationContract(IsOneWay = true)]
        void IsAutoAvailable(ReservationDto reservation);
    }
}
