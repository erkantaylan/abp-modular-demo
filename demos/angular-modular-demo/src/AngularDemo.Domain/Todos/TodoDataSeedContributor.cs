using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Uow;

namespace AngularDemo.Todos;

public class TodoDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly IIdentityUserRepository _userRepository;
    private readonly IIdentityRoleRepository _roleRepository;
    private readonly IdentityUserManager _userManager;
    private readonly IdentityRoleManager _roleManager;
    private readonly IPermissionManager _permissionManager;
    private readonly IRepository<Todo, Guid> _todoRepository;
    private readonly IGuidGenerator _guidGenerator;

    public TodoDataSeedContributor(
        IIdentityUserRepository userRepository,
        IIdentityRoleRepository roleRepository,
        IdentityUserManager userManager,
        IdentityRoleManager roleManager,
        IPermissionManager permissionManager,
        IRepository<Todo, Guid> todoRepository,
        IGuidGenerator guidGenerator)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _userManager = userManager;
        _roleManager = roleManager;
        _permissionManager = permissionManager;
        _todoRepository = todoRepository;
        _guidGenerator = guidGenerator;
    }

    [UnitOfWork]
    public async Task SeedAsync(DataSeedContext context)
    {
        await SeedRolesAsync();
        await SeedUsersAsync();
        await SeedTodosAsync();
    }

    private async Task SeedRolesAsync()
    {
        // TodoCreator role
        if (await _roleRepository.FindByNormalizedNameAsync("TODOCREATOR") == null)
        {
            var role = new IdentityRole(_guidGenerator.Create(), "TodoCreator");
            await _roleManager.CreateAsync(role);
            await _permissionManager.SetForRoleAsync("TodoCreator", "AngularDemo.Todos", true);
            await _permissionManager.SetForRoleAsync("TodoCreator", "AngularDemo.Todos.Create", true);
        }

        // TodoCompleter role
        if (await _roleRepository.FindByNormalizedNameAsync("TODOCOMPLETER") == null)
        {
            var role = new IdentityRole(_guidGenerator.Create(), "TodoCompleter");
            await _roleManager.CreateAsync(role);
            await _permissionManager.SetForRoleAsync("TodoCompleter", "AngularDemo.Todos", true);
            await _permissionManager.SetForRoleAsync("TodoCompleter", "AngularDemo.Todos.Complete", true);
        }
    }

    private async Task SeedUsersAsync()
    {
        // alice - TodoCreator (can create tasks)
        if (await _userRepository.FindByNormalizedUserNameAsync("ALICE") == null)
        {
            var alice = new IdentityUser(_guidGenerator.Create(), "alice", "alice@abp.io");
            await _userManager.CreateAsync(alice, "1q2w3E*");
            await _userManager.AddToRoleAsync(alice, "TodoCreator");
        }

        // bob - TodoCompleter (can finish tasks)
        if (await _userRepository.FindByNormalizedUserNameAsync("BOB") == null)
        {
            var bob = new IdentityUser(_guidGenerator.Create(), "bob", "bob@abp.io");
            await _userManager.CreateAsync(bob, "1q2w3E*");
            await _userManager.AddToRoleAsync(bob, "TodoCompleter");
        }

        // charlie - both roles
        if (await _userRepository.FindByNormalizedUserNameAsync("CHARLIE") == null)
        {
            var charlie = new IdentityUser(_guidGenerator.Create(), "charlie", "charlie@abp.io");
            await _userManager.CreateAsync(charlie, "1q2w3E*");
            await _userManager.AddToRoleAsync(charlie, "TodoCreator");
            await _userManager.AddToRoleAsync(charlie, "TodoCompleter");
        }

        // Grant admin all todo permissions
        await _permissionManager.SetForRoleAsync("admin", "AngularDemo.Todos", true);
        await _permissionManager.SetForRoleAsync("admin", "AngularDemo.Todos.Create", true);
        await _permissionManager.SetForRoleAsync("admin", "AngularDemo.Todos.Complete", true);
    }

    private async Task SeedTodosAsync()
    {
        if (await _todoRepository.GetCountAsync() > 0)
        {
            return;
        }

        await _todoRepository.InsertAsync(
            new Todo(_guidGenerator.Create(), "Set up development environment", "Install .NET SDK, Node.js, and PostgreSQL"));

        await _todoRepository.InsertAsync(
            new Todo(_guidGenerator.Create(), "Review ABP documentation", "Read through the ABP framework getting started guide"));

        await _todoRepository.InsertAsync(
            new Todo(_guidGenerator.Create(), "Design the database schema", "Create ER diagram for the todo application"));

        await _todoRepository.InsertAsync(
            new Todo(_guidGenerator.Create(), "Write unit tests", "Add test coverage for the Todo application service"));
    }
}
