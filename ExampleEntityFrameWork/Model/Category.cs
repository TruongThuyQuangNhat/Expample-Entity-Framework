using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ef02.Model
{
    [Table("Category")]
    public class Category
    {
        [Key]
        public int CategoryId { set; get; }

        [StringLength(100)]
        public string Name { set; get; }

        [Column(TypeName = "TEXT")]
        public string Description { set; get; }

        public virtual List<Product> Products { set; get; }
    }
}
