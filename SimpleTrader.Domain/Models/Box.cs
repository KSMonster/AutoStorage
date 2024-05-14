using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleTrader.Domain.Models
{
    public class Box : DomainObject
    {
        
        public ObservableCollection<Item> FilteredItems;

        public int BoxNumber { get; set; }
        public int BoxRFID { get; set; }
        public bool isFull { get; set; }

    }
}
