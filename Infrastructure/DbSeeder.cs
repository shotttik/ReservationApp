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
                       },
                       new Company
                       {
                           Name = "Blue Ocean Enterprises",
                           IN = "111222333",
                           Email = "contact@blueocean.com",
                           Phone = "111222333",
                           Description = "Innovative marine solutions",
                           IsActive = true
                       },
                       new Company
                       {
                           Name = "Skyline Ventures",
                           IN = "444555666",
                           Email = "info@skylineventures.com",
                           Phone = "444555666",
                           Description = "Real estate and investment services",
                           IsActive = true
                       },
                       new Company
                       {
                           Name = "NextGen Robotics",
                           IN = "777888999",
                           Email = "support@nextgenrobotics.com",
                           Phone = "777888999",
                           Description = "Advanced robotics and AI solutions",
                           IsActive = true
                       },
                       new Company
                       {
                           Name = "EcoBuild Solutions",
                           IN = "123123123",
                           Email = "contact@ecobuild.com",
                           Phone = "123123123",
                           Description = "Sustainable construction materials",
                           IsActive = true
                       },
                       new Company
                       {
                           Name = "HealthFirst Clinics",
                           IN = "456456456",
                           Email = "info@healthfirst.com",
                           Phone = "456456456",
                           Description = "Comprehensive healthcare services",
                           IsActive = true
                       },
                       new Company
                       {
                           Name = "BrightFuture Education",
                           IN = "789789789",
                           Email = "support@brightfuture.com",
                           Phone = "789789789",
                           Description = "Educational resources and training",
                           IsActive = true
                       },
                       new Company
                       {
                           Name = "Global Freight Logistics",
                           IN = "321321321",
                           Email = "info@globalfreight.com",
                           Phone = "321321321",
                           Description = "Worldwide shipping and logistics",
                           IsActive = true
                       },
                       new Company
                       {
                           Name = "TechWave Solutions",
                           IN = "654654654",
                           Email = "contact@techwave.com",
                           Phone = "654654654",
                           Description = "IT consulting and software development",
                           IsActive = true
                       },
                       new Company
                       {
                           Name = "GreenTech Innovations",
                           IN = "987987987",
                           Email = "info@greentech.com",
                           Phone = "987987987",
                           Description = "Eco-friendly technology solutions",
                           IsActive = true
                       },
                       new Company
                       {
                           Name = "UrbanStyle Interiors",
                           IN = "147147147",
                           Email = "support@urbanstyle.com",
                           Phone = "147147147",
                           Description = "Modern interior design services",
                           IsActive = true
                       },
                       new Company
                       {
                           Name = "Apex Financial Group",
                           IN = "258258258",
                           Email = "info@apexfinancial.com",
                           Phone = "258258258",
                           Description = "Financial planning and investment",
                           IsActive = true
                       },
                       new Company
                       {
                           Name = "SolarBright Energy",
                           IN = "369369369",
                           Email = "contact@solarbright.com",
                           Phone = "369369369",
                           Description = "Renewable energy solutions",
                           IsActive = true
                       },
                       new Company
                       {
                           Name = "Elite Fitness Centers",
                           IN = "741741741",
                           Email = "info@elitefitness.com",
                           Phone = "741741741",
                           Description = "State-of-the-art fitness facilities",
                           IsActive = true
                       },
                       new Company
                       {
                           Name = "PrimeTech Systems",
                           IN = "852852852",
                           Email = "support@primetech.com",
                           Phone = "852852852",
                           Description = "Advanced IT infrastructure solutions",
                           IsActive = true
                       },
                       new Company
                       {
                           Name = "FreshHarvest Organics",
                           IN = "963963963",
                           Email = "info@freshharvest.com",
                           Phone = "963963963",
                           Description = "Organic food and produce",
                           IsActive = true
                       },
                       new Company
                       {
                           Name = "Visionary Media Group",
                           IN = "159159159",
                           Email = "contact@visionarymedia.com",
                           Phone = "159159159",
                           Description = "Creative media and advertising",
                           IsActive = true
                       },
                       new Company
                       {
                           Name = "Pinnacle Engineering",
                           IN = "753753753",
                           Email = "info@pinnacleengineering.com",
                           Phone = "753753753",
                           Description = "Innovative engineering solutions",
                           IsActive = true
                       },
                       new Company
                       {
                           Name = "CarePlus Pharmaceuticals",
                           IN = "951951951",
                           Email = "support@careplus.com",
                           Phone = "951951951",
                           Description = "Quality pharmaceutical products",
                           IsActive = true
                       },
                       new Company
                       {
                           Name = "Summit Travel Agency",
                           IN = "357357357",
                           Email = "info@summittravel.com",
                           Phone = "357357357",
                           Description = "Personalized travel planning",
                           IsActive = true
                       },
                       new Company
                       {
                           Name = "Infinity Software Solutions",
                           IN = "753159753",
                           Email = "contact@infinitysoftware.com",
                           Phone = "753159753",
                           Description = "Custom software development",
                           IsActive = true
                       },
                       new Company
                       {
                           Name = "EcoLife Products",
                           IN = "951753951",
                           Email = "info@ecolife.com",
                           Phone = "951753951",
                           Description = "Sustainable lifestyle products",
                           IsActive = true
                       },
                       new Company
                       {
                           Name = "BrightPath Education",
                           IN = "357951357",
                           Email = "support@brightpath.com",
                           Phone = "357951357",
                           Description = "Innovative educational tools",
                           IsActive = true
                       },
                       new Company
                       {
                           Name = "GlobalTech Solutions",
                           IN = "159753159",
                           Email = "info@globaltech.com",
                           Phone = "159753159",
                           Description = "Comprehensive IT services",
                           IsActive = true
                       },
                       new Company
                       {
                           Name = "UrbanGreen Landscaping",
                           IN = "753951753",
                           Email = "contact@urbangreen.com",
                           Phone = "753951753",
                           Description = "Eco-friendly landscaping services",
                           IsActive = true
                       },
                       new Company
                       {
                           Name = "NextLevel Marketing",
                           IN = "951357951",
                           Email = "info@nextlevel.com",
                           Phone = "951357951",
                           Description = "Cutting-edge marketing strategies",
                           IsActive = true
                       },
                       new Company
                       {
                           Name = "SkyHigh Aviation",
                           IN = "357159357",
                           Email = "support@skyhigh.com",
                           Phone = "357159357",
                           Description = "Private and commercial aviation services",
                           IsActive = true
                       },
                       new Company
                       {
                           Name = "PureWater Solutions",
                           IN = "159357159",
                           Email = "info@purewater.com",
                           Phone = "159357159",
                           Description = "Clean water technologies",
                           IsActive = true
                       },
