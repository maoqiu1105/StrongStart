using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StrongStart.Class;
using StrongStart.Data;
using StrongStart.Models;
using StrongStart.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Server;
using Microsoft.AspNetCore.Hosting;

namespace StrongStart.Controllers
{
    public class RegistrationsController : Controller
    {
        private readonly StrongStartContext _context;
        private readonly ApplicationDbContext appcontext;
        private readonly IHostingEnvironment _appEnvironment;
        public RegistrationsController(StrongStartContext context, IHostingEnvironment appEnvironment)
        {
            _context = context;
            UserManager<ApplicationUser> _userManager;
        }

        // GET: Registrations
        public async Task<IActionResult> Index(string schoolName, string postalCode)
        {

            List<Training> lst = null;

            if (schoolName != null)
            {
                var schoolNameList = _context.Trainings.Include(t => t.site).Include(t => t.term)
                    .Where(t => t.site.siteName.Contains(schoolName));
                lst = await schoolNameList.ToListAsync();
            }
            else if (postalCode != null)
            {
                GeoCode geoCode = new GeoCode(postalCode);
                string regionName = "";

                if (geoCode.GetGeoResources() != null)
                {
                    regionName = geoCode.GetGeoResources().Address.adminDistrict2;
                }
                else
                {
                    ViewBag.errorPostalCode = "can not find any site in your postal code.";
                }

                var regionNameList = _context.Trainings.Include(t => t.site).Include(t => t.site.region).Include(t => t.term)
                    .Where(t => t.site.region.regionName.Contains(regionName));

                lst = await regionNameList.ToListAsync();

            }
            else if (schoolName != null && postalCode != null)
            {
                GeoCode geoCode = new GeoCode(postalCode);
                string regionName = "";

                if (geoCode.GetGeoResources() != null)
                {
                    regionName = geoCode.GetGeoResources().Address.adminDistrict2;
                }
                else
                {
                    ViewBag.errorPostalCode = "can not find any site in your postal code.";
                }

                var shcoolregionList = _context.Trainings.Include(t => t.site).Include(t => t.site.region).Include(t => t.term)
                    .Where(t => t.site.region.regionName.Contains(regionName))
                    .Where(t => t.site.region.regionName.Contains(schoolName));

                lst = await shcoolregionList.ToListAsync();
            }
            else
            {
                var strongStartContext = _context.Trainings.Include(t => t.site).Include(t => t.term);
                lst = await strongStartContext.ToListAsync();
            }

            string trainingID;
            trainingID = HttpContext.Session.GetString("training_id");

            if (trainingID != null)
            {
                int trainId = Convert.ToInt32(trainingID);
                HttpContext.Session.Remove("training_id");
                return RedirectToAction("Details", new { id = trainId });
            }
            else if (trainingID == null)
            {

            }

            List<RegistrationViewModel> vmList = new List<RegistrationViewModel>();

            foreach (Training t in lst)
            {
                RegistrationViewModel model = new RegistrationViewModel();
                int attendanceCount = _context.Training_Volunteers.Where(i => i.trainingID == t.trainingID).Count();
                var capacity = t.Capacity;
                double capDiv = (double)attendanceCount / capacity;
                model.siteName = t.site.siteName;
                model.Date = t.Date.ToLongDateString();
                model.startTime = t.startTime.ToShortTimeString();
                model.endTime = t.endTime.ToShortTimeString();
                model.part = t.part;
                model.training_Status = t.training_Status;
                model.Training_Progress_Status = t.training_Progress_Status;
                model.trainingID = t.trainingID;

                if ((capDiv) <= 0.5)
                {
                    model.Capacity = "Lots of Space!";

                }
                else if (capDiv >= 0.5 && capDiv < 1)
                {
                    model.Capacity = "Almost Full!";
                }
                else if (capDiv == 1)
                {
                    model.Capacity = "Full!";
                }
                else
                {
                    model.Capacity = "null";
                }
                vmList.Add(model);
            }

            return View(vmList);
        }

        // GET: Registrations/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var approvedTraining = await _context.Trainings
                .Include(t => t.site)
                .Include(t => t.term)
                .FirstOrDefaultAsync(m => m.trainingID == id);

            if (approvedTraining == null)
            {
                return NotFound();
            }

            return View(approvedTraining);
        }

