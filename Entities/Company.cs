using Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Company : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Inn { get; set; }
        public string LegalAddress { get; set; }
    }
}
