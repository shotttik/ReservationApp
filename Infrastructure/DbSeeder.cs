using Application.Authentication;
using Domain.Entities.Common;
using Domain.Entities.CompanyReleated;
using Domain.Entities.LocationReleated;
using Domain.Entities.User;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Infrastructure
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            try
            {
                var random = new Random();

                // 1) Users that don't depend on companies
                await EnsureSuperAdminAsync(context);
                await EnsurePublicUserAsync(context);

                // 2) Base Locations + Companies together (keep 1:1)
                if (!await context.Locations.AnyAsync() && !await context.Companies.AnyAsync())
                {
                    string [] countries = { "USA", "Canada", "Germany", "France", "UK" };
                    string [] states = { "California", "Ontario", "Bavaria", "Île-de-France", "London" };
                    string [] cities = { "Los Angeles", "Toronto", "Munich", "Paris", "London" };
                    string [] streets = { "Main St", "2nd Ave", "Elm Rd", "Maple Blvd", "Oak St" };

                    var locations = new List<Location>(50);
                    for (int i = 0; i < 50; i++)
                    {
                        locations.Add(new Location
                        {
                            Country = countries [random.Next(countries.Length)],
                            State = states [random.Next(states.Length)],
                            City = cities [random.Next(cities.Length)],
                            AddressLine1 = $"{random.Next(1, 9999)} {streets [random.Next(streets.Length)]}",
                            PostalCode = random.Next(10000, 99999).ToString(),
                            Latitude = Convert.ToDecimal(random.NextDouble() * 180 - 90),
                            Longitude = Convert.ToDecimal(random.NextDouble() * 360 - 180)
                        });
                    }

                    await context.Locations.AddRangeAsync(locations);
                    await context.SaveChangesAsync();

                    var companies = new List<Company>(50);
                    for (int i = 0; i < 50; i++)
                    {
                        var name = $"Company {i + 1}";
                        var iN = random.Next(100_000_000, 999_999_999).ToString();
                        var email = $"company{i + 1}@example.com";
                        var phone = $"555-01{i:D3}";
                        var description = $"""
                            <p>Welcome to <strong>{name}</strong>, where <em>creativity</em> meets <u>technology</u>.</p>
                            <ul><li>Integrity</li><li>Innovation</li><li>Customer Success</li></ul>
                            <blockquote>“The best way to predict the future is to invent it.” — Alan Kay</blockquote>
                            {i + 1}
                            """;

                        companies.Add(new Company
                        {
                            Name = name,
                            IN = iN,
                            Email = email,
                            Phone = phone,
                            Description = description,
                            ActiveStatus = Domain.Enums.ActiveStatus.Active,
                            LocationID = locations [i].ID
                        });
                    }

                    await context.Companies.AddRangeAsync(companies);
                    await context.SaveChangesAsync();
                }

                // 3) Ensure the special company exists (before users/services/media)
                var viehe = await EnsureVieheCompanyAsync(context);

                // 4) Company users (depend on Viehe)
                var companyAdmin = await EnsureCompanyUserAsync(context, viehe?.ID, Role.CompanyAdmin.ID,
                                        "companyAdmin@example.com", "CompanyAdmin", "Company Admin");
                var companyEmployee = await EnsureCompanyUserAsync(context, viehe?.ID, Role.CompanyEmployee.ID,
                                        "companyEmployee@example.com", "CompanyEmployee", "Company Employee");

                // 5) Services (attach to actual company IDs)
                await EnsureServicesAsync(context);

                // 6) Work schedules (after users; per-user/per-day)
                if (companyAdmin?.UserAccount != null)
                    await EnsureDefaultScheduleAsync(context, companyAdmin.UserAccount.ID);

                if (companyEmployee?.UserAccount != null)
                    await EnsureDefaultScheduleAsync(context, companyEmployee.UserAccount.ID);

                // 7) Media last (so every company is present)
                await SeedMediaAsync(context);

                Debug.WriteLine("Database seeding completed successfully.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred while seeding the database: {ex}");
            }
        }

        private static async Task EnsureSuperAdminAsync(ApplicationDbContext context)
        {
            if (!await context.UserAccounts.AnyAsync(u => u.RoleID == Role.SuperAdmin.ID))
            {
                var user = new UserAccount
                {
                    FirstName = "Super",
                    LastName = "Admin",
                    RoleID = Role.SuperAdmin.ID
                };
                var (hash, salt) = PasswordHasher.HashPassword("SuperAdminPassword123!"); // change in prod

                context.UserLoginDatas.Add(new UserLoginData
                {
                    Email = "superadmin@example.com",
                    UserAccount = user,
                    VerificationStatus = Domain.Enums.VerificationStatus.Verified,
                    PasswordHash = hash,
                    PasswordSalt = salt
                });
                await context.SaveChangesAsync();
            }
        }

        private static async Task EnsurePublicUserAsync(ApplicationDbContext context)
        {
            if (!await context.UserAccounts.AnyAsync(u => u.RoleID == Role.PublicUser.ID))
            {
                var user = new UserAccount
                {
                    FirstName = "PublicUser",
                    LastName = "PublicUser",
                    RoleID = Role.PublicUser.ID
                };
                var (hash, salt) = PasswordHasher.HashPassword("SuperAdminPassword123!"); // change in prod

                context.UserLoginDatas.Add(new UserLoginData
                {
                    Email = "publicUser@example.com",
                    UserAccount = user,
                    VerificationStatus = Domain.Enums.VerificationStatus.Verified,
                    PasswordHash = hash,
                    PasswordSalt = salt
                });
                await context.SaveChangesAsync();
            }
        }

        private static async Task<UserLoginData?> EnsureCompanyUserAsync(
            ApplicationDbContext context,
            int? companyId,
            int roleId,
            string email,
            string firstName,
            string lastName)
        {
            var existing = await context.UserLoginDatas
                .Include(x => x.UserAccount)
                .FirstOrDefaultAsync(x => x.Email == email);
            if (existing != null) return existing;

            var (hash, salt) = PasswordHasher.HashPassword("SuperAdminPassword123!"); // change in prod

            var account = new UserAccount
            {
                FirstName = firstName,
                LastName = lastName,
                RoleID = roleId,
                CompanyID = companyId
            };

            var login = new UserLoginData
            {
                Email = email,
                UserAccount = account,
                VerificationStatus = Domain.Enums.VerificationStatus.Verified,
                PasswordHash = hash,
                PasswordSalt = salt
            };

            context.UserLoginDatas.Add(login);
            await context.SaveChangesAsync();
            return login;
        }

        private static async Task EnsureDefaultScheduleAsync(ApplicationDbContext context, int userAccountId)
        {
            var days = new []
            {
                DayOfWeek.Monday,
                DayOfWeek.Wednesday,
                DayOfWeek.Thursday,
                DayOfWeek.Friday,
                DayOfWeek.Saturday
            };

            foreach (var day in days)
            {
                var exists = await context.WorkSchedules.AnyAsync(ws =>
                    ws.UserAccountID == userAccountId && ws.DayOfWeek == day);

                if (!exists)
                {
                    context.WorkSchedules.Add(new WorkSchedule
                    {
                        UserAccountID = userAccountId,
                        DayOfWeek = day,
                        StartTime = TimeOnly.FromTimeSpan(TimeSpan.FromHours(9)),
                        EndTime = TimeOnly.FromTimeSpan(TimeSpan.FromHours(17)),
                    });
                }
            }

            await context.SaveChangesAsync();
        }

        private static async Task<Company?> EnsureVieheCompanyAsync(ApplicationDbContext context)
        {
            var existing = await context.Companies.FirstOrDefaultAsync(c => c.Email == "vieheCorporation@example.com");
            if (existing != null) return existing;

            var random = new Random();
            var company = new Company
            {
                Name = "Viehe corporation",
                IN = random.Next(100_000_000, 999_999_999).ToString(),
                Email = "vieheCorporation@example.com",
                Phone = "555-0112-1231",
                Description = "<p>Welcome to <strong>Viehe corporation</strong>...</p>",
                ActiveStatus = Domain.Enums.ActiveStatus.Active,
                Location = new Location
                {
                    Country = "Georgia",
                    State = "Shida-Kartli",
                    City = "Kaspi",
                    AddressLine1 = $"{random.Next(1, 9999)} avtandilis nomeri kucha",
                    PostalCode = random.Next(10000, 99999).ToString(),
                    Latitude = Convert.ToDecimal(41.7855048),
                    Longitude = Convert.ToDecimal(44.7529183)
                },
                Services =
                [
                    new Service { Name = "Consultation",     Description = "One-on-one consultation service",   Duration = 15, Price = 100.00m  },
                    new Service { Name = "Web Development",  Description = "Custom web development services",   Duration = 15, Price = 5000.00m },
                    new Service { Name = "SEO Optimization", Description = "Search engine optimization services",Duration = 15, Price = 1500.00m },
                ]
            };

            context.Companies.Add(company);
            await context.SaveChangesAsync();
            return company;
        }

        private static async Task EnsureServicesAsync(ApplicationDbContext context)
        {
            if (await context.Services.AnyAsync())
            {
                return;
            }

            var companyIds = await context.Companies
                .OrderBy(c => c.ID)
                .Select(c => c.ID)
                .Take(5)
                .ToListAsync();

            if (companyIds.Count == 0) return;

            int c1 = companyIds [0];
            int c2 = companyIds.Count > 1 ? companyIds [1] : c1;
            int c3 = companyIds.Count > 2 ? companyIds [2] : c1;
            int c4 = companyIds.Count > 3 ? companyIds [3] : c1;
            int c5 = companyIds.Count > 4 ? companyIds [4] : c1;

            var servicesToSeed = new List<Service>
            {
                new() { Name = "Consultation", CompanyID = c1, Description = "...", Duration = 15, Price = 100.00m },
                new() { Name = "Web Development", CompanyID = c2, Description = "...", Duration = 15, Price = 5000.00m },
                new() { Name = "SEO Optimization", CompanyID = c3, Description = "...", Duration = 15, Price = 1500.00m },
                new() { Name = "Mobile App Development", CompanyID = c1, Description = "...", Duration = 15, Price = 3000.00m },
                new() { Name = "Digital Marketing", CompanyID = c2, Description = "...", Duration = 15, Price = 2000.00m },
                new() { Name = "Graphic Design", CompanyID = c3, Description = "...", Duration = 15, Price = 800.00m },
                new() { Name = "Content Writing", CompanyID = c4, Description = "...", Duration = 15, Price = 500.00m },
                new() { Name = "Social Media Management", CompanyID = c5, Description = "...", Duration = 15, Price = 1200.00m },
                new() { Name = "Data Analysis", CompanyID = c1, Description = "...", Duration = 15, Price = 2500.00m },
                new() { Name = "Email Marketing", CompanyID = c2, Description = "...", Duration = 15, Price = 700.00m },
                new() { Name = "Brand Strategy", CompanyID = c3, Description = "...", Duration = 15, Price = 1800.00m }
            };

            context.Services.AddRange(servicesToSeed);
            await context.SaveChangesAsync();
        }

        public static async Task SeedMediaAsync(ApplicationDbContext context)
        {
            if (await context.Media.AnyAsync()) return;

            var random = new Random();
            var companies = await context.Companies.ToListAsync();
            if (companies.Count == 0) return;

            var media = new List<Media>();
            foreach (var company in companies)
            {
                for (int i = 0; i < 5; i++)
                {
                    media.Add(new Media
                    {
                        OriginalName = $"Media_{company.Name}_{i + 1}",
                        RemoteUrl = $"https://unsplash.it/1000/1000?nounce={Guid.NewGuid()}",
                        FileSizeInBytes = random.Next(1000, 5000),
                        FileType = "image/jpeg",
                        CompanyMedia = new List<CompanyMedia>
                        {
                            new CompanyMedia
                            {
                                CompanyID = company.ID,
                                IsMain    = i == 0
                            }
                        }
                    });
                }
            }

            await context.Media.AddRangeAsync(media);
            await context.SaveChangesAsync();
        }
    }
}
