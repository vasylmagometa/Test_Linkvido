using System;

namespace Likvido.CreditRisk.Utils.Exensions
{
    public static class GenericExtensions
    {
        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source?.IndexOf(toCheck, comp) >= 0;
        }
    }
}
