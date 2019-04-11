using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StrongStart.Data;
using StrongStart.Models;

namespace StrongStart.Areas.Identity.Pages.Account.Manage
{
    public class YourTraining: PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<YourTraining> _logger;
        private readonly StrongStartContext _context;

        public YourTraining(
            UserManager<ApplicationUser> userManager,
            ILogger<YourTraining> logger,
            StrongStartContext context)
        {
            _userManager = userManager;
            _logger = logger;
            _context = context;
        }
        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            ViewData["training_volunteer"] = _context.Training_Volunteers
                .Include(t=>t.training)
                .Include(t=>t.training.site)
                .Where(t => t.volunteerID == user.Id);

            return Page();
        }

    }
}
