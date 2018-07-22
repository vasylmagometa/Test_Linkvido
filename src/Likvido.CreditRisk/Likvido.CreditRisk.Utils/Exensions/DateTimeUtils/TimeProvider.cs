using System;

namespace Likvido.CreditRisk.Utils.DateTimeUtils
{
    public abstract class TimeProvider
    {
        private static TimeProvider current =
            DefaultTimeProvider.Instance;

        public static TimeProvider Current
        {
            get
            {
                return current;
            }

            set
            {
                current = value ?? throw new ArgumentNullException("value");
            }
        }

        public abstract DateTime UtcNow { get; }

        public static void ResetToDefault()
        {
            current = DefaultTimeProvider.Instance;
        }
    }
}
