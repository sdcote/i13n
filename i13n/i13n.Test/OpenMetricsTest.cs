using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Threading.Tasks;
using System;

namespace i13n.Test
{
    [TestClass]
    public class OpenMetricsTest
    {
        [TestMethod]
        public void BasicUseCase()
        {
            // Ensure timing is enabled
            OpenMetrics.IsTimingEnabled = true;

            // Start a timer running and get a reference to it so we can stop it later
            ITimer timer = OpenMetrics.StartTimer("dummy_method");

            // perform your operations
            Task.Delay(500).Wait();

            // Stop this timer instance.
            timer.Stop();

            // Create an OpenMetrics format of the monitor we wrap
            string metrics = OpenMetrics.Serialize();
            Assert.IsNotNull(metrics);

            Assert.IsTrue(metrics.Contains("# TYPE dummy_method_std gauge"));
            Assert.IsTrue(metrics.Contains("dummy_method_hits 1"));
            Debug.WriteLine(metrics);
        }

    }
}
