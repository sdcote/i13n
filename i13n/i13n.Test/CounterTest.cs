using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Threading.Tasks;


namespace i13n.Test
{
    [TestClass]
    public class CounterTest
    {
        [TestMethod]
        public void Increment()
        {
            string NAME = "testIncrement";
            long LIMIT = 10;

            Counter counter = new Counter(NAME);

            for (int x = 0; x < LIMIT; x++) { counter.Increment(); }

            Assert.IsTrue(counter.Value == LIMIT, "Value is " + counter.Value + " and should be " + LIMIT);
            Assert.IsTrue(counter.MaxValue == LIMIT, "MaxValue is " + counter.MaxValue + " and should be " + LIMIT);
            Assert.IsTrue(counter.MinValue == 0, "MinValue is " + counter.MinValue + " and should be 0");
            Assert.IsTrue(counter.UpdateCount == LIMIT, "UpdateCount is " + counter.UpdateCount + " and should be " + LIMIT);
        }




        [TestMethod]
        public void testConstructor()
        {
            string NAME = "test";
            Counter counter = new Counter(NAME);

            Assert.IsTrue(counter.Name.Equals(NAME), "Name is wrong");
            Assert.IsTrue(counter.Value == 0, "Value is wrong");
            Assert.IsTrue(counter.MaxValue == 0, "MaxValue is wrong");
            Assert.IsTrue(counter.MinValue == 0, "MinValue is wrong");
            Assert.IsTrue(counter.Units == null, "Units is wrong");
            Assert.IsTrue(counter.UpdateCount == 0, "UpdateCount is wrong");
        }




        [TestMethod]
        public void testReset()
        {
            string NAME = "testReset";
            long LIMIT = 10;

            Counter counter = new Counter(NAME);

            for (int x = 0; x < LIMIT; x++) { counter.Increment(); }

            Counter delta = counter.Reset();

            Assert.IsTrue(delta.Name.Equals(NAME), "Delta Name is " + delta.Name + " and should be " + NAME);
            Assert.IsTrue(delta.Value == LIMIT, "Delta Value is " + delta.Value + " and should be " + LIMIT);
            Assert.IsTrue(delta.MaxValue == LIMIT, "Delta MaxValue is " + delta.MaxValue + " and should be " + LIMIT);
            Assert.IsTrue(delta.MinValue == 0, "Delta MinValue is " + delta.MinValue + " and should be 0");
            Assert.IsTrue(delta.Units == null, "Delta Units are " + delta.Units + " and should be null");
            Assert.IsTrue(delta.UpdateCount == LIMIT, "Delta UpdateCount is " + delta.UpdateCount + " and should be " + LIMIT);

            Assert.IsTrue(counter.Name.Equals(NAME), "Counter Name is " + counter.Name + " and should be " + NAME);
            Assert.IsTrue(counter.Value == 0, "Counter Value is " + counter.Value + " and should be 0");
            Assert.IsTrue(counter.MaxValue == 0, "Counter MaxValue is " + counter.MaxValue + " and should be 0");
            Assert.IsTrue(counter.MinValue == 0, "Counter MinValue is " + counter.MinValue + " and should be 0");
            Assert.IsTrue(counter.Units == null, "Counter Units are " + counter.Units + " and should be null");
            Assert.IsTrue(counter.UpdateCount == 0, "Counter UpdateCount is " + counter.UpdateCount + " and should be 0");

        }



    }
}
