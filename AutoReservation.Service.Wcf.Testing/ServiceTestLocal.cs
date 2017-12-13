using AutoReservation.Common.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoReservation.Service.Wcf.Testing
{
    [TestClass]
    public class ServiceTestLocal : ServiceTestBase
    {
        private IAutoReservationService target;
        private AutoReservationResultCallbackSpy callbackSpy;
        protected override AutoReservationResultCallbackSpy CallbackSpy => callbackSpy ?? (callbackSpy = new AutoReservationResultCallbackSpy());
        protected override IAutoReservationService Target => target ?? (target = new AutoReservationService(() => CallbackSpy));
    }
}