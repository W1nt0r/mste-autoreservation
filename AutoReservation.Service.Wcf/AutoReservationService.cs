using System;
using System.Diagnostics;
using AutoReservation.Common.DataTransferObjects.Faults;
using AutoReservation.Common.DataTransferObjects;
using AutoReservation.Common.Interfaces;
using AutoReservation.BusinessLayer;
using AutoReservation.BusinessLayer.Exceptions;
using AutoReservation.Dal.Entities;
using System.ServiceModel;

namespace AutoReservation.Service.Wcf
{
    public class AutoReservationService : IAutoReservationService 
    {
        private AutoManager autoManager;
        private KundeManager kundeManager;
        private ReservationManager reservationManager;

        private static void WriteActualMethod()
            => Console.WriteLine($"Calling: {new StackTrace().GetFrame(1).GetMethod().Name}");

        public AutoReservationService()
        {
            autoManager = new AutoManager();
            kundeManager = new KundeManager();
            reservationManager = new ReservationManager();
        }

        public void AddAuto(AutoDto auto)
        {
            WriteActualMethod();
            try
            {
                Auto insertedAuto = autoManager.Insert(auto.ConvertToEntity());
                IAutoReservationResultCallback cb = OperationContext.Current.GetCallbackChannel<IAutoReservationResultCallback>();
                cb.SendAuto(insertedAuto.ConvertToDto());
            }
            catch (DatabaseChangeException ex)
            {
                throw new FaultException<DatabaseChangeFault>(new DatabaseChangeFault
                {
                    Message = ex.Message
                });
            }
        }

        public void AddKunde(KundeDto kunde)
        {
            WriteActualMethod();
            try
            {
                Kunde insertedKunde = kundeManager.Insert(kunde.ConvertToEntity());
                IAutoReservationResultCallback cb = OperationContext.Current.GetCallbackChannel<IAutoReservationResultCallback>();
                cb.SendKunde(insertedKunde.ConvertToDto());
            }
            catch (DatabaseChangeException ex)
            {
                throw new FaultException<DatabaseChangeFault>(new DatabaseChangeFault
                {
                    Message = ex.Message
                });
            }
        }

        public void AddReservation(ReservationDto reservation)
        {
            WriteActualMethod();
            try
            {
                Reservation insertedReservation = reservationManager.Insert(reservation.ConvertToEntity());
                IAutoReservationResultCallback cb = OperationContext.Current.GetCallbackChannel<IAutoReservationResultCallback>();
                cb.SendReservation(insertedReservation.ConvertToDto());
            }
            catch (DatabaseChangeException ex)
            {
                throw new FaultException<DatabaseChangeFault>(new DatabaseChangeFault
                {
                    Message = ex.Message
                });
            }
            catch (AutoUnavaliableException ex)
            {
                throw new FaultException<AutoUnavailableFault>(new AutoUnavailableFault
                {
                    Message = ex.Message
                });
            }
            catch (InvalidDateRangeException ex)
            {
                throw new FaultException<InvalidDateRangeFault>(new InvalidDateRangeFault
                {
                    Message = ex.Message
                });
            }
        }

        public void GetAllAutos()
        {
            WriteActualMethod();
            IAutoReservationResultCallback cb = OperationContext.Current.GetCallbackChannel<IAutoReservationResultCallback>();
            cb.SendAllAutos(autoManager.List.ConvertToDtos());
        }

        public void GetAllKunden()
        {
            WriteActualMethod();
            IAutoReservationResultCallback cb = OperationContext.Current.GetCallbackChannel<IAutoReservationResultCallback>();
            cb.SendAllKunden(kundeManager.List.ConvertToDtos());
        }

        public void GetAllReservationen()
        {
            WriteActualMethod();
            IAutoReservationResultCallback cb = OperationContext.Current.GetCallbackChannel<IAutoReservationResultCallback>();
            cb.SendAllReservationen(reservationManager.List.ConvertToDtos());
        }

        public void GetAuto(int id)
        {
            WriteActualMethod();
            try
            {
                IAutoReservationResultCallback cb = OperationContext.Current.GetCallbackChannel<IAutoReservationResultCallback>();
                cb.SendAuto(autoManager.Auto(id).ConvertToDto());
            }
            catch (EntityNotFoundException ex)
            {
                throw new FaultException<EntityNotFoundFault>(new EntityNotFoundFault
                {
                    Message = ex.Message
                });
            }
        }

        public void GetKunde(int id)
        {
            WriteActualMethod();
            try
            {
                IAutoReservationResultCallback cb = OperationContext.Current.GetCallbackChannel<IAutoReservationResultCallback>();
                cb.SendKunde(kundeManager.Kunde(id).ConvertToDto());
            }
            catch (EntityNotFoundException ex)
            {
                throw new FaultException<EntityNotFoundFault>(new EntityNotFoundFault
                {
                    Message = ex.Message
                });
            }
        }

        public void GetReservation(int id)
        {
            WriteActualMethod();
            try
            {
                IAutoReservationResultCallback cb = OperationContext.Current.GetCallbackChannel<IAutoReservationResultCallback>();
                cb.SendReservation(reservationManager.Reservation(id).ConvertToDto());
            }
            catch (EntityNotFoundException ex)
            {
                throw new FaultException<EntityNotFoundFault>(new EntityNotFoundFault
                {
                    Message = ex.Message
                });
            }
        }

        public void IsAutoAvailable(ReservationDto reservation)
        {
            WriteActualMethod();
            IAutoReservationResultCallback cb = OperationContext.Current.GetCallbackChannel<IAutoReservationResultCallback>();
            cb.SendAutoAvailability(reservationManager.IsAutoAvailable(reservation.ConvertToEntity()));
        }


