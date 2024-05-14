using System.ComponentModel.DataAnnotations;

namespace SimpleTrader.Domain.Models {
    public class DomainObject {
        [Key]
        public int Id { get; set; }
    }
}
