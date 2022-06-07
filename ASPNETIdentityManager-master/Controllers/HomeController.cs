using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ASPNETIdentityManager.Models;
using Microsoft.AspNetCore.Identity;
using ASPNETIdentityManager.Entities;
using ASPNETIdentityManager.Contexts;
using ASPNETIdentityManager.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace ASPNETIdentityManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private SignInManager<User> signInManager;

        public HomeController(ILogger<HomeController> logger)
        {
            this.logger = logger;
        }
        public IActionResult Index([FromServices] UserDBContext dBContext, string userName, string email)
        {
            UsersAndRolesViewModel model = new UsersAndRolesViewModel();
            model.Users = dBContext.Users.Where(u =>
            (string.IsNullOrEmpty(userName) ? u.UserName != null : u.UserName.Contains(userName))
            &&
            (string.IsNullOrEmpty(email) ? true : u.Email != null && u.Email.Contains(email))
            ).Select(u => new User()
            {
                Id = u.Id,
                Email = u.Email,
                UserName = u.UserName,
                UserRoles = (from r in dBContext.Roles
                             join ur in dBContext.UserRoles.Where(ur => ur.UserId == u.Id) on r.Id equals ur.RoleId
                             select new Role()
                             {
                                 IdentityRole = r,
                                 RoleClaims = dBContext.RoleClaims.Where(rc => rc.RoleId == r.Id).ToList()
                             }).ToList(),
                UserClaims = dBContext.UserClaims.Where(uc => uc.UserId == u.Id).ToList()
            }).ToList();
            return View(model);
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult GestisciPrenotazioni([FromServices] UserDBContext dBContext, string userName, string email)
        {
            UsersAndRolesViewModel model = new UsersAndRolesViewModel();
            model.Users = dBContext.Users.Where(u =>
            (string.IsNullOrEmpty(userName) ? u.UserName != null : u.UserName.Contains(userName))
            &&
            (string.IsNullOrEmpty(email) ? true : u.Email != null && u.Email.Contains(email))
            ).Select(u => new User()
            {
                Id = u.Id,
                Email = u.Email,
                UserName = u.UserName,
                UserRoles = (from r in dBContext.Roles
                             join ur in dBContext.UserRoles.Where(ur => ur.UserId == u.Id) on r.Id equals ur.RoleId
                             select new Role()
                             {
                                 IdentityRole = r,
                                 RoleClaims = dBContext.RoleClaims.Where(rc => rc.RoleId == r.Id).ToList()
                             }).ToList(),
                UserClaims = dBContext.UserClaims.Where(uc => uc.UserId == u.Id).ToList() 
            }).ToList();
            return View(model);
        }
        #region users 
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromServices] UserManager<User> userManager, UsersAndRolesViewModel usersViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    User user = await userManager.FindByEmailAsync(usersViewModel.Email);
                    if (user == null)
                    {
                        user = new User
                        {
                            UserName = usersViewModel.UserName,
                            Email = usersViewModel.Email
                        };
                        IdentityResult result = await userManager.CreateAsync(user, usersViewModel.Password);
                        if (result.Succeeded)
                            return Json("OK");

                        string errors = string.Empty;
                        foreach (IdentityError error in result.Errors)
                            errors += error.Code + ": " + error.Description + "\n";
                        return Json(errors);
                    }
                    else
                        return Json("Email is already taken");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
            return Json("Invalid request");
        }

        [HttpPost]
        public async Task<IActionResult> Prenota([FromServices] UserManager<User> userManager, ContattoModel contattoModel)
        {

                if (ModelState.IsValid)
                {
                    Contatto contatto = new Contatto
                    {
                        Nome = contattoModel.Nome,
                        Cognome = contattoModel.Cognome
                    };
                return Json("OK");
            }
                return Json("Error");

        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser([FromServices] UserDBContext dBContext, [FromServices] UserManager<User> userManager, string userId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    User user = await userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        List<IdentityUserClaim<string>> userClaims = dBContext.UserClaims.Where(uc => uc.UserId == user.Id).ToList();
                        dBContext.UserClaims.RemoveRange(userClaims);

                        List<IdentityUserRole<string>> userRoles = dBContext.UserRoles.Where(ur => ur.UserId == user.Id).ToList();
                        dBContext.UserRoles.RemoveRange(userRoles);

                        await dBContext.SaveChangesAsync();

                        IdentityResult result = await userManager.DeleteAsync(user);

                        if (result.Succeeded)
                            return Json("OK");

                        string errors = string.Empty;
                        foreach (IdentityError error in result.Errors)
                            errors += error.Code + ": " + error.Description;
                        return Json(errors);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
            return Json("Invalid request");
        }

        [HttpPost]
        public async Task<IActionResult> AddRoleToUser([FromServices] UserManager<User> userManager, string userID, string roleName)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    User user = await userManager.FindByIdAsync(userID);
                    await userManager.AddToRoleAsync(user, roleName);
                    await userManager.UpdateSecurityStampAsync(user);
                    return Json(ConstantValues.OK);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
            return Json("Invalid request");
        }
        [HttpPost]
        public async Task<IActionResult> RemoveRoleFromUser([FromServices] UserManager<User> userManager, string userID, string roleName)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    User user = await userManager.FindByIdAsync(userID);
                    await userManager.RemoveFromRoleAsync(user, roleName);
                    await userManager.UpdateSecurityStampAsync(user);
                    return Json(ConstantValues.OK);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
            return Json("Invalid request");
        }
        [HttpPost]
        public async Task<IActionResult> AddClaimToUser([FromServices] UserDBContext dBContext, [FromServices] UserManager<User> userManager, string userID, string claimType, string claimValue)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!string.IsNullOrEmpty(claimType) && !string.IsNullOrEmpty(claimValue))
                    {
                        dBContext.UserClaims.Add(new IdentityUserClaim<string>()
                        {
                            UserId = userID,
                            ClaimType = claimType,
                            ClaimValue = claimValue
                        });
                        await dBContext.SaveChangesAsync();

                        User user = await userManager.FindByIdAsync(userID);
                        await userManager.UpdateSecurityStampAsync(user);

                        return Json("OK");
                    }
                    else
                        return Json("Values cant be null");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
            return Json("Invalid request");
        }
        [HttpPost]
        public async Task<IActionResult> RemoveClaimFromUser([FromServices] UserDBContext dBContext, [FromServices] UserManager<User> userManager, string userID, string claimType, string claimValue)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    List<IdentityUserClaim<string>> claims = dBContext.UserClaims.Where(uc => uc.UserId == userID && uc.ClaimType == claimType && uc.ClaimValue == claimValue).ToList();
                    if (claims.Count > 0)
                    {
                        foreach (IdentityUserClaim<string> claim in claims)
                            dBContext.UserClaims.Remove(claim);
                        await dBContext.SaveChangesAsync();

                        User user = await userManager.FindByIdAsync(userID);
                        await userManager.UpdateSecurityStampAsync(user);

                        return Json("OK");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
            return Json("Invalid request");
        }
        #endregion

        public async Task<IActionResult> EditUser([FromServices] UserManager<User> userManager, [FromServices] UserDBContext userDbContext, UsersAndRolesViewModel model)
        {
            try
            {
                User user = await userDbContext.Users.Where(u => u.Id == model.UserID).FirstOrDefaultAsync();
                if (ModelState.IsValid)
                {
                    string response = string.Empty;
                    if (!string.IsNullOrEmpty(model.UserName))
                    {
                        if (await userDbContext.Users.Where(u => u.UserName == model.UserName).FirstOrDefaultAsync() == null)
                        {
                            IdentityResult result = await userManager.SetUserNameAsync(user, model.UserName);
                            if (result.Succeeded)
                            {
                                response += "The UserName has been changed. The user has been automatically logged out" + "\n";
                            }
                            else
                            {
                                foreach (IdentityError error in result.Errors)
                                    response += error.Code + ": " + error.Description + "\n";
                            }
                        }
                        else
                            response += "Email " + model.Email + " is already taken" + "\n";
                    }
                    if (!string.IsNullOrEmpty(model.Email))
                    {
                        if (await userDbContext.Users.Where(u => u.Email == model.Email).FirstOrDefaultAsync() == null)
                        {
                            IdentityResult result = await userManager.SetEmailAsync(user, model.Email);
                            if (result.Succeeded)
                            {
                                response += "The email has been changed. The user has been automatically logged out" + "\n";
                            }
                            else
                            {

                                foreach (IdentityError error in result.Errors)
                                    response += error.Code + ": " + error.Description + "\n";
                            }
                        }
                        else
                            response += "Email " + model.Email + " is already taken" + "\n";
                    }
                    if (!string.IsNullOrEmpty(model.Password))
                    {
                        string token = await userManager.GeneratePasswordResetTokenAsync(user);
                        IdentityResult result = await userManager.ResetPasswordAsync(user, token, model.Password);
                        if (result.Succeeded)
                        {
                            await userManager.UpdateSecurityStampAsync(user);
                            response += "The password has been changed" + "\n";
                        }
                        else
                        {
                            foreach (IdentityError error in result.Errors)
                                response += error.Code + ": " + error.Description + "\n";
                        }
                    }
                    if (string.IsNullOrEmpty(model.Email) && string.IsNullOrEmpty(model.Password))
                        response += "Nothing to do" + "\n";
                    return Json(response);
                }
                /*model.TotalEntitiesNumber = userManager.Users.Count();
                model.CurrentPage = 0;
                model.Users = model.Users.Take(ConstantValues.searchPageSize).ToList();*/
                return Json("Invalid request");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
            return View("Index", new UsersAndRolesViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromServices] SignInManager<User> signInManager, [FromServices] UserManager<User> userManager, UsersAndRolesViewModel usersViewModel)
        {
            try
            {
                User user = await userManager.FindByNameAsync(usersViewModel.UserName);
                if (user != null)
                {
                    // This doesn't count login failures towards account lockout
                    // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                    Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(usersViewModel.UserName, usersViewModel.Password, true, lockoutOnFailure: false);
                    if (result.Succeeded)
                        return LocalRedirect("/");
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid Login");
                        return LocalRedirect("/");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User doesn't exist");
                    return LocalRedirect("/");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
            return LocalRedirect("/");
        }
        [Authorize]
        public async Task<IActionResult> Logout([FromServices] SignInManager<User> signInManager)
        {
            try
            {
                if (signInManager.IsSignedIn(User))
                {
                    await signInManager.SignOutAsync();
                    return LocalRedirect("/");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
            return LocalRedirect("/");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
