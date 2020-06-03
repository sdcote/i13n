using System;
using System.Collections.Generic;
using System.Text;

namespace i13n
{

    /// <summary>
    /// Creates a do-nothing timer to facilitate the disabling of timers while not affecting the compiled code of any callers.
    /// </summary>
    /// <remarks>
    /// <para>When a NullTimer is returned, performs no logic when it is stopped an therefore allows for very fast operation when the timer is disabled.</para>
    /// <para>See Martin Fowler's refactoring book for details on using Null Objects in software.</para>
    /// </remarks>
    class NullTimer : AbstractTimer
    {

        /// <summary>
        /// Create a new no-op timer with a null master.
        /// </summary>
        public NullTimer() : base(AbstractTimer.NULL_MASTER) { }


        /// <summary>
        /// Create a new no-op timer with the given master.
        /// </summary>
        /// <param name="TimingMaster"></param>
        /// <param name=""></param>
        public NullTimer(ITimerMaster master) : base(master) { }

    }
}
