using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleTrader.Domain.Models
{
    public class Log : DomainObject
    {
        public string Operation { get; set; }
        public DateTime Date { get; set; }

    }
}
