using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ModernPhysics.Models;
using ModernPhysics.Web.Data;

namespace ModernPhysics.Web.Areas.Admin.Pages.Manage.Users
{
    [Authorize(Policy = "IsAdmin")]
    public class AddModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly WebIdentityDbContext _context;

        public AddModel(UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            WebIdentityDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }
        public List<SelectListItem> Roles { get; set; }

        [TempData]
        public string Result { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Nazwa użytkownika")]
            public string UserName { get; set; }

            [Required]
            [Display(Name = "Rola użytkownika")]
            public string UserRole { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "{0} musi mieć conajmniej {2} znaków i conajwyżej {1} znaków.", MinimumLength = 8)]
            [DataType(DataType.Password)]
            [Display(Name = "Hasło")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Potwierdź hasło")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public void OnGet()
        {
            Roles = _context.Roles.Select(r => new SelectListItem 
            {
                Text = r.Name,
                Value = r.Id
            }).ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = Input.UserName };
                var role = await _roleManager.FindByIdAsync(Input.UserRole);

                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, role.Name);
                    return new RedirectToPageResult("/Manage/Users", new { area = "Admin" });            
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            Roles = _context.Roles.Select(r => new SelectListItem 
            {
                Text = r.Name,
                Value = r.Id
            }).ToList();

            return Page();
        } 
    }
}