        public async Task<IActionResult> Register(int id, string loginStatus)
        {
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("strongstarttesting@gmail.com", "Flakkie4u2");


            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var mail = this.User.Identities.FirstOrDefault().Name;
            if (loginStatus == "loggedin")
            {
                var trnVol = new Training_Volunteer();
                trnVol.trainingID = id;
                trnVol.volunteerID = userId;
                trnVol.training = _context.Trainings.Where(x => x.trainingID == id).FirstOrDefault();

                var tempTraining_Volunteer = _context.Training_Volunteers.Where(t => t.volunteerID == userId).Where(t => t.trainingID == id).FirstOrDefault();

                if (tempTraining_Volunteer == null)
                {
                    _context.Add(trnVol);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    TempData["hasRegistered"] = "Sorry, you have registered this training, try others";
                    return RedirectToAction("Index");
                }
            }
            else if (loginStatus == "NOTloggedin")
            {
                HttpContext.Session.SetString("training_id", id.ToString());
                return Redirect("~/Identity/Account/Login");
            }

            var registeringTraining = await _context.Trainings
                     .Include(t => t.site)
                     .Include(t => t.term)
                     .FirstOrDefaultAsync(m => m.trainingID == id);

            Training_Volunteer registrationPart2 = new Training_Volunteer();
            var training = _context.Trainings.Where(x => x.linkID == id && x.part.ToString().Equals("part2")).FirstOrDefault();
            string month;
            string day;
            if (registeringTraining.Date.Month < 10)
            {
                month = "0" + registeringTraining.Date.Month;
            }
            else
            {
                month = registeringTraining.Date.Month.ToString();
            }

            if (registeringTraining.Date.Day < 10)
            {
                day = "0" + registeringTraining.Date.Day;
            }
            else
            {
                day = registeringTraining.Date.Day.ToString();
            }


            string date = month + "/" + day + "/" + registeringTraining.Date.Year;
            string startdate = date + " " + registeringTraining.startTime.ToShortTimeString();
            string enddate = date + " " + registeringTraining.endTime.ToShortTimeString();
            string location = registeringTraining.site.Address;
            string title = registeringTraining.site.siteName + " Training Session: StrongStart";
            string time = registeringTraining.startTime.ToShortTimeString();

            if (training != null)
            {
                registrationPart2.trainingID = training.trainingID;
                registrationPart2.volunteerID = userId;
                registrationPart2.training = training;
                _context.Add(registrationPart2);
                await _context.SaveChangesAsync();

                MailMessage mailMessage2 = new MailMessage();
                mailMessage2.From = new MailAddress("strongstarttesting@gmail.com");

                mailMessage2.From = new MailAddress("strongstarttesting@gmail.com");
                string body2 = string.Empty;
                using (StreamReader reader = new StreamReader(@"~\regEmail.html"))
                {
                    body2 = reader.ReadToEnd();
                }
                body2 = body2.Replace("{name}", mail);
                body2 = body2.Replace("{sitename}", "site");
                body2 = body2.Replace("{address}", "address");
                body2 = body2.Replace("{date}", "date");
                body2 = body2.Replace("{time}", "time");
                body2 = body2.Replace("{start}","e");
                body2 = body2.Replace("{end}","enddate");
                body2 = body2.Replace("{title}", "title");
                body2 = body2.Replace("{location}", "location");

                mailMessage2.To.Add(mail);
                mailMessage2.IsBodyHtml = true;
                mailMessage2.Body = body2;
                mailMessage2.Subject = "Reminder of your Strong Start Volunteer Coach Training Session at " + "Paisley Road Public" + ", " + "Kitchener" + ", on " + "08/24/2019 " + " - " + "Part 2";

                client.Send(mailMessage2);
            }



            HttpContext.Session.SetString("title", title);
            HttpContext.Session.SetString("end", enddate);
            HttpContext.Session.SetString("start", startdate);
            HttpContext.Session.SetString("location", location);

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("strongstarttesting@gmail.com");

            string sitename = registeringTraining.site.siteName;
            mailMessage.From = new MailAddress("strongstarttesting@gmail.com");
            string body = string.Empty;
                using (StreamReader reader = new StreamReader(@"~\regEmail.html"))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{name}", mail);
            body = body.Replace("{sitename}", sitename);
            body = body.Replace("{address}", registeringTraining.site.Address);
            body = body.Replace("{date}", date);
            body = body.Replace("{time}", time);
            body = body.Replace("{start}", startdate);
            body = body.Replace("{end}", enddate);
            body = body.Replace("{title}", title);
            body = body.Replace("{location}", location);

            mailMessage.To.Add(mail);
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = body;
            mailMessage.Subject = "Reminder of your Strong Start Volunteer Coach Training Session at " + sitename + ", " + registeringTraining.site.City + ", on " + date + " - " + registeringTraining.part;

            client.Send(mailMessage);

            return View("Success");

        }


        public async Task<IActionResult> Success()
        {

            return View();
        }

        private bool RegistrationExists(int id)
        {
            return _context.Training_Volunteers.Any(e => e.ID == id);
        }

        public ActionResult RemoveTraining(int trainingId)
        {
            string volunteerId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var training = _context.Training_Volunteers.Where(i => i.volunteerID == volunteerId && i.trainingID == trainingId).FirstOrDefault();

            _context.Training_Volunteers.Remove(training);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
