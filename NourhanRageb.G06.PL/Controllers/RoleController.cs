 using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NourhanRageb.G06.DAL.Models;
using NourhanRageb.G06.PL.ViewModels;

namespace NourhanRageb.G06.PL.Controllers
{
    //[Authorize (Roles ="Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserManager<ApplicationUser> UserManager { get; }

        // Get , GetAll , Add , Update , Delete
        // Index , Create , Details , Edit , Delete
        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            UserManager = userManager;
        }

        public async Task<IActionResult> Index(string SearchInput) 
        {
            var roles = Enumerable.Empty<RoleViewModel>();

            if (string.IsNullOrEmpty(SearchInput))
            {
                roles = await _roleManager.Roles.Select(R => new RoleViewModel()
                {
                    Id = R.Id,
                    RoleName = R.Name

                }).ToListAsync();
            }
            else
            {
                roles = await _roleManager.Roles.Where(R => R.Name
                                                .ToLower()
                                                .Contains(SearchInput.ToLower()))
                                                .Select(R => new RoleViewModel()
                                                {
                                                    Id = R.Id,
                                                    RoleName = R.Name

                                                }).ToListAsync();
            }
            return View(roles);
        }

        [HttpGet]
        public IActionResult Create()
        {
           return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role = new IdentityRole()
                {
                    Name = model.RoleName
                };
                await _roleManager.CreateAsync(role);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string? id, string ViewName = "Details")
        {
            if (id is null)
            {
                return BadRequest();
            }

            var roleFromDb = await _roleManager.FindByIdAsync(id);

            if (roleFromDb is null)
            {
                return NotFound();
            }
            var role = new RoleViewModel()
            {
                Id = roleFromDb.Id,
                RoleName = roleFromDb.Name

            };

            return View(ViewName, role);

        }

        [HttpGet]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string? id)
        {
            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> Edit([FromRoute] string? id, RoleViewModel model)
        {
            try
            {

                if (id != model.Id)
                {
                    return BadRequest();
                }

                if (ModelState.IsValid)
                {
                    var roleFromDb = await _roleManager.FindByIdAsync(id);

                    if (roleFromDb is null)
                    {
                        return NotFound();
                    }

                    roleFromDb.Name = model.RoleName;


                    await _roleManager.UpdateAsync(roleFromDb);

                    return RedirectToAction(nameof(Index));

                }
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View(model);

        }

        public async Task<IActionResult> Delete(string? id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] string? id, RoleViewModel model)
        {
            try
            {

                if (id != model.Id)
                {
                    return BadRequest();
                }

                if (ModelState.IsValid)
                {
                    var roleFromDb = await _roleManager.FindByIdAsync(id);

                    if (roleFromDb is null)
                    {
                        return NotFound();
                    }

                    await _roleManager.DeleteAsync(roleFromDb);

                    return RedirectToAction(nameof(Index));

                }
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View(model);




        }

        [HttpGet]
        public async Task<IActionResult> AddOrRemoveUsers(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role is null)
            {
                return NotFound();
            }

            ViewData["RoleId"] = roleId;

            var UsersInRole = new List<UsersInRoleViewModel>();
            var Users = await UserManager.Users.ToListAsync();
            foreach (var user in Users)
            {
                var UserInRole = new UsersInRoleViewModel()
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };

                if (await UserManager.IsInRoleAsync(user, role.Name))
                {
                    UserInRole.IsSelect = true;
                }
                else
                {
                    UserInRole.IsSelect = false;
                }
                UsersInRole.Add(UserInRole);
            }
            return View(UsersInRole);



        }

        [HttpPost]
        public async Task<IActionResult> AddOrRemoveUsers(string roleId, List<UsersInRoleViewModel> users)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role is null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                foreach (var user in users)
                {
                    var appUser = await UserManager.FindByIdAsync(user.UserId);
                    if (appUser is not null)
                    {
                        if (user.IsSelect && ! await UserManager.IsInRoleAsync(appUser , role.Name))
                        {
                            await UserManager.AddToRoleAsync(appUser, role.Name);
                        }
                        else if(user.IsSelect && await UserManager.IsInRoleAsync(appUser, role.Name))
                        {
                            await UserManager.RemoveFromRoleAsync(appUser, role.Name);
                        }
                    }
                }
                return RedirectToAction(nameof(Edit) , new { id = roleId });
            }
            return View(users);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
