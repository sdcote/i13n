using System;

namespace i13n
{
    /// <summary>
    /// The TimingTimer class models an actual working implementation of an ITimer as opposed to the NullTimer.
    /// </summary>
    public class TimingTimer : AbstractTimer
    {
       

        /// <summary>
        /// Create a new timer with a null master.
        /// </summary>
        public TimingTimer() : base(AbstractTimer.NULL_MASTER) { }


        /// <summary>
        /// Create a new timer with the given master.
        /// </summary>
        /// <param name="master">The timing master to collect our data.</param>
        public TimingTimer(TimingMaster master) : base(master) { }



        /// <summary>
        /// Returns the number of ticks since the timer has been started.
        /// </summary>
        /// <returns></returns>
        protected long TimeElapsedSinceLastStart()
        {
            if (IsRunning) { return DateTime.Now.Ticks - StartTimeTicks; }
            else { return 0; }
        }


        /// <summary>
        /// Increase the time by the specified amount of Ticks.
        /// </summary>
        /// <remarks><para>This is the method that keeps track of the various statistics being tracked.</para></remarks>
        /// <param name="value">The amount to increase the accrued value.</param>
        protected void Increase(long value)
        {
            if (IsRunning) { Accrued += value; }
        }


        public override void Start() {
            if (!IsRunning)
            {
                StartTimeTicks = DateTime.Now.Ticks;
                IsRunning = true;
                Master.Start(this);
            }
        }

        public override void Stop() {
            if (IsRunning)
            {
                Increase(TimeElapsedSinceLastStart());
                Master.Increase(Accrued);
                Master.Stop(this);
                IsRunning = false;
            }
        }

    }
}
