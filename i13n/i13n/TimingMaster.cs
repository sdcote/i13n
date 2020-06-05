using System;
using System.Globalization;
using System.Text;
using System.Threading;

namespace i13n
{

    /// <summary>
    /// he TimingMaster class models the master of all timers with a given name.
    /// </summary>
    /// <remarks>
    /// <para>This class is used to summarize all the timers in its list.</para>
    /// <para>Timer Master can be enabled and disabled. This allows some timers to track timing while others ignore timing operations by returing NullTimers which ignore processing.</para>
    /// </remarks>
    public class TimingMaster : ITimerMaster
    {
        private static readonly int TICKS_PER_MILLISECOND = 10000;
        private static readonly string MILLISECONDS = "ms";
        private static readonly string NONE = "";
        private static readonly string TOTAL = "Total";
        private static readonly string MIN = "Min Value";
        private static readonly string MAX = "Max Value";
        private static readonly string HITS = "Hits";
        private static readonly string AVG = "Avg";
        private static readonly string STANDARD_DEVIATION = "Std Dev";
        private static readonly string ACTIVE = "Active";
        private static readonly string MAXACTIVE = "Max Active";
        private static readonly string FIRSTACCESS = "First Access";
        private static readonly string LASTACCESS = "Last Access";

        /// <summary>
        /// How many timers are currently active
        /// </summary>
        private long activeCounter = 0;

        /// <summary>
        /// The total number of ticks accrued by stopped timers so far.
        /// </summary>
        private long total;

        /// <summary>
        /// The smallest increment to the accrued total
        /// </summary>
        private long min = Int64.MaxValue;

        /// <summary>
        /// The largest increment to the accrued total
        /// </summary>
        private long max = Int64.MinValue;

        /// <summary>
        /// The number of times this master has created a timing timer,
        /// </summary>
        private int hits;

        /// <summary>
        /// The sum of squares total used in standard deviation calculation
        /// </summary>
        private long sumOfSquares;

        /// <summary>
        /// The number of global timers currently active.
        /// </summary>
        private static long globalCounter = 0;

        /// <summary>
        /// Flag indicating whether or not to store the first accessed time
        /// </summary>
        private bool isFirstAccess = true;

        /// <summary>
        /// Epoch time in ticks when this timer was first accessed
        /// </summary>
        private long firstAccessTime = 0;

        /// <summary>
        /// Time in ticks when this timer was last accessed
        /// </summary>
        private long lastAccessTime = 0;

        /// <summary>
        /// The maximum number of timers running at the same time.
        /// </summary>
        private long maxActive = 0;


        /// <summary>
        /// The average time for all stopped timers for this master list.
        /// </summary>
        public long AverageTime {
            get {
                // we can only average the total number of closures not just the hits
                long closures = (hits - activeCounter);
                if (closures == 0) { return 0; }
                else { return total / closures; }
            }
        }


        /// <summary>
        /// The name of the timer master.
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// True indicates this master will return TimingTimers, false indicates null (no-op) timers will be returned.
        /// </summary>
        public bool Enabled { get; set; }


        /// <summary>
        /// Get the accrued number of ticks for all timers stopped in this master. Note there are 10,000 ticks to one millisecond, 10 ticks to a microsecond, or 1 tick = 100 nanoseconds)
        /// </summary>
        public long Accrued { get { return total; } }


        /// <summary>
        /// Get the number of times this timer (master) created and started a timer. This does not include start/stop requests at the timer level.
        /// </summary>
        public long Hits { get { return hits; } }


        /// <summary>
        /// This is the main constructor for this timer; all timers should have a name.
        /// </summary>
        /// <param name="name"></param>
        public TimingMaster(string name)
        {
            Name = name;
            Enabled = true;
        }


        /// <summary>
        /// Increase the number ticks accrued for all timers this master created.
        /// </summary>
        /// <param name="value">The amount to increase the accrued value.</param>
        public void Increase(long value)
        {
            if (value < min) { min = value; }
            if (value > max) { max = value; }
            total += value;
            sumOfSquares += value * value;
        }


