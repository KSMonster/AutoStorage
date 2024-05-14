using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleTrader.Domain.Models
{
    public class ItemType : DomainObject
    {
        public string ItemCode { get; set; }
        public string Name { get; set; }
    }
}
