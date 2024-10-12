﻿using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NourhanRageb.G06.DAL.Models;
using NourhanRageb.G06.PL.Helpers;
using NourhanRageb.G06.PL.ViewModels;

namespace NourhanRageb.G06.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
		private readonly SignInManager<ApplicationUser> signInManager;

		public AccountController(UserManager<ApplicationUser> userManager , SignInManager<ApplicationUser> signInManager )
        {
            this.userManager = userManager;
			this.signInManager = signInManager;
		}

        #region SignUp

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewMoedel model)
        {
            // Code To Registration

            if (ModelState.IsValid) //Server Side Validation
            {
                var user = await userManager.FindByNameAsync(model.UserName);

                if (user is null)
                {
                    // Mapping : SignUpViewModel To ApplicationUser
                    user = await userManager.FindByEmailAsync(model.Email);

                    if (user is null)
                    {
                        user = new ApplicationUser()
                        {
                            UserName = model.UserName,
                            Email = model.Email,
                            Fname = model.FristName,
                            Lname = model.LastName,
                        };
                        var Result = await userManager.CreateAsync(user, model.Password);

                        if (Result.Succeeded)
                        {
                            return RedirectToAction("SignIn");
                        }
                        foreach (var Error in Result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, Error.Description);
                        }

                    }

                    ModelState.AddModelError(string.Empty, "Email is already exits(:");

                    return View(model);
                }


                ModelState.AddModelError(string.Empty, "UserName is already exits(:");
            }

            return View(model);
        }
		#endregion

		#region SignIn

		[HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await userManager.FindByEmailAsync(model.Email);

                    if (user is not null)
                    {
                        // Check Password
                        var flag = await userManager.CheckPasswordAsync(user, model.Password);
                        if (flag)
                        {
                            //SignIn
                            var result = await signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                            if (result.Succeeded)
                            {
                                return RedirectToAction("Index", "Home");
                            }
                        }

                    }

                    ModelState.AddModelError(string.Empty, "Invalid Login !!");

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            return View(model);

        }
        #endregion

        #region SignOut
        public new async Task<IActionResult> SignOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction(nameof(SignIn));
        }


        #endregion

        #region Forget Password

        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendResetPasswordUrl(ForgetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);

                if (user is not null)
                {
                    //Create Token

                    var token = await userManager.GeneratePasswordResetTokenAsync(user);

                    //Create Reset Password URL

                    var url = Url.Action("ResetPassword", "Account", new { Email = model.Email, token = token }, Request.Scheme);

                    //Create Email

                    var email = new Emails()
                    {
                        To = model.Email,
                        Subject = "Reset Password",
                        Body = url
                    };


                    //Send Email

                    EmailSettings.SendEmail(email);
                                   
                    return RedirectToAction(nameof(CheckYourInbox));

                }
                ModelState.AddModelError(string.Empty, "Invalid Operation , Please Try Agin !!");
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult CheckYourInbox()
        {
            return View();
        } 
        #endregion

        #region Reset Password
        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var email = TempData["email"] as string;
                var token = TempData["token"] as string;

                var user = await userManager.FindByEmailAsync(email);
                if (user is not null)
                {
                    var result = await userManager.ResetPasswordAsync(user, token, model.Password);
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(SignIn));
                    }

                }
            }
            ModelState.AddModelError(string.Empty, "Invalid Operation , please Try Again");
            return View();
        } 
        #endregion



    }
}
