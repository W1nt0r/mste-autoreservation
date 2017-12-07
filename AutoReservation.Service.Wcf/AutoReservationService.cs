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
                autoManager.Insert(auto.ConvertToEntity());

                //IAutoReservationResultCallback cb = OperationContext.Current.GetCallbackChannel<IAutoReservationResultCallback>();

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
                kundeManager.Insert(kunde.ConvertToEntity());
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
                reservationManager.Insert(reservation.ConvertToEntity());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void GetAllAutos()
        {
            WriteActualMethod();
            autoManager.List.ConvertToDtos();
        }

        public void GetAllKunden()
        {
            WriteActualMethod();
        }

        public void GetAllReservationen()
        {
            WriteActualMethod();
        }

        public void GetAuto(int id)
        {
            WriteActualMethod();

        }

        public void GetKunde(int id)
        {
            WriteActualMethod();

        }

        public void GetReservation(int id)
        {
            WriteActualMethod();

        }

        public void IsAutoAvailable(ReservationDto kunde)
        {
            WriteActualMethod();
        }


        public void RemoveAuto(AutoDto auto)
        {
            WriteActualMethod();
            throw new NotImplementedException();
        }

        public void RemoveKunde(KundeDto kunde)
        {
            WriteActualMethod();
            throw new NotImplementedException();
        }

        public void RemoveReservation(ReservationDto reservation)
        {
            WriteActualMethod();
            throw new NotImplementedException();
        }

        public void UpdateAuto(AutoDto auto)
        {
            WriteActualMethod();
            throw new NotImplementedException();
        }

        public void UpdateKunde(KundeDto kunde)
        {
            WriteActualMethod();
            throw new NotImplementedException();
        }

        public void UpdateReservation(ReservationDto reservation)
        {
            WriteActualMethod();
            throw new NotImplementedException();
        }
    }
}