using AutoReservation.Common.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ServiceModel;

namespace AutoReservation.Service.Wcf.Testing
{
    [TestClass]
    public class ServiceTestRemote : ServiceTestBase
    {
        private static ServiceHost serviceHost;

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            serviceHost = new ServiceHost(typeof(AutoReservationService));
            serviceHost.Open();
        }

        [ClassCleanup]
        public static void TearDown()
        {
            if (serviceHost.State != CommunicationState.Closed)
            {
                serviceHost.Close();
            }
        }

        private IAutoReservationService target;
        protected override IAutoReservationService Target
        {
            get
            {
                if (target == null)
                {
                    DuplexChannelFactory<IAutoReservationService> channelFactory 
                        = new DuplexChannelFactory<IAutoReservationService>(new InstanceContext(CallbackSpy), "AutoReservationService");
                    target = channelFactory.CreateChannel();
                }
                return target;
            }
        }

        private AutoReservationResultCallbackSpy callbackSpy;
        protected override AutoReservationResultCallbackSpy CallbackSpy => callbackSpy ?? (callbackSpy = new AutoReservationResultCallbackSpy());
    }
}