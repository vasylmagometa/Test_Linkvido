using System.Collections.Generic;
using System.Linq;
using Likvido.CreditRisk.Utils.Exensions;

namespace Likvido.CreditRisk.Models
{
    public class CollectionSpecification<T>
    {
        public string Ids { get; set; }        

        public List<T> ResolveIds()
        {
            return string.IsNullOrWhiteSpace(Ids)
                ? new List<T>()
                : Ids.Split(',').ToList().Select(s => s.Convert<T>()).ToList();
        }
    }
}
