using _0611_2.Data;
using _0611_2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _0611_2.Controllers
{
    public class SchedulesTable : Controller
    {
        private readonly _0611_2Context _context;

        public SchedulesTable(_0611_2Context context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(DateTime? startDate)
        {
            DateTime monthStart = startDate ?? DateTime.Today;
            ViewData["StartDate"] = monthStart;

            var model = await GetMonthlySchedules(monthStart);
            return View(model);
        }

        private async Task<Dictionary<(DateTime Date, int TimeSlot), List<(int DoctorId, string DoctorName)>>>
            GetMonthlySchedules(DateTime startDate)
        {
            var schedules = new Dictionary<(DateTime Date, int TimeSlot), List<(int DoctorId, string DoctorName)>>();
            var daysInMonth = DateTime.DaysInMonth(startDate.Year, startDate.Month);

            for (int day = 1; day <= daysInMonth; day++)
            {
                var date = new DateTime(startDate.Year, startDate.Month, day);

                var appointments = await _context.Schedule
                    .Include(s => s.Doctor)
                    .Where(s => s.Date == date)
                    .ToListAsync();

                foreach (var appointment in appointments)
                {
                    var key = (date, appointment.TimeSlot);
                    if (!schedules.ContainsKey(key))
                    {
                        schedules[key] = new List<(int, string)>();
                    }
                    schedules[key].Add((appointment.DoctorId.Value, appointment.Doctor.DoctorName));
                }
            }

            return schedules;
        }

        public async Task<IActionResult> Appointment(int doctorId, DateTime date, int timeSlot)
        {
            var customers = await _context.Customer.ToListAsync();
            var model = new Appointment
            {
                Schedule = new Schedule
                {
                    DoctorId = doctorId,
                    Date = date,
                    TimeSlot = timeSlot
                },
                Customers = customers
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> MakeAppointment(int doctorId, DateTime date, int timeSlot, int customerId)
        {
            var schedule = await _context.Schedule
                .FirstOrDefaultAsync(s => s.DoctorId == doctorId && s.Date == date && s.TimeSlot == timeSlot);

            if (schedule != null)
            {
                schedule.CustomerId = customerId;
                _context.Schedule.Update(schedule);
                await _context.SaveChangesAsync();
            }
            else
            {
                return NotFound("预约项未找到");
            }

            return RedirectToAction("Index");
        }
    }
}
