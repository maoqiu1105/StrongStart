using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using StrongStart.Data;
using StrongStart.Models;

namespace StrongStart.Areas.Identity.Pages.Account.Manage
{
    public class YourVolunteerModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<YourTraining> _logger;
        private readonly ApplicationDbContext _applicationContext ;

        public YourVolunteerModel(
            UserManager<ApplicationUser> userManager,
            ILogger<YourTraining> logger, 
            ApplicationDbContext applicationContex)
        {
            _userManager = userManager;
            _logger = logger;
            _applicationContext = applicationContex;
        }
        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            ViewData["volunteers"] = _applicationContext.Users.Where(u => u.isQualify == YesNO.Yes);
                

            return Page();
        }
    }
}