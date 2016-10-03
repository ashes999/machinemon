using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineMon.Core.Domain
{
    public class Host
    {
        public Guid Id { get; set; }
        public string Fqdn { get; set; }
        public string FriendlyName { get; set; }
    }
}
