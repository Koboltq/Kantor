using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Models
{
    public class Wallet
    {
        [Display(Name = "Id: ")]
        public int Id { get; set; }
        [Display(Name = "Ilosc portfela: ")]
        [RegularExpression(@"^\d+.\d{0,2}$")]
        [Range(0, 9999999999999999.99)]
        public decimal Quantity { get; set; }
        [Display(Name = "Kurs: ")]
        public string Currency { get; set; }
        public string UserId { get; set; }
        [NotMapped]
        public virtual decimal Money { get; set; }
        [NotMapped]
        public virtual BuyAndSell BuyAndSell { get; set; }
        [NotMapped]
        public virtual Currency CurrencyCantor { get; set; }
    }

    public class BuyAndSell
    {
        [Display(Name = "Kup: ")]
        [RegularExpression(@"^\d+.\d{0,2}$")]
        [Range(0, 9999999999999999.99)]
        public decimal BuyQuantity { get; set; }
        [Display(Name = "Sprzedaj: ")]
        [RegularExpression(@"^\d+.\d{0,2}$")]
        [Range(0, 9999999999999999.99)]
        public decimal SellQuantity { get; set; }
    }
}