new Company
                      {
                          Name = "Quantum Dynamics",
                          IN = "111111111",
                          Email = "info@quantumdynamics.com",
                          Phone = "111111111",
                          Description = "Advanced quantum computing solutions",
                          IsActive = true
                      },
                      new Company
                      {
                          Name = "EcoSmart Technologies",
                          IN = "222222222",
                          Email = "contact@ecosmart.com",
                          Phone = "222222222",
                          Description = "Sustainable technology innovations",
                          IsActive = true
                      },
                      new Company
                      {
                          Name = "FutureVision Studios",
                          IN = "333333333",
                          Email = "hello@futurevision.com",
                          Phone = "333333333",
                          Description = "Creative media and animation",
                          IsActive = true
                      },
                      new Company
                      {
                          Name = "AeroSpace Innovations",
                          IN = "444444444",
                          Email = "info@aerospace.com",
                          Phone = "444444444",
                          Description = "Cutting-edge aerospace technology",
                          IsActive = true
                      },
                      new Company
                      {
                          Name = "BioHealth Solutions",
                          IN = "555555555",
                          Email = "support@biohealth.com",
                          Phone = "555555555",
                          Description = "Innovative healthcare products",
                          IsActive = true
                      },
                      new Company
                      {
                          Name = "UrbanTech Labs",
                          IN = "666666666",
                          Email = "contact@urbantech.com",
                          Phone = "666666666",
                          Description = "Smart city technology solutions",
                          IsActive = true
                      },
                      new Company
                      {
                          Name = "AgriFuture Innovations",
                          IN = "777777777",
                          Email = "info@agrifuture.com",
                          Phone = "777777777",
                          Description = "Modern agricultural technologies",
                          IsActive = true
                      },
                      new Company
                      {
                          Name = "Oceanic Ventures",
                          IN = "888888888",
                          Email = "hello@oceanicventures.com",
                          Phone = "888888888",
                          Description = "Marine exploration and research",
                          IsActive = true
                      },
                      new Company
                      {
                          Name = "NextEra Energy",
                          IN = "999999999",
                          Email = "contact@nextera.com",
                          Phone = "999999999",
                          Description = "Renewable energy solutions",
                          IsActive = true
                      },
                      new Company
                      {
                          Name = "Alpha Robotics",
                          IN = "101010101",
                          Email = "info@alpharobotics.com",
                          Phone = "101010101",
                          Description = "Advanced robotics and automation",
                          IsActive = true
                      },
                      new Company
                      {
                          Name = "SmartHome Systems",
                          IN = "202020202",
                          Email = "support@smarthome.com",
                          Phone = "202020202",
                          Description = "Home automation and IoT solutions",
                          IsActive = true
                      },
                      new Company
                      {
                          Name = "GreenPlanet Initiatives",
                          IN = "303030303",
                          Email = "info@greenplanet.com",
                          Phone = "303030303",
                          Description = "Environmental conservation projects",
                          IsActive = true
                      },
                      new Company
                      {
                          Name = "CyberShield Security",
                          IN = "404040404",
                          Email = "contact@cybershield.com",
                          Phone = "404040404",
                          Description = "Cybersecurity and data protection",
                          IsActive = true
                      },
                      new Company
                      {
                          Name = "EduTech Innovations",
                          IN = "505050505",
                          Email = "hello@edutech.com",
                          Phone = "505050505",
                          Description = "Educational technology solutions",
                          IsActive = true
                      },
                      new Company
                      {
                          Name = "NanoTech Solutions",
                          IN = "606060606",
                          Email = "info@nanotech.com",
                          Phone = "606060606",
                          Description = "Nanotechnology research and development",
                          IsActive = true
                      },
                      new Company
                      {
                          Name = "AquaPure Systems",
                          IN = "707070707",
                          Email = "support@aquapure.com",
                          Phone = "707070707",
                          Description = "Water purification technologies",
                          IsActive = true
                      },
                      new Company
                      {
                          Name = "SolarEdge Innovations",
                          IN = "808080808",
                          Email = "contact@solaredge.com",
                          Phone = "808080808",
                          Description = "Solar energy solutions",
                          IsActive = true
                      },
                      new Company
                      {
                          Name = "MedTech Solutions",
                          IN = "909090909",
                          Email = "info@medtech.com",
                          Phone = "909090909",
                          Description = "Medical technology advancements",
                          IsActive = true
                      },
                      new Company
                      {
                          Name = "Infinity Logistics",
                          IN = "121212121",
                          Email = "hello@infinitylogistics.com",
                          Phone = "121212121",
                          Description = "Global logistics and supply chain",
                          IsActive = true
                      },
                      new Company
                      {
                          Name = "BrightFuture Labs",
                          IN = "131313131",
                          Email = "support@brightfuturelabs.com",
                          Phone = "131313131",
                          Description = "Innovative research and development",
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
    }
}
