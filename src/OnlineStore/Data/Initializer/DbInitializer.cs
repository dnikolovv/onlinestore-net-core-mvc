namespace OnlineStore.Data.Initializer
{
    using System.Linq;
    using Infrastructure.Constants;
    using Infrastructure.Util;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Models;

    public class DbInitializer
    {
        public const string ADMIN_DEFAULT_USERNAME = "Admin";
        public const string ADMIN_DEFAULT_PASSWORD = "Secret123$";

        public static void InitializeDb(IApplicationBuilder app)
        {
            PopulateDb(app);
            SeedIdentityData(app);
        }

        private static void PopulateDb(IApplicationBuilder app)
        {
            ApplicationDbContext context = app.ApplicationServices
                .GetRequiredService<ApplicationDbContext>();

            if (!context.Products.Any() && !context.Categories.Any())
            {
                Category[] categories = {
                    new Category() { Name = "Bags" },
                    new Category() { Name = "Parfumes" },
                    new Category() { Name = "Jewelery" },
                    new Category() { Name = "Watches" },
                    new Category() { Name = "Shoes" },
                };

                context.Categories.AddRange(categories);
                context.SaveChanges();

                Category bagsCategory = context.Categories.FirstOrDefault(c => c.Name == "Bags");

                var bags = new Product[]
                {
                    new Product
                    {
                        Name = "Black bag",
                        Description = "An amazing black bag.",
                        Category = bagsCategory,
                        Price = 275.00m,
                        ImagePath = "~/images/ba.jpg"
                    },
                    new Product
                    {
                        Name = "White bag",
                        Description = "An amazing white bag.",
                        Category = bagsCategory,
                        Price = 224.00m,
                        ImagePath = "~/images/pi1.jpg"
                    },
                    new Product
                    {
                        Name = "Backpack",
                        Description = "An amazing backpack.",
                        Category = bagsCategory,
                        Price = 453.00m,
                        ImagePath = "~/images/bag.jpg"
                    },
                    new Product
                    {
                        Name = "Brown bag",
                        Description = "An amazing brown bag.",
                        Category = bagsCategory,
                        Price = 134.00m,
                        ImagePath = "~/images/baa.jpg"
                    },
                    new Product
                    {
                        Name = "Leather bag",
                        Description = "An amazing leather bag.",
                        Category = bagsCategory,
                        Price = 275.00m,
                        ImagePath = "~/images/bag1.jpg"
                    }
                };

                Category parfumesCategory = context.Categories.FirstOrDefault(c => c.Name == "Parfumes");

                var parfumes = new Product[]
                {
                    new Product
                    {
                        Name = "Blue parfume",
                        Description = "An amazing blue parfume.",
                        Category = parfumesCategory,
                        Price = 100.00m,
                        ImagePath = "~/images/bo.jpg"
                    },
                    new Product
                    {
                        Name = "Calvin Klein parfume",
                        Description = "The best Calvin Klein parfume.",
                        Category = parfumesCategory,
                        Price = 180.00m,
                        ImagePath = "~/images/bott.jpg"
                    },
                    new Product
                    {
                        Name = "Parfume bottle",
                        Description = "An amazing parfume bottle.",
                        Category = parfumesCategory,
                        Price = 77.90m,
                        ImagePath = "~/images/bottle.jpg"
                    }
                };

                Category jeweleryCategory = context.Categories.FirstOrDefault(c => c.Name == "Jewelery");

                var jewelery = new Product[]
                {
                    new Product
                    {
                        Name = "Blue crystal necklace",
                        Description = "An amazing blue crystal necklace.",
                        Category = jeweleryCategory,
                        Price = 90.90m,
                        ImagePath = "~/images/ch.jpg"
                    },
                    new Product
                    {
                        Name = "Note necklace",
                        Description = "An amazing note necklace.",
                        Category = jeweleryCategory,
                        Price = 180.90m,
                        ImagePath = "~/images/pi3.jpg"
                    },
                    new Product
                    {
                        Name = "White crystal necklace",
                        Description = "An amazing white crystal necklace.",
                        Category = jeweleryCategory,
                        Price = 109.90m,
                        ImagePath = "~/images/pi.jpg"
                    }
                };

                Category watchesCategory = context.Categories.FirstOrDefault(c => c.Name == "Watches");

                var watches = new Product[]
                {
                    new Product
                    {
                        Name = "Golden women's watch",
                        Description = "An amazing golden women's watch.",
                        Category = watchesCategory,
                        Price = 129.90m,
                        ImagePath = "~/images/pi2.jpg"
                    },
                    new Product
                    {
                        Name = "Black watch",
                        Description = "An amazing black watch.",
                        Category = watchesCategory,
                        Price = 149.90m,
                        ImagePath = "~/images/pi4.jpg"
                    },
                    new Product
                    {
                        Name = "Blue watch",
                        Description = "An amazing blue watch.",
                        Category = watchesCategory,
                        Price = 118.00m,
                        ImagePath = "~/images/pic2.jpg"
                    }
                };

                Category shoesCategory = context.Categories.FirstOrDefault(c => c.Name == "Shoes");

                var shoes = new Product[]
                {
                    new Product
                    {
                        Name = "Pink women's shoes",
                        Description = "Amazing pink women's shoes.",
                        Category = shoesCategory,
                        Price = 128.00m,
                        ImagePath = "~/images/pic12.jpg"
                    },
                    new Product
                    {
                        Name = "Black heels",
                        Description = "Amazing black heels.",
                        Category = shoesCategory,
                        Price = 107.00m,
                        ImagePath = "~/images/pic3.jpg"
                    },
                    new Product
                    {
                        Name = "Blue sneakers",
                        Description = "Astonishing blue sneakers.",
                        Category = shoesCategory,
                        Price = 59.99m,
                        ImagePath = "~/images/sh.jpg"
                    }
                };

                context.Products.AddRange(ArrayHelper.ConcatArrays(bags, parfumes, jewelery, watches, shoes));
            }
        }

        private static async void SeedIdentityData(IApplicationBuilder app)
        {
            UserManager<User> userManager = app.ApplicationServices
                .GetRequiredService<UserManager<User>>();

            User user = await userManager.FindByNameAsync("Admin");

            if (user == null)
            {
                ApplicationDbContext context = app.ApplicationServices
                    .GetRequiredService<ApplicationDbContext>();

                RoleManager<UserRole> roleManager = app.ApplicationServices
                    .GetRequiredService<RoleManager<UserRole>>();
                
                user = new User() { UserName = ADMIN_DEFAULT_USERNAME, FirstName = "JohnTheAdmin", Cart = new Cart() };
                await userManager.CreateAsync(user, ADMIN_DEFAULT_PASSWORD);

                user.Cart.User = user;
                user.Cart.UserId = user.Id;

                if (!(await roleManager.RoleExistsAsync(Roles.ADMIN_ROLE)))
                {
                    UserRole adminRole = new UserRole()
                    {
                        Name = Roles.ADMIN_ROLE
                    };

                    await roleManager.CreateAsync(adminRole);
                }

                await userManager.AddToRolesAsync(user, new[] { Roles.ADMIN_ROLE });
            }
        }
    }
}