        public void RemoveAuto(AutoDto auto)
        {
            WriteActualMethod();
            try
            {
                IAutoReservationResultCallback cb = OperationContext.Current.GetCallbackChannel<IAutoReservationResultCallback>();
                cb.SendAuto(autoManager.Delete(auto.ConvertToEntity()).ConvertToDto());
            }
            catch(OptimisticConcurrencyException<Auto> ex)
            {
                throw new FaultException<OptimisticConcurrencyFault>(new OptimisticConcurrencyFault
                {
                    Message = ex.Message
                });
            }
            catch (DatabaseChangeException ex)
            {
                throw new FaultException<DatabaseChangeFault>(new DatabaseChangeFault
                {
                    Message = ex.Message
                });
            }
            catch (EntityNotFoundException ex)
            {
                throw new FaultException<EntityNotFoundFault>(new EntityNotFoundFault
                {
                    Message = ex.Message
                });
            }
        }

        public void RemoveKunde(KundeDto kunde)
        {
            WriteActualMethod();
            try
            {
                IAutoReservationResultCallback cb = OperationContext.Current.GetCallbackChannel<IAutoReservationResultCallback>();
                cb.SendKunde(kundeManager.Delete(kunde.ConvertToEntity()).ConvertToDto());
            }
            catch (OptimisticConcurrencyException<Auto> ex)
            {
                throw new FaultException<OptimisticConcurrencyFault>(new OptimisticConcurrencyFault
                {
                    Message = ex.Message
                });
            }
            catch (DatabaseChangeException ex)
            {
                throw new FaultException<DatabaseChangeFault>(new DatabaseChangeFault
                {
                    Message = ex.Message
                });
            }
            catch (EntityNotFoundException ex)
            {
                throw new FaultException<EntityNotFoundFault>(new EntityNotFoundFault
                {
                    Message = ex.Message
                });
            }
        }

        public void RemoveReservation(ReservationDto reservation)
        {
            WriteActualMethod();
            try
            {
                IAutoReservationResultCallback cb = OperationContext.Current.GetCallbackChannel<IAutoReservationResultCallback>();
                cb.SendReservation(reservationManager.Delete(reservation.ConvertToEntity()).ConvertToDto());
            }
            catch (OptimisticConcurrencyException<Auto> ex)
            {
                throw new FaultException<OptimisticConcurrencyFault>(new OptimisticConcurrencyFault
                {
                    Message = ex.Message
                });
            }
            catch (DatabaseChangeException ex)
            {
                throw new FaultException<DatabaseChangeFault>(new DatabaseChangeFault
                {
                    Message = ex.Message
                });
            }
            catch (EntityNotFoundException ex)
            {
                throw new FaultException<EntityNotFoundFault>(new EntityNotFoundFault
                {
                    Message = ex.Message
                });
            }
        }

        public void UpdateAuto(AutoDto auto)
        {
            WriteActualMethod();
            try
            {
                IAutoReservationResultCallback cb = OperationContext.Current.GetCallbackChannel<IAutoReservationResultCallback>();
                cb.SendAuto(autoManager.Update(auto.ConvertToEntity()).ConvertToDto());
            }
            catch (OptimisticConcurrencyException<Auto> ex)
            {
                throw new FaultException<OptimisticConcurrencyFault>(new OptimisticConcurrencyFault
                {
                    Message = ex.Message
                });
            }
            catch (DatabaseChangeException ex)
            {
                throw new FaultException<DatabaseChangeFault>(new DatabaseChangeFault
                {
                    Message = ex.Message
                });
            }
            catch (EntityNotFoundException ex)
            {
                throw new FaultException<EntityNotFoundFault>(new EntityNotFoundFault
                {
                    Message = ex.Message
                });
            }
        }

        public void UpdateKunde(KundeDto kunde)
        {
            WriteActualMethod();
            WriteActualMethod();
            try
            {
                IAutoReservationResultCallback cb = OperationContext.Current.GetCallbackChannel<IAutoReservationResultCallback>();
                cb.SendKunde(kundeManager.Update(kunde.ConvertToEntity()).ConvertToDto());
            }
            catch (OptimisticConcurrencyException<Auto> ex)
            {
                throw new FaultException<OptimisticConcurrencyFault>(new OptimisticConcurrencyFault
                {
                    Message = ex.Message
                });
            }
            catch (DatabaseChangeException ex)
            {
                throw new FaultException<DatabaseChangeFault>(new DatabaseChangeFault
                {
                    Message = ex.Message
                });
            }
            catch (EntityNotFoundException ex)
            {
                throw new FaultException<EntityNotFoundFault>(new EntityNotFoundFault
                {
                    Message = ex.Message
                });
            }
        }

        public void UpdateReservation(ReservationDto reservation)
        {
            WriteActualMethod();
            WriteActualMethod();
            try
            {
                IAutoReservationResultCallback cb = OperationContext.Current.GetCallbackChannel<IAutoReservationResultCallback>();
                cb.SendReservation(reservationManager.Update(reservation.ConvertToEntity()).ConvertToDto());
            }
            catch (OptimisticConcurrencyException<Auto> ex)
            {
                throw new FaultException<OptimisticConcurrencyFault>(new OptimisticConcurrencyFault
                {
                    Message = ex.Message
                });
            }
            catch (DatabaseChangeException ex)
            {
                throw new FaultException<DatabaseChangeFault>(new DatabaseChangeFault
                {
                    Message = ex.Message
                });
            }
            catch (EntityNotFoundException ex)
            {
                throw new FaultException<EntityNotFoundFault>(new EntityNotFoundFault
                {
                    Message = ex.Message
                });
            }
        }
    }
}