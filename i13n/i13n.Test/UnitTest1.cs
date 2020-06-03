using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Threading.Tasks;

namespace i13n.Test
{
    [TestClass]
    public class UnitTest1
    {

        [TestMethod]
        public void TestMethod1()
        {
            ITimer timer = new TimingTimer();

            for (int x = 0; x < 10; x++)
            {
                timer.Start();
                Task.Delay(10).Wait(); // 100,000 ticks
                timer.Stop();
                Task.Delay(500).Wait(); // 5,000,000 ticks
                Debug.WriteLine(timer.Accrued);
            }
            Assert.IsTrue((timer.Accrued < 2000000), "Too much time accrued");
            Debug.WriteLine(timer.Accrued);
        }
    }
}
