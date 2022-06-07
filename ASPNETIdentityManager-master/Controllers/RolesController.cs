using ASPNETIdentityManager.Contexts;
using ASPNETIdentityManager.Entities;
using ASPNETIdentityManager.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETIdentityManager.Controllers
{
    public class RolesController : Controller
    {
        private readonly ILogger<RolesController> logger;

        public RolesController(ILogger<RolesController> logger)
        {
            this.logger = logger;
        }

        public IActionResult Index([FromServices] UserDBContext dBContext, string roleName)
        {
            UsersAndRolesViewModel model = new UsersAndRolesViewModel();
            model.Roles = dBContext.Roles.Where(r =>
            string.IsNullOrEmpty(roleName) ? r.Name != null : r.Name.Contains(roleName)).Select(r => new Role()
            {
                IdentityRole = r,
                RoleClaims = dBContext.RoleClaims.Where(rc => rc.RoleId == r.Id).ToList()
            }).ToList();
            return View(model);
        }

        #region roles
        [HttpPost]
        public async Task<IActionResult> CreateRole([FromServices] UserDBContext dBContext, string roleName)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!string.IsNullOrEmpty(roleName))
                    {
                        dBContext.Roles.Add(new IdentityRole()
                        {
                            Name = roleName,
                            NormalizedName = roleName.ToUpperInvariant()
                        });
                        await dBContext.SaveChangesAsync();
                        return Json("OK");
                    }
                    else
                        return Json("Invalid request");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
            return Json("Invalid request");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteRole([FromServices] UserDBContext dBContext, [FromServices] UserManager<User> userManager, string roleID)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    IdentityRole role = dBContext.Roles.Where(r => r.Id == roleID).FirstOrDefault();
                    if (role != null)
                    {
                        List<IdentityRoleClaim<string>> claims = dBContext.RoleClaims.Where(uc => uc.RoleId == role.Id).ToList();
                        dBContext.RoleClaims.RemoveRange(claims);

                        List<IdentityUserRole<string>> userRoles = dBContext.UserRoles.Where(ur => ur.RoleId == role.Id).ToList();
                        dBContext.UserRoles.RemoveRange(userRoles);

                        List<User> users = (from ur in dBContext.UserRoles
                                            join u in dBContext.Users on ur.UserId equals u.Id
                                            where ur.RoleId == roleID
                                            select u).ToList();
                        foreach (User user in users)
                            await userManager.UpdateSecurityStampAsync(user);

                        dBContext.Roles.Remove(role);
                        await dBContext.SaveChangesAsync();
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
        [HttpPost]
        public async Task<IActionResult> AddClaimToRole([FromServices] UserDBContext dBContext, [FromServices] UserManager<User> userManager, string roleID, string claimType, string claimValue)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!string.IsNullOrEmpty(claimType) && !string.IsNullOrEmpty(claimValue))
                    {
                        dBContext.RoleClaims.Add(new IdentityRoleClaim<string>()
                        {
                            RoleId = roleID,
                            ClaimType = claimType,
                            ClaimValue = claimValue
                        });
                        await dBContext.SaveChangesAsync();

                        List<User> users = (from ur in dBContext.UserRoles
                                            join u in dBContext.Users on ur.UserId equals u.Id
                                            where ur.RoleId == roleID
                                            select u).ToList();
                        foreach (User user in users)
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
        public async Task<IActionResult> RemoveClaimFromRole([FromServices] UserDBContext dBContext, [FromServices] UserManager<User> userManager, string roleID, string claimType, string claimValue)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    List<IdentityRoleClaim<string>> claims = dBContext.RoleClaims.Where(uc => uc.RoleId == roleID && uc.ClaimType == claimType && uc.ClaimValue == claimValue).ToList();
                    if (claims.Count > 0)
                    {
                        foreach (IdentityRoleClaim<string> claim in claims)
                            dBContext.RoleClaims.Remove(claim);
                        await dBContext.SaveChangesAsync();

                        List<User> users = (from ur in dBContext.UserRoles
                                            join u in dBContext.Users on ur.UserId equals u.Id
                                            where ur.RoleId == roleID
                                            select u).ToList();
                        foreach (User user in users)
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
    }
}
