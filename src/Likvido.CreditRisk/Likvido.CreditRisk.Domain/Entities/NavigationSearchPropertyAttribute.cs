using System;

namespace Likvido.CreditRisk.Domain.Entities
{
    [AttributeUsage(AttributeTargets.Property)]
    public class NavigationSearchPropertyAttribute : Attribute
    {
        public NavigationSearchPropertyAttribute(string name)
        {
            this.Name = name;
            this.Names = new string[1];
            this.Names[0] = name;
        }

        public NavigationSearchPropertyAttribute(params string[] names)
        {
            this.Names = names;
            this.Name = names[0];
        }

        public string Name { get; private set; }

        public string[] Names { get; private set; }
    }
}
