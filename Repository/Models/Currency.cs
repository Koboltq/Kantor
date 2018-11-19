using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Repository.Models
{
    public class Currency
    {
        [Display(Name="Id: ")]
        public int Id { get; set; }
        [Display(Name= "Nazwa kursu: ")]
        [MaxLength(3)]
        public string Name { get; set; }
        [Display(Name = "Ilość waluty w kantorze: ")]
        [RegularExpression(@"^\d+.\d{0,2}$")]
        [Range(0, 9999999999999999.99)]
        public decimal Quantity { get; set; }
        [Display(Name = "Kursy zakupu: ")]
        public float PurchasePrice { get; set; }
        [Display(Name = "Kurs sprzedaży: ")]
        public float SalesPrice { get; set; }
        [Display(Name = "Data dodania: ")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime AddedDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime? UpdatedDate { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }

    }
}
