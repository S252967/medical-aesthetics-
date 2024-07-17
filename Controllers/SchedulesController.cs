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
        public async Task<IActionResult> Index()
        {
            var schedules = _context.Schedule.Include(s => s.Customer).Include(s => s.Doctor);
            return View(await schedules.ToListAsync());
        }

        // GET: Schedules/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Schedule == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedule
                .Include(s => s.Customer)
                .Include(s => s.Doctor)
                .FirstOrDefaultAsync(m => m.ScheduleId == id);
            if (schedule == null)
            {
                return NotFound();
            }

            return View(schedule);
        }

        // GET: Schedules/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Set<Customer>(), "CustomerId", "CustomerName");
            ViewData["DoctorId"] = new SelectList(_context.Set<Doctor>(), "DoctorId", "DoctorName");
            return View();
        }

        // POST: Schedules/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ScheduleId,DoctorId,Date,TimeSlot,AppointmentStatus,CustomerId")] Schedule schedule)
        {
            // if (ModelState.IsValid)
            // {
            _context.Add(schedule);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            // }
            // ViewData["CustomerId"] = new SelectList(_context.Set<Customer>(), "CustomerId", "CustomerName", schedule.CustomerId);
            // ViewData["DoctorId"] = new SelectList(_context.Set<Doctor>(), "DoctorId", "DoctorName", schedule.DoctorId);
            // return View(schedule);
        }

        // GET: Schedules/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Schedule == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedule.FindAsync(id);
            if (schedule == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Set<Customer>(), "CustomerId", "CustomerName", schedule.CustomerId);
            ViewData["DoctorId"] = new SelectList(_context.Set<Doctor>(), "DoctorId", "DoctorName", schedule.DoctorId);
            return View(schedule);
        }

        // POST: Schedules/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ScheduleId,DoctorId,Date,TimeSlot,AppointmentStatus,CustomerId")] Schedule schedule)
        {
            if (id != schedule.ScheduleId)
            {
                return NotFound();
            }

            ///if (ModelState.IsValid)
            ///{
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
            ///}
            //ViewData["CustomerId"] = new SelectList(_context.Set<Customer>(), "CustomerId", "CustomerName", schedule.CustomerId);
            //ViewData["DoctorId"] = new SelectList(_context.Set<Doctor>(), "DoctorId", "DoctorName", schedule.DoctorId);
            //return View(schedule);
        }

        // GET: Schedules/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Schedule == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedule
                .Include(s => s.Customer)
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
            if (_context.Schedule == null)
            {
                return Problem("Entity set '_0611_2Context.Schedule'  is null.");
            }
            var schedule = await _context.Schedule.FindAsync(id);
            if (schedule != null)
            {
                _context.Schedule.Remove(schedule);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScheduleExists(int id)
        {
            return (_context.Schedule?.Any(e => e.ScheduleId == id)).GetValueOrDefault();
        }
    }
}
