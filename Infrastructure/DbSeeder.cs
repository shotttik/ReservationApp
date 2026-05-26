using Application.Authentication;
using Domain.Entities.BranchReleated;
using Domain.Entities.Common;
using Domain.Entities.CompanyReleated;
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
                // EnsureGeorgiaLocationSeeded
                await EnsureGeorgiaLocationSeeded(context);
                // Subscription
                await EnsureSubscriptionPlansExists(context);
                // 2) Base Branches + Companies together (keep 1:1)
                if (!await context.Branches.AnyAsync() && !await context.Companies.AnyAsync())
                {
                    var freeSubscriptionPlan = await context.SubscriptionPlans.Where(e => e.Name == "FREE").FirstAsync();
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
                        var companySubscription = new CompanySubscription()
                        {
                            SubscriptionPlan = freeSubscriptionPlan,
                            StartDate = DateTime.Now,
                            EndDate = DateTime.Now.AddMonths(12),
                            Status = Domain.Enums.SubscriptionStatus.Active,
                            AutoRenew = false
                        };
                        companies.Add(new Company
                        {
                            Name = name,
                            IN = iN,
                            Email = email,
                            Phone = phone,
                            Description = description,
                            ActiveStatus = Domain.Enums.ActiveStatus.Active,
                            Subscription = companySubscription
                        });
                    }

                    await context.Companies.AddRangeAsync(companies);
                    await context.SaveChangesAsync();

                    string [] countries = { "USA", "Canada", "Germany", "France", "UK" };
                    string [] states = { "California", "Ontario", "Bavaria", "Île-de-France", "London" };
                    string [] cities = { "Los Angeles", "Toronto", "Munich", "Paris", "London" };
                    string [] streets = { "Main St", "2nd Ave", "Elm Rd", "Maple Blvd", "Oak St" };

                    var branches = new List<Branch>(50);
                    for (int i = 0; i < 50; i++)
                    {
                        branches.Add(new Branch
                        {
                            Country = countries [random.Next(countries.Length)],
                            State = states [random.Next(states.Length)],
                            City = cities [random.Next(cities.Length)],
                            AddressLine1 = $"{random.Next(1, 9999)} {streets [random.Next(streets.Length)]}",
                            PostalCode = random.Next(10000, 99999).ToString(),
                            Latitude = Convert.ToDecimal(random.NextDouble() * 180 - 90),
                            Longitude = Convert.ToDecimal(random.NextDouble() * 360 - 180),
                            CompanyId = companies [i].Id
                        });
                    }

                    await context.Branches.AddRangeAsync(branches);
                    await context.SaveChangesAsync();

                }

                // 3) Ensure the special company exists (before users/services/media)
                var viehe = await EnsureVieheCompanyAsync(context);

                // 4) Company users (depend on Viehe)
                var companyAdmin = await EnsureCompanyUserAsync(context, viehe?.Id, Role.CompanyAdmin.ID, null,
                                        "companyAdmin@example.com", "CompanyAdmin", "Company Admin");
                var companyEmployee = await EnsureCompanyUserAsync(context, viehe?.Id, Role.CompanyEmployee.ID, viehe?.Branches.FirstOrDefault()?.Id,
                                        "companyEmployee@example.com", "CompanyEmployee", "Company Employee");

                // 5) Services (attach to actual company IDs)
                await EnsureServicesAsync(context);
                // 6) Ensure Employee Services exists that attached to company
                await EnsureEmployeeServicesExists(context);
                // 7) Work schedules (after users; per-user/per-day)
                if (companyAdmin?.UserAccount != null)
                    await EnsureDefaultScheduleAsync(context, companyAdmin.UserAccount.Id);

                if (companyEmployee?.UserAccount != null)
                    await EnsureDefaultScheduleAsync(context, companyEmployee.UserAccount.Id);

                // 8) Media last (so every company is present)
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
                    EmailVerificationStatus = Domain.Enums.VerificationStatus.Verified,
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
                    EmailVerificationStatus = Domain.Enums.VerificationStatus.Verified,
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
            int? branchId,
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
                CompanyID = companyId,
                BranchId = branchId
            };
            var login = new UserLoginData
            {
                Email = email,
                UserAccount = account,
                EmailVerificationStatus = Domain.Enums.VerificationStatus.Verified,
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
            var enterpriseSubscriptionPlan = await context.SubscriptionPlans.Where(e => e.Name == "ENTERPRISE").FirstAsync();
            var companySubscription = new CompanySubscription()
            {
                SubscriptionPlan = enterpriseSubscriptionPlan,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(12),
                Status = Domain.Enums.SubscriptionStatus.Active,
                AutoRenew = false
            };

            var random = new Random();
            var company = new Company
            {
                Name = "Viehe corporation",
                IN = random.Next(100_000_000, 999_999_999).ToString(),
                Email = "vieheCorporation@example.com",
                Phone = "555-0112-1231",
                Description = "<p>Welcome to <strong>Viehe corporation</strong>...</p>",
                ActiveStatus = Domain.Enums.ActiveStatus.Active,
                Branches = [new Branch
                {
                    Country = "Georgia",
                    State = "Shida-Kartli",
                    City = "Kaspi",
                    AddressLine1 = $"{random.Next(1, 9999)} avtandilis nomeri kucha",
                    PostalCode = random.Next(10000, 99999).ToString(),
                    Latitude = Convert.ToDecimal(41.7855048),
                    Longitude = Convert.ToDecimal(44.7529183)
                }],
                Services =
                [
                    new Service { Name = "Consultation",     Description = "One-on-one consultation service",   Duration = 15, Price = 100.00m  },
                    new Service { Name = "Web Development",  Description = "Custom web development services",   Duration = 15, Price = 5000.00m },
                    new Service { Name = "SEO Optimization", Description = "Search engine optimization services",Duration = 15, Price = 1500.00m },
                ],
                Subscription = companySubscription
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
                .OrderBy(c => c.Id)
                .Select(c => c.Id)
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
                new() { Name = "Web Development", CompanyID = c2, Description = "...", Duration = 30, Price = 5000.00m },
                new() { Name = "SEO Optimization", CompanyID = c3, Description = "...", Duration = 45, Price = 1500.00m },
                new() { Name = "Mobile App Development", CompanyID = c1, Description = "...", Duration = 60, Price = 3000.00m },
                new() { Name = "Digital Marketing", CompanyID = c2, Description = "...", Duration = 15, Price = 2000.00m },
                new() { Name = "Graphic Design", CompanyID = c3, Description = "...", Duration = 120, Price = 800.00m },
                new() { Name = "Content Writing", CompanyID = c4, Description = "...", Duration = 10, Price = 500.00m },
                new() { Name = "Social Media Management", CompanyID = c5, Description = "...", Duration = 5, Price = 1200.00m },
                new() { Name = "Data Analysis", CompanyID = c1, Description = "...", Duration = 25, Price = 2500.00m },
                new() { Name = "Email Marketing", CompanyID = c2, Description = "...", Duration = 35, Price = 700.00m },
                new() { Name = "Brand Strategy", CompanyID = c3, Description = "...", Duration = 55, Price = 1800.00m }
            };

            context.Services.AddRange(servicesToSeed);
            await context.SaveChangesAsync();
        }
        private static async Task EnsureEmployeeServicesExists(ApplicationDbContext context)
        {
            var employee = await context.UserAccounts.Where(e => e.Role == Role.CompanyEmployee).FirstOrDefaultAsync();
            if (employee != null)
            {
                var comapnyServices = await context.Services.Where(e => e.CompanyID == employee.CompanyID).ToArrayAsync();
                if (comapnyServices != null)
                {
                    foreach (var service in comapnyServices)
                    {
                        employee.EmployeeServices.Add(new EmployeeService() { EmployeeId = employee.Id, ServiceId = service.Id });
                    }
                }
            }
            await context.SaveChangesAsync();

        }
        private static async Task EnsureSubscriptionPlansExists(ApplicationDbContext context)
        {
            if (await context.SubscriptionPlans.AnyAsync())
            {
                return;
            }
            var free = new SubscriptionPlan()
            {
                Name = "FREE",
                PriceMonthly = 0,
                MaxEmployees = 1,
                MaxBookingsPerMonth = 20,
                MaxBranches = 1
            };
            var basic = new SubscriptionPlan()
            {
                Name = "BASIC",
                PriceMonthly = 25,
                MaxEmployees = 2,
                MaxBookingsPerMonth = 999,
                MaxBranches = 1
            };
            var pro = new SubscriptionPlan()
            {
                Name = "Pro",
                PriceMonthly = 50,
                MaxEmployees = 10,
                MaxBookingsPerMonth = 9999,
                MaxBranches = 5
            };
            var enterprise = new SubscriptionPlan()
            {
                Name = "ENTERPRISE",
                PriceMonthly = 100,
                MaxEmployees = 100,
                MaxBookingsPerMonth = 9999999,
                MaxBranches = 10
            };
            context.AddRange([free, basic, pro, enterprise]);
            await context.SaveChangesAsync();
        }
        private static async Task EnsureGeorgiaLocationSeeded(ApplicationDbContext context)
        {
            if (await context.Countries.AnyAsync(c => c.Iso2 == "GE"))
            {
                return;
            }

            var now = DateTime.UtcNow;

            var georgia = new Country
            {
                Name = "Georgia",
                Iso2 = "GE",
                Iso3 = "GEO",
                NumericCode = "268",
                PhoneCode = "995",
                Capital = "Tbilisi",
                Currency = "GEL",
                CurrencyName = "Georgian Lari",
                CurrencySymbol = "₾",
                Tld = ".ge",
                Native = "საქართველო",
                Region = "Asia",
                Subregion = "Western Asia",
                Nationality = "Georgian",
                Latitude = 42.3154m,
                Longitude = 43.3569m,
                Emoji = "🇬🇪",
                EmojiU = "U+1F1EC U+1F1EA",
                Flag = true,
                CreatedAt = now
            };

            context.Countries.Add(georgia);
            await context.SaveChangesAsync();

            var tbilisiState = new State
            {
                Name = "Tbilisi",
                CountryId = georgia.Id,
                CountryCode = georgia.Iso2!,
                CountryName = georgia.Name,
                StateCode = "TB",
                Type = "City",
                Latitude = 41.7151m,
                Longitude = 44.8271m,
                CreatedAt = now
            };

            var adjaraState = new State
            {
                Name = "Adjara",
                CountryId = georgia.Id,
                CountryCode = georgia.Iso2!,
                CountryName = georgia.Name,
                StateCode = "AJ",
                Type = "Autonomous Republic",
                Latitude = 41.6006m,
                Longitude = 42.0688m,
                CreatedAt = now
            };

            var imeretiState = new State
            {
                Name = "Imereti",
                CountryId = georgia.Id,
                CountryCode = georgia.Iso2!,
                CountryName = georgia.Name,
                StateCode = "IM",
                Type = "Region",
                Latitude = 42.2301m,
                Longitude = 42.9000m,
                CreatedAt = now
            };

            context.States.AddRange(tbilisiState, adjaraState, imeretiState);
            await context.SaveChangesAsync();

            var tbilisiCity = new City
            {
                Name = "Tbilisi",
                StateId = tbilisiState.Id,
                StateCode = tbilisiState.StateCode!,
                StateName = tbilisiState.Name,
                CountryId = georgia.Id,
                CountryCode = georgia.Iso2,
                CountryName = georgia.Name,
                Latitude = 41.7151m,
                Longitude = 44.8271m,
                Flag = true,
                CreatedAt = now
            };

            var batumiCity = new City
            {
                Name = "Batumi",
                StateId = adjaraState.Id,
                StateCode = adjaraState.StateCode!,
                StateName = adjaraState.Name,
                CountryId = georgia.Id,
                CountryCode = georgia.Iso2,
                CountryName = georgia.Name,
                Latitude = 41.6168m,
                Longitude = 41.6367m,
                Flag = true,
                CreatedAt = now
            };

            var kutaisiCity = new City
            {
                Name = "Kutaisi",
                StateId = imeretiState.Id,
                StateCode = imeretiState.StateCode!,
                StateName = imeretiState.Name,
                CountryId = georgia.Id,
                CountryCode = georgia.Iso2,
                CountryName = georgia.Name,
                Latitude = 42.2662m,
                Longitude = 42.7180m,
                Flag = true,
                CreatedAt = now
            };

            context.Cities.AddRange(tbilisiCity, batumiCity, kutaisiCity);

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
                        OriginalUrl = $"https://unsplash.it/1000/1000?nounce={Guid.NewGuid()}",
                        FileSizeInBytes = random.Next(1000, 5000),
                        FileType = "image/jpeg",
                        CompanyMedia = new List<CompanyMedia>
                        {
                            new CompanyMedia
                            {
                                CompanyID = company.Id,
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
