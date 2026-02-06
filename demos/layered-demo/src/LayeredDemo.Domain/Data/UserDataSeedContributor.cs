using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Uow;

namespace LayeredDemo.Data;

public class UserDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly IdentityUserManager _userManager;
    private readonly IdentityRoleManager _roleManager;
    private readonly IGuidGenerator _guidGenerator;
    private readonly IPermissionManager _permissionManager;
    private readonly ILogger<UserDataSeedContributor> _logger;

    public UserDataSeedContributor(
        IdentityUserManager userManager,
        IdentityRoleManager roleManager,
        IGuidGenerator guidGenerator,
        IPermissionManager permissionManager,
        ILogger<UserDataSeedContributor> logger)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _guidGenerator = guidGenerator;
        _permissionManager = permissionManager;
        _logger = logger;
    }

    [UnitOfWork]
    public virtual async Task SeedAsync(DataSeedContext context)
    {
        await CreateUserAsync("alice", "Alice1234*", "alice@layereddemo.com", isAdmin: true);
        await CreateUserAsync("bob", "Bob12345*", "bob@layereddemo.com", isAdmin: false);
        await CreateUserAsync("charlie", "Charlie1*", "charlie@layereddemo.com", isAdmin: false);

        // Grant Todo permissions to all authenticated users via role
        await GrantTodoPermissionsToRoleAsync("admin");
    }

    private async Task CreateUserAsync(string userName, string password, string email, bool isAdmin)
    {
        var existingUser = await _userManager.FindByNameAsync(userName);
        if (existingUser != null)
        {
            _logger.LogInformation("User '{UserName}' already exists, skipping.", userName);
            return;
        }

        var user = new IdentityUser(
            _guidGenerator.Create(),
            userName,
            email
        );

        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            _logger.LogWarning(
                "Could not create user '{UserName}': {Errors}",
                userName,
                string.Join(", ", result.Errors));
            return;
        }

        _logger.LogInformation("Created user '{UserName}'.", userName);

        if (isAdmin)
        {
            var roleExists = await _roleManager.RoleExistsAsync("admin");
            if (roleExists)
            {
                await _userManager.AddToRoleAsync(user, "admin");
                _logger.LogInformation("Assigned role 'admin' to user '{UserName}'.", userName);
            }
        }

        // Grant Todo permissions directly to the user
        await GrantTodoPermissionsToUserAsync(user);
    }

    private static readonly string[] TodoPermissions =
    [
        "LayeredDemo.Todos",
        "LayeredDemo.Todos.Create",
        "LayeredDemo.Todos.Edit",
        "LayeredDemo.Todos.Delete"
    ];

    private async Task GrantTodoPermissionsToUserAsync(IdentityUser user)
    {
        var permissions = TodoPermissions;

        foreach (var permission in permissions)
        {
            await _permissionManager.SetForUserAsync(user.Id, permission, true);
        }
    }

    private async Task GrantTodoPermissionsToRoleAsync(string roleName)
    {
        var permissions = TodoPermissions;

        foreach (var permission in permissions)
        {
            await _permissionManager.SetForRoleAsync(roleName, permission, true);
        }
    }
}
