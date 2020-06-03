namespace i13n
{
    internal class NullMaster : ITimerMaster
    {
        /// <summary>
        /// This class models a no-op implementation of a timer master. No data are collected.
        /// </summary>
        public string Name { get { return ""; } }

        public bool Enabled { get; set; }

        public ITimer CreateTimer()
        {
            throw new System.NotImplementedException();
        }

        public void Increase(long value) { }

        public void Start(ITimer mon) { }

        public void Stop(ITimer mon) { }

    }
}