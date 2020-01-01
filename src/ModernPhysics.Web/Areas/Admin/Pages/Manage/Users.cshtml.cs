using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ModernPhysics.Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace ModernPhysics.Web.Areas.Admin.Pages.Manage
{
    [Authorize(Policy = "IsAdmin")]
    public class UsersModel : PageModel
    {
        private WebIdentityDbContext _context;
        private UserManager<IdentityUser> _userManager;
        private IConfiguration _configuration;

        public UsersModel(WebIdentityDbContext context, UserManager<IdentityUser> userManager, 
            IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
        }

        public class UserModel
        {
            public string Id { get; set; }
            public string UserName { get; set; }
            public string UserRole { get; set; }
        }


        [TempData]
        public string ErrorMessage { get; set; }

        public List<UserModel> Users { get; set; }

        public async Task OnGet()
        {
            var users = _context.Users;
            
            Users = new List<UserModel>();

            foreach(var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var userRole = roles.FirstOrDefault();
                Users.Add(new UserModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    UserRole = userRole
                });
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(string id)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestResult();
            }

            var user = await _userManager.FindByIdAsync(id);

            if(string.Equals(user.UserName, _configuration["AdminUsername"]) == false)
            {
                await _userManager.DeleteAsync(user);
            }
            else
            {
                var adminUserName = _configuration["AdminUsername"];
                ErrorMessage = $"{adminUserName} nie może zostać usunięty.";
            }

            return RedirectToPage("./Users");
        }
    }
}