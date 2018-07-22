using System;
using System.Reflection;

namespace Likvido.CreditRisk.DataAccess.Extensions
{
    public static class MemberInfoExtensions
    {
        public static bool HasAttribute<TAttribute>(this MemberInfo member, bool inherit = false)
            where TAttribute : Attribute
        {
            return member.GetCustomAttribute<TAttribute>(inherit) != null;
        }
    }
}
