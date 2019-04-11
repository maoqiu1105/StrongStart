using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StrongStart.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using StrongStart.Data;
using Microsoft.AspNetCore.Identity;
using OfficeOpenXml;

namespace StrongStart.Controllers
{
    [Authorize(Roles ="Admin,RPC")]
    public class TrainingsController : Controller
    {
        private readonly StrongStartContext _context;
        private readonly ApplicationDbContext _appContext;

        public TrainingsController(
            StrongStartContext context, 
            ApplicationDbContext appContext,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _appContext = appContext;
        }

        // GET: Trainings by different filter
        public async Task<IActionResult> Index(string Training_progress_status,string SchoolName)
        {
            //get basic all training list
            var trainings = _context.Trainings.Include(t => t.site).Include(t => t.term).Cast<Training>();

            //get training progress status is only created
            if (Training_progress_status == "created")
            {
                trainings = trainings.Where(t => t.training_Progress_Status == Training_Progress_Status.Created);
                ViewBag.progress = "Not Approved";
            }

            //get training progress status is only approved
            else if (Training_progress_status == "Approved")
            {
                trainings = trainings.Where(t => t.training_Progress_Status == Training_Progress_Status.Approved);
                ViewBag.progress = "Approved";
            }

            //get training progress status is only published
            else if (Training_progress_status == "Published")
            {
                trainings = trainings.Where(t => t.training_Progress_Status == Training_Progress_Status.Published);
                ViewBag.progress = "Published";
            }

            //get training progress status is only finished
            else if (Training_progress_status == "Finished")
            {
                trainings = trainings.Where(t => t.training_Progress_Status == Training_Progress_Status.Finished);
                ViewBag.progress = "Finished";
            }
            else
            {

            }

            //narrow the filtered training list filter by school name 
            if (SchoolName != null)
            {
                trainings = trainings.Where(t => t.site.siteName.Contains(SchoolName));
                ViewBag.SchoolName = SchoolName;
            }

            return View(await trainings.ToListAsync());
        }

        public async Task<IActionResult> exportExcel()
        {

            var stream = new System.IO.MemoryStream();
            using (ExcelPackage package = new ExcelPackage(stream))
            {
                //get the training list
                var trainings = _context.Trainings.Include(t => t.site).Include(t => t.term).Include(t => t.site.region);

                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Trainings");

                worksheet.Cells[1, 1].Value = "Region";
                worksheet.Cells[1, 2].Value = "Site Name";
                worksheet.Cells[1, 3].Value = "Date";
                worksheet.Cells[1, 4].Value = "Time";
                worksheet.Cells[1, 5].Value = "Part 1 or 2";
                worksheet.Cells[1, 6].Value = "Trainer #1";
                worksheet.Cells[1, 7].Value = "Trainer #2";
                worksheet.Row(1).Style.Font.Bold = true;

                int row = 2;
                int trainerColumn = 6;
                foreach (var item in trainings)
                {

                    worksheet.Cells[row, 1].Value = item.site.region.regionCode;
                    worksheet.Cells[row, 2].Value = item.site.siteName;
                    worksheet.Cells[row, 3].Value = item.Date.ToShortDateString();
                    worksheet.Cells[row, 4].Value = item.startTime.ToShortTimeString() + " to " + item.endTime.ToShortTimeString();
                    worksheet.Cells[row, 5].Value = item.part;

                    if (_context.Training_Trainers.Select(t => t.trainingID).Contains(item.trainingID))
                    {
                        var tempTrainers = _context.Training_Trainers.Include(t => t.trainer).Where(t => t.trainingID == item.trainingID).Select(t => t.trainer);
                        foreach (var trainer in tempTrainers)
                        {
                            worksheet.Cells[row, trainerColumn].Value = trainer.firstName + " " + trainer.lastName;
                            trainerColumn++;
                        }
                    }

                    row++;
                    trainerColumn = 6;
                }


                package.Save();
            }

            string fileName = @"Trainings.xlsx";
            string fileType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            stream.Position = 0;
            return File(stream, fileType, fileName);


        }

