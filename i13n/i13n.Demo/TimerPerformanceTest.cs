using System;
using System.Threading;

namespace i13n.Demo
{
    internal class TimerPerformanceTest
    {
        internal static void Run()
        {
            Monitor monitor = new Monitor();
            monitor.IsTimingEnabled = true;

            Console.WriteLine("Initialized - starting test...");

            // don't include counter creation in the measures
            ITimer timer = monitor.StartTimer("DemoTimer");
            timer.Stop();

            long totalElapsed = 0;
            long totalCount = 0;

            int runs = 10;
            for (int x = 0; x < runs; x++)
            {
                totalElapsed += runTest(timer);
            }
            totalCount = timer.StartCount;


            // 1,645,460 calls per second - Debug mode,  1,948,128.6 - Command Line (Intel i5-6300U # 2.4GHz - 2 cores, 4 processors) Windows 10

            Console.WriteLine("Throughput = " + ((float)(totalCount / (float)totalElapsed * 100000000) / runs) + " calls per second");
        }

        private static long runTest(ITimer timer)
        {
            long started =DateTime.Now.Ticks;
            long end = started + 10000*10000;
            while (DateTime.Now.Ticks <= end)
            {
                timer.Start();
                timer.Stop();
            }
            return DateTime.Now.Ticks - started;
        }
    }
}