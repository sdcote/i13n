using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Threading.Tasks;

namespace i13n.Test
{
    [TestClass]
    public class MonitorTest
    {


        [TestMethod]
        public void BasicUseCase()
        {
            // Create a monitor. Normally this would be handled by dependency injection
            Monitor monitor = new Monitor();

            // Timing is disabled by default, enable it
            monitor.IsTimingEnabled = true;

            // All monitor instances are uniquely identified.
            Assert.IsNotNull(monitor.Identifier);

            // Start a timer running and get a reference to it so we can stop it later
            ITimer timer = monitor.StartTimer("Bob");

            // perform your operations
            Task.Delay(500).Wait(); // 5,000,000 ticks

            // Stop this timer instance.
            timer.Stop();
        }


        [TestMethod]
        public void MultipleTimers()
        {
            // Create a monitor. Normally this would be handled by dependency injection
            Monitor monitor = new Monitor();

            // Timing is disabled by default, enable it
            monitor.IsTimingEnabled = true;

            // Call the start timer method on the scorecard to start a timer with a 
            // correlating name
            ITimer t1 = monitor.StartTimer("Demo");

            // Different named timers roll-up statistics separately
            ITimer t2 = monitor.StartTimer("Test");

            // Stopping a timer totals the number of milliseconds between the start and stop calls
            t1.Stop();

            // Timers can be re-started and stopped as necessary to accrue total time this is helpful when trying to measure only 
            //the time spent in methods and not waiting for calls to external systems.
            t1.Start(); // maybe we entered our method
            t1.Stop(); // making a call to an external system not to be included in our time
            Task.Delay(500).Wait(); // pretend it took half a second
            t1.Start(); // start the time back up since we are processing the results
            t1.Stop(); // finally completed our processing and leaving our method.

            // Measure total time spent in this method
            t2.Stop(); // Test timer is stopped only once to give us a total time spent, not just our method processing time

            Debug.WriteLine(t1);
            Debug.WriteLine(t2);

            ITimerMaster master = monitor.RetrieveTimerMaster("Demo");
            Assert.IsNotNull(master);

        }


        [TestMethod]
        public void ActiveTest()
        {
            Monitor monitor = new Monitor();
            monitor.IsTimingEnabled = true;
            ITimer t1 = monitor.StartTimer("Demo");
            t1.Stop();
            TimingMaster master = (TimingMaster)t1.Master;

            t1.Start();
            t1.Stop();
            Task.Delay(500).Wait();
            t1.Start();
            t1.Stop();

            // Average Active should be 1
            Debug.WriteLine("AverageActive: " + master.AvgActive);
        }

    }
}