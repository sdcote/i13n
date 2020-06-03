using System;
using System.Collections.Generic;
using System.Text;

namespace i13n
{
    public class Counter
    {
        private String _units = null;
        private long _value = 0;
        private long _minValue = 0;
        private long _maxValue = 0;
        protected string _name = null;
        protected long _updateCount = 0;


        public string Name { get { return _name; } private set { _name = value; } }


        /// <summary>
        /// Create a named counter.
        /// </summary>
        /// <param name="name">The name of the counter</param>
        public Counter(String name)
        {
            Name = name;
        }

        /// <summary>
        /// Create a deep copy of this counter.
        /// </summary>
        /// <returns></returns>
        public Object Clone()
        {
            Counter retval = new Counter(_name);
            retval._units = _units;
            retval._value = _value;
            retval._minValue = _minValue;
            retval._maxValue = _maxValue;
            retval._updateCount = _updateCount;
            return retval;
        }


        /**
        * Decrease the counter by the given amount.
        *
        * @param amt The amount to subtract from the counter.
        *
        * @return The final value of the counter after the operation.
        */
        public long Decrease(long amt)
        {
            lock (_name)
            {
                _updateCount++;
                _value -= amt;
                if (_value < _minValue) { _minValue = _value; }
                if (_value > _maxValue) { _maxValue = _value; }
                return _value;
            }
        }




        /**
         * Decrement the counter by one.
         *
         * @return The final value of the counter after the operation.
         */
        public long Decrement()
        {
            lock (_name)
            {
                _updateCount++;
                _value--;
                if (_value < _minValue) { _minValue = _value; }
                return _value;
            }
        }




        /**
         * @return Returns the maximum value the counter ever represented.
         */
        public long MaxValue {
            get { lock (_name) { return _maxValue; } }
        }




        /**
         * @return Returns the minimum value the counter ever represented.
         */
        public long MinValue {
            get { lock (_name) { return _minValue; } }
        }




        /**
         * @return Returns the units the counter measures.
         */
        public String Units { get { return _units; } }




        /**
         * @return Returns the current value of the counter.
         */
        public long Value { get { lock (_name) { return _value; } } }


        public long UpdateCount { get { lock (_name) { return _updateCount; } } }


        /**
         * Increase the counter by the given amount.
         *
         * @param amt The amount to add to the counter.
         *
         * @return The final value of the counter after the operation.
         */
        public long Increase(long amt)
        {
            lock (_name)
            {
                _updateCount++;
                _value += amt;
                if (_value < _minValue) { _minValue = _value; }
                if (_value > _maxValue) { _maxValue = _value; }
                return _value;
            }
        }




        /**
         * Increment the counter by one.
         *
         * @return The final value of the counter after the operation.
         */
        public long Increment()
        {
            lock (_name)
            {
                _updateCount++;
                _value++;
                if (_value > _maxValue) { _maxValue = _value; }
                return _value;
            }
        }




        /**
         * Set the current, update count and Min/Max values to zero.
         *
         * <p>The return value will represent a copy of the counter prior to the
         * reset and is useful for applications that desire delta values. These delta
         * values are simply the return values of successive reset calls.
         *
         * @return a counter representing the state prior to the reset.
         */
        public Counter Reset()
        {
            lock (_name)
            {
                Counter retval = (Counter)Clone();
                _value = 0;
                _minValue = 0;
                _maxValue = 0;
                _updateCount = 0;
                return retval;
            }
        }







        /**
         * Return the human-readable form of this counter.
         */
        public override string ToString()
        {
            lock (Name)
            {
                StringBuilder buff = new StringBuilder(Name);
                buff.Append("=");
                buff.Append(Value.ToString());
                if (Units != null)
                {
                    buff.Append(Units);
                }
                buff.Append("[min=");
                buff.Append(MinValue.ToString());
                buff.Append(":max=");
                buff.Append(MaxValue.ToString());
                buff.Append("]");

                return buff.ToString();
            }
        }


    }
}
