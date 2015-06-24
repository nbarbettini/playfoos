using System;
using Microsoft.SPOT;
using System.Collections;

namespace NetchemiaFooz
{
    class CullingDateTimeArray
    {
        private ArrayList _values;
        private ArrayList _times;

        public CullingDateTimeArray()
        {
            _values = new ArrayList();
            _times = new ArrayList();
        }

        public void Add(int value, DateTime time)
        {
            _times.Add(time);
            _values.Add(value);
        }

        public int CountValues(int minValue)
        {
            int count = 0;
            for (int i = 0; i < _values.Count; i++)
            {
                if ((int)_values[i] <= minValue)
                    count++;
            }
            return count;
        }

        public int Count()
        {
            return _times.Count;
        }

        public void ClearAll()
        {
            _times.Clear();
            _values.Clear();
        }

        public int RemoveBefore(DateTime time)
        {
            var newTimes = new ArrayList();
            var newValues = new ArrayList();

            int removed = 0;
            for (int i = 0; i < _times.Count; i++)
            {
                if ((DateTime)_times[i] > time)
                {
                    newTimes.Add(_times[i]);
                    newValues.Add(_values[i]);
                }
                else
                {
                    removed++;
                }
            }

            if (removed > 0)
            {
                _times.Clear();
                _values.Clear();

                if (newTimes.Count > 0)
                {
                    for (int i = 0; i < newTimes.Count; i++)
                    {
                        _times.Add(newTimes[i]);
                        _values.Add(newValues[i]);
                    }
                }
            }
            return removed;
        }
    }
}