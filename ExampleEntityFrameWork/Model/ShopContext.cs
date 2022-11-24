using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using System.Text;

namespace ef02.Model
{
    public class ShopContext : DbContext
    {
        protected string connect_str = "Host=localhost; Database=postgres; Username=postgres; Password=123";
        public DbSet<Product> products { set; get; }
        public DbSet<Category> categories { set; get; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);


            optionsBuilder.UseNpgsql(connect_str).UseLazyLoadingProxies();

        }

        // Tạo database
        public async Task CreateDatabase()
        {
            String databasename = Database.GetDbConnection().Database;

            Console.WriteLine("Tạo " + databasename);
            bool result = await Database.EnsureCreatedAsync();
            string resultstring = result ? "tạo  thành  công" : "đã có trước đó";
            Console.WriteLine($"CSDL {databasename} : {resultstring}");
        }

        // Xóa Database
        public async Task DeleteDatabase()
        {
            String databasename = Database.GetDbConnection().Database;
            Console.Write($"Có chắc chắn xóa {databasename} (y) ? ");
            string input = Console.ReadLine();

            // // Hỏi lại cho chắc
            if (input.ToLower() == "y")
            {
                bool deleted = await Database.EnsureDeletedAsync();
                string deletionInfo = deleted ? "đã xóa" : "không xóa được";
                Console.WriteLine($"{databasename} {deletionInfo}");
            }
        }

        public async Task InsertSampleData()
        {
            var cate1 = new Category() { Name = "Cate1", Description = "Description1" };
            var cate2 = new Category() { Name = "Cate2", Description = "Description2" };
            await AddRangeAsync(cate1, cate2);
            await SaveChangesAsync();

            await AddRangeAsync(
                new Product() { Name = "Sản phẩm 1", Price = 12, Category = cate2 },
                new Product() { Name = "Sản phẩm 2", Price = 11, Category = cate2 },
                new Product() { Name = "Sản phẩm 3", Price = 33, Category = cate2 },
                new Product() { Name = "Sản phẩm 4(1)", Price = 323, Category = cate1 },
                new Product() { Name = "Sản phẩm 5(1)", Price = 333, Category = cate1 }

            );
            await SaveChangesAsync();
            foreach (var item in products)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append($"ID: {item.ProductId}");
                stringBuilder.Append($"tên: {item.Name}");
                stringBuilder.Append($"Danh mục {item.CategoryId}({item.Category.Name})");
                Console.WriteLine(stringBuilder);
            }

        }

        public async Task<Product> FindProduct(int id)
        {

            var p = await (from c in products where c.ProductId == id select c).FirstOrDefaultAsync();
            await Entry(p)                    // lấy DbEntityEntry liên quan đến p
                   .Reference(x => x.Category) // lấy tham chiếu, liên quan đến thuộc tính Category
                   .LoadAsync();               // nạp thuộc tính từ DB
            return p;
        }

        public async Task<Category> FindCategory(int id)
        {

            var cate = await (from c in categories where c.CategoryId == id select c).FirstOrDefaultAsync();
            await Entry(cate)                     // lấy DbEntityEntry liên quan đến p
                   .Collection(cc => cc.Products)  // lấy thuộc tính tập hợp, danh sách các sản phẩm
                   .LoadAsync();                   // nạp thuộc tính từ DB
            return cate;
        }
    }
}