using AssetManagement.Core.Entities;
using AssetManagement.Core.Enums;
using AssetManagement.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetManagement.Api.SeedData
{
    public class DbInitializer
    {
        public static async Task InitDb(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;

            var userManager = services.GetRequiredService<UserManager<Account>>()
                ?? throw new InvalidOperationException("Failed to retrieve user manager");
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>()
                ?? throw new InvalidOperationException("Failed to retrieve role manager");
            var dbContext = services.GetRequiredService<AppDbContext>()
                ?? throw new InvalidOperationException("Failed to retrieve db context");

            await SeedRoles(roleManager);
            await SeedUsersAndAccounts(userManager, dbContext);
            await SeedCategories(dbContext);
            await SeedAssets(dbContext);
            await SeedAssignments(dbContext);
            await SeedReturningRequests(dbContext);
        }

        private static async Task SeedRoles(RoleManager<IdentityRole<Guid>> roleManager)
        {
            if (!roleManager.Roles.Any())
            {
                foreach (var role in Enum.GetValues<ERole>())
                {
                    if (!await roleManager.RoleExistsAsync(role.ToString()))
                    {
                        await roleManager.CreateAsync(new IdentityRole<Guid>(role.ToString()));
                    }
                }
            }
        }

        private static async Task SeedUsersAndAccounts(UserManager<Account> userManager, AppDbContext dbContext)
        {
            if (!await dbContext.Staffs.AnyAsync())
            {
                var password = "Password123!";
                var userDataList = new List<(User User, ERole Role)>
                {
                    (
                        new User
                        {
                            StaffCode = "SC001",
                            UserName = "john.doe",
                            FirstName = "John",
                            LastName = "Doe",
                            DateOfBirth = new DateTime(1990, 5, 15),
                            JoinedDate = new DateTime(2023, 1, 10),
                            Gender = true,
                            Type = ERole.Admin,
                            Location = ELocation.HN,
                            IsDisabled = false
                        },
                        ERole.Admin
                    ),
                    (
                        new User
                        {
                            StaffCode = "SC002",
                            UserName = "jane.smith",
                            FirstName = "Jane",
                            LastName = "Smith",
                            DateOfBirth = new DateTime(1988, 11, 22),
                            JoinedDate = new DateTime(2022, 6, 15),
                            Gender = false,
                            Type = ERole.Staff,
                            Location = ELocation.HCM,
                            IsDisabled = false
                        },
                        ERole.Staff
                    ),
                    (
                        new User
                        {
                            StaffCode = "SC003",
                            UserName = "alice.johnson",
                            FirstName = "Alice",
                            LastName = "Johnson",
                            DateOfBirth = new DateTime(1995, 3, 8),
                            JoinedDate = new DateTime(2024, 2, 20),
                            Gender = false,
                            Type = ERole.Staff,
                            Location = ELocation.HN,
                            IsDisabled = true
                        },
                        ERole.Staff
                    )
                };

                foreach (var (user, role) in userDataList)
                {
                    await dbContext.Staffs.AddAsync(user);
                    await dbContext.SaveChangesAsync();

                    var account = new Account
                    {
                        Id = Guid.NewGuid(),
                        UserName = user.UserName,
                        StaffCode = user.StaffCode,
                        CreatedDate = DateTime.UtcNow,
                        User = user
                    };

                    var result = await userManager.CreateAsync(account, password);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(account, role.ToString());
                    }
                    else
                    {
                        dbContext.Staffs.Remove(user);
                        await dbContext.SaveChangesAsync();
                        throw new InvalidOperationException($"Failed to create account for {user.UserName}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    }
                }
            }
        }

        private static async Task SeedCategories(AppDbContext dbContext)
        {
            if (!await dbContext.Categories.AnyAsync())
            {
                var categories = new List<Category>
                {
                    new Category
                    {
                        Prefix = "ELEC",
                        CategoryName = "Electronics",
                        Total = 50
                    },
                    new Category
                    {
                        Prefix = "FURN",
                        CategoryName = "Furniture",
                        Total = 20
                    }
                };

                await dbContext.Categories.AddRangeAsync(categories);
                await dbContext.SaveChangesAsync();
            }
        }

        private static async Task SeedAssets(AppDbContext dbContext)
        {
            if (!await dbContext.Assets.AnyAsync())
            {
                var electronicsCategory = await dbContext.Categories.FirstOrDefaultAsync(c => c.Prefix == "ELEC");
                var furnitureCategory = await dbContext.Categories.FirstOrDefaultAsync(c => c.Prefix == "FURN");

                if (electronicsCategory == null || furnitureCategory == null)
                {
                    throw new InvalidOperationException("Categories must be seeded before assets.");
                }

                var assets = new List<Asset>
                {
                    new Asset
                    {
                        AssetCode = "A001",
                        AssetName = "Laptop Dell XPS 15",
                        Specification = "Intel Core i7, 16GB RAM, 512GB SSD",
                        State = AssetStatus.Available,
                        Location = ELocation.HCM,
                        InstalledDate = new DateTime(2023, 5, 10),
                        CategoryId = electronicsCategory.Id
                    },
                    new Asset
                    {
                        AssetCode = "A002",
                        AssetName = "HP Laser Printer",
                        Specification = "Color LaserJet Pro, Wi-Fi enabled",
                        State = AssetStatus.Available,
                        Location = ELocation.HN,
                        InstalledDate = new DateTime(2022, 8, 15),
                        CategoryId = electronicsCategory.Id
                    },
                    new Asset
                    {
                        AssetCode = "A003",
                        AssetName = "Ergonomic Office Chair",
                        Specification = "Mesh back, adjustable armrests",
                        State = AssetStatus.Available,
                        Location = ELocation.DN,
                        InstalledDate = new DateTime(2020, 3, 5),
                        CategoryId = furnitureCategory.Id
                    },
                    new Asset
                    {
                        AssetCode = "A004",
                        AssetName = "Samsung 24-inch Monitor",
                        Specification = "Full HD, 75Hz, HDMI & DisplayPort",
                        State = AssetStatus.Available,
                        Location = ELocation.HCM,
                        InstalledDate = new DateTime(2021, 11, 20),
                        CategoryId = electronicsCategory.Id
                    },
                    new Asset
                    {
                        AssetCode = "A005",
                        AssetName = "Mechanical Keyboard - Keychron K8",
                        Specification = "Bluetooth/Wired, RGB Backlight, Brown Switches",
                        State = AssetStatus.Available,
                        Location = ELocation.HN,
                        InstalledDate = new DateTime(2023, 1, 15),
                        CategoryId = electronicsCategory.Id
                    },
                    new Asset
                    {
                        AssetCode = "A006",
                        AssetName = "Sony Noise-Canceling Headphones",
                        Specification = "Over-ear, Bluetooth, ANC, 30-hour battery life",
                        State = AssetStatus.Available,
                        Location = ELocation.DN,
                        InstalledDate = new DateTime(2022, 6, 5),
                        CategoryId = electronicsCategory.Id
                    }
                };

                await dbContext.Assets.AddRangeAsync(assets);
                await dbContext.SaveChangesAsync();
            }
        }

        private static async Task SeedAssignments(AppDbContext dbContext)
        {
            if (!await dbContext.Assignments.AnyAsync())
            {
                var assignments = new List<Assignment>
                {
                    new Assignment
                    {
                        AssetCode = "A001",
                        AssignedTo = "SC002", // Jane Smith
                        AssignedBy = "SC001", // John Doe (Admin)
                        AssignedDate = new DateTime(2023, 10, 15),
                        State = AssignmentStatus.Accepted,
                        Note = "Assigned for development work"
                    },
                    new Assignment
                    {
                        AssetCode = "A002",
                        AssignedTo = "SC003", // Alice Johnson
                        AssignedBy = "SC001", // John Doe (Admin)
                        AssignedDate = new DateTime(2024, 2, 20),
                        State = AssignmentStatus.WaitForAcceptance,
                        Note = "Pending acceptance"
                    },
                    new Assignment
                    {
                        AssetCode = "A003",
                        AssignedTo = "SC002", // Jane Smith
                        AssignedBy = "SC001", // John Doe (Admin)
                        AssignedDate = new DateTime(2023, 12, 10),
                        State = AssignmentStatus.Accepted,
                        Note = "Assigned for testing"
                    },
                    new Assignment
                    {
                        AssetCode = "A001",
                        AssignedTo = "SC002", // Jane Smith
                        AssignedBy = "SC001", // John Doe (Admin)
                        AssignedDate = new DateTime(2023, 10, 15),
                        State = AssignmentStatus.Accepted,
                        Note = "Assigned for development work"
                    },
                };

                await dbContext.Assignments.AddRangeAsync(assignments);
                await dbContext.SaveChangesAsync();
            }
        }

        private static async Task SeedReturningRequests(AppDbContext dbContext)
        {
            if (!await dbContext.ReturnRequests.AnyAsync())
            {
                var assignments = await dbContext.Assignments.ToListAsync();
                var returningRequests = new List<ReturningRequest>
                {
                    new ReturningRequest
                    {
                        AssignmentId = assignments.FirstOrDefault(a => a.AssetCode == "A001")?.Id
                            ?? throw new InvalidOperationException("Assignment for A001 not found"),
                        RequestedBy = "SC002", // Jane Smith
                        AcceptedBy = "SC001", // John Doe (Admin)
                        State = ReturningRequestStatus.Completed,
                        ReturnedDate = new DateTime(2024, 1, 16)
                    },
                    new ReturningRequest
                    {
                        AssignmentId = assignments.FirstOrDefault(a => a.AssetCode == "A003")?.Id
                            ?? throw new InvalidOperationException("Assignment for A003 not found"),
                        RequestedBy = "SC002", // Jane Smith
                        AcceptedBy = null,
                        State = ReturningRequestStatus.WaitForReturning,
                        ReturnedDate = null
                    }
                };

                await dbContext.ReturnRequests.AddRangeAsync(returningRequests);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}