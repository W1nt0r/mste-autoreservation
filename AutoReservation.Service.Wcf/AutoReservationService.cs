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
        private readonly Func<IAutoReservationResultCallback> _createCallbackChannel;

        private static void WriteActualMethod()
            => Console.WriteLine($"Calling: {new StackTrace().GetFrame(1).GetMethod().Name}");

        public AutoReservationService() : this(() => OperationContext.Current.GetCallbackChannel<IAutoReservationResultCallback>())
        {
            
        }

        public AutoReservationService(Func<IAutoReservationResultCallback> callbackCreator)
        {
            autoManager = new AutoManager();
            kundeManager = new KundeManager();
            reservationManager = new ReservationManager();

            _createCallbackChannel = callbackCreator;
        }

        public void AddAuto(AutoDto auto)
        {
            WriteActualMethod();
            IAutoReservationResultCallback cb = _createCallbackChannel();
            try
            {
                Auto insertedAuto = autoManager.Insert(auto.ConvertToEntity());
                cb.SendAuto(insertedAuto.ConvertToDto());
            }
            catch (DatabaseChangeException ex)
            {
                cb.SendFault(new CommunicationFault { Exception = ex });
            }
        }

        public void AddKunde(KundeDto kunde)
        {
            WriteActualMethod();
            IAutoReservationResultCallback cb = _createCallbackChannel();
            try
            {
                Kunde insertedKunde = kundeManager.Insert(kunde.ConvertToEntity());
                cb.SendKunde(insertedKunde.ConvertToDto());
            }
            catch (DatabaseChangeException ex)
            {
                cb.SendFault(new CommunicationFault { Exception = ex });
            }
        }

        public void AddReservation(ReservationDto reservation)
        {
            WriteActualMethod();
            IAutoReservationResultCallback cb = _createCallbackChannel();
            try
            {
                Reservation insertedReservation = reservationManager.Insert(reservation.ConvertToEntity());
                cb.SendReservation(insertedReservation.ConvertToDto());
            }
            catch (Exception ex)
            {
                cb.SendFault(new CommunicationFault { Exception = ex });
            }
        }

        public void GetAllAutos()
        {
            WriteActualMethod();
            IAutoReservationResultCallback cb = _createCallbackChannel();
            cb.SendAllAutos(autoManager.List.ConvertToDtos());
        }

        public void GetAllKunden()
        {
            WriteActualMethod();
            IAutoReservationResultCallback cb = _createCallbackChannel();
            cb.SendAllKunden(kundeManager.List.ConvertToDtos());
        }

        public void GetAllReservationen()
        {
            WriteActualMethod();
            IAutoReservationResultCallback cb = _createCallbackChannel();
            cb.SendAllReservationen(reservationManager.List.ConvertToDtos());
        }

        public void GetAuto(int id)
        {
            WriteActualMethod();
            IAutoReservationResultCallback cb = _createCallbackChannel();
            try
            {
                cb.SendAuto(autoManager.Auto(id).ConvertToDto());
            }
            catch (EntityNotFoundException ex)
            {
                cb.SendFault(new CommunicationFault { Exception = ex });
            }
        }

        public void GetKunde(int id)
        {
            WriteActualMethod();
            IAutoReservationResultCallback cb = _createCallbackChannel();
            try
            {
                cb.SendKunde(kundeManager.Kunde(id).ConvertToDto());
            }
            catch (EntityNotFoundException ex)
            {
                cb.SendFault(new CommunicationFault { Exception = ex });
            }
        }

        public void GetReservation(int id)
        {
            WriteActualMethod();
            IAutoReservationResultCallback cb = _createCallbackChannel();
            try
            {
                cb.SendReservation(reservationManager.Reservation(id).ConvertToDto());
            }
            catch (EntityNotFoundException ex)
            {
                cb.SendFault(new CommunicationFault { Exception = ex });
            }
        }

        public void IsAutoAvailable(ReservationDto reservation)
        {
            WriteActualMethod();
            IAutoReservationResultCallback cb = _createCallbackChannel();
            cb.SendAutoAvailability(reservationManager.IsAutoAvailable(reservation.ConvertToEntity()));
        }


        public void RemoveAuto(AutoDto auto)
        {
            WriteActualMethod();
            IAutoReservationResultCallback cb = _createCallbackChannel();
            try
            {
                cb.SendAuto(autoManager.Delete(auto.ConvertToEntity()).ConvertToDto());
            }
            catch(Exception ex)
            {
                cb.SendFault(new CommunicationFault { Exception = ex });
            }
        }

        public void RemoveKunde(KundeDto kunde)
        {
            WriteActualMethod();
            IAutoReservationResultCallback cb = _createCallbackChannel();
            try
            {
                cb.SendKunde(kundeManager.Delete(kunde.ConvertToEntity()).ConvertToDto());
            }
            catch (Exception ex)
            {
                cb.SendFault(new CommunicationFault { Exception = ex });
            }
        }

        public void RemoveReservation(ReservationDto reservation)
        {
            WriteActualMethod();
            IAutoReservationResultCallback cb = _createCallbackChannel();
            try
            {
                cb.SendReservation(reservationManager.Delete(reservation.ConvertToEntity()).ConvertToDto());
            }
            catch (Exception ex)
            {
                cb.SendFault(new CommunicationFault { Exception = ex });
            }
        }

        public void UpdateAuto(AutoDto auto)
        {
            WriteActualMethod();
            IAutoReservationResultCallback cb = _createCallbackChannel();
            try
            {
                cb.SendAuto(autoManager.Update(auto.ConvertToEntity()).ConvertToDto());
            }
            catch (Exception ex)
            {
                cb.SendFault(new CommunicationFault { Exception = ex });
            }
        }

        public void UpdateKunde(KundeDto kunde)
        {
            WriteActualMethod();
            IAutoReservationResultCallback cb = _createCallbackChannel();
            try
            {
                cb.SendKunde(kundeManager.Update(kunde.ConvertToEntity()).ConvertToDto());
            }
            catch (Exception ex)
            {
                cb.SendFault(new CommunicationFault { Exception = ex });
            }
        }

        public void UpdateReservation(ReservationDto reservation)
        {
            WriteActualMethod();
            IAutoReservationResultCallback cb = _createCallbackChannel();
            try
            {
                cb.SendReservation(reservationManager.Update(reservation.ConvertToEntity()).ConvertToDto());
            }
            catch (Exception ex)
            {
                cb.SendFault(new CommunicationFault { Exception = ex });
            }
        }
    }
}