        // GET: get Attandance list by training ID
        public async Task<IActionResult> Attendance(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var VolunteerIDList =  _context.Training_Volunteers.Where(r => r.trainingID == Convert.ToInt32(id)).Select(r => r.volunteerID).ToList();
            

            List<ApplicationUser> volunteerList = new List<ApplicationUser>();

            for (int i = 0; i < VolunteerIDList.Count(); i++)
            {
                //get the temp Volunteer belong to this training
                var tempVolunteer = _appContext.Users.SingleOrDefault(u => u.Id == VolunteerIDList[i]);

                //apply volunteer name in full name format
                volunteerList.Add(tempVolunteer);
            }

            ViewData["volunteerList"] = volunteerList;

            var training = await _context.Trainings
                .Include(t => t.site)
                .Include(t => t.term)
                .FirstOrDefaultAsync(m => m.trainingID == id);

            //To help display basic training info in attendance page
            HttpContext.Session.SetString("trainingID", id.ToString());
            HttpContext.Session.SetString("siteName", training.site.siteName);
            HttpContext.Session.SetString("Date", training.Date.ToShortDateString());
            HttpContext.Session.SetString("startTime", training.startTime.ToShortTimeString());
            HttpContext.Session.SetString("endTime", training.endTime.ToShortTimeString());
            HttpContext.Session.SetString("Part", training.part.ToString());
            return View();
        }

