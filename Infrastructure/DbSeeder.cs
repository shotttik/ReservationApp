using Application.Authentication;
using Domain.Entities;
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
                // Seed SuperAdmin User
                if (!await context.UserAccounts.AnyAsync(u => u.Role != null && u.Role.ID == 1))
                {

                    var superAdmin = new UserAccount
                    {
                        FirstName = "Super",
                        LastName = "Admin",
                        RoleID = Role.SuperAdmin.ID,

                    };
                    context.UserAccounts.Add(superAdmin);
                    await context.SaveChangesAsync();

                    // Create password hash and salt
                    var password = "SuperAdminPassword123!"; // Change this in production
                    (byte [] hash, byte [] salt) = PasswordHasher.HashPassword(password);

                    var superAdminLogin = new UserLoginData
                    {
                        Email = "superadmin@example.com",
                        UserAccountID = superAdmin.ID,
                        VerificationStatus = Domain.Enums.VerificationStatus.Verified, // Set as needed
                        PasswordHash = hash,
                        PasswordSalt = salt
                    };
                    context.UserLoginDatas.Add(superAdminLogin);
                    await context.SaveChangesAsync();
                }

                // Seed multiple companies
                if (!await context.Companies.AnyAsync())
                {

                    var companiesToSeed = new List<Company>
                {
                    new Company
                    {
                        Name = "Default Company",
                        IN = "123456789",
                        Email = "info@defaultcompany.com",
                        Phone = "123456789",
                        Description = "Seeded default company",
                        IsActive = true
                    },
                    new Company
                    {
                        Name = "Acme Corporation",
                        IN = "987654321",
                        Email = "contact@acme.com",
                        Phone = "987654321",
                        Description = "Acme's main office",
                        IsActive = true
                    },
                    new Company
                    {
                        Name = "Globex Ltd.",
                        IN = "555666777",
                        Email = "hello@globex.com",
                        Phone = "555666777",
                        Description = "Globex international",
                        IsActive = true
                    },
                   new Company
                   {
                       Name = "Tech Innovators Inc.",
                       IN = "112233445",
                       Email = "contact@techinnovators.com",
                       Phone = "112233445",
                       Description = "Leading technology solutions provider",
                       IsActive = true
                   },
                   new Company
                   {
                       Name = "Green Earth Solutions",
                       IN = "998877665",
                       Email = "info@greenearth.com",
                       Phone = "998877665",
                       Description = "Sustainable and eco-friendly services",
                       IsActive = true
                   }
                };

                    foreach (var company in companiesToSeed)
                    {
                        if (!await context.Companies.AnyAsync(c => c.IN == company.IN))
                        {
                            context.Companies.Add(company);
                        }
                    }
                    await context.SaveChangesAsync();
                }
                // Seed multiple services
                if (!await context.Services.AnyAsync())
                {
                    // After seeding companies, fetch their IDs by IN or Name  
                    var companyDict = await context.Companies
                        .Where(c => c.IN == "123456789" || c.IN == "987654321" || c.IN == "555666777" || c.IN == "112233445" || c.IN == "998877665")
                        .ToDictionaryAsync(c => c.IN, c => c.ID);

                    // Now use these IDs when seeding services  
                    var servicesToSeed = new List<Service>
                       {
                           new Service
                           {
                               Name = "Consultation",
                               Description = "One-on-one consultation service",
                               Duration = 15,
                               Price = 100.00m,
                               CompanyID = companyDict["123456789"] // Default Company  
                           },
                           new Service
                           {
                               Name = "Web Development",
                               Description = "Custom web development services",
                               Duration = 15,
                               Price = 5000.00m,
                               CompanyID = companyDict["987654321"] // Acme Corporation  
                           },
                           new Service
                           {
                               Name = "SEO Optimization",
                               Description = "Search engine optimization services",
                               Duration = 15,
                               Price = 1500.00m,
                               CompanyID = companyDict["555666777"] // Globex Ltd.  
                           },
                           new Service
                           {
                               Name = "Mobile App Development",
                               Description = "Development of mobile applications",
                               Duration = 15,
                               Price = 3000.00m,
                               CompanyID = companyDict["123456789"] // Default Company  
                           },
                           new Service
                           {
                               Name = "Digital Marketing",
                               Description = "Comprehensive digital marketing services",
                               Duration = 15,
                               Price = 2000.00m,
                               CompanyID = companyDict["987654321"] // Acme Corporation  
                           }
                       };

                    foreach (var service in servicesToSeed)
                    {
                        if (!await context.Services.AnyAsync(s => s.Name == service.Name && s.CompanyID == service.CompanyID))
                        {
                            context.Services.Add(service);
                        }
                    }

                    await context.SaveChangesAsync();
                }
                // check if work schedules doesn't exists then create
                if (!await context.WorkSchedules.AnyAsync())
                {
                    // After seeding companies, fetch their IDs by IN or Name  
                    var companyDict = await context.Companies
                        .Where(c => c.IN == "123456789" || c.IN == "987654321" || c.IN == "555666777" || c.IN == "112233445" || c.IN == "998877665")
                        .ToDictionaryAsync(c => c.IN, c => c.ID);
                    // seed 7 days of week work schedules for companies
                    var workSchedulesToSeed = new List<WorkSchedule>
                {
                   new WorkSchedule
                   {
                       DayOfWeek = DayOfWeek.Monday,
                       StartTime = TimeOnly.FromTimeSpan(TimeSpan.FromHours(9)),
                       EndTime = TimeOnly.FromTimeSpan(TimeSpan.FromHours(17)),
                       IsWorkingDay = true,
                       CompanyID = companyDict["123456789"]
                   },
                   new WorkSchedule
                   {
                       DayOfWeek = DayOfWeek.Tuesday,
                       StartTime = TimeOnly.FromTimeSpan(TimeSpan.FromHours(9)),
                       EndTime = TimeOnly.FromTimeSpan(TimeSpan.FromHours(17)),
                       IsWorkingDay = true,
                       CompanyID = companyDict["123456789"]
                   },
                   new WorkSchedule
                   {
                       DayOfWeek = DayOfWeek.Wednesday,
                       StartTime = TimeOnly.FromTimeSpan(TimeSpan.FromHours(9)),
                       EndTime = TimeOnly.FromTimeSpan(TimeSpan.FromHours(17)),
                       IsWorkingDay = true,
                       CompanyID = companyDict["123456789"]
                   },
                   new WorkSchedule
                   {
                       DayOfWeek = DayOfWeek.Thursday,
                       StartTime = TimeOnly.FromTimeSpan(TimeSpan.FromHours(9)),
                       EndTime = TimeOnly.FromTimeSpan(TimeSpan.FromHours(17)),
                       IsWorkingDay = true,
                       CompanyID = companyDict["123456789"]
                   },
                   new WorkSchedule
                   {
                       DayOfWeek = DayOfWeek.Friday,
                       StartTime = TimeOnly.FromTimeSpan(TimeSpan.FromHours(9)),
                       EndTime = TimeOnly.FromTimeSpan(TimeSpan.FromHours(17)),
                       IsWorkingDay = true,
                       CompanyID = companyDict["123456789"]
                   },
                   new WorkSchedule
                   {
                       DayOfWeek = DayOfWeek.Saturday,
                       StartTime = TimeOnly.FromTimeSpan(TimeSpan.FromHours(10)),
                       EndTime = TimeOnly.FromTimeSpan(TimeSpan.FromHours(14)),
                       IsWorkingDay = true,
                       CompanyID = companyDict["123456789"]
                   },
                   new WorkSchedule
                   {
                       DayOfWeek = DayOfWeek.Sunday,
                       IsWorkingDay = false,
                       CompanyID = companyDict["123456789"]
                   }, 
                   // repeat for one more company
                     new WorkSchedule
                     {
                          DayOfWeek = DayOfWeek.Monday,
                          StartTime = TimeOnly.FromTimeSpan(TimeSpan.FromHours(9)),
                          EndTime = TimeOnly.FromTimeSpan(TimeSpan.FromHours(17)),
                          IsWorkingDay = true,
                          CompanyID = companyDict["987654321"]
                     },
                        new WorkSchedule
                        {
                            DayOfWeek = DayOfWeek.Tuesday,
                            StartTime = TimeOnly.FromTimeSpan(TimeSpan.FromHours(9)),
                            EndTime = TimeOnly.FromTimeSpan(TimeSpan.FromHours(17)),
                            IsWorkingDay = true,
                            CompanyID = companyDict["987654321"]
                        },
                        new WorkSchedule
                        {
                            DayOfWeek = DayOfWeek.Wednesday,
                            StartTime = TimeOnly.FromTimeSpan(TimeSpan.FromHours(9)),
                            EndTime = TimeOnly.FromTimeSpan(TimeSpan.FromHours(17)),
                            IsWorkingDay = true,
                            CompanyID = companyDict["987654321"]
                        },
                        new WorkSchedule
                        {
                            DayOfWeek = DayOfWeek.Thursday,
                            StartTime = TimeOnly.FromTimeSpan(TimeSpan.FromHours(9)),
                            EndTime = TimeOnly.FromTimeSpan(TimeSpan.FromHours(17)),
                            IsWorkingDay = true,
                            CompanyID = companyDict["987654321"]
                        },
                        new WorkSchedule
                        {
                            DayOfWeek = DayOfWeek.Friday,
                            StartTime = TimeOnly.FromTimeSpan(TimeSpan.FromHours(9)),
                            EndTime = TimeOnly.FromTimeSpan(TimeSpan.FromHours(17)),
                            IsWorkingDay = true,
                            CompanyID = companyDict["987654321"]
                        },
                        new WorkSchedule
                        {
                            DayOfWeek = DayOfWeek.Saturday,
                            StartTime = TimeOnly.FromTimeSpan(TimeSpan.FromHours(10)),
                            EndTime = TimeOnly.FromTimeSpan(TimeSpan.FromHours(14)),
                            IsWorkingDay = true,
                            CompanyID = companyDict["987654321"]
                        },
                        new WorkSchedule
                        {
                            DayOfWeek = DayOfWeek.Sunday,
                            IsWorkingDay = false,
                            CompanyID = companyDict["987654321"]
                        }
                };
                    foreach (var workSchedule in workSchedulesToSeed)
                    {
                        if (!await context.WorkSchedules.AnyAsync())
                        {
                            context.WorkSchedules.Add(workSchedule);
                        }
                    }
                    await context.SaveChangesAsync();

                }
                // seed basic users with role publicuser and company member
                if (!await context.UserAccounts.AnyAsync(u => u.Role != null && u.Role.ID == Role.PublicUser.ID))
                {

                    var usersToSeed = new List<UserAccount>
                {
                    new UserAccount
                    {
                        FirstName = "John",
                        LastName = "Doe",
                        RoleID = Role.PublicUser.ID,
                        CompanyID = 1
                    },
                    new UserAccount
                    {
                        FirstName = "Jane",
                        LastName = "Smith",
                        RoleID = Role.PublicUser.ID,
                        CompanyID = 2
                    },
                    new UserAccount
                    {
                        FirstName = "Alice",
                        LastName = "Johnson",
                        RoleID = Role.PublicUser.ID,
                        CompanyID = 3
                    },
                    // seed with role CompanyMember a few users
                    new UserAccount
                    {
                        FirstName = "Bob",
                        LastName = "Brown",
                        RoleID = Role.CompanyMember.ID,
                        CompanyID = 1
                    },
                    new UserAccount
                    {
                        FirstName = "Charlie",
                        LastName = "Davis",
                        RoleID = Role.CompanyMember.ID,
                        CompanyID = 2
                    },
                    new UserAccount
                    {
                        FirstName = "Eve",
                        LastName = "Wilson",
                        RoleID = Role.CompanyMember.ID,
                        CompanyID = 3
                    },
                    new UserAccount
                    {
                        FirstName = "Frank",
                        LastName = "Garcia",
                        RoleID = Role.CompanyMember.ID,
                        CompanyID = 1
                    },
                };
                    foreach (var user in usersToSeed)
                    {
                        if (!await context.UserAccounts.AnyAsync(u => u.FirstName == user.FirstName && u.LastName == user.LastName && u.CompanyID == user.CompanyID))
                        {
                            context.UserAccounts.Add(user);
                        }
                    }
                    await context.SaveChangesAsync();

                }

                Debug.Write("Database seeding completed successfully.");
            }
            catch (Exception ex)
            {
                Debug.Write(ex, "An error occurred while seeding the database.");
            }
        }
    }
}
