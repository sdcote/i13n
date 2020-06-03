using System;

namespace i13n
{
    /**
 * 
 *
 */
    /// <summary>
    /// Timers are devices for measuring the time something takes and the Timer interface models a contract for all timers.
    /// </summary>
    /// <remarks>
    /// <para>Timers measure the time between phases of execution. Once a Timer is started, its stores the time and waits for a 
    /// call to stop(). Once stopped, it calculates the total elapsed time for that run.</para>
    /// <para>Later a Timer(actually the entire set) can be rolled-up to provide the number of invocations, mean, long and short 
    /// elapsed intervals and several other phase-oriented metrics in a manner similar to those of counters and states.</para>
    /// <para>The fixture tracks a Timer by its name where each name represents a Master Timer that is used to accrue all the data 
    /// of Timers with the same name.</para>
    /// <para>There are two types of Timers, a Timed Timer and a Null Timer. During normal operation the fixture issues a Timed 
    /// Timer that tracks the time between its start and stop methods are called finally placing the results in its master Timer. 
    /// If monitoring has been disabled for either the entire fixture or for a specific named Timer, then a Null Timer is issued. 
    /// It implements the exact same interface as the timed Timer, but the Null Timer contains no logic thereby saving on 
    /// processing when monitoring is not desired.</para>
    /// <para>A single Timer reference can be started and stopped several times, each interval between the start-stop calls being 
    /// added to the accrued value of the Timer.</para>
    /// </remarks>
    public interface ITimer
    {

        /// <summary>
        /// The value of the datum collected.
        /// </summary>
        public long Accrued { get; }


        /// <summary>
        /// The master timer that tracks data for all timers in the named set.
        /// </summary>
        public ITimerMaster Master { get; }


        /// <summary>
        /// The name of the timer.
        /// </summary>
        public string Name { get; }


        /// <summary>
        /// Start this timer collecting datum. 
        /// </summary>
        public void Start();


        /// <summary>
        /// Stop this timer from collecting datum.
        /// </summary>
        public void Stop();

    }
}
