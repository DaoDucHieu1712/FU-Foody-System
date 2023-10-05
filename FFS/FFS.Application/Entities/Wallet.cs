using FFS.Application.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace FFS.Application.Entities
{
    [Table("Wallet")]
    public class Wallet : BaseEntity<int>
    { 
        public decimal Amount { get; set; }
        public string BankName { get; set; }
        public string STK { get; set; }
    }
}
