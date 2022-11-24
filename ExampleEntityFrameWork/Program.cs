using ef02.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ExampleEntityFrameWork
{
    class Program
    {
        static async Task Main(string[] args)
        {
            ShopContext context = new ShopContext();

            await context.CreateDatabase();
        }
    }
}
