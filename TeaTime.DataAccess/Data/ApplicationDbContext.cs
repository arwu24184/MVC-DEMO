using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using TeaTimeDemo.Models;

namespace TeaTimeDemo.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public
        ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}//撰寫C#處傳給dbcontext
        public DbSet<Category> Categories { get; set; }
        //新增產品集合
        public DbSet<Product> Products { get; set; }

        //新增分店集合
        public DbSet<Store> Stores { get; set; }

        //新增購物車集合
        public DbSet<ShoppingCart> ShoppingCarts {  get; set; }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>().HasData(
                   new Category { Id = 1, Name = "果汁", DisplayOrder = 1 },
                   new Category { Id = 2, Name = "茶", DisplayOrder = 2 },
                   new Category { Id = 3, Name = "咖啡", DisplayOrder = 3 }
                   );
            //產品預設資料seed
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "台灣水果茶",
                    Size = "大杯",
                    Description = "天然果飲，迷人多變。",
                    Price = 60,
                    CategoryId = 1,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 2,
                    Name = "鐵觀音",
                    Size = "中杯",
                    Description = "品鐵觀音，享人生的味道。",
                    Price = 35,
                    CategoryId = 2,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 3,
                    Name = "美式咖啡",
                    Size = "中杯",
                    Description = "用咖啡體悟悠閒時光。",
                    Price = 50,
                    CategoryId = 3,
                    ImageUrl = ""
                }
                );
            modelBuilder.Entity<Store>().HasData(
                new Store 
                {
                    Id = 1,
                    Name = "台北信義店",
                    Address = "台北市信義區忠孝東路65號",
                    City = "台北市",
                    PhoneNumber = "0912345678",
                    Description = "好貴好貴的店租。"
                },
                new Store 
                {
                    Id = 2,
                    Name = "台北大安店",
                    Address = "台北市大安區大安路一段11號",
                    City = "台北市",
                    PhoneNumber = "0922222222",
                    Description = "熱鬧哦。"
                },
                new Store 
                {
                    Id = 3,
                    Name = "新北建中店",
                    Address = "台北市新莊區建中街989號",
                    City = "新北市",
                    PhoneNumber = "0933333333",
                    Description = "便宜的地方。"
                }
                );
        }
    }
}
