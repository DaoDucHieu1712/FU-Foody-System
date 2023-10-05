using FFS.Application.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace FFS.Application.Entities
{
    [Table("Payment")]
    public class Payment : BaseEntity<int>
    {
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public string TransactionCode { get; set; }
        public int Status { get; set; }

    }
}
