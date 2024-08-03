using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using _0611_2.Data;
using _0611_2.Models;

namespace _0611_2.Controllers
{
    public class SchedulesController : Controller
    {
        private readonly _0611_2Context _context;

        public SchedulesController(_0611_2Context context)
        {
            _context = context;
        }

        // GET: Schedules
        public async Task<IActionResult> Index(DateTime? month)
        {
            DateTime firstDayOfMonth = month.GetValueOrDefault(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1));
            ViewData["Month"] = firstDayOfMonth;

            var schedules = await _context.Schedule
                .Include(s => s.Doctor)
                .Where(s => s.Date >= firstDayOfMonth && s.Date < firstDayOfMonth.AddMonths(1))
                .ToListAsync();

            var model = new Dictionary<(DateTime Date, int TimeSlot), List<(int DoctorId, string DoctorName)>>();
            foreach (var schedule in schedules)
            {
                var key = (schedule.Date.Date, (int)schedule.TimeSlot);
                if (!model.ContainsKey(key))
                {
                    model[key] = new List<(int DoctorId, string DoctorName)>();
                }
                model[key].Add((schedule.DoctorId, schedule.Doctor?.DoctorName ?? "Unknown Doctor"));
            }

            // Fetch all doctors for the dropdown
            ViewData["Doctors"] = await _context.Doctor.ToListAsync();

            return View(model);
        }



        // GET: Schedules/Create
        public async Task<IActionResult> Create(DateTime? date)
        {
            ViewBag.Doctors = new SelectList(await _context.Doctor.ToListAsync(), "DoctorId", "DoctorName");
            ViewBag.Customers = new SelectList(await _context.Customer.ToListAsync(), "CustomerId", "CustomerName");
            ViewData["SelectedDate"] = date ?? DateTime.Now; // 如果未提供日期，则使用当前日期
            return View();
        }

        // POST: Schedules/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DoctorId,Date,TimeSlot,CustomerId")] Schedule schedule)
        {
            // 记录绑定的模型数据以调试
            Console.WriteLine($"DoctorId: {schedule.DoctorId}, CustomerId: {schedule.CustomerId}");

            //if (ModelState.IsValid)
            //{
                try
                {
                    _context.Add(schedule);
                    await _context.SaveChangesAsync();

                    Console.WriteLine("Schedule added successfully.");
                    TempData["SuccessMessage"] = $"Schedule for {schedule.Doctor?.DoctorName ?? "Unknown Doctor"} added successfully.";
                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception occurred: {ex.Message}");
                    return Json(new { success = false, error = "An error occurred while saving the schedule. Please try again later." });
                }
            //}

            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            return Json(new { success = false, errors = errors });
        }







        // GET: Schedules/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedule
                .Include(s => s.Doctor)
                .FirstOrDefaultAsync(m => m.ScheduleId == id);
            if (schedule == null)
            {
                return NotFound();
            }

            return View(schedule);
        }

        // GET: Schedules/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedule.FindAsync(id);
            if (schedule == null)
            {
                return NotFound();
            }

            ViewData["DoctorId"] = new SelectList(await _context.Doctor.ToListAsync(), "DoctorId", "DoctorName", schedule.DoctorId);
            return View(schedule);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ScheduleId,DoctorId,Date,TimeSlot,AppointmentStatus,CustomerId")] Schedule schedule)
        {
            if (id != schedule.ScheduleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(schedule);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScheduleExists(schedule.ScheduleId))
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

            ViewData["DoctorId"] = new SelectList(await _context.Doctor.ToListAsync(), "DoctorId", "DoctorName", schedule.DoctorId);
            return View(schedule);
        }


        // GET: Schedules/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedule
                .Include(s => s.Doctor)
                .FirstOrDefaultAsync(m => m.ScheduleId == id);
            if (schedule == null)
            {
                return NotFound();
            }

            return View(schedule);
        }

        // POST: Schedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var schedule = await _context.Schedule.FindAsync(id);
            _context.Schedule.Remove(schedule);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScheduleExists(int id)
        {
            return _context.Schedule.Any(e => e.ScheduleId == id);
        }

        // GET: Schedules/DoctorSchedules/5
        public async Task<IActionResult> DoctorSchedules(int? doctorId)
        {
            if (doctorId == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctor.FindAsync(doctorId);
            if (doctor == null)
            {
                return NotFound();
            }

            var schedules = await _context.Schedule
                .Where(s => s.DoctorId == doctorId)
                .OrderBy(s => s.Date)
                .ThenBy(s => s.TimeSlot)
                .ToListAsync();

            ViewData["Doctor"] = doctor;

            return View(schedules);
        }
        public IActionResult test()
        {
            return View();
        }

    }
}
