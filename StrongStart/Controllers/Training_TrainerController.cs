using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StrongStart.Models;

namespace StrongStart.Controllers
{
    public class Training_TrainerController : Controller
    {
        private readonly StrongStartContext _context;

        public Training_TrainerController(StrongStartContext context)
        {
            _context = context;
        }

        // GET: Training_Trainer
        public async Task<IActionResult> Index(int? id)
        {
            var strongStartContext = _context.Training_Trainers.Include(t => t.trainer).Include(t => t.training).Where(t => t.trainingID == id);
            ViewData["trainingSite"] = _context.Trainings.Include(t => t.site).FirstOrDefault(t => t.trainingID == id).site.siteName;
            ViewData["trainingDate"] = _context.Trainings.FirstOrDefault(t => t.trainingID == id).Date.ToShortDateString();
            ViewData["trainingID"] = id;
            
            return View(await strongStartContext.ToListAsync());
        }
        
        // GET: Training_Trainer/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var training_Trainer = await _context.Training_Trainers
                .Include(t => t.trainer)
                .Include(t => t.training)
                .Include(t => t.training.site)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (training_Trainer == null)
            {
                return NotFound();
            }

            return View(training_Trainer);
        }

        // GET: Training_Trainer/Create
        public IActionResult Create(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            ViewData["trainerID"] = new SelectList(_context.Trainers, "trainerID", "firstName");
            ViewData["trainingID"] = id;
            ViewData["trainingSite"] = _context.Trainings.Include(t => t.site).FirstOrDefault(t => t.trainingID == id).site.siteName;
            ViewData["trainingDate"] = _context.Trainings.FirstOrDefault(t => t.trainingID == id).Date.ToShortDateString();

            return View();
        }

        // POST: Training_Trainer/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("trainingID,trainerID,hasKit,driveDistance,becomeTrainer,traineeStatus")] Training_Trainer training_Trainer)
        {
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("strongstarttesting@gmail.com", "Flakkie4u2");

            if (training_Trainer.becomeTrainer.ToString() == "No")
            {
                training_Trainer.traineeStatus = Status.NA;
            }
            if (training_Trainer.driveDistance == null)
            {
                training_Trainer.driveDistance = "0";
            }
            if (_context.Training_Trainers.Where(t => t.trainingID == training_Trainer.trainingID).Select(t => t.trainerID).ToList().Contains(training_Trainer.trainerID))
            {
                TempData["errMsg"] = "This Trainer has been signed for this tranining";

            }
            else
            {
                if (ModelState.IsValid)
                {
                    _context.Add(training_Trainer);
                    MailMessage mailMessage2 = new MailMessage();
                    mailMessage2.From = new MailAddress("strongstarttesting@gmail.com");

                    mailMessage2.From = new MailAddress("strongstarttesting@gmail.com");
                    string body2 = string.Empty;
                    Trainer trainer = _context.Trainers.Where(t => t.trainerID == training_Trainer.trainerID).FirstOrDefault();
                    var mail = trainer.Email;
                    var siteID = _context.Trainings.Include(t => t.site).FirstOrDefault(t => t.trainingID == training_Trainer.trainingID).siteID;
                    var site = _context.Sites.FirstOrDefault(t => t.siteID == siteID);
                    var training = _context.Trainings.FirstOrDefault(t => t.trainingID == training_Trainer.trainingID);

                    using (StreamReader reader = new StreamReader(@"~\trnEmail.html"))
                    {
                        body2 = reader.ReadToEnd();
                    }
                    body2 = body2.Replace("{name}", trainer.firstName);
                    body2 = body2.Replace("{sitename}", site.siteName);
                    body2 = body2.Replace("{address}", site.Address);
                    body2 = body2.Replace("{date}", training.Date.ToShortDateString());
                    body2 = body2.Replace("{time}", training_Trainer.training.startTime.ToShortTimeString());
                    body2 = body2.Replace("{kit}", training_Trainer.hasKit.ToString()); 

                    mailMessage2.To.Add(mail);
                    mailMessage2.IsBodyHtml = true;
                    mailMessage2.Body = body2;
                    mailMessage2.Subject = "You have been assigned to a training Session at " + site.siteName + ", " + site.City + ", on " + training.Date.ToShortDateString() + " - " + training.part;

                    client.Send(mailMessage2);
                    await _context.SaveChangesAsync();


                    return RedirectToAction("Index", new { id = training_Trainer.trainingID });
            }
            }
            ViewData["trainingID"] = training_Trainer.trainingID;
            ViewData["trainingSite"] = _context.Trainings.Include(t => t.site).FirstOrDefault(t => t.trainingID == training_Trainer.trainingID).site.siteName;
            ViewData["trainingDate"] = _context.Trainings.FirstOrDefault(t => t.trainingID == training_Trainer.trainingID).Date.ToShortDateString();
            ViewData["trainerID"] = new SelectList(_context.Trainers, "trainerID", "firstName", training_Trainer.trainerID);
            return View(training_Trainer);
        }

