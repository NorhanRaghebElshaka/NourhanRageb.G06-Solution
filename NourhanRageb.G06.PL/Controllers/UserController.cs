using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NourhanRageb.G06.DAL.Models;
using NourhanRageb.G06.PL.Helpers;
using NourhanRageb.G06.PL.ViewModels;
using System.Collections.ObjectModel;

namespace NourhanRageb.G06.PL.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;

        // Get , GetAll , Add , Update , Delete
        // Index , Details , Edit , Delete
        public UserController(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index(string SearchInput)
        {
            var Users = Enumerable.Empty<UserViewModel>();

            if (string.IsNullOrEmpty(SearchInput))
            {
                Users = await userManager.Users.Select(U => new UserViewModel()
                {
                    Id = U.Id,
                    FirstName = U.Fname,
                    LastName = U.Lname,
                    Email = U.Email,
                    Roles = userManager.GetRolesAsync(U).Result


                }).ToListAsync();
            }
            else
            {
                Users = await userManager.Users.Where(U => U.Email
                                                .ToLower()
                                                .Contains(SearchInput.ToLower()))
                                                .Select(U => new UserViewModel()
                                                {
                                                    Id = U.Id,
                                                    FirstName = U.Fname,
                                                    LastName = U.Lname,
                                                    Email = U.Email,
                                                    Roles = userManager.GetRolesAsync(U).Result


                                                }).ToListAsync();
            }
            return View(Users);
        }

        public async Task<IActionResult> Details(string? id, string ViewName = "Details")
        {
            if (id is null)
            {
                return BadRequest();
            }

            var UserFromDb = await userManager.FindByIdAsync(id);

            if (UserFromDb is null)
            {
                return NotFound();
            }
            var user = new UserViewModel()
            {
                Id = UserFromDb.Id,
                FirstName = UserFromDb.Fname,
                LastName = UserFromDb.Lname,
                Email = UserFromDb.Email,
                Roles = userManager.GetRolesAsync(UserFromDb).Result
            };

            return View(ViewName, user);

        }

        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {
            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //Request بتمنع ان اي حد من برا يبعت 
        public async Task<IActionResult> Edit([FromRoute] string? id, UserViewModel model)
        {
            try
            {

                if (id != model.Id)
                {
                    return BadRequest();
                }

                if (ModelState.IsValid)
                {
                    var UserFromDb = await userManager.FindByIdAsync(id);

                    if (UserFromDb is null)
                    {
                        return NotFound();
                    }

                    UserFromDb.Fname = model.FirstName;
                    UserFromDb.Lname = model.LastName;
                    UserFromDb.Email = model.Email;

                    await userManager.UpdateAsync(UserFromDb);

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
        public async Task<IActionResult> Delete([FromRoute] string? id, UserViewModel model)
        {
            try
            {

                if (id != model.Id)
                {
                    return BadRequest();
                }

                if (ModelState.IsValid)
                {
                    var UserFromDb = await userManager.FindByIdAsync(id);

                    if (UserFromDb is null)
                    {
                        return NotFound();
                    }

                    await userManager.DeleteAsync(UserFromDb);

                    return RedirectToAction(nameof(Index));

                }
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View(model);




        }
    }
}
