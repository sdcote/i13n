using System;
using System.Collections.Generic;
using System.Text;

namespace i13n
{
    public static class OpenMetrics
    {
        private static Monitor monitor = new Monitor();

        /// <summary>
        /// Return the Monitor instance for this fixture.
        /// </summary>
        public static Monitor Monitor { get { return monitor; } }


        /// <summary>
        /// The unique identifier of this monitor.
        /// </summary>
        public static string Identifier { get { return monitor.Identifier; } }


        /// <summary>
        /// True means returned timers will track elapsed time, false means null (no-op) timers will be returned resulting in no time tracking.
        /// </summary>
        public static bool IsTimingEnabled { get { return monitor.IsTimingEnabled; } set { monitor.IsTimingEnabled = value; } }


        /// <summary>
        /// The number of counters in the statboard at the present time.
        /// </summary>
        public static int CounterCount { get { return monitor.CounterCount; } }


        /// <summary>
        /// Retrieve a list of counters in this monitor
        /// </summary>
        /// <remarks>
        /// <para>This list is detached from the backing store such that modifying the list will have no effect on the monitors collection of counters. Changing the counter instance will affect the counter stored in the monitor.</para>
        /// <para>If you wish to remove one of the counters, you must do so by its name and the RemoveCounter(string) method.</para>
        /// </remarks>
        public static List<Counter> Counters { get { return monitor.Counters; } }


        /// <summary>
        /// Retrieve a list of timer (masters) in this monitor
        /// </summary>
        /// <remarks>
        /// <para>This list is detached from the backing store such that modifying the list will have no effect on the monitors collection of timer masters. Changing the timer master instance will affect the timers stored in the monitor.</para>
        /// <para>If you wish to remove one of the masters, you must do so by its name and the RemoveTimer(string) method.</para>
        /// </remarks>
        public static List<ITimerMaster> Timers { get { return monitor.Timers; } }


        /// <summary>
        /// Decrease the value with the given name by the given amount.
        /// </summary>
        /// <remarks>
        /// <para>This method retrieves the counter with the given name or creates one by that name if it does not yet exist. The retrieved counter is then decreased by the given amount.</para>
        /// </remarks>
        /// <param name="name">The name of the counter to decrease.</param>
        /// <returns>The final value of the counter after the operation.</returns>
        public static long Decrease(string name, long value) { return monitor.Decrease(name, value); }


        /// <summary>
        /// Decrement the value with the given name.
        /// </summary>
        /// <remarks>
        /// <para>This method retrieves the counter with the given name or creates one by that name if it does not yet exist. The retrieved counter is then decreased by one (1).</para>
        /// </remarks>
        /// <param name="name">The name of the counter to decrement.</param>
        /// <returns>The final value of the counter after the operation.</returns>
        public static long Decrement(string name) { return monitor.Decrement(name); }


        /// <summary>
        /// Return the counter with the given name.
        /// </summary>
        /// <remarks>
        /// <para>If the counter does not exist, one will be created and added to the list of counters for later retrieval.</para>
        /// </remarks>
        /// <param name="name">The name of the counter to return.</param>
        /// <returns>The counter with the given name.</returns>
        public static Counter RetrieveCounter(string name) { return monitor.RetrieveCounter(name); }


        /// <summary>
        /// Increase the value with the given name by the given amount.
        /// </summary>
        /// <remarks>
        /// <para>This method retrieves the counter with the given name or creates one by that name if it does not yet exist. The retrieved counter is then increased by the given amount.</para>
        /// </remarks>
        /// <param name="name">The name of the counter to increase.</param>
        /// <returns>The final value of the counter after the operation.</returns>
        public static long Increase(string name, long value) { return monitor.Increase(name, value); }


        /// <summary>
        /// Increment the value with the given name.
        /// </summary>
        /// <remarks>
        /// <para>This method retrieves the counter with the given name or creates one by that name if it does not yet exist. The retrieved counter is then increased by one (1).</para>
        /// </remarks>
        /// <param name="name">The name of the counter to increment.</param>
        /// <returns>The final value of the counter after the operation.</returns>
        public static long Increment(string name) { return monitor.Increment(name); }


        /// <summary>
        /// Remove the counter with the given name.
        /// </summary>
        /// <param name="name">Name of the counter to remove.</param>
        /// <returns>The removed counter or null if the counter was not found.</returns>
        public static Counter RemoveCounter(string name) { return monitor.RemoveCounter(name); }


        /// <summary>
        /// Reset the counter with the given name returning a copy of the counter before the reset occurred.
        /// </summary>
        /// <remarks>
        /// <para>The return value will represent a copy of the counter prior to the reset and is useful for applications that desire delta values. These delta values are simply the return values of successive reset calls.</para>
        /// <para>If the counter does not exist, it will be created prior to being reset. The return value will reflect an empty counter with the given name.</para>
        /// </remarks>
        /// <param name="name">The name of the counter to reset.</param>
        /// <returns>a counter containing the values of the counter prior to the reset.</returns>
        public static Counter ResetCounter(string name) { return monitor.ResetCounter(name); }


