namespace i13n
{
    /// <summary>
    /// The TimerMaster class models the master of all timers with a given name.
    /// </summary>
    public interface ITimerMaster
    {
        public bool Enabled { get; set; }


        public string Name { get; }


        public void Increase(long value);


        public void Start(ITimer mon);


        public void Stop(ITimer mon);

        ITimer CreateTimer();
    }
}