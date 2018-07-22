using System;

namespace Likvido.CreditRisk.Utils.DateTimeUtils
{
    public class DefaultTimeProvider : TimeProvider
    {
        private static TimeProvider instance;

        private DefaultTimeProvider()
        {
        }

        public static TimeProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DefaultTimeProvider();
                }

                return instance;
            }
        }

        public override DateTime UtcNow
        {
            get
            {
                return DateTime.UtcNow;
            }
        }
    }
}
