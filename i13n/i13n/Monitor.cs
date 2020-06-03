using System;
using System.Collections.Generic;

namespace i13n
{
    /// <summary>
    /// Models a collection of timers and counters to assist in the instrumentation of applications.
    /// </summary>
    public class Monitor
    {
        /// <summary>
        /// The unique identifier of this monitor.
        /// </summary>
        public string Identifier { get; private set; }


        /// <summary>
        /// True means returned timers will track elapsed time, false means null (no-op) timers will be returned resulting in no time tracking.
        /// </summary>
        public bool IsTimingEnabled { get; set; }


        /// <summary>
        /// The number of counters in the statboard at the present time.
        /// </summary>
        public int CounterCount { get { return counters.Count; } }


        /// <summary>
        /// Retrieve a list of counters in this monitor
        /// </summary>
        /// <remarks>
        /// <para>This list is detached from the backing store such that modifying the list will have no effect on the monitors collection of counters. Changing the counter instance will affect the counter stored in the monitor.</para>
        /// <para>If you wish to remove one of the counters, you must do so by its name and the RemoveCounter(string) method.</para>
        /// </remarks>
        public List<Counter> Counters {
            get {
                List<Counter> list = new List<Counter>();
                foreach (Counter counter in counters.Values)
                {
                    list.Add(counter);
                }
                return list;
            }
        }


        /// <summary>
        /// Retrieve a list of timer (masters) in this monitor
        /// </summary>
        /// <remarks>
        /// <para>This list is detached from the backing store such that modifying the list will have no effect on the monitors collection of timer masters. Changing the timer master instance will affect the timers stored in the monitor.</para>
        /// <para>If you wish to remove one of the masters, you must do so by its name and the RemoveTimer(string) method.</para>
        /// </remarks>
        public List<ITimerMaster> Timers {
            get {
                List<ITimerMaster> list = new List<ITimerMaster>();
                foreach (ITimerMaster master in masterTimers.Values)
                {
                    list.Add(master);
                }
                return list;
            }
        }


        /// <summary>
        /// This is our map of names to timer masters.
        /// </summary>
        private readonly Dictionary<string, ITimerMaster> masterTimers = new Dictionary<string, ITimerMaster>();


        /// <summary>
        /// This is our map of names to counters.
        /// </summary>
        private readonly Dictionary<string, Counter> counters = new Dictionary<string, Counter>();


        /// <summary>
        /// Re-usable null timer to save object creation and garbage collection
        /// </summary>
        private readonly ITimer NULL_TIMER = new NullTimer(null);


        /// <summary>
        /// Create an instance of a monitor.
        /// </summary>
        /// <remarks>
        /// <para>Timing is disabled by default so that performance is improved when used in production.</para>
        /// </remarks>
        public Monitor()
        {
            Identifier = new Guid().ToString();
            IsTimingEnabled = false;
        }


        /// <summary>
        /// Decrease the value with the given name by the given amount.
        /// </summary>
        /// <remarks>
        /// <para>This method retrieves the counter with the given name or creates one by that name if it does not yet exist. The retrieved counter is then decreased by the given amount.</para>
        /// </remarks>
        /// <param name="name">The name of the counter to decrease.</param>
        /// <returns>The final value of the counter after the operation.</returns>
        public long Decrease(string name, long value)
        {
            return RetrieveCounter(name).Decrease(value);
        }


        /// <summary>
        /// Decrement the value with the given name.
        /// </summary>
        /// <remarks>
        /// <para>This method retrieves the counter with the given name or creates one by that name if it does not yet exist. The retrieved counter is then decreased by one (1).</para>
        /// </remarks>
        /// <param name="name">The name of the counter to decrement.</param>
        /// <returns>The final value of the counter after the operation.</returns>
        public long Decrement(string name) { return RetrieveCounter(name).Decrement(); }


        /// <summary>
        /// Return the counter with the given name.
        /// </summary>
        /// <remarks>
        /// <para>If the counter does not exist, one will be created and added to the list of counters for later retrieval.</para>
        /// </remarks>
        /// <param name="name">The name of the counter to return.</param>
        /// <returns>The counter with the given name.</returns>
        public Counter RetrieveCounter(string name)
        {
            Counter retval = null;
            lock (counters)
            {
                if (!counters.TryGetValue(name, out Counter counter))
                {
                    counter = new Counter(name);
                    counters.Add(name, counter);
                }
                retval = counter;
            }
            return retval;
        }


        /// <summary>
        /// Increase the value with the given name by the given amount.
        /// </summary>
        /// <remarks>
        /// <para>This method retrieves the counter with the given name or creates one by that name if it does not yet exist. The retrieved counter is then increased by the given amount.</para>
        /// </remarks>
        /// <param name="name">The name of the counter to increase.</param>
        /// <returns>The final value of the counter after the operation.</returns>
        public long Increase(string name, long value) { return RetrieveCounter(name).Increase(value); }


