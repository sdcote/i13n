using System;

namespace i13n
{
    public class AbstractTimer : ITimer
    {
        private const long TICKS_PER_MILLISECOND = 10000;
        private const long TICKS_PER_SECOND = TICKS_PER_MILLISECOND * 1000;

        protected readonly static ITimerMaster NULL_MASTER = new NullMaster();

        /// <summary>
        /// Flag indicating if the timer is running
        /// </summary>
        public bool IsRunning { get; set; }

        public long Accrued { get; protected set; }

        /// <summary>
        /// The master timer that accumulates our data.
        /// </summary>
        public ITimerMaster Master { get; set; }

        /// <summary>
        /// The name of this timer
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// The time this timer was started last.
        /// </summary>
        public long StartTimeTicks { get; protected set; }

        public long StartCount { get; set; }

        public long StopCount { get; set; }

        protected AbstractTimer(ITimerMaster master) { Master = master; }

        public virtual void Start() { }

        public virtual void Stop() { }

        public override string ToString()
        {
            if (Master != null) { return Master.ToString(); }
            return "";
        }

    }

}