        // GET: Training_Trainer/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var training_Trainer = await _context.Training_Trainers.FindAsync(id);
            if (training_Trainer == null)
            {
                return NotFound();
            }
            ViewData["trainerID"] = new SelectList(_context.Trainers, "trainerID", "firstName", training_Trainer.trainerID);
            ViewData["trainingID"] = training_Trainer.trainingID;
            ViewData["trainingSite"] = _context.Trainings.Include(t => t.site).FirstOrDefault(t => t.trainingID == training_Trainer.trainingID).site.siteName;
            ViewData["trainingDate"] = _context.Trainings.FirstOrDefault(t => t.trainingID == training_Trainer.trainingID).Date.ToShortDateString();

            return View(training_Trainer);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,trainerID,hasKit,driveDistance")] Training_Trainer training_Trainer,int trainingID)
        {
            if (id != training_Trainer.ID)
            {
                return NotFound();
            }
            if (training_Trainer.becomeTrainer.ToString() == "No")
            {
                training_Trainer.traineeStatus = Status.NA;
            }
            if (training_Trainer.driveDistance == null)
            {
                training_Trainer.driveDistance = "0";
            }
            training_Trainer.trainingID = trainingID;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(training_Trainer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Training_TrainerExists(training_Trainer.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index",new { id = trainingID});
            }
            ViewData["trainerID"] = new SelectList(_context.Trainers, "trainerID", "firstName", training_Trainer.trainerID);
            return View(training_Trainer);
        }

        // GET: Training_Trainer/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            await _context.SaveChangesAsync(); var training_Trainer = await _context.Training_Trainers
    .Include(t => t.trainer)
    .Include(t => t.training)
    .FirstOrDefaultAsync(m => m.ID == id);
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("strongstarttesting@gmail.com", "Flakkie4u2");


            MailMessage mailMessage2 = new MailMessage();
            mailMessage2.From = new MailAddress("strongstarttesting@gmail.com");

            mailMessage2.From = new MailAddress("strongstarttesting@gmail.com");
            string body2 = string.Empty;
            Trainer trainer = _context.Trainers.Where(t => t.trainerID == training_Trainer.trainerID).FirstOrDefault();
            var mail = trainer.Email;
            var siteID = _context.Trainings.Include(t => t.site).FirstOrDefault(t => t.trainingID == training_Trainer.trainingID).siteID;
            var site = _context.Sites.FirstOrDefault(t => t.siteID == siteID);
            var training = _context.Trainings.FirstOrDefault(t => t.trainingID == training_Trainer.trainingID);

            using (StreamReader reader = new StreamReader(@"~\trnremoveEmail.html"))
            {
                body2 = reader.ReadToEnd();
            }
            body2 = body2.Replace("{name}", trainer.firstName);
            body2 = body2.Replace("{sitename}", site.siteName);
            body2 = body2.Replace("{address}", site.Address);
            body2 = body2.Replace("{date}", training.Date.ToShortDateString());
            body2 = body2.Replace("{time}", training_Trainer.training.startTime.ToShortTimeString());

            mailMessage2.To.Add(mail);
            mailMessage2.IsBodyHtml = true;
            mailMessage2.Body = body2;
            mailMessage2.Subject = "You have been removed from a training Session at " + site.siteName + ", " + site.City + ", on " + training.Date.ToShortDateString() + " - " + training.part;

            client.Send(mailMessage2);

            if (training_Trainer == null)
            {
                return NotFound();
            }

            return View(training_Trainer);
        }

        // POST: Training_Trainer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var training_Trainer = await _context.Training_Trainers.FindAsync(id);
            _context.Training_Trainers.Remove(training_Trainer);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { id = training_Trainer.trainingID });
        }

        private bool Training_TrainerExists(int id)
        {
            return _context.Training_Trainers.Any(e => e.ID == id);
        }
    }
}
