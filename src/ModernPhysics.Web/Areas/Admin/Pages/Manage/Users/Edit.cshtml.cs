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
    public class EditModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly WebIdentityDbContext _context;

        public EditModel(UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            WebIdentityDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }
        [BindProperty(SupportsGet = true)]
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
        }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if(id == null)
            {
                return RedirectToPage("/NotFound");
            }
            
            Roles = _context.Roles.Select(r => new SelectListItem 
            {
                Text = r.Name,
                Value = r.Id
            }).ToList();

            var user = await _userManager.FindByIdAsync(id.ToString());
            var roles = await _userManager.GetRolesAsync(user);
            var role = await _roleManager.FindByNameAsync(roles.FirstOrDefault());

            Input = new InputModel
            {
                UserName = user.UserName,
                UserRole = role.Id
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
                await _userManager.SetUserNameAsync(user, Input.UserName);
                
                var roles = await _userManager.GetRolesAsync(user);
                var role = await _roleManager.FindByIdAsync(Input.UserRole);

                if(role.Name != roles.FirstOrDefault())
                {
                    await _userManager.RemoveFromRolesAsync(user, roles);
                    await _userManager.AddToRoleAsync(user, role.Name);
                }

                return new RedirectToPageResult("/Manage/Users", new { area = "Admin" });
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