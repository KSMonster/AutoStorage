using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleTrader.Domain.Models
{
    public class Item : DomainObject
    {
        public string Name { get; set; }
        public int Count { get; set; }

        public int fk_BoxId { get; set; }
 
        [ForeignKey(nameof(fk_BoxId))]
        public virtual Box Box { get; set; }
        
        public int fk_ItemTypeId { get; set; }

        [ForeignKey(nameof(fk_ItemTypeId))]
        public virtual ItemType ItemType { get; set; }  
    }
}