        /// <summary>
        /// Disable the timer with the given name.
        /// </summary>
        /// <remarks>
        /// <para>Disabling a timer will cause all new timers with the given name to skip processing reducing the amount of processing performed by the timers without losing the existing data in the timer. Any existing timers will continue to accumulate data.</para>
        /// <para>If a timer is disabled that has not already been created, a disabled timer will be created in memory that can be enabled at a later time.</para>
        /// </remarks>
        /// <param name="name">The name of the timer to disable.</param>
        public static void DisableTimer(string name) { monitor.DisableTimer(name); }


        /// <summary>
        /// Enable the timer with the given name.
        /// </summary>
        /// <remarks><para>If a timer is enabled that has not already been created, a new timer will be created in the monitor.</para></remarks>
        /// <param name="name">The name of the timer to enable.</param>
        public static void EnableTimer(string name) { monitor.EnableTimer(name); }


        /// <summary>
        /// Get the master timer with the given name.
        /// </summary>
        /// <param name="name">The name of the master timer to locate.</param>
        /// <returns>The master timer with the given name or null if that timer does not exist.</returns>
        public static ITimerMaster LocateTimerMaster(string name) { return monitor.LocateTimerMaster(name); }


        /// <summary>
        /// Removes all timers from the monitor and frees them up for garbage collection.
        /// </summary>
        public static void RemoveAllTimers() { monitor.RemoveAllTimers(); }


        /// <summary>
        /// Removes the timer (master) with the given name.
        /// </summary>
        /// <param name="name">Name of the timer to remove from the monitor</param>
        /// <returns>The removed timer master or null if not timer with that name was found in the monitor.</returns>
        public static ITimerMaster RemoveTimer(string name) { return monitor.RemoveTimer(name); }


        /// <summary>
        /// Start a timer with the given name.
        /// </summary>
        /// <remarks>
        /// <para>Use the returned Timer to stop the interval measurement.</para>
        /// <para>If timing is not enabled, null (no-op) timers will be returend and no time tracking is performed.</para>
        /// </remarks>
        /// <param name="name">The name of the timer instance to start.</param>
        /// <returns>The timer instance that should be stopped when the interval is completed.</returns>
        public static ITimer StartTimer(string name) { return monitor.StartTimer(name); }


        /// <summary>
        /// Retrieve a timer (master) with the given name. If one is not found with that name, it will be created.
        /// </summary>
        /// <param name="name">The name of the timer master to retrieve/create.</param>
        /// <returns>A new or existing timer master with that name. This will never return null.</returns>
        public static ITimerMaster RetrieveTimerMaster(string name) { return monitor.RetrieveTimerMaster(name); }


        /// <summary>
        /// Serializes all counters and metrics into OpenMetrics format.
        /// </summary>
        /// <returns>A string representing all instrumentation results in OpenMetrics format.</returns>
        public static string Serialize()
        {
            StringBuilder b = new StringBuilder();

            foreach (Counter counter in monitor.Counters)
            {
                b.Append($"# TYPE {counter.Name} counter\r\n");
                b.Append($"{counter.Name} {counter.Value}\r\n");
            }

            foreach (ITimerMaster timer in monitor.Timers)
            {
                b.Append($"# HELP {timer.Name}_hits Number of times the timer was created.\r\n");
                b.Append($"# TYPE {timer.Name}_hits counter\r\n");
                b.Append($"{timer.Name}_hits {timer.Hits}\r\n");
                b.Append($"# HELP {timer.Name}_avg The average duration of all timer instances.\r\n");
                b.Append($"# TYPE {timer.Name}_avg gauge\r\n");
                b.Append($"{timer.Name}_avg {timer.Average}\r\n");
                b.Append($"# HELP {timer.Name}_total The total time in milliseconds spent in all instances of this timer.\r\n");
                b.Append($"# TYPE {timer.Name}_total counter\r\n");
                b.Append($"{timer.Name}_total {timer.Total}\r\n");
                b.Append($"# HELP {timer.Name}_min The minimum time in milliseconds spent in any instance of this timer.\r\n");
                b.Append($"# TYPE {timer.Name}_min gauge\r\n");
                b.Append($"{timer.Name}_min {timer.Minimum}\r\n");
                b.Append($"# HELP {timer.Name}_max The maximum time in milliseconds spent in any instance of this timer.\r\n");
                b.Append($"# TYPE {timer.Name}_max counter\r\n");
                b.Append($"{timer.Name}_max {timer.Maximum}\r\n");
                b.Append($"# HELP {timer.Name}_std The standard deviation of time in milliseconds spent in all instances of this timer.\r\n");
                b.Append($"# TYPE {timer.Name}_std gauge\r\n");
                b.Append($"{timer.Name}_std {timer.StandardDeviation}\r\n");

                b.Append($"# HELP {timer.Name}_active The number of timers currently active.\r\n");
                b.Append($"# TYPE {timer.Name}_active gauge\r\n");
                b.Append($"{timer.Name}_active {timer.Active}\r\n");

                b.Append($"# HELP {timer.Name}_maxactive The maximum number of instances of this timer active at one time.\r\n");
                b.Append($"# TYPE {timer.Name}_maxactive gauge\r\n");
                b.Append($"{timer.Name}_maxactive {timer.MaxActive}\r\n");

            }

            return b.ToString();
        }

    }

}
