using System;
using System.Diagnostics;
using AutoReservation.Common.DataTransferObjects;
using AutoReservation.Common.Interfaces;
using AutoReservation.BusinessLayer;
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void GetAllAutos()
        {
            WriteActualMethod();
            try
            {
                IAutoReservationResultCallback cb = OperationContext.Current.GetCallbackChannel<IAutoReservationResultCallback>();
                cb.SendAllAutos(autoManager.List.ConvertToDtos());
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void GetAllKunden()
        {
            WriteActualMethod();
            try
            {
                IAutoReservationResultCallback cb = OperationContext.Current.GetCallbackChannel<IAutoReservationResultCallback>();
                cb.SendAllKunden(kundeManager.List.ConvertToDtos());
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void GetAllReservationen()
        {
            WriteActualMethod();
            try
            {
                IAutoReservationResultCallback cb = OperationContext.Current.GetCallbackChannel<IAutoReservationResultCallback>();
                cb.SendAllReservationen(reservationManager.List.ConvertToDtos());
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void GetAuto(int id)
        {
            WriteActualMethod();
            try
            {
                IAutoReservationResultCallback cb = OperationContext.Current.GetCallbackChannel<IAutoReservationResultCallback>();
                cb.SendAuto(autoManager.Auto(id).ConvertToDto());
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void IsAutoAvailable(ReservationDto reservation)
        {
            WriteActualMethod();
            try
            {
                IAutoReservationResultCallback cb = OperationContext.Current.GetCallbackChannel<IAutoReservationResultCallback>();
                cb.SendAutoAvailability(reservationManager.IsAutoAvailable(reservation.ConvertToEntity()));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        public void RemoveAuto(AutoDto auto)
        {
            WriteActualMethod();
            try
            {
                IAutoReservationResultCallback cb = OperationContext.Current.GetCallbackChannel<IAutoReservationResultCallback>();
                cb.SendAuto(autoManager.Delete(auto.ConvertToEntity()).ConvertToDto());
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}