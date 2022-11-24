using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ef02.Model
{
    [Table("Products")]
    public class Product
    {
        [Key]
        public int ProductId { set; get; }

        [Required]
        [StringLength(50)]
        public string Name { set; get; }

        [Column(TypeName = "Money")]
        public decimal Price { set; get; }

        [Required]
        public int CategoryId { set; get; }
        public virtual Category Category { set; get; } // Thuộc tính tạo ra FK

        public int? CategorySecondId;
        [ForeignKey("CategorySecondId")]
        [InverseProperty("products")]
        public virtual Category SecondCategory { set; get; }
    }
}