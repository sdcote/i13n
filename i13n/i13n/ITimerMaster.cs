namespace i13n
{
    /// <summary>
    /// The TimerMaster class models the master of all timers with a given name.
    /// </summary>
    public interface ITimerMaster
    {

        /// <summary>
        /// Flag indicating this timer is currently recording duration metrics.
        /// </summary>
        public bool Enabled { get; set; }


        /// <summary>
        /// The name of this timer.
        /// </summary>
        public string Name { get; }


        /// <summary>
        /// Get the number of times this timer (master) created and started a timer. This does not include start/stop requests at the timer instance level.
        /// </summary>
        int Hits { get; }


        /// <summary>
        /// The average number of milliseconds spent in this timer.
        /// </summary>
        long Average { get; }


        /// <summary>
        /// The total number of milliseconds spent in all instances of this timer.
        /// </summary>
        long Total { get; }


        /// <summary>
        /// The maximum number of milliseconds spent in this timer.
        /// </summary>
        long Maximum { get; }


        /// <summary>
        /// The minimum number of milliseconds spent in this timer.
        /// </summary>
        long Minimum { get; }


        /// <summary>
        /// The standard deviation of time, in milliseconds, spent in all instances of this timer.
        /// </summary>
        long StandardDeviation { get; }


        /// <summary>
        /// The number of timers currently active in this master.
        /// </summary>
        public int Active { get; }


        /// <summary>
        /// The maximum number of instances of this timer active at one time.
        /// </summary>
        public int MaxActive { get; }


        /// <summary>
        /// Increase the number ticks accrued for all timers this master created.
        /// </summary>
        public void Increase(long value);


        /// <summary>
        /// Start a timer in the context of this timer master.
        /// </summary>
        /// <param name="timer">The timer to start</param>
        public void Start(ITimer timer);


        /// <summary>
        /// Stop a timer in the context of this timer master.
        /// </summary>
        /// <param name="timer">The timer to stop</param>
        public void Stop(ITimer timer);


        /// <summary>
        /// Create a timer associated with this master.
        /// </summary>
        /// <returns>A new, unstarted timer.</returns>
        public ITimer CreateTimer();

    }

}