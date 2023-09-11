using Microsoft.AspNetCore.Identity;
using WebAppCarClub.Data.Enum;
using WebAppCarClub.Models;
using static System.Net.WebRequestMethods;

namespace WebAppCarClub.Data
{
    public class Seed
    {
        public static void SeedData(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

                context.Database.EnsureCreated();

                // Clubs
                if (!context.Clubs.Any())
                {
                    context.Clubs.AddRange(new List<Club>()
                    {
                        new Club()
                        {
                            Title = "Road Rash 99",
                            Image = "https://www.pcarmarket.com/static/media/uploads/galleries/photos/uploads/galleries/21787-cole-white-gt500/.thumbnails/7EA2BB74-6FC0-4BD3-8D24-04DE388D420C.jpg/7EA2BB74-6FC0-4BD3-8D24-04DE388D420C-tiny-2048x0-0.5x0.jpg",
                            Description = "This is the description of the road rash 99 club",
                            ClubCategory = ClubCategory.City,
                            Address = new Address()
                            {
                                Street = "123 Main St",
                                City = "Charlotte",
                                State = "NC"
                            }
                         },
                        new Club()
                        {
                            Title = "Death Cruze",
                            Image = "https://media.ed.edmunds-media.com/mercedes-benz/glc-class/2023/oem/2023_mercedes-benz_glc-class_4dr-suv_glc-300_fq_oem_1_1600.jpg",
                            Description = "This is the description of the death cruze club",
                            ClubCategory = ClubCategory.Endurance,
                            Address = new Address()
                            {
                                Street = "123 Main St",
                                City = "Charlotte",
                                State = "NC"
                            }
                        },
                        new Club()
                        {
                            Title = "Blazing Cars",
                            Image = "https://cdn.motor1.com/images/mgl/40ePwz/s1/2023-bmw-7-series-rendering.jpg",
                            Description = "This is the description of the blazing cars club",
                            ClubCategory = ClubCategory.Trail,
                            Address = new Address()
                            {
                                Street = "123 Main St",
                                City = "Charlotte",
                                State = "NC"
                            }
                        },
                        new Club()
                        {
                            Title = "Burnout",
                            Image = "https://e1.pxfuel.com/desktop-wallpaper/301/493/desktop-wallpaper-hummer-car-2018-hummer.jpg",
                            Description = "This is the description of the burnout club",
                            ClubCategory = ClubCategory.City,
                            Address = new Address()
                            {
                                Street = "123 Main St",
                                City = "Michigan",
                                State = "NC"
                            }
                        }
                    });
                    context.SaveChanges();
                }

                // Races
                if (!context.Races.Any())
                {
                    context.Races.AddRange(new List<Race>()
                    {
                        new Race()
                        {
                            Title = "Speed Race 1",
                            Image = "https://www.pcarmarket.com/static/media/uploads/galleries/photos/uploads/galleries/21787-cole-white-gt500/.thumbnails/7EA2BB74-6FC0-4BD3-8D24-04DE388D420C.jpg/7EA2BB74-6FC0-4BD3-8D24-04DE388D420C-tiny-2048x0-0.5x0.jpg",
                            Description = "This is the description of the first race",
                            RaceCategory = RaceCategory.DragRacing,
                            Address = new Address()
                            {
                                Street = "123 Main St",
                                City = "Charlotte",
                                State = "NC"
                            }
                        },
                        new Race()
                        {
                            Title = "Speed Race 2",
                            Image = "https://www.pcarmarket.com/static/media/uploads/galleries/photos/uploads/galleries/21787-cole-white-gt500/.thumbnails/7EA2BB74-6FC0-4BD3-8D24-04DE388D420C.jpg/7EA2BB74-6FC0-4BD3-8D24-04DE388D420C-tiny-2048x0-0.5x0.jpg",
                            Description = "This is the description of the first race",
                            RaceCategory = RaceCategory.SprintCarRacing,
                            AddressId = 5,
                            Address = new Address()
                            {
                                Street = "123 Main St",
                                City = "Charlotte",
                                State = "NC"
                            }
                        }
                    });
                    context.SaveChanges();
                }
            }
        }

        // Comment task if IdentityUser is not implemented
        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                // Roles
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                if (!await roleManager.RoleExistsAsync(UserRoles.User))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

                // Users
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();

                string adminUserEmail = "krazier.eights@gmail.com";
                var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
                if (adminUser == null)
                {
                    var newAdminUser = new User()
                    {
                        UserName = "krazy-8",
                        Email = adminUserEmail,
                        EmailConfirmed = true,
                        Address = new Address()
                        {
                            Street = "123 main st",
                            City = "charlotte",
                            State = "nc"
                        }
                    };
                    await userManager.CreateAsync(newAdminUser, "Coding@1234?");
                    await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
                }

                string appUserEmail = "user@etickets.com";
                var appUser = await userManager.FindByEmailAsync(appUserEmail);
                if (appUser == null)
                {
                    var newUser = new User()
                    {
                        UserName = "app-user",
                        Email = appUserEmail,
                        EmailConfirmed = true,
                        Address = new Address()
                        {
                            Street = "123 main st",
                            City = "charlotte",
                            State = "nc"
                        }
                    };
                    await userManager.CreateAsync(newUser, "Coding@1234?");
                    await userManager.AddToRoleAsync(newUser, UserRoles.User);
                }
            }
        }
    }
}