        [Authorize(Roles = "RPC")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmAttendance(List<string> volunteerAttended,string trainingID)
        {
            var training = _context.Trainings.FirstOrDefault(t => t.trainingID == int.Parse(trainingID));
            for (int i = 0; i < volunteerAttended.Count(); i++)
            {
                var tempUser = _appContext.Users.FirstOrDefault(u => u.Id == volunteerAttended[i]);
                if (training.part == Part.part1)
                {
                    tempUser.FinishPart1 = int.Parse(trainingID);
                }
                else
                {
                    tempUser.FinishPart2 = int.Parse(trainingID);
                }

                if (tempUser.FinishPart1 != 0 && tempUser.FinishPart2 != 0)
                {
                    tempUser.isQualify = YesNO.Yes;
                }
                _appContext.Update(tempUser);
                await _appContext.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }


        //Switch Created Status to Approved
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> approve(List<int> trainingArr)
        {
            for (int i = 0; i < trainingArr.Count(); i++)
            {
                var training = _context.Trainings.FirstOrDefault(t => t.trainingID == trainingArr[i]);
                training.training_Progress_Status = Training_Progress_Status.Approved;
                _context.Update(training);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
        
        //Switch Approved Status to Published
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> publish(List<int> trainingArr)
        {
            for (int i = 0; i < trainingArr.Count(); i++)
            {
                var training = _context.Trainings.FirstOrDefault(t => t.trainingID == trainingArr[i]);
                training.training_Progress_Status = Training_Progress_Status.Published;
                _context.Update(training);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        //Start to finish training, display training info and trainer info
        public async Task<IActionResult> Finalize_Training(int? id, int training_trainerID)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            var training = await _context.Trainings
                .Include(t => t.site)
                .Include(t => t.term)
                .FirstOrDefaultAsync(t => t.trainingID == id);

            ViewData["trainerList"] = _context.Training_Trainers.Include(t => t.trainer).Where(t => t.trainingID == id);

            return View(training);
        }

        //confirm tranining and trainer, persist in DB
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Finalize_Training(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var training = _context.Trainings.Where(t => t.trainingID == id).FirstOrDefault();
            training.training_Progress_Status = Training_Progress_Status.Finished;

            var volunteerList = _context.Training_Volunteers.Where(t => t.trainingID == id).Select(t => t.volunteerID).ToList();

            for (int i = 0; i < volunteerList.Count(); i++)
            {
                var tempUser = _appContext.Users.FirstOrDefault(u => u.Id == volunteerList[i]);
                if (training.part == Part.part1)
                {
                    tempUser.FinishPart1 = id;
                }
                else
                {
                    tempUser.FinishPart2 = id;
                }

                if (tempUser.FinishPart1 != 0 && tempUser.FinishPart2 != 0)
                {
                    tempUser.isQualify = YesNO.Yes;
                }
                _appContext.Update(tempUser);
                await _appContext.SaveChangesAsync();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(training);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrainingExists(training.trainingID))
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

            return View();
        }

        public async Task<IActionResult> Finalize_trainer(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");

            }
            var training_trainer = await _context.Training_Trainers
               .Include(t => t.training)
               .Include(t => t.training.site)
               .Include(t => t.trainer)
               .Include(t => t.training.term)
               .FirstOrDefaultAsync(t => t.ID == id);

            return View(training_trainer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Finalize_trainer(int id, [Bind("hasKit", "driveDistance")] Training_Trainer Training_Trainer)
        {
            var final_Training_Trainer = _context.Training_Trainers.Where(t => t.ID == id).FirstOrDefault();

            final_Training_Trainer.hasKit = Training_Trainer.hasKit;
            final_Training_Trainer.driveDistance = Training_Trainer.driveDistance;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(final_Training_Trainer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Training_TrainerExists(final_Training_Trainer.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Finalize_Training", new { id = final_Training_Trainer.trainingID });
            }
            return View();
        }

        
        // GET: Trainings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var training = await _context.Trainings
                .Include(t => t.site)
                .Include(t => t.term)
                .FirstOrDefaultAsync(m => m.trainingID == id);
            if (training == null)
            {
                return NotFound();
            }
            return View(training);
        }
        
        // GET: Trainings/Create
        public IActionResult Create()
        {
            ViewData["siteID"] = new SelectList(_context.Sites, "siteID", "siteName");
            ViewData["termID"] = new SelectList(_context.Terms, "termID", "termName");
            ViewData["linkTraining"] =_context.Trainings.Where(x => x.part.ToString() == "part1" && x.trainingName != null);
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("trainingID,siteID,termID,startTime,endTime,Date,permit,specInstructions,part,Capacity,linkID,traing_Progress_Status")] Training training, string doAddPart2)
        {
            training.site = _context.Sites.Where(x => x.siteID == training.siteID).FirstOrDefault();
            training.trainingName = training.site.siteName; 
            if (training.part.ToString() == "part1")
            {
                training.linkID = null;
            }

            if (ModelState.IsValid)
            {
                _context.Add(training);
                await _context.SaveChangesAsync();

            }
            return RedirectToAction("Create", "Training_Trainer", new { id = training.trainingID });
            ViewData["siteID"] = new SelectList(_context.Sites, "siteID", "siteName");
            ViewData["termID"] = new SelectList(_context.Terms, "termID", "termName");
            ViewData["linkID"] = new SelectList(_context.Trainings.Where(x => x.part.ToString() == "part1"), "trainingID", "trainingName");
        }

        // GET: Trainings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var training = await _context.Trainings.FindAsync(id);
            if (training == null)
            {
                return NotFound();
            }
            ViewData["siteID"] = new SelectList(_context.Sites, "siteID", "siteName", training.siteID);
            ViewData["termID"] = new SelectList(_context.Terms, "termID", "termName", training.termID);
            return View(training);
        }

        // POST: Trainings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("trainingID,siteID,termID,startTime,endTime,Date,permit,specInstructions,part,Capacity,linkID,traing_Progress_Status")] Training training)
        {
            if (id != training.trainingID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(training);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrainingExists(training.trainingID))
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
            ViewData["siteID"] = new SelectList(_context.Sites, "siteID", "siteName", training.siteID);
            ViewData["termID"] = new SelectList(_context.Terms, "termID", "termName", training.termID);
            return View(training);
        }

        // GET: Trainings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var training = await _context.Trainings
                .Include(t => t.site)
                .Include(t => t.term)
                .FirstOrDefaultAsync(m => m.trainingID == id);
            if (training == null)
            {
                return NotFound();
            }

            return View(training);
        }

        // POST: Trainings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var training = await _context.Trainings.FindAsync(id);
            _context.Trainings.Remove(training);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrainingExists(int id)
        {
            return _context.Trainings.Any(e => e.trainingID == id);
        }
        private bool Training_TrainerExists(int id)
        {
            return _context.Training_Trainers.Any(e => e.ID == id);
        }
    }
}
