using AutoReservation.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoReservation.Common.DataTransferObjects;
using AutoReservation.Common.DataTransferObjects.Faults;
using System.Diagnostics;
using System.Threading;

namespace AutoReservation.Service.Wcf.Testing
{
    public class AutoReservationResultCallbackSpy : IAutoReservationResultCallback
    {
        public List<AutoDto> AutoSpy { get; set; }
        public List<KundeDto> KundeSpy { get; set; }
        public List<ReservationDto> ReservationSpy { get; set; }
        public bool? IsAvailable { get; set; }
        public string ExceptionSpy { get; set; }

        private bool _answered;

        private static void WriteActualMethod()
            => Console.WriteLine($"Calling: {new StackTrace().GetFrame(1).GetMethod().Name}");

        public AutoReservationResultCallbackSpy()
        {
            WriteActualMethod();
            AutoSpy = new List<AutoDto>();
            KundeSpy = new List<KundeDto>();
            ReservationSpy = new List<ReservationDto>();
            IsAvailable = null;
        }

        public void SendAllAutos(List<AutoDto> autos)
        {
            WriteActualMethod();
            AutoSpy = autos;
            _answered = true;
        }

        public void SendAllKunden(List<KundeDto> kunden)
        {
            WriteActualMethod();
            KundeSpy = kunden;
            _answered = true;
        }

        public void SendAllReservationen(List<ReservationDto> reservationen)
        {
            WriteActualMethod();
            ReservationSpy = reservationen;
            _answered = true;
        }

        public void SendAuto(AutoDto auto)
        {
            WriteActualMethod();
            Console.WriteLine(auto);
            Console.WriteLine(auto.Marke);
            Console.WriteLine(auto.Id);
            AutoSpy.Add(auto);
            _answered = true;
        }

        public void SendAutoAvailability(bool available)
        {
            WriteActualMethod();
            IsAvailable = available;
            _answered = true;
        }

        public void SendKunde(KundeDto kunde)
        {
            WriteActualMethod();
            KundeSpy.Add(kunde);
            _answered = true;
        }

        public void SendReservation(ReservationDto reservation)
        {
            WriteActualMethod();
            ReservationSpy.Add(reservation);
            _answered = true;
        }

        public void SendFault(CommunicationFault fault)
        {
            WriteActualMethod();
            _answered = true;
            ExceptionSpy = fault.Exception;
        }

        public void WaitForAnswer()
        {
            while (!_answered)
            {
                Thread.Sleep(100);
            }
            _answered = false;
        }
    }
}
