using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StrongStart.Models;
using StrongStart.Class;

namespace StrongStart.Controllers
{
    public class VolunteersController : Controller
    {
        private readonly StrongStartContext _context;

        public VolunteersController(StrongStartContext context)
        {
            _context = context;
        }

        // GET: Volunteers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Volunteers.ToListAsync());
        }

        // GET: Volunteers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var volunteer = await _context.Volunteers
                .FirstOrDefaultAsync(m => m.volunteerID == id);
            if (volunteer == null)
            {
                return NotFound();
            }

            return View(volunteer);
        }

        // GET: Volunteers/Create
        public IActionResult Create()
        {
            ViewData["preSchool"] = new SelectList(_context.Sites, "siteID", "siteName");
            return View();
        }

        // POST: Volunteers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("volunteerID,firstName,lastName,Email,Confirm_Email,Password,Confirm_Password,Address,Phone,prefSchool,infoID,City,Province")] VolunteerRegister volunteerRegister)
        {
            Volunteer volunteer = new Volunteer();
            if (volunteerRegister.Email == volunteerRegister.Confirm_Email && volunteerRegister.Password == volunteerRegister.Confirm_Password)
            {
                if (volunteerRegister.Password != null)
                {
                    byte[] passwordSalt = PasswordHashing.GenSalt();
                    string passwordHash = PasswordHashing.GenHash(volunteerRegister.Password, passwordSalt);
                    volunteer.PasswordSalt = Convert.ToBase64String(passwordSalt);
                    volunteer.PasswordHash = passwordHash;
                }
                else
                {
                    ModelState.AddModelError("", $"Error: please enter password");
                }
                volunteer.volunteerID = volunteerRegister.volunteerID;
                volunteer.firstName = volunteerRegister.firstName;
                volunteer.lastName = volunteerRegister.lastName;
                volunteer.Email = volunteerRegister.Email;
                volunteer.Address = volunteerRegister.Address;
                volunteer.Phone = volunteerRegister.Phone   ;
                volunteer.prefSchool = volunteerRegister.prefSchool;
                volunteer.infoID = volunteerRegister.infoID;
                volunteer.City = volunteerRegister.City;
                volunteer.Province = volunteerRegister.Province;
            }
            else
            {
                ModelState.AddModelError("", $"Error: please confirm password and email");
            }
            
            if (ModelState.IsValid)
            {
                _context.Add(volunteer);
                await _context.SaveChangesAsync();
                return RedirectToAction("Success","Registrations",new { id = volunteer.volunteerID });
            }
            ViewData["preSchool"] = new SelectList(_context.Sites, "siteID", "siteName");
            return View(volunteerRegister);
        }

        // GET: Volunteers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var volunteer = await _context.Volunteers.FindAsync(id);
            if (volunteer == null)
            {
                return NotFound();
            }
            return View(volunteer);
        }

        // POST: Volunteers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("volunteerID,firstName,lastName,Email,PasswordSalt,PasswordHash,Address,Phone,prefSchool,infoID,City,Province")] Volunteer volunteer)
        {
            if (id != volunteer.volunteerID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(volunteer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VolunteerExists(volunteer.volunteerID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(volunteer);
        }

        // GET: Volunteers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var volunteer = await _context.Volunteers
                .FirstOrDefaultAsync(m => m.volunteerID == id);
            if (volunteer == null)
            {
                return NotFound();
            }

            return View(volunteer);
        }

        // POST: Volunteers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var volunteer = await _context.Volunteers.FindAsync(id);
            _context.Volunteers.Remove(volunteer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VolunteerExists(int id)
        {
            return _context.Volunteers.Any(e => e.volunteerID == id);
        }
    }
}
