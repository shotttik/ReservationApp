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
                // seed multiple locations same count as a companies
                if (!await context.Locations.AnyAsync() || !await context.Companies.AnyAsync())

                {
                    string [] countries = { "USA", "Canada", "Germany", "France", "UK" };
                    string [] states = { "California", "Ontario", "Bavaria", "Île-de-France", "London" };
                    string [] cities = { "Los Angeles", "Toronto", "Munich", "Paris", "London" };
                    string [] streets = { "Main St", "2nd Ave", "Elm Rd", "Maple Blvd", "Oak St" };

                    var locations = new List<Location>();

                    for (int i = 1; i <= 50; i++)
                    {
                        var location = new Location
                        {
                            Country = countries [random.Next(countries.Length)],
                            State = states [random.Next(states.Length)],
                            City = cities [random.Next(cities.Length)],
                            AddressLine1 = $"{random.Next(1, 9999)} {streets [random.Next(streets.Length)]}",
                            PostalCode = random.Next(10000, 99999).ToString(),
                            Latitude = Convert.ToDecimal(random.NextDouble() * (90 - (-90)) + (-90)),
                            Longitude = Convert.ToDecimal(random.NextDouble() * (180 - (-180)) + (-180))
                        };

                        locations.Add(location);
                    }

                    await context.Locations.AddRangeAsync(locations);
                    await context.SaveChangesAsync(); // Save first to get IDs

                    // Seed multiple companies
                    var companies = new List<Company>();

                    for (int i = 0; i < 50; i++)
                    {
                        var name = $"Company {i + 1}";
                        var iN = random.Next(100000000, 999999999).ToString();
                        var email = $"company{i + 1}@example.com";
                        var phone = $"555-01{i:D3}";
                        var description = $"""
                            <p>Welcome to <strong>{name}</strong>, where <em>creativity</em> meets <u>technology</u>. Since our founding in <span style="color: #555;">2010</span>, we have delivered top-notch solutions to clients worldwide.</p>

                            <p>Our core values include:</p>

                            <ul>
                              <li>Integrity</li>
                              <li>Innovation</li>
                              <li>Customer Success</li>
                            </ul>

                            <blockquote>
                              “The best way to predict the future is to invent it.” — Alan Kay
                            </blockquote>

                            <p>We specialize in:</p>

                            <ol>
                              <li>Web Development</li>
                              <li>Mobile Applications</li>
                              <li>Cloud Solutions</li>
                            </ol>

                            <p>To learn more, visit our <a href="https://www.example.com" target="_blank">official website</a> or follow us on social media.</p>

                            <hr>

                            <h3>Contact Us</h3>
                            <p>📞 <a href="tel:+1234567890">{phone}</a><br>
                            📧 <a href="mailto:info@example.com">{email}</a></p>

                            <p>
                              <img src="https://via.placeholder.com/400x200" alt="Company Team Photo" style="max-width: 100%; border-radius: 8px;">
                            </p>

                            <p style="background-color: #f9f9f9; padding: 10px; border-left: 4px solid #00AF87;">
                              <strong>Note:</strong> We are committed to <mark>continuous improvement</mark> and welcome your feedback.
                            </p>

                            <p>
                              <small>Last updated: June 2025</small>
                            </p>
                            {i + 1}
                            """;
                        companies.Add(new Company
                        {
                            Name = name,
                            IN = iN,
                            Email = email,
                            Phone = phone,
                            Description = description,
                            IsActive = true,
                            LocationID = locations [i].ID // One-to-one
                        });
                    }

                    await context.Companies.AddRangeAsync(companies);
                    await context.SaveChangesAsync();

                    foreach (var company in companies)
                    {
                        if (!await context.Companies.AnyAsync(c => c.IN == company.IN))
                        {
                            context.Companies.Add(company);
                        }
                    }
                    await context.SaveChangesAsync();
                }
                await SeedMediasAsync(context);
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
                           },
                            new Service
                            {
                                 Name = "Graphic Design",
                                 Description = "Creative graphic design services",
                                 Duration = 15,
                                 Price = 800.00m,
                                 CompanyID = companyDict["555666777"] // Globex Ltd.  
                            },
                            new Service
                            {
                                 Name = "Content Writing",
                                 Description = "Professional content writing services",
                                 Duration = 15,
                                 Price = 500.00m,
                                 CompanyID = companyDict["112233445"] // Tech Innovators Inc.  
                            },
                            new Service
                            {
                                 Name = "Social Media Management",
                                 Description = "Management of social media accounts",
                                 Duration = 15,
                                 Price = 1200.00m,
                                 CompanyID = companyDict["998877665"] // Green Earth Solutions  
                            },
                            new Service
                            {
                                 Name = "Data Analysis",
                                 Description = "In-depth data analysis services",
                                 Duration = 15,
                                 Price = 2500.00m,
                                 CompanyID = companyDict["123456789"] // Default Company  
                            },
                            new Service
                            {
                                 Name = "Email Marketing",
                                 Description = "Targeted email marketing campaigns",
                                 Duration = 15,
                                 Price = 700.00m,
                                 CompanyID = companyDict["987654321"] // Acme Corporation  
                            },
                            new Service
                            {
                                 Name = "Brand Strategy",
                                 Description = "Comprehensive brand strategy development",
                                 Duration = 15,
                                 Price = 1800.00m,
                                 CompanyID = companyDict["555666777"] // Globex Ltd.  
                            },
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
        public static async Task SeedMediasAsync(ApplicationDbContext context)
        {
            var random = new Random();

            // Check if media already exists  
            if (!await context.Medias.AnyAsync())
            {
                var companies = await context.Companies.ToListAsync();
                var medias = new List<Media>();

                foreach (var company in companies)
                {
                    for (int i = 0; i < 5; i++) // Generate 5 media items per company  
                    {
                        var media = new Media
                        {
                            OriginalName = $"Media_{company.Name}_{i + 1}",
                            RemoteUrl = $"https://unsplash.it/1000/1000?nounce={Guid.NewGuid()}",
                            FileSizeInBytes = random.Next(1000, 5000), // Random file size in bytes  
                            FileType = "image/jpeg",
                            CompanyMedias = new List<CompanyMedia>
                                       {
                                           new CompanyMedia
                                           {
                                               CompanyID = company.ID,
                                               IsMain = i == 0 // Only the first media is marked as IsMain=true  
                                           }
                                       }
                        };

                        medias.Add(media);
                    }
                }

                await context.Medias.AddRangeAsync(medias);
                await context.SaveChangesAsync();
            }

        }
    }
}