        /// <summary>
        /// Increment the value with the given name.
        /// </summary>
        /// <remarks>
        /// <para>This method retrieves the counter with the given name or creates one by that name if it does not yet exist. The retrieved counter is then increased by one (1).</para>
        /// </remarks>
        /// <param name="name">The name of the counter to increment.</param>
        /// <returns>The final value of the counter after the operation.</returns>
        public long Increment(string name) { return RetrieveCounter(name).Increment(); }


        /// <summary>
        /// Remove the counter with the given name.
        /// </summary>
        /// <param name="name">Name of the counter to remove.</param>
        /// <returns>The removed counter or null if the counter was not found.</returns>
        public Counter RemoveCounter(string name)
        {
            Counter retval = null;
            lock (counters)
            {
                if (counters.TryGetValue(name, out Counter counter))
                {
                    retval = counter;
                    counters.Remove(name);
                }
            }
            return retval;
        }


        /// <summary>
        /// Reset the counter with the given name returning a copy of the counter before the reset occurred.
        /// </summary>
        /// <remarks>
        /// <para>The return value will represent a copy of the counter prior to the reset and is useful for applications that desire delta values. These delta values are simply the return values of successive reset calls.</para>
        /// <para>If the counter does not exist, it will be created prior to being reset. The return value will reflect an empty counter with the given name.</para>
        /// </remarks>
        /// <param name="name">The name of the counter to reset.</param>
        /// <returns>a counter containing the values of the counter prior to the reset.</returns>
        public Counter ResetCounter(string name)
        {
            Counter retval = null;
            Counter counter = RetrieveCounter(name);
            if (counter != null)
            {
                retval = counter.Reset();
            }
            return retval;
        }


        /// <summary>
        /// Disable the timer with the given name.
        /// </summary>
        /// <remarks>
        /// <para>Disabling a timer will cause all new timers with the given name to skip processing reducing the amount of processing performed by the timers without losing the existing data in the timer. Any existing timers will continue to accumulate data.</para>
        /// <para>If a timer is disabled that has not already been created, a disabled timer will be created in memory that can be enabled at a later time.</para>
        /// </remarks>
        /// <param name="name">The name of the timer to disable.</param>
        public void DisableTimer(string name)
        {
            RetrieveTimerMaster(name).Enabled = false;
        }

        
        /// <summary>
        /// Enable the timer with the given name.
        /// </summary>
        /// <remarks><para>If a timer is enabled that has not already been created, a new timer will be created in the monitor.</para></remarks>
        /// <param name="name">The name of the timer to enable.</param>
        public void EnableTimer(string name)
        {
            RetrieveTimerMaster(name).Enabled = true;
        }


        /// <summary>
        /// Get the master timer with the given name.
        /// </summary>
        /// <param name="name">The name of the master timer to locate.</param>
        /// <returns>The master timer with the given name or null if that timer does not exist.</returns>
        public ITimerMaster LocateTimerMaster(string name)
        {
            ITimerMaster retval = null;
            lock (masterTimers)
            {
                if (masterTimers.TryGetValue(name, out ITimerMaster master)) { retval = master; }
            }
            return retval;
        }


        /// <summary>
        /// Removes all timers from the monitor and frees them up for garbage collection.
        /// </summary>
        public void RemoveAllTimers()
        {
            lock (masterTimers)
            {
                masterTimers.Clear();
            }
        }


        /// <summary>
        /// Removes the timer (master) with the given name.
        /// </summary>
        /// <param name="name">Name of the timer to remove from the monitor</param>
        /// <returns>The removed timer master or null if not timer with that name was found in the monitor.</returns>
        public ITimerMaster RemoveTimer(string name)
        {
            ITimerMaster retval = null;
            lock (masterTimers)
            {
                if (masterTimers.TryGetValue(name, out ITimerMaster master)) { retval = master; }
            }
            return retval;
        }


        /// <summary>
        /// Start a timer with the given name.
        /// </summary>
        /// <remarks>
        /// <para>Use the returned Timer to stop the interval measurement.</para>
        /// <para>If timing is not enabled, null (no-op) timers will be returend and no time tracking is performed.</para>
        /// </remarks>
        /// <param name="name">The name of the timer instance to start.</param>
        /// <returns>The timer instance that should be stopped when the interval is completed.</returns>
        public ITimer StartTimer(string name)
        {
            ITimer retval = null;
            if (IsTimingEnabled)
            {
                lock (masterTimers)
                {
                    if (!masterTimers.TryGetValue(name, out ITimerMaster master))
                    {
                        master = new TimingMaster(name);
                        masterTimers.Add(name, master);
                    }
                    retval = master.CreateTimer();
                    retval.Start();
                }
            }
            else
            {
                retval = NULL_TIMER;
            }
            return retval;
        }


        /// <summary>
        /// Retrieve a timer (master) with the given name. If one is not found with that name, it will be created.
        /// </summary>
        /// <param name="name">The name of the timer master to retrieve/create.</param>
        /// <returns>A new or existing timer master with that name. This will never return null.</returns>
        public ITimerMaster RetrieveTimerMaster(string name)
        {
            masterTimers.TryGetValue(name, out ITimerMaster retval);
            return retval;
        }

    }

}