        /// <summary>
        /// Access the current standard deviation for all stopped timers using the Sum of Squares algorithm.
        /// </summary>
        /// <returns>The amount of one standard deviation of all the interval times.</returns>
        public long StandardDeviation {
            get {
                long stdDeviation = 0;
                if (hits > 1)
                {
                    long sumOfX = total;
                    int n = hits;
                    int nMinus1 = (n <= 1) ? 1 : n - 1; // avoid 0 divides;
                    long numerator = sumOfSquares - ((sumOfX * sumOfX) / n);
                    stdDeviation = (long)Math.Sqrt(numerator / nMinus1);
                }
                return stdDeviation;
            }
        }


        /// <summary>
        /// Start a timer in the context for this timer master.
        /// </summary>
        /// <param name="timer">The timer to start</param>
        public void Start(ITimer timer)
        {
            lock (this)
            {
                activeCounter++;
                TimingMaster.globalCounter++;

                if (activeCounter > maxActive) { maxActive = activeCounter; }

                long now = DateTime.Now.Ticks;
                lastAccessTime = now;

                if (isFirstAccess)
                {
                    isFirstAccess = false;
                    firstAccessTime = now;
                }
                timer.StartCount++;
            }
        }

        /// <summary>
        /// Stop a timer in the context for this timer master.
        /// </summary>
        /// <param name="timer">The timer to stop</param>
        public void Stop(ITimer timer)
        {
            lock (this)
            {
                activeCounter--;
                TimingMaster.globalCounter--;
                Increase(timer.Accrued);
                timer.Accrued = 0;
                timer.StopCount++;
            }
        }


        /// <summary>
        /// Create a timer associated with this master.
        /// </summary>
        /// <returns>A new, unstarted timer.</returns>
        public ITimer CreateTimer()
        {
            ITimer retval;
            if (Enabled)
            {
                retval = new TimingTimer(this);
                hits++;
            }
            else
            {
                retval = new NullTimer(this);
            }
            return retval;
        }


        /// <summary>
        /// Display the master timer data in a human readable format.
        /// </summary>
        /// <returns>Nicely formatted timer data.</returns>
        public override string ToString()
        {
            StringBuilder message = new StringBuilder(Name);
            message.Append(": ");
            message.Append(getDisplayString(TimingMaster.HITS, ConvertToString(hits), TimingMaster.NONE));

            if ((hits - activeCounter) > 0)
            {
                message.Append(getDisplayString(TimingMaster.AVG, ConvertToString(AverageTime / TICKS_PER_MILLISECOND), TimingMaster.MILLISECONDS));
                message.Append(getDisplayString(TimingMaster.TOTAL, ConvertToString(total / TICKS_PER_MILLISECOND), TimingMaster.MILLISECONDS));
                message.Append(getDisplayString(TimingMaster.STANDARD_DEVIATION, ConvertToString(StandardDeviation / TICKS_PER_MILLISECOND), TimingMaster.MILLISECONDS));
                message.Append(getDisplayString(TimingMaster.MIN, ConvertToString(min / TICKS_PER_MILLISECOND), TimingMaster.MILLISECONDS));
                message.Append(getDisplayString(TimingMaster.MAX, ConvertToString(max / TICKS_PER_MILLISECOND), TimingMaster.MILLISECONDS));
            }
            message.Append(getDisplayString(TimingMaster.ACTIVE, ConvertToString(activeCounter), TimingMaster.NONE));
            message.Append(getDisplayString(TimingMaster.MAXACTIVE, ConvertToString(maxActive), TimingMaster.NONE));
            message.Append(getDisplayString(TimingMaster.FIRSTACCESS, GetDateString(firstAccessTime), TimingMaster.NONE));
            message.Append(getDisplayString(TimingMaster.LASTACCESS, GetDateString(lastAccessTime), TimingMaster.NONE));

            return message.ToString();
        }

        private string GetDateString(long time)
        {
            if (time == 0) { return ""; }
            else { return new DateTime(time).ToString("yyyy-MM-dd HH:mm:ss zzz", CultureInfo.InvariantCulture); }
        }


        private string ConvertToString(double number)
        {
            return number.ToString("#,##0.#");
        }
        private string ConvertToString(long number)
        {
            return number.ToString("#,##0");
        }

        private string getDisplayString(string type, string value, string units)
        {
            if (TimingMaster.NONE.Equals(units)) { return type + "=" + value + ", "; }
            else { return type + "=" + value + " " + units + ", "; }
        }
    }

}