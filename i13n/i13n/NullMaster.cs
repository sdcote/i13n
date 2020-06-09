namespace i13n
{
    /// <summary>
    /// This class models a no-op implementation of a timer master. No data are collected.
    /// </summary>
    internal class NullMaster : ITimerMaster
    {
        private static ITimer NULL_TIMER = new NullTimer();

        public string Name { get { return ""; } }

        public bool Enabled { get; set; }

        public int Hits { get { return 0; } }

        public long Average { get { return 0L; } }

        public long Total { get { return 0L; } }

        public long Maximum { get { return 0L; } }

        public long Minimum { get { return 0L; } }

        public int Active { get; }

        public int MaxActive { get; }

        public long StandardDeviation { get { return 0L; } }

        public ITimer CreateTimer() { return NULL_TIMER; }

        public void Increase(long value) { }

        public void Start(ITimer mon) { }

        public void Stop(ITimer mon) { }

    }
}