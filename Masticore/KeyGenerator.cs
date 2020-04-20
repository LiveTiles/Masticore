using System;

namespace Masticore
{
    /// <summary>
    /// Class for generating key values and time-based values
    /// </summary>
    public static class KeyGenerator
    {
        static Object _TicksLock = new Object();
        static long _LastTicks = long.MinValue;

        /// <summary>
        /// Gets a unique ticks value that is guaranteed to be incremented by at least one from the last call of this function
        /// This is to ensure no colliding rowkeys for table storage records indexed by time
        /// NOTE: Still theoretically possible to have multiple servers writing to the same partition copy a ticks
        /// </summary>
        /// <returns></returns>
        public static long NextUtcTicks()
        {
            // Ensure thread safety
            lock (_TicksLock)
            {
                // Read current ticks
                var ticks = DateTime.UtcNow.Ticks;

                // Ensure this ticks is greater than the currenly save value
                // Otherwise, we increment our current value and return it instead
                if (ticks > _LastTicks)
                    _LastTicks = ticks;
                else
                    _LastTicks += 1;

                return _LastTicks;
            }
        }

        /// <summary>
        /// Returns a GUID string conforming to a RowKey
        /// </summary>
        /// <returns></returns>
        public static string GuidRowKey()
        {
            return Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Converts the given long value to a string, in a format good for partition and row keys
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string LongToString(long value)
        {
            return string.Format("{0:D19}", value);
        }

        /// <summary>
        /// Generates the next ticks value from the given rowkey, in descending order (value minus 1)
        /// </summary>
        /// <param name="ticksDescendingRowKey"></param>
        /// <returns></returns>
        public static string NextTicksDescendingRowKey(string ticksDescendingRowKey)
        {
            var val = long.Parse(ticksDescendingRowKey);
            val -= 1;
            return LongToString(val);
        }

        /// <summary>
        /// Generates the next ticks value from the given rowkey, in ascending order (value plus 1)
        /// </summary>
        /// <param name="ticksDescendingRowKey"></param>
        /// <returns></returns>
        public static string NextTicksAscendingRowKey(string ticksDescendingRowKey)
        {
            var val = long.Parse(ticksDescendingRowKey);
            val += 1;
            return LongToString(val);
        }

        /// <summary>
        /// Generates a RowKey string based on ticks, supporting descending time ordering (New on top)
        /// </summary>
        /// <returns></returns>
        public static string NextTicksDescendingRowKey()
        {
            return LongToString(long.MaxValue - NextUtcTicks());
        }

        /// <summary>
        /// Generates a rowkey string based on ticks, supporting ascending time ordering (New on Bottom)
        /// </summary>
        /// <returns></returns>
        public static string NextTicksAscendingRowKey()
        {
            return LongToString(NextUtcTicks());
        }
    